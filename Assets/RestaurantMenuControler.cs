using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class RestaurantMenuControler : MonoBehaviour
{
    [SerializeField]
    private GameObject menuItemPrefab;
    [SerializeField]
    private Transform menuScrollContent;

    private void Start()
    {
       RestController.OnMenuLoaded += InitializeMenuItems; 
    }

    public void InitializeMenuItems(DataContainers.Menu iMenu)
    {
        foreach (var food in iMenu.foods)
        {
            GameObject go = Instantiate(menuItemPrefab, menuScrollContent);
            go.GetComponent<Prefabs.MenuItemPrefab>().Initialize(food.Value);
        }
    }
}
