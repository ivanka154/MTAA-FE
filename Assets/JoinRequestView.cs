using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class JoinRequestView : UIView
{
    [SerializeField]
    private TextMeshProUGUI userName;
    [SerializeField]
    private Button accept;
    [SerializeField]
    private Button reject;
    public override void Initialize()
    {
        userName.text = UserController.Instance.order.activeUsers[RestaurantController.Instance.joinRequest.requirer].user.name;
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
