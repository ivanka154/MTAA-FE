using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserController : MonoBehaviour
{
    private static UserController _instance;

    public DataContainers.User user;

    public static UserController Instance
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
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            logIn("test1@email.com", "Alder2020");
        }

    }
    public bool logIn(string email, string password)
    {
        Debug.Log("email: " + email + "\tpassword: " + password);
        StartCoroutine(RestController.Instance.LogIn(email, password));
        return true;
    }
}
