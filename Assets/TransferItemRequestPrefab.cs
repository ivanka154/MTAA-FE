using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Prefabs
{
    public class TransferItemRequestPrefab : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI itemName;
        [SerializeField]
        private TextMeshProUGUI amount;
        [SerializeField]
        private TextMeshProUGUI userName;
        [SerializeField]
        private Button requestButton;

        private DataContainers.TransferRequest request;

        public void Initialize(DataContainers.TransferRequest iRequest, Michsky.UI.ModernUIPack.ModalWindowManager iMwm)
        {
            request = iRequest;
            itemName.text = UserController.Instance.menu.foods[request.item.id].name;
            amount.text = request.item.amount + "x";
            userName.text = UserController.Instance.order.activeUsers[iRequest.requirer].user.name; 
            requestButton.onClick.RemoveAllListeners();

            requestButton.onClick.AddListener(() => {
                RestaurantController.Instance.transferRequest = request;
                UIViewManager.Instance.OpenPopUpPanel("TransferRequestPopUp");
                iMwm.OpenWindow();
            });
        }
        

    }
}

