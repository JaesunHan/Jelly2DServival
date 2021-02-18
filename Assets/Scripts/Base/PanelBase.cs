using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelBase : MonoBehaviour
{

    public bool bIsShow { get; private set; } = false;
    private void Awake()
    {
        OnAwake();
    }

    protected virtual void OnAwake()
    {
        ButtonHelper.DoInit_HasUIElement(this);
    }

    //public virtual void DoInit()
    //{
        
    //}

    public virtual void DoShow()
    {
        bIsShow = true;
        gameObject.SetActive(true);
    }

    public virtual void DoHide()
    {
        bIsShow = false;
        gameObject.SetActive(false);
    }

}
