using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyUIManager : MonoSingleton<LobbyUIManager>
{
    public enum EPanel
    {
        Canvas_Lobby_Top, 
    }

    public struct SelectWidgetMoneyBtnMessage
    {
        public EMoneyType eMoneyType;

        public SelectWidgetMoneyBtnMessage(EMoneyType eMoneyType)
        {
            this.eMoneyType = eMoneyType;
        }
    }


    [GetComponentInChildren]
    Dictionary<EPanel, PanelBase> _mapPanel = new Dictionary<EPanel, PanelBase>();

    Observer_Pattern<SelectWidgetMoneyBtnMessage> OnSelect_Widget_MoneyBtn = Observer_Pattern<SelectWidgetMoneyBtnMessage>.instance;

    protected override void OnAwake()
    {
        base.OnAwake();

        OnSelect_Widget_MoneyBtn.Subscribe += OnSelect_Widget_MoneyBtn_Func;
    }

    private void OnDestroy()
    {
        OnSelect_Widget_MoneyBtn.DoRemove_All_Observer();
    }

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

        _mapPanel[EPanel.Canvas_Lobby_Top].DoShow();
    }

    private void OnSelect_Widget_MoneyBtn_Func(SelectWidgetMoneyBtnMessage pMessage)
    {
        switch (pMessage.eMoneyType)
        {
            case EMoneyType.Money_Gold: // 골드 버튼을 누르면, 상점을 띄울 때 골드 구매 탭을 띄우고
                break;
            case EMoneyType.Money_Dia:  // 다이아 버튼을 누르면, 상점을 띄울 때 다이아 구매 탭을 띄우기
                break;

            default:
                break;
        }
    
    }

}
