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
    private Button signuUpButton;
    [SerializeField]
    private Button forgetPasswordButton;
    [SerializeField]
    private TMP_InputField email;
    [SerializeField]
    private TMP_InputField password;

    public override void Initialize()
    {
        loginButton.onClick.RemoveAllListeners();
        loginButton.onClick.AddListener(() => {
       //     UIViewManager.Instance.OpenPanel("TableView");
            UserController.Instance.logIn(email.text, password.text);
       //     UIViewManager.Instance.OpenPanel("TableView");
        });
        signuUpButton.onClick.RemoveAllListeners();
        signuUpButton.onClick.AddListener(() => {
            UIViewManager.Instance.OpenPanel("RegisterView");
        });
        forgetPasswordButton.onClick.RemoveAllListeners();
        forgetPasswordButton.onClick.AddListener(() => {
            UIViewManager.Instance.ErrorNotification("Button not yet impelented");
        });
    }

    public override void Reinitialize()
    {
    }
}
