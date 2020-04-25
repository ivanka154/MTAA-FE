using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserController : MonoBehaviour
{
    private static UserController _instance;

    public DataContainers.User user;

    public string table;
    public string Restaurant;
    public string order;

    public static UserController Instance
    {
        get { return _instance; }
    }

    private void Start()
    {
        RestController.OnUserLogedIn += setLoggedUser;
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

    public bool logIn(string email, string password)
    {
        Debug.Log("email: " + email + "\tpassword: " + password);
        Debug.Log(email.Length + "\tpassword: " + password.Length);
        if (email.Equals("test11@email.com"))
        {
            Debug.Log("jeee");
        }
        if (password.Equals("Adler2020"))
        {
            Debug.Log("jeee2");
        }

        StartCoroutine(RestController.Instance.LogIn(email, password));
        return true;
    }

    public void setLoggedUser(DataContainers.User iUser)
    {
        user = iUser;
        UIViewManager.Instance.OpenPanel("TableView");
    }
}
