using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectBase : ObjectBase
{
    private EffectData _pEffectData = null;

    protected override void OnAwake()
    {
        base.OnAwake();


    }


    public void DoInit(EffectData pEffectData)
    {
        _pEffectData = pEffectData;
    }

    public void DoPlayEffect()
    { 
    
    }
}
