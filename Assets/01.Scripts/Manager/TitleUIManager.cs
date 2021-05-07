using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleUIManager : MonoSingleton<TitleUIManager>
{
    public enum EPanel
    {
        Canvas_Title_Main,
        //Canvas_Loading,
    }

    [GetComponentInChildren]
    Dictionary<EPanel, PanelBase> _mapPanel = new Dictionary<EPanel, PanelBase>();

    protected override void OnAwake()
    {
        base.OnAwake();


    }

    protected override IEnumerator OnEnableCoroutine()
    {
        yield return base.OnEnableCoroutine();

        if (null == DataManager.instance)
        {
            DebugLogManager.Log("데이터 매니저의 인스턴스가 널입니다. 따라서 새 오브젝트를 만들고 데이터매니저 컴포넌트를 추가합니다.");
        }

        foreach (var pPanel in _mapPanel)
        {
            pPanel.Value.DoHide();
        }
        _mapPanel[EPanel.Canvas_Title_Main].DoShow();

        //_mapPanel[EPanel.Canvas_Loading].DoShow();
        while (!DataManager.bIsLoaded_AllResource)
        {
            yield return null;
        }

        //_mapPanel[EPanel.Canvas_Loading].DoHide();
    }


    
}
