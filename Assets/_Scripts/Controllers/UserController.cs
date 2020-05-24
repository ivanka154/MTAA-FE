using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserController : MonoBehaviour
{
    private static UserController _instance;

    public DataContainers.User user;
    public DataContainers.Menu menu;
    public DataContainers.Order order;

    public string tableId;
    public string restaurantId;
    public string orderId;

    public static UserController Instance
    {
        get { return _instance; }
    }

    private void Start()
    {
        RestController.OnUserLogedIn += setLoggedUser;
        RestController.OnMenuLoaded += setMenu;
        RestController.OnOrderLoaded += setOrder;
        DB.Order.OnOrderLoaded += setOrder;
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

    public void logIn(string email, string password)
    {
        StartCoroutine(RestController.Instance.LogIn(email, password));
    }

    public void register(string name, string email, string password)
    {
        StartCoroutine(RestController.Instance.Register(name, email, password));
    }

    public void setLoggedUser(DataContainers.User iUser)
    {
        user = iUser;
        UIViewManager.Instance.Username.text = iUser.name;
        UIViewManager.Instance.OpenPanel("CreateOrderView");
    }
    public void setMenu(DataContainers.Menu iMenu)
    {
        menu = iMenu;
    }
    public void setOrder(DataContainers.Order iOrder)
    {
        order = iOrder;
        UIViewManager.Instance.InitializePanel("TableView");
    }
}
