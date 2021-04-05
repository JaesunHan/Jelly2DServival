using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel_Select_Scroll : PanelBase, IHas_UIButton<Panel_Select_Scroll.Ebutton>
{
    const int const_iSlot_Count = 4;
    public enum Ebutton
    {
        Button_Confirm,
    }

    [GetComponentInChildren]
    private Widget_Scroll _pOriginal_Widget_Scroll = null;

    [GetComponentInChildren]
    private List<Widget_Scroll> _list_Widget = new List<Widget_Scroll>();


    protected override void OnAwake()
    {
        base.OnAwake();

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
        List<ESkill> listSkillData =DataManager.DoGet_Select_Skill_List(const_iSlot_Count);

        for (int i = 0; i < listSkillData.Count; ++i)
        {
            _list_Widget[i].DoInit(listSkillData[i].GetSkillData());
        }

        //_pOriginal_Widget_Scroll.DoInit(ESkill.Skill_Summon_Fairy.GetSkillData());
    }

    public void IHas_UIButton_OnClickButton(UIButtonMessage<Ebutton> sButtonMsg)
    {
        switch (sButtonMsg.eButtonName)
        {
            case Ebutton.Button_Confirm:
                this.DoHide();
                break;
        }
    }
}
