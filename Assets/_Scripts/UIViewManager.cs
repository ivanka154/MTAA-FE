using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIViewManager : MonoBehaviour
{
    private static UIViewManager _instance;
    public static UIViewManager Instance
    {
        get { return _instance; }
    }

    [SerializeField]
    private List<UIView> uiViews;

    [SerializeField]
    private Michsky.UI.ModernUIPack.WindowManager windowManager;
    [SerializeField]
    private Michsky.UI.ModernUIPack.NotificationManager succesNotification;
    [SerializeField]
    private Michsky.UI.ModernUIPack.NotificationManager errorNotification;

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

    private void Start()
    {
        OpenPanel(uiViews[0].name);
    }

    public void OpenPanel(string panelName) 
    {
        foreach (var item in uiViews)
        {
            if (item.name.Equals(panelName))
            {
                item.Initialize();
                break;
            }
        }
        windowManager.OpenPanel(panelName);
    }

    public void SuccesNotification(string iNotification)
    {
        succesNotification.description = iNotification;
        succesNotification.OpenNotification();
    }

    public void ErrorNotification(string iNotification)
    {
        errorNotification.description = iNotification;
        errorNotification.OpenNotification();
    }
}
