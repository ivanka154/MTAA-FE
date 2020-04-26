using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CreateOrderView : UIView
{
    [SerializeField]
    private TMP_InputField restaurantId;
    [SerializeField]
    private TMP_InputField tableId;
    [SerializeField]
    private Button createNewOrder;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            StartCoroutine(RestController.Instance.CreateNewOrder(restaurantId.text, tableId.text, UserController.Instance.user.id));
        }

    }

    public override void Initialize()
    {
        createNewOrder.onClick.RemoveAllListeners();
        createNewOrder.onClick.AddListener(() => {
            StartCoroutine(RestController.Instance.CreateNewOrder(restaurantId.text, tableId.text, UserController.Instance.user.id));
            StartCoroutine(RestController.Instance.GetMenu(restaurantId.text));
        });
    }

    public override void Reinitialize()
    {
        throw new System.NotImplementedException();
    }
}
