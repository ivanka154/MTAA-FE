using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Prefabs
{
    public class SetAmountToMemberPrefab : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI memberName;
        [SerializeField]
        private TextMeshProUGUI amountText;
        [SerializeField]
        private Button plus;
        [SerializeField]
        private Button minus;

        private int amount;
        private DataContainers.User user;

        public void Initialize(DataContainers.User iUser)
        {
            user = iUser;
            plus.onClick.AddListener(() =>
            {
                AddAmount();
            });
            minus.onClick.AddListener(() =>
            {
                RemoveAmmount();
            });
            memberName.text = user.name; 
            amount = 0;
            amountText.text = amount.ToString() + "x";
        }
        private void AddAmount()
        {
            if (amount == RestaurantController.Instance.TransferItem.amount)
            {
                return;
            }
            RestaurantController.Instance.addItemToTransfer(user.id);
            amount++;
            amountText.text = amount.ToString();

        }
        private void RemoveAmmount()
        {

            if (amount == 0)
            {
                return;
            }
            RestaurantController.Instance.removeItemToTransfer(user.id);
            amount--;
            amountText.text = amount.ToString();
        }
    }
}

