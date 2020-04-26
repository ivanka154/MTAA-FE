using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Prefabs
{
    public class OrderItem : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI itemName;
        [SerializeField]
        private TextMeshProUGUI amount;
        [SerializeField]
        private TextMeshProUGUI price;

        [SerializeField]
        private Button transferItemButton;

        private DataContainers.OrderItem item;

        public void Initialize(DataContainers.OrderItem iItem, bool iActivateButton = false)
        {
            item = iItem;
            UpdateTexts();
            transferItemButton.onClick.RemoveAllListeners();
            if (iActivateButton)
            {
                transferItemButton.onClick.AddListener(() => {
                    RestaurantController.Instance.TransferItem = item;
                    UIViewManager.Instance.OpenPanel("TransferItemView");
                    RestaurantController.Instance.TransferItem = item;

                });
            }
        }

        private void UpdateTexts()
        {
            itemName.text = item.name;
            amount.text = item.amount.ToString() + "x";
            price.text = item.price.ToString() + " €";
        }
    }
}

