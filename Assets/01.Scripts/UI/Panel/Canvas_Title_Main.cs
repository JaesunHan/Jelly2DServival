using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canvas_Title_Main : PanelBase, IHas_UIButton<Canvas_Title_Main.EButton>
{
    public enum EButton
    {
        Button_Touch_Screen,
    }

    public override void DoShow()
    {
        base.DoShow();

        DoInit();
    }

    public void DoInit()
    { 
    
    }

    public void IHas_UIButton_OnClickButton(UIButtonMessage<EButton> sButtonMsg)
    {
        switch (sButtonMsg.eButtonName)
        {
            case EButton.Button_Touch_Screen:
                SceneLoadManager.DoChangeScene(SceneLoadManager.EScene_Where.InGameScene);
                break;
            default:
                break;
        }
    }
}
