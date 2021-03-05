using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Main : PanelBase
{

    public float fMax_MP = 100;
    public float fCur_MP { get; private set; } = 0f;

    private Image _pImage_Slider_Front = null;
    
    protected override void OnAwake()
    {
        base.OnAwake();

        if (null == _pImage_Slider_Front)
        {
            var arrImages = GetComponentsInChildren<Image>();

            for (int i = 0; i < arrImages.Length; ++i)
            {
                if ("Image_Slider_Front" == arrImages[i].name)
                {
                    _pImage_Slider_Front = arrImages[i];

                    break;
                }
            }
        }

        EnemyManager.instance.OnReturn_Enemy.Subscribe += OnRetrun_Enemy_Func;
    }

    private void OnDestroy()
    {
        EnemyManager.instance.OnReturn_Enemy.Subscribe -= OnRetrun_Enemy_Func;
    }

    public override void DoShow()
    {
        base.DoShow();

        DoInit();
    }

    public void DoInit()
    {
        fCur_MP = 1;
        _pImage_Slider_Front.fillAmount = fCur_MP / fMax_MP;
    }

    /// <summary>
    /// 적을 처치했을 때 구독하고 있는 OnReturn_Enemy 로부터 정보를 받는다.
    /// </summary>
    private void OnRetrun_Enemy_Func(EnemyManager.ReturnEnemyMessage pMessage)
    {
        if (!pMessage.bIsAlive)
        {
            fCur_MP += pMessage.pEnemyBase.pEnemyData.fGetMP;

            _pImage_Slider_Front.fillAmount = fCur_MP / fMax_MP;
        }
    
    }
}
