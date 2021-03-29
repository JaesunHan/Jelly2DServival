using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel_Select_Scroll : PanelBase, IHas_UIButton<Panel_Select_Scroll.Ebutton>
{
    public enum Ebutton
    {
        Button_Confirm,
    }

    [GetComponentInChildren]
    private Widget_Scroll _pOriginal_Widget_Scroll = null;

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
        _pOriginal_Widget_Scroll.DoInit(ESkill.Skill_Summon_Fairy.GetSkillData());
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
