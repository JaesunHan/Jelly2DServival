using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBase : MonoBehaviour
{
    private void Awake()
    {
        OnAwake();
    }

    protected virtual void OnAwake()
    {
        GetComponentAttributeSetter.DoUpdate_GetComponentAttribute(this);
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

    protected virtual void OnDisableObject(bool bIsQuit_Application) { }

    protected virtual IEnumerator OnEnableCoroutine() { yield break; }

    public virtual void SetActive(bool bIsActive)
    {
        gameObject.SetActive(bIsActive);
    }

    public void DoAwake()
    {
        Awake();
    }
}
