using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Widget_Money_Button : PanelBase, IHas_UIButton<Widget_Money_Button.EButton>
{
    public enum EButton
    {
        Button_Select,
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
