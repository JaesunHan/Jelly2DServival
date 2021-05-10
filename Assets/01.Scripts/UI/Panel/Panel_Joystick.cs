using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Joystick : PanelBase
{
    public enum EImage
    {
        Image_Arrow_Down,
        Image_Arrow_UP,
        Image_Arrow_Left,
        Image_Arrow_Right,
    }

    [GetComponentInChildren]
    private Dictionary<EImage, Image> _mapImages = new Dictionary<EImage, Image>();

    protected override void OnAwake()
    {
        base.OnAwake();

    }

    public void DoInit()
    { 
        
    }


    private void OnStick_Move_Func(PlayerManager_HJS.MoveJoystickMessage pMessage)
    {
        Vector2 vecDir = pMessage.vecMoveDir;



    }


}
