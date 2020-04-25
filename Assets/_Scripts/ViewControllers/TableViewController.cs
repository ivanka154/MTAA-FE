using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableViewController : UIView
{
    [SerializeField]
    private GameObject orderItemPrefab;
    [SerializeField]
    private Transform allItemsScrollContent;
    [SerializeField]
    private Transform myItemsScrollContent;
    [SerializeField]
    private Transform membersScrollContent;

    private void Start()
    {
        RestController.OnOrderLoaded()
    }

    public override void Initialize()
    {
        //throw new System.NotImplementedException();
    }

    public void InitializeMenuItems(DataContainers.Order iOrder, DataContainers.Menu iMenu)
    {
        var allItems = iOrder.GetAllItems(iMenu);
        foreach (var food in )
        {
            GameObject go = Instantiate(orderItemPrefab, allItemsScrollContent);
            go.GetComponent<Prefabs.OrderItem>().Initialize(food.Value);
        }
    }

    public override void Reinitialize()
    {
        throw new System.NotImplementedException();
    }
}
