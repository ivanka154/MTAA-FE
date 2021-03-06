﻿using System.Collections;
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
    private List<UIView> popUpViews;
    [SerializeField]
    private Michsky.UI.ModernUIPack.WindowManager windowManager;
    [SerializeField]
    private Michsky.UI.ModernUIPack.NotificationManager succesNotification;
    [SerializeField]
    private Michsky.UI.ModernUIPack.NotificationManager errorNotification;

    [SerializeField]
    private UnityEngine.UI.Button refresh;

    [SerializeField]
    public TMPro.TextMeshProUGUI Username;

    public string testPanelName;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            OpenPanel(testPanelName);
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

    private void Start()
    {
        OpenPanel(uiViews[0].name);
        refresh.onClick.RemoveAllListeners();
        refresh.onClick.AddListener(() => {
            OpenPanel("GalleryView");
           // StartCoroutine(RestController.Instance.GetOrder(UserController.Instance.order.restaurant, UserController.Instance.order.table, UserController.Instance.order.id));
        });
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

    public void OpenPopUpPanel(string panelName)
    {
        foreach (var item in popUpViews)
        {
            if (item.name.Equals(panelName))
            {
                item.Initialize();
                break;
            }
        }
    }

    public void InitializePanel(string panelName)
    {
        foreach (var item in uiViews)
        {
            if (item.name.Equals(panelName))
            {
                item.Initialize();
            }
        }
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
