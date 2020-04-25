using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableViewController : UIView
{
    [SerializeField]
    private GameObject orderItemPrefab;
    [SerializeField]
    private Transform orderScrollContent;

    public override void Initialize()
    {
        //throw new System.NotImplementedException();
    }

    public override void Reinitialize()
    {
        throw new System.NotImplementedException();
    }
}
