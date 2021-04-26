using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameUIManager : MonoSingleton<IngameUIManager>
{
    public enum EPanel
    {
        Panel_Joystick,
        Panel_Main,
        Panel_Select_Scroll,

        Panel_Count,
    }

    public struct SelectScrollMessage
    { 
        public ESkill eSkill;

        public SelectScrollMessage(ESkill eSkill)
        {
            this.eSkill = eSkill;
        }

    }

    Dictionary<EPanel, PanelBase> _mapPanel = new Dictionary<EPanel, PanelBase>();
    public Observer_Pattern<SelectScrollMessage> OnSelect_Scroll { get; private set; } = Observer_Pattern<SelectScrollMessage>.instance;

    protected override void OnAwake()
    {
        base.OnAwake();

        var arrPanel = GetComponentsInChildren<PanelBase>(true);

        for (EPanel ePanel = (EPanel)0; ePanel < EPanel.Panel_Count; ++ePanel)
        {
            for (int i = 0; i < arrPanel.Length; ++i)
            {
                if (ePanel.ToString() == arrPanel[i].name)
                {
                    _mapPanel.Add(ePanel, arrPanel[i]);
                    arrPanel[i].DoAwake();
                    break;
                }
            }
        }
    }

    protected override IEnumerator OnEnableCoroutine()
    {
        yield return base.OnEnableCoroutine();
        

        while (!DataManager.bIsLoaded_AllResource)
        {
            yield return null;  
        }
        //LanguageManager.instance.OnSetLanguage.DoNotify(SystemLanguage.Korean);
        Init();
    }

    public void DoShowPanel(EPanel eShowPanelType)
    {
        _mapPanel[eShowPanelType].DoShow();
        LanguageManager.instance.OnSetLanguage.DoNotify(SystemLanguage.Korean);
    }


    private void Init()
    {
        for (int i = 0; i < _mapPanel.Count; ++i)
        {
            _mapPanel[(EPanel)i].DoHide();
        }

        _mapPanel[EPanel.Panel_Joystick].DoShow();
        _mapPanel[EPanel.Panel_Main].DoShow();
    }
}
