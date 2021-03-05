using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameUIManager : MonoSingleton<IngameUIManager>
{
    public enum EPanel
    {
        Panel_Joystick,
        Panel_Main, 

        Panel_Count,
    }

    Dictionary<EPanel, PanelBase> _mapPanel = new Dictionary<EPanel, PanelBase>();

    protected override void OnAwake()
    {
        base.OnAwake();

        var arrPanel = GetComponentsInChildren<PanelBase>();

        for (EPanel ePanel = (EPanel)0; ePanel < EPanel.Panel_Count; ++ePanel)
        {
            for (int i = 0; i < arrPanel.Length; ++i)
            {
                if (ePanel.ToString() == arrPanel[i].name)
                {
                    _mapPanel.Add(ePanel, arrPanel[i]);
                    break;
                }
            }
        }
    }


    private void Start()
    {
        for (int i = 0; i < _mapPanel.Count; ++i)
        {
            _mapPanel[(EPanel)i].DoHide();
        }

        _mapPanel[EPanel.Panel_Joystick].DoShow();
        _mapPanel[EPanel.Panel_Main].DoShow();
        
    }
}
