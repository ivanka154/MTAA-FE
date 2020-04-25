using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoginViewController : UIView
{
    [SerializeField]
    private Button loginButton;
    [SerializeField]
    private TMP_InputField email;
    [SerializeField]
    private TMP_InputField password;

    public override void Initialize()
    {
        loginButton.onClick.RemoveAllListeners();
        loginButton.onClick.AddListener(() => {
            UserController.Instance.logIn(email.text, password.text);
          //  UIViewManager.Instance.OpenPanel("TableView");
        });
    }

    public override void Reinitialize()
    {
    }
}
