using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Prefabs
{
    public class OrderUser : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI userName;
        [SerializeField]
        private Michsky.UI.ModernUIPack.ButtonManager statusButton;

        private DataContainers.OrderUser user;

        public void Initialize(DataContainers.OrderUser User, Michsky.UI.ModernUIPack.ModalWindowManager iMwm)
        {
            user = User;
            UpdateTexts();
            if (user.status.Equals("requested"))
            {
                statusButton.buttonEvent.RemoveAllListeners();
                statusButton.buttonEvent.AddListener(() =>
                {
                    foreach (var item in UserController.Instance.order.joinRequests.Values)
                    {
                        if (item.requirer.Equals(User.user.id))
                        {
                            RestaurantController.Instance.joinRequest = item;
                            iMwm.OpenWindow();
                            UIViewManager.Instance.OpenPopUpPanel("JoinRequestPopUp");
                        }
                    }
                });
            }
        }

        private void UpdateTexts()
        {
            userName.text = user.user.name;
            statusButton.buttonText = user.status;
        }
    }
}

