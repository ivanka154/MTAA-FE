using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TableViewController : UIView
{
    [SerializeField]
    private GameObject orderItemPrefab;
    [SerializeField]
    private GameObject memberItemPrefab;
    [SerializeField]
    private GameObject transferItemRequestPrefab;
    [SerializeField]
    private Transform allItemsScrollContent;
    [SerializeField]
    private Transform myItemsScrollContent;
    [SerializeField]
    private Transform membersScrollContent;
    [SerializeField]
    private Transform requestsScrollContent;
    [SerializeField]
    private Button orderButton;
    [SerializeField]
    private Button paymentButton;

    [SerializeField]
    private Michsky.UI.ModernUIPack.ModalWindowManager joinRequestPopUp;
    [SerializeField]
    private Michsky.UI.ModernUIPack.ModalWindowManager transferRequestPopUp;
    public override void Initialize()
    {
        orderButton.onClick.RemoveAllListeners();
        orderButton.onClick.AddListener(() => {
            UIViewManager.Instance.OpenPanel("RestaurantMenuView");
        });
        InitializeAllItems(UserController.Instance.order);
        InitializeMyItems(UserController.Instance.order);
        InitializeMembers(UserController.Instance.order);
        InitializeRequests(UserController.Instance.order);
        paymentButton.onClick.RemoveAllListeners();
        paymentButton.onClick.AddListener(async () => {
            List<string> transferRequestIds = new List<string>();
            foreach (var item in UserController.Instance.order.transferRequests.Keys)
            {
                transferRequestIds.Add(item);
            }
            foreach (var item in UserController.Instance.order.transferRequests.Values)
            {
                if (item.requirer.Equals(UserController.Instance.user.id) || item.aprover.Equals(UserController.Instance.user.id))
                {
                    UIViewManager.Instance.ErrorNotification("User has pending items");
                    return;
                }
            }
            DataContainers.Order newOrder = UserController.Instance.order;
            var suborder = newOrder.suborders[UserController.Instance.user.id];
            suborder.items = new Dictionary<string, DataContainers.OrderItem>();
            await DB.Order.UpdateOrder(UserController.Instance.order.restaurant, UserController.Instance.order.table, UserController.Instance.order.id, newOrder);
            //StartCoroutine(RestController.Instance.Payment(
            //    UserController.Instance.order.restaurant,
            //    UserController.Instance.order.table,
            //    UserController.Instance.user.id,
            //    "1",
            //    UserController.Instance.order.id,
            //    transferRequestIds
            //    ));
        });
        //throw new System.NotImplementedException();
    }

    public void InitializeAllItems(DataContainers.Order iOrder)
    {
        for (int i = 0; i < allItemsScrollContent.childCount; i++)
        {
            Destroy(allItemsScrollContent.GetChild(i).gameObject);
        }
        var allItems = iOrder.GetAllItems();
        foreach (var item in allItems)
        {
            GameObject go = Instantiate(orderItemPrefab, allItemsScrollContent);
            go.GetComponent<Prefabs.OrderItem>().Initialize(item.Value);
        }
    }

    public void InitializeMyItems(DataContainers.Order iOrder)
    {
        for (int i = 0; i < myItemsScrollContent.childCount; i++)
        {
            Destroy(myItemsScrollContent.GetChild(i).gameObject);
        }
        var myItems = iOrder.GetUsersItems(UserController.Instance.user.id);
        foreach (var item in myItems)
        {
            GameObject go = Instantiate(orderItemPrefab, myItemsScrollContent);
            go.GetComponent<Prefabs.OrderItem>().Initialize(item.Value, true);
        }
    }

    public void InitializeRequests(DataContainers.Order iOrder)
    {
        for (int i = 0; i < requestsScrollContent.childCount; i++)
        {
            Destroy(requestsScrollContent.GetChild(i).gameObject);
        }
        foreach (var item in iOrder.transferRequests.Values)
        {
            Debug.Log("request aprover: " + item.aprover);
            if (item.aprover.Equals(UserController.Instance.user.id))
            {
                GameObject go = Instantiate(transferItemRequestPrefab, requestsScrollContent);
                go.GetComponent<Prefabs.TransferItemRequestPrefab>().Initialize(item, transferRequestPopUp);
            }
        }
    }

    public void InitializeMembers(DataContainers.Order iOrder)
    {
        for (int i = 0; i < membersScrollContent.childCount; i++)
        {
            Destroy(membersScrollContent.GetChild(i).gameObject);
        }
        foreach (var item in iOrder.activeUsers)
        {
            Debug.Log("member id: " + item.Value.user.id);
            GameObject go = Instantiate(memberItemPrefab, membersScrollContent);
            go.GetComponent<Prefabs.OrderUser>().Initialize(item.Value, joinRequestPopUp);
        }
    }



    public override void Reinitialize()
    {
        throw new System.NotImplementedException();
    }
}
