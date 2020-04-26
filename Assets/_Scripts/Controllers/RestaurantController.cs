using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Globalization;

public class RestaurantController : MonoBehaviour
{
    public delegate void SumUploaded(float iSum);
    public static SumUploaded OnSumUploaded;

    private static RestaurantController _instance;

    public DataContainers.Menu restaurantMenu;
    public Dictionary<string, DataContainers.OrderItem> newOrderItems;

    private void Start()
    {
        CultureInfo.CurrentCulture = new CultureInfo("en-US", false);
    }

    public void Initialize()
    {
        newOrderItems = new Dictionary<string, DataContainers.OrderItem>();
    }

    public static RestaurantController Instance
    {
        get { return _instance; }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            OrderSelectedItems();
        }
    }

    public void OrderSelectedItems()
    {
        Debug.Log("0");
        if (newOrderItems.Count <= 0)
            return;
        StartCoroutine(RestController.Instance.AddNewItemToOrder(UserController.Instance.order.restaurant, UserController.Instance.order.table, UserController.Instance.user.id, UserController.Instance.order.id));
    
    }

    public void addItemToOrder(DataContainers.MenuItem iItem)
    {
        if (newOrderItems.ContainsKey(iItem.id))
        {
            newOrderItems[iItem.id].amount += 1;
            newOrderItems[iItem.id].price += 1 * iItem.price;
        }
        else
        {
            newOrderItems.Add(iItem.id, new DataContainers.OrderItem());
            newOrderItems[iItem.id].amount = 1;
            newOrderItems[iItem.id].id = iItem.id;
            newOrderItems[iItem.id].price = 1 * iItem.price;
            newOrderItems[iItem.id].name = iItem.name;
        }
        float f = 0.00f;
        foreach (var item in newOrderItems.Values)
        {
            f += item.price;
        }
        OnSumUploaded?.Invoke(f);
    }

    public void removeItemFromOrder(DataContainers.MenuItem iItem)
    {
        if (newOrderItems.ContainsKey(iItem.id))
        {
            if (newOrderItems[iItem.id].amount == 1)
            {
                newOrderItems.Remove(iItem.id);
            }
            else
            {
                newOrderItems[iItem.id].amount -= 1;
                newOrderItems[iItem.id].price -= 1 * iItem.price;
            }
            float f = 0.00f;
            foreach (var item in newOrderItems.Values)
            {
                f += item.price;
            }
            OnSumUploaded?.Invoke(f);
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

    }
}
