using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Widget_Scroll : PanelBase, IHas_UIButton<Widget_Scroll.EButton>
{
    private SkillData _pSkillData = null;


    public enum EButton
    {
        Button_Select,
    }

    public enum EText
    {
        Text_Scroll_Title,
        Text_Scroll_Desc,
        Text_Scroll_Lv, 
    }

    [GetComponentInChildren("Image_Icon")]
    private Image _pImage_Icon = null;

    [GetComponentInChildren]
    private Dictionary<EText, Text> _mapText = new Dictionary<EText, Text>();


    protected override void OnAwake()
    {
        base.OnAwake();

        
    }

    public void DoInit(SkillData pSkillData)
    {
        if (null == pSkillData)
        {
            DebugLogManager.LogError("DoInit() - 매개변수로 받은 pSkillData 가 null 입니다.");
            return; 
        }

        _pSkillData = pSkillData;

        _mapText[EText.Text_Scroll_Title].text = DataManager.GetLocalText(_pSkillData.eSkillName);
        _mapText[EText.Text_Scroll_Desc].text = DataManager.GetLocalText(_pSkillData.eSkillDesc);
        int iLv = 0;
        _mapText[EText.Text_Scroll_Lv].text = $"+{iLv}";

        _pImage_Icon.sprite = DataManager.GetSprite_InAtlas(_pSkillData.strIconAtlasName, _pSkillData.strIconSpriteName);
    }


    public void IHas_UIButton_OnClickButton(UIButtonMessage<EButton> sButtonMsg)
    {
        throw new System.NotImplementedException();
    }
}
