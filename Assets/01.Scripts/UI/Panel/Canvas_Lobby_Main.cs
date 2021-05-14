using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canvas_Lobby_Main : PanelBase, IHas_UIButton<Canvas_Lobby_Main.EButton>
{
    public enum EButton
    {
        Button_Start_Game,
    }

    public override void DoShow()
    {
        base.DoShow();
    }

    public void DoInit()
    { 
    
    }

    public void IHas_UIButton_OnClickButton(UIButtonMessage<EButton> sButtonMsg)
    {
        switch (sButtonMsg.eButtonName)
        {
            case EButton.Button_Start_Game:
                SceneLoadManager.DoChangeScene(SceneLoadManager.EScene_Where.InGameScene);
                break;
        }
    }
}
