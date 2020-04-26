using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TransferItemAcceptPopUp : UIView
{
    [SerializeField]
    private TextMeshProUGUI userName;
    [SerializeField]
    private TextMeshProUGUI itemName;
    [SerializeField]
    private TextMeshProUGUI amount;
    [SerializeField]
    private Button accept;
    [SerializeField]
    private Button reject;
    public override void Initialize()
    {
        userName.text = UserController.Instance.order.activeUsers[RestaurantController.Instance.transferRequest.requirer].user.name;
        itemName.text = UserController.Instance.menu.foods[RestaurantController.Instance.transferRequest.item.id].name;
        amount.text = RestaurantController.Instance.transferRequest.item.amount + "x";
        accept.onClick.RemoveAllListeners();
        accept.onClick.AddListener(() => {
            
        });
        reject.onClick.RemoveAllListeners();
        reject.onClick.AddListener(() => {
        });
    }

    public override void Reinitialize()
    {
        throw new System.NotImplementedException();
    }
}
