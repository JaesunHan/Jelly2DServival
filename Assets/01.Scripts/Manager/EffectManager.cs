using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoSingleton<EffectManager>
{
    public struct ShowEffectMessag
    {
        public EEffectName eEffectName;
        public Vector2 vecShowPos;

        public ShowEffectMessag(EEffectName effectName, Vector2 vecShowPos)
        {
            this.eEffectName = effectName;
            this.vecShowPos = vecShowPos;
        }
    }


    public Observer_Pattern<ShowEffectMessag> OnShowEffect { get; private set; } = Observer_Pattern<ShowEffectMessag>.instance;

    [GetComponentInChildren]
    private EffectBase _pOriginal_EffectBase = null;

    private List<EffectBase> _list_Effects = new List<EffectBase>();
    private Pooling_Component<EffectBase> _pPool_Effect = Pooling_Component<EffectBase>.instance;


    protected override void OnAwake()
    {
        base.OnAwake();

        
    }

    private void OnDestroy()
    {
        OnShowEffect.DoRemove_All_Observer();
    }

    public void DoInit()
    {
        //var pEffectData = EEffectName.Default_Damage.GetEffectData();
        //_pOriginal_EffectBase = pEffectData.pFilePrefab.GetComponent<EffectBase>();
        _pOriginal_EffectBase.DoAwake();
        if (null != _pOriginal_EffectBase)
            _pPool_Effect.DoInit_Pool(_pOriginal_EffectBase);

        for (int i = 0; i < _list_Effects.Count; ++i)
        {
            _list_Effects[i].SetActive(false);
            _pPool_Effect.DoPush(_list_Effects[i]);
        }

        OnShowEffect.Subscribe += OnShowEffect_Func;
    }

    /// <summary>
    /// 이펙트 보여주는 함수
    /// </summary>
    /// <param name="pMessage"></param>
    private void OnShowEffect_Func(ShowEffectMessag pMessage)
    {
        EffectData pEffectData = pMessage.eEffectName.GetEffectData();

        if (null != pEffectData)
        {
            var pEffectPref = _pPool_Effect.DoPop();
            pEffectPref.DoAwake();
            pEffectPref.transform.SetParent(transform);
            pEffectPref.transform.position = pMessage.vecShowPos;

            pEffectPref.DoInit(pEffectData);


            pEffectPref.SetActive(true);
        }

    }
}
