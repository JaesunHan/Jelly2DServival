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
        GetComponentAttributeSetter.DoUpdate_GetComponentAttribute(this);
    }

    private void OnEnable()
    {
        OnEnableObject();
    }

    protected virtual void OnEnableObject()
    {
#if UNITY_EDITOR
        if (Application.isPlaying == false)
            return;
#endif

        if (gameObject.activeInHierarchy)
        {
            StopCoroutine(nameof(OnEnableCoroutine));
            StartCoroutine(nameof(OnEnableCoroutine));
        }

    }

    protected virtual IEnumerator OnEnableCoroutine() { yield break; }

    //public virtual void DoInit()
    //{

    //}

    public void DoAwake()
    {
        Awake();
    }

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
