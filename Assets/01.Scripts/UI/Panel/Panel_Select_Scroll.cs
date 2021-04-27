using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Select_Scroll : PanelBase, IHas_UIButton<Panel_Select_Scroll.EButton>
{
    const int const_iSlot_Count = 4;
    public enum EButton
    {
        Button_Confirm,
    }

    [GetComponentInChildren]
    private List<Widget_Scroll> _list_Widget = new List<Widget_Scroll>();

    [GetComponentInChildren]
    private Dictionary<EButton, Button> _mapButton = new Dictionary<EButton, Button>();

    private int _iSelect_Skill;
    private ESkill _eSelect_Skill;


    protected override void OnAwake()
    {
        base.OnAwake();
    }

    protected override IEnumerator OnEnableCoroutine()
    {
        while (!DataManager.bIsLoaded_AllResource)
        {
            //DebugLogManager.Log("IngameUIManager 의 인스턴스가 아직 없다");
            yield return null;
        }

        Text pText_Confirm = _mapButton[EButton.Button_Confirm].GetComponentInChildren<Text>();
        pText_Confirm.text = DataManager.GetLocalText(ELanguageKey.Confirm);

        IngameUIManager.instance.OnSelect_Scroll.Subscribe += OnSelect_Scroll;
    }

    

    public override void DoShow()
    {
        base.DoShow();

        Init();

        Time.timeScale = 0;
    }

    public override void DoHide()
    {
        base.DoHide();

        Time.timeScale = 1;
    }

    private void Init()
    {
        List<ESkill> listSkillData = DataManager.DoGet_Select_Skill_List(const_iSlot_Count);

        for (int i = 0; i < listSkillData.Count; ++i)
        {
            _list_Widget[i].DoInit(listSkillData[i].GetSkillData());
        }

        _iSelect_Skill = -1;
        //_pOriginal_Widget_Scroll.DoInit(ESkill.Skill_Summon_Fairy.GetSkillData());
        //_eSelect_Skill = ESkill.Skill_Summon_Fairy;
    }

    public void IHas_UIButton_OnClickButton(UIButtonMessage<EButton> sButtonMsg)
    {
        switch (sButtonMsg.eButtonName)
        {
            case EButton.Button_Confirm:
                if (-1 == _iSelect_Skill)
                {
                    break;
                }

                if (_eSelect_Skill != ESkill.Skill_Summon_Fairy)
                {
                    //Show System Message
                    SystemMsgUIManager.instance.OnOccur_System_Message.DoNotify(new SystemMsgUIManager.SystemMessage(ELanguageKey.System_Prepare));
                    break;
                }
                

                PlayerManager_HJS.instance.OnApply_Scroll.DoNotify(new PlayerManager_HJS.ApplyScrollMessage(_eSelect_Skill));

                this.DoHide();
                break;
        }
    }


    private void OnSelect_Scroll(IngameUIManager.SelectScrollMessage pMessage)
    {
        _iSelect_Skill = (int)pMessage.eSkill;
        _eSelect_Skill = pMessage.eSkill;
    }
}
