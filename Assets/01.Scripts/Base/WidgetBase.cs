using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WidgetBase : MonoBehaviour
{
    private void Awake()
    {
        OnAwake();
    }

    protected virtual void OnAwake()
    {
        ButtonHelper.DoInit_HasUIElement(this);
    }
}
