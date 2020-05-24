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
    public delegate void AmountUploaded(int iAmount);
    public static AmountUploaded OnAmountUploaded;

    private static RestaurantController _instance;

    public DataContainers.Menu restaurantMenu;
    public DataContainers.MenuItem openedMenuItem;
    private Dictionary<string, DataContainers.OrderItem> newOrderItems;
    private Dictionary<string, int> newTransfers;
    public DataContainers.OrderItem TransferItem;
    public DataContainers.TransferRequest transferRequest;
    public DataContainers.JoinRequest joinRequest;


    private void Start()
    {
        CultureInfo.CurrentCulture = new CultureInfo("en-US", false);
    }

    public void Initialize()
    {
        newOrderItems = new Dictionary<string, DataContainers.OrderItem>();
        newTransfers = new Dictionary<string, int>();
    }

    public static RestaurantController Instance
    {
        get { return _instance; }
    }

    public async void OrderSelectedItems()
    {
        Debug.Log("0");
        if (newOrderItems.Count <= 0)
            return;
        //StartCoroutine(RestController.Instance.AddNewItemToOrder(
        //    UserController.Instance.order.restaurant, 
        //    UserController.Instance.order.table, 
        //    UserController.Instance.user.id, 
        //    UserController.Instance.order.id, 
        //    newOrderItems));

        DataContainers.Order newOrder = UserController.Instance.order;
        var suborder = newOrder.suborders[UserController.Instance.user.id];
        foreach (var item in newOrderItems)
        {
            if (suborder.items.ContainsKey(item.Key))
            {
                suborder.items[item.Key].delivered += item.Value.amount;
            }
            else
            {
                suborder.items.Add(item.Key, new DataContainers.OrderItem(item.Value.amount, item.Key));
            }
        }
        await DB.Order.UpdateOrder(UserController.Instance.order.restaurant, UserController.Instance.order.table, UserController.Instance.order.id, newOrder);
    }

    public async void TransferItems()
    {
        DataContainers.Order newOrder = UserController.Instance.order;
        var suborder = newOrder.suborders[UserController.Instance.user.id];

        foreach (var item in newTransfers)
        {
            //  Debug.Log(item);
            await DB.Order.CreateNewTransferRequest(
                UserController.Instance.order.restaurant, 
                UserController.Instance.order.table, 
                UserController.Instance.user.id, 
                item.Key, 
                UserController.Instance.order.id, 
                new DataContainers.transferItem(TransferItem.id, item.Value),
                suborder.items[TransferItem.id].delivered - item.Value, suborder.items[TransferItem.id].transfered + item.Value
                );

            //StartCoroutine(RestController.Instance.CreateNewTransferRequest(UserController.Instance.order.restaurant, UserController.Instance.order.table, UserController.Instance.user.id, item.Key, UserController.Instance.order.id, new DataContainers.transferItem(TransferItem.id, item.Value)));
        }
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

    public void addItemToTransfer(string iUserToSendId)
    {
        TransferItem.amount--;
        if (newTransfers.ContainsKey(iUserToSendId))
        {
            newTransfers[iUserToSendId] += 1;
        }
        else
        {
            newTransfers.Add(iUserToSendId, 1);
        }
        OnAmountUploaded?.Invoke(TransferItem.amount);

    }

    public void removeItemToTransfer(string iUserToSendId)
    {
        if (newTransfers.ContainsKey(iUserToSendId))
        {
            TransferItem.amount++;
            if (newTransfers[iUserToSendId] == 1)
            {
                newTransfers.Remove(iUserToSendId);
            }
            else
            {
                newTransfers[iUserToSendId] -= 1;
            }
        }
        OnAmountUploaded?.Invoke(TransferItem.amount);
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
