using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIView : MonoBehaviour
{
    public string panelName;
    abstract public void Initialize();
    abstract public void Reinitialize();
}
