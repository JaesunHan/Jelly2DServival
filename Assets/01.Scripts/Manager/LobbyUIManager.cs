using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyUIManager : MonoSingleton<LobbyUIManager>
{
    public enum EPanel
    {
        Canvas_Lobby_Main, 
    }

    [GetComponentInChildren]
    Dictionary<EPanel, PanelBase> _mapPanel = new Dictionary<EPanel, PanelBase>();

    protected override IEnumerator OnEnableCoroutine()
    {
        yield return base.OnEnableCoroutine();

        if (null == DataManager.instance)
        {
            DebugLogManager.Log("데이터 매니저의 인스턴스가 널입니다. 따라서 새 오브젝트를 만들고 데이터매니저 컴포넌트를 추가합니다.");
        }

        while (!DataManager.bIsLoaded_AllResource)
        {
            yield return null;
        }

        Init();
    }

    public void DoShowPanel(EPanel eShowPanelType)
    {
        _mapPanel[eShowPanelType].DoShow();
    }


    private void Init()
    {
        for (int i = 0; i < _mapPanel.Count; ++i)
        {
            _mapPanel[(EPanel)i].DoHide();
        }

        _mapPanel[EPanel.Canvas_Lobby_Main].DoShow();
    }

}
