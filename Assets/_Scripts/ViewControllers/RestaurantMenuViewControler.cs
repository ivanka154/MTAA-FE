using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class RestaurantMenuViewControler : UIView
{
    [SerializeField]
    private GameObject menuItemPrefab;
    [SerializeField]
    private Transform menuScrollContent;
    [SerializeField]
    private Button orderButton;
    [SerializeField]
    private Button backButton;
    [SerializeField]
    private TextMeshProUGUI sum;
    [SerializeField]
    private Michsky.UI.ModernUIPack.ModalWindowManager mwm;
    private void Start()
    {
        RestaurantController.OnSumUploaded += UpdateSum;
     //  RestController.OnMenuLoaded += InitializeMenuItems; 
    }

    public override void Initialize()
    {
        UpdateSum(0.00f);
        InitializeMenuItems(UserController.Instance.menu);
        backButton.onClick.RemoveAllListeners();
        backButton.onClick.AddListener(() => {
            UIViewManager.Instance.OpenPanel("TableView");
        });
        orderButton.onClick.RemoveAllListeners();
        orderButton.onClick.AddListener(() => {
            RestaurantController.Instance.OrderSelectedItems();
        });
        RestaurantController.Instance.Initialize();
        //throw new System.NotImplementedException();
    }

    public void UpdateSum(float iSum)
    {
        sum.text = iSum.ToString() + "€";
    }

    public override void Reinitialize()
    {
        throw new System.NotImplementedException();
    }

    public void InitializeMenuItems(DataContainers.Menu iMenu)
    {
        for (int i = 0; i < menuScrollContent.childCount; i++)
        {
            Debug.Log(menuScrollContent.GetChild(i).name);
            Destroy(menuScrollContent.GetChild(i).gameObject);
        }
        foreach (var food in iMenu.foods)
        {
            GameObject go = Instantiate(menuItemPrefab, menuScrollContent);
            go.GetComponent<Prefabs.MenuItemPrefab>().Initialize(food.Value, mwm);
        }
    }



}
