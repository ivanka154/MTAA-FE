using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TransferItemViewController : UIView
{
    [SerializeField]
    private GameObject setAmountToMemberPrefab;
    [SerializeField]
    private Transform membersScrollContent;
    [SerializeField]
    private TextMeshProUGUI itemName;
    [SerializeField]
    private TextMeshProUGUI itemAmount;
    [SerializeField]
    private Button transferButton;
    [SerializeField]
    private Button backButton;
    private void Start()
    {
        RestaurantController.OnAmountUploaded += UpdateAmount;
        //  RestController.OnMenuLoaded += InitializeMenuItems; 
    }

    public override void Initialize()
    {
        backButton.onClick.RemoveAllListeners();
        backButton.onClick.AddListener(() => {
            UIViewManager.Instance.OpenPanel("TableView");
        });
        transferButton.onClick.RemoveAllListeners();
        transferButton.onClick.AddListener(() => {
            RestaurantController.Instance.TransferItems();
            UIViewManager.Instance.OpenPanel("TableView");
        });
        itemName.text = RestaurantController.Instance.TransferItem.name;
        itemAmount.text = RestaurantController.Instance.TransferItem.amount.ToString();
        for (int i = 0; i < membersScrollContent.childCount; i++)
        {
            Destroy(membersScrollContent.GetChild(i).gameObject);
        }
        foreach (var item in UserController.Instance.order.activeUsers.Values)
        {
            if (!item.status.Equals("active") || item.user.id.Equals(UserController.Instance.user.id))
            {
                continue;
            }
            GameObject go = Instantiate(setAmountToMemberPrefab, membersScrollContent);
            go.GetComponent<Prefabs.SetAmountToMemberPrefab>().Initialize(item.user);
        }
        RestaurantController.Instance.Initialize();

    }

    public void UpdateAmount(int iAmount) 
    {
        itemAmount.text = iAmount.ToString();
    }

    public override void Reinitialize()
    {
        throw new System.NotImplementedException();
    }
}
