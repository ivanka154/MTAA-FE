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
        private TextMeshProUGUI members;

        private DataContainers.OrderItem item;

        public void Initialize(DataContainers.OrderItem iItem)
        {
            item = iItem;
            UpdateTexts();
        }

        private void UpdateTexts()
        {
            itemName.text = item.name;
            amount.text = item.amount.ToString() + "x";
            price.text = item.price.ToString() + " €";
        }
    }
}

