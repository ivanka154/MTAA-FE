using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CreateOrderView : UIView
{
    [SerializeField]
    private TMP_InputField restaurantId;
    [SerializeField]
    private TMP_InputField tableId;
    [SerializeField]
    private Button createNewOrder;

    public override void Initialize()
    {
        createNewOrder.onClick.RemoveAllListeners();
        createNewOrder.onClick.AddListener(async () => {
            //StartCoroutine(RestController.Instance.CreateNewOrder(restaurantId.text, tableId.text, UserController.Instance.user.id));
            //StartCoroutine(RestController.Instance.GetMenu(restaurantId.text));
            string orderId = await DB.Order.CheackIfTableIsEmpty(restaurantId.text, tableId.text);
            if (orderId == null)
            {
                Debug.Log("No active order, create new");
                string newOrderId = await DB.Order.OrderPushKey(restaurantId.text, tableId.text);
                DataContainers.Order newOrder = new DataContainers.Order(newOrderId, tableId.text, restaurantId.text);
                var v = await DB.Order.UpdateOrder(restaurantId.text, tableId.text, newOrderId, newOrder);
                Debug.Log(v);
            }
            else
            {
                Debug.Log(orderId);
                var o = await DB.Order.ReadOrder(restaurantId.text, tableId.text, orderId);
                if (o.activeUsers.ContainsKey(UserController.Instance.user.id))
                {
                    //user is in order
                    if (o.activeUsers[UserController.Instance.user.id].status.Equals("active"))
                    {
                        //user is in order as active
                        UserController.Instance.menu = await DB.Order.ReadMenu(restaurantId.text);
                        UserController.Instance.order = o;
                        await DB.Order.ListenOnOrder(restaurantId.text, tableId.text, orderId);
                        UIViewManager.Instance.OpenPanel("TableView");
                      
                    }
                    else
                    {
                        //user has had request resolved
                        UIViewManager.Instance.ErrorNotification("Request allready send");
                    }
                }
                else
                {
                    var v = await DB.Order.AddJoinRequest(restaurantId.text, tableId.text, orderId,
                        UserController.Instance.user.id, o.owner);
                    Debug.Log(v);

                    //user is not in order send join request
                }
                UserController.Instance.menu = await DB.Order.ReadMenu(restaurantId.text);
                UserController.Instance.order = await DB.Order.ReadOrder(restaurantId.text, tableId.text, orderId);
            }
        });
    }

    public override void Reinitialize()
    {
        throw new System.NotImplementedException();
    }
}
