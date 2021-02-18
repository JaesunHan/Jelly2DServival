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
    
    }
}
