using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RegisterViewController : UIView
{

    [SerializeField]
    private Button signuUpButton;
    [SerializeField]
    private Button backToLoginButton;
    [SerializeField]
    private TMP_InputField name;
    [SerializeField]
    private TMP_InputField email;
    [SerializeField]
    private TMP_InputField password;

    public override void Initialize()
    {
        signuUpButton.onClick.RemoveAllListeners();
        signuUpButton.onClick.AddListener(() => {
            if (name.text.Length <= 1)
            {
                UIViewManager.Instance.ErrorNotification("Enter valid name");
            }
            else
            {
                UserController.Instance.register(name.text, email.text, password.text);
            }
        });
        backToLoginButton.onClick.RemoveAllListeners();
        backToLoginButton.onClick.AddListener(() => {
            UIViewManager.Instance.OpenPanel("LoginView");
        });
    }

    public override void Reinitialize()
    {
    }
}
