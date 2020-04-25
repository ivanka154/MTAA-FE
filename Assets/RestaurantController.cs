using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class RestaurantController : MonoBehaviour
{

    private static RestaurantController _instance;

    public DataContainers.Menu restaurantMenu;


    public static RestaurantController Instance
    {
        get { return _instance; }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            StartCoroutine(RestController.Instance.GetMenu());
        }

    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

    }



}
