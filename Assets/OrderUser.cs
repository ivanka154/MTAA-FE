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

        public void Initialize(DataContainers.OrderUser User)
        {
            user = User;
            UpdateTexts();
            if (user.status.Equals("requested"))
            {
                statusButton.buttonEvent.RemoveAllListeners();
                statusButton.buttonEvent.AddListener(() =>
                {
                    UIViewManager.Instance.OpenPanel("JoinRequestView");
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

