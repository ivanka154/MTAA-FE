using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Prefabs
{
    public class MenuItemPrefab : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI name;
        [SerializeField]
        private TextMeshProUGUI price;
        [SerializeField]
        private TextMeshProUGUI amountText;
        [SerializeField]
        private Button plus;
        [SerializeField]
        private Button minus;

        private DataContainers.MenuItem item;
        [SerializeField]
        private int amount;
        [SerializeField]
        private Button openFoodInfo;

        public void Initialize(DataContainers.MenuItem iItem, Michsky.UI.ModernUIPack.ModalWindowManager mwm)
        {
            item = iItem;
            name.text = item.name;
            price.text = item.price.ToString() + "€";
            amountText.text = "0";
            amount = 0;
            plus.onClick.AddListener(() =>
            {
                AddAmount();
            });
            minus.onClick.AddListener(() =>
            {
                RemoveAmmount();
            });
            openFoodInfo.onClick.AddListener(() =>
            {
                mwm.OpenWindow();
            });
        }

        private void AddAmount()
        {
            RestaurantController.Instance.addItemToOrder(item);

            amount = amount +1;
            amountText.text = amount.ToString();
        }

        private void RemoveAmmount()
        {

            if (amount == 0)
            {
                return;
            }
            RestaurantController.Instance.removeItemFromOrder(item);
            amount--;
            amountText.text = amount.ToString();
        }
    }
}

