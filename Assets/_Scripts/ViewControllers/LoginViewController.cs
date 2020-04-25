using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginViewController : UIView
{
    [SerializeField]
    private Button loginButton;
    public override void Initialize()
    {
        loginButton.onClick.RemoveAllListeners();
        loginButton.onClick.AddListener(() => {
            UIViewManager.Instance.OpenPanel("TableView");
        });
    }

    public override void Reinitialize()
    {
    }
}
