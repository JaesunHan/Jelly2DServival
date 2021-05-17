using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectBase : ObjectBase
{
    private EffectData _pEffectData = null;

    [GetComponentInChildren]
    private SpriteRenderer _pSprite = null;

    [GetComponentInChildren]
    private Animator _pAnim = null;

    protected override void OnAwake()
    {
        base.OnAwake();

    }


    public void DoInit(EffectData pEffectData)
    {
        _pEffectData = pEffectData;

        //_pSprite = DataManager.GetSprite_InAtlas("EffectImage", "Fire8_0");
        

        _pAnim.runtimeAnimatorController = _pEffectData.pAnim;
        DebugLogManager.Log($"Effect Sprite : {_pSprite.sprite}");
    }

    public void DoPlayEffect()
    { 
    
    }

    /// <summary>
    /// 애니메이션이 끝까지 재생되고 나면 오브젝트를 비활성화 한다.
    /// </summary>
    public void DoDisable_Object()
    {
        SetActive(false);
    }
}
