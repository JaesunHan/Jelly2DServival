using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Widget_Money_Button : PanelBase, IHas_UIButton<Widget_Money_Button.EButton>
{
    public enum EButton
    {
        Button_Select,
    }

    [GetComponentInChildren("Image_Icon")]
    private Image _pImage_Icon = null;

    [GetComponentInChildren("Text_Amount")]
    private Text _pText_Amount = null;

    public void DoInit(EMoneyType eMoneyType)
    {
        MoneyData pMoneyData = eMoneyType.GetMoneyData();

        if (null != pMoneyData)
        {
            _pImage_Icon.sprite = DataManager.GetSprite_InAtlas(pMoneyData.strIconAtlasName, pMoneyData.strIconSpriteName);

            int iAmount = 0;
            string strUnit = "";
            _pText_Amount.text = $"{iAmount} {strUnit}";
        }
    
    }

    public void IHas_UIButton_OnClickButton(UIButtonMessage<EButton> sButtonMsg)
    {
        switch (sButtonMsg.eButtonName)
        {
            case EButton.Button_Select:


                break;

            default:
                break;
        }
    }
}
