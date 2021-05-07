using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Main : PanelBase
{
    /// <summary>
    /// 백분율로 표시한다.
    /// </summary>
    public float fMax_Value = 100;
    public float fCur_MP { get; private set; } = 0f;

    //private Image _pImage_Slider_Front = null;
    private Slider _pSlider_MP = null;

    [GetComponentInChildren("Text_Mana")]
    private Text _pText_Mana = null;
    
    protected override void OnAwake()
    {
        base.OnAwake();

        if (null == _pSlider_MP)
        {
            var arrImages = GetComponentsInChildren<Slider>();

            for (int i = 0; i < arrImages.Length; ++i)
            {
                if ("Slider_MP" == arrImages[i].name)
                {
                    _pSlider_MP = arrImages[i];

                    break;
                }
            }
        }

        //EnemyManager.instance.OnReturn_Enemy.Subscribe += OnRetrun_Enemy_Func;
        ManaManager.instance.OnChange_MP.Subscribe += OnChange_MP_Func;
    }

    private void OnDestroy()
    {
        //EnemyManager.instance.OnReturn_Enemy.Subscribe -= OnRetrun_Enemy_Func;
    }

    public override void DoShow()
    {
        base.DoShow();

        DoInit();
    }

    public void DoInit()
    {
        fCur_MP = 1;
        //_pImage_Slider_Front.fillAmount = fCur_MP / fMax_MP;
        _pSlider_MP.interactable = false;
        _pSlider_MP.maxValue = fMax_Value;
        _pSlider_MP.minValue = 1;
        _pSlider_MP.value = ((float)fCur_MP / (float)fMax_Value ) * (float)100;

        _pText_Mana.text = $"{fCur_MP} / {fMax_Value}";
    }

    ///// <summary>
    ///// 적을 처치했을 때 구독하고 있는 OnReturn_Enemy 로부터 정보를 받는다.
    ///// </summary>
    //private void OnRetrun_Enemy_Func(EnemyManager.ReturnEnemyMessage pMessage)
    //{
    //    if (!pMessage.bIsAlive)
    //    {
    //        fCur_MP += pMessage.pEnemyBase.pEnemyData.fGetMP;

    //        _pSlider_MP.value = fCur_MP / fMax_MP;
    //    }
    //}

    private void OnChange_MP_Func(ManaManager.ChangeMPMessage pMessage)
    {
        fCur_MP = pMessage.fCurMP;
        fMax_Value = pMessage.fMaxMP;

        _pSlider_MP.value = (fCur_MP / fMax_Value) * (float)100;
        _pText_Mana.text = $"{fCur_MP} / {fMax_Value}";
    }
}
