using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommonData;

public class PlayerManager : MonoSingleton<PlayerManager>
{
    public struct MoveJoystickMessage
    {
        public Vector2 vecMoveDir;

        public MoveJoystickMessage(Vector2 vecMoveDir)
        {
            this.vecMoveDir = vecMoveDir;
        }
    }

    [SerializeField]
    private PlayerCharacter _pOriginal_PlayerCharacter = null;

    [SerializeField]
    private PlayerCharacter _pCharacter = null;

    private EDir _eDir = EDir.Dir_None;


    public Observer_Pattern<MoveJoystickMessage> OnMove_Stick { get; private set; } = Observer_Pattern<MoveJoystickMessage>.instance;
    protected override void OnAwake()
    {
        base.OnAwake();

        if (null == _pCharacter)
        {
            if (null != _pOriginal_PlayerCharacter)
            {
                _pCharacter = Instantiate<PlayerCharacter>(_pOriginal_PlayerCharacter);
                _pCharacter.transform.localPosition = Vector2.zero;

                _pCharacter.transform.SetParent(transform);
            }
        }

        OnMove_Stick.Subscribe += OnMove_Stick_Func;

    }


    private void OnDestroy()
    {
        OnMove_Stick.Subscribe -= OnMove_Stick_Func;

    }

    private void OnMove_Stick_Func(MoveJoystickMessage pMessage)
    {
        Vector2 vecMoveDire = pMessage.vecMoveDir;

        if (pMessage.vecMoveDir == Vector2.zero)
        {
            _pCharacter.DoPlay_IdleAnim();
        }
        else
        {
            _pCharacter.DoPlay_WalkAnim();
            if (pMessage.vecMoveDir.x > 0)
            {
                _pCharacter.DoChange_Dir(EDir.Dir_Right);
            }
            else
            {
                _pCharacter.DoChange_Dir(EDir.Dir_Left);
            }
        }
    }
}
