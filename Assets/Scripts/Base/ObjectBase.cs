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

    public virtual void SetActive(bool bIsActive)
    {
        gameObject.SetActive(bIsActive);
    }

    
}
