using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Globalization;

public class RestaurantController : MonoBehaviour
{

    private static RestaurantController _instance;

    public DataContainers.Menu restaurantMenu;

    private void Start()
    {
        CultureInfo.CurrentCulture = new CultureInfo("en-US", false);
    }

    public static RestaurantController Instance
    {
        get { return _instance; }
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
