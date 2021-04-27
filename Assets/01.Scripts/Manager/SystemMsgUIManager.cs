using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemMsgUIManager : MonoSingleton<SystemMsgUIManager>
{
    public enum EPanel
    {
        Canvas_System_Message, 
    }

    public struct SystemMessage
    {
        public ELanguageKey eLangKey;

        public SystemMessage(ELanguageKey eLangKey)
        {
            this.eLangKey = eLangKey;
        }
    }

    public Observer_Pattern<SystemMessage> OnOccur_System_Message = Observer_Pattern<SystemMessage>.instance;

    [GetComponentInChildren]
    private Dictionary<EPanel, PanelBase> _mapPanel = new Dictionary<EPanel, PanelBase>();

    protected override void OnAwake()
    {
        base.OnAwake();

        OnOccur_System_Message.Subscribe += OnOccur_System_Message_Func;
    }

    private void OnDestroy()
    {
        OnOccur_System_Message.DoRemove_All_Observer();
    }

    protected override IEnumerator OnEnableCoroutine()
    {
        yield return base.OnEnableCoroutine();

        while (!DataManager.bIsLoaded_AllResource)
        {
            yield return null;
        }

        Init();
    }

    private void Init()
    {
        foreach (var pPanel in _mapPanel)
        {
            pPanel.Value.DoHide();
        }

        _mapPanel[EPanel.Canvas_System_Message].DoShow();
    }

    private void OnOccur_System_Message_Func(SystemMessage pMessage)
    {
        var pCanvas = (Canvas_System_Message)_mapPanel[EPanel.Canvas_System_Message];
        if (null != pCanvas)
        {
            pCanvas.DoShow_Message_Anim(pMessage.eLangKey);
        }
    
    }
}
