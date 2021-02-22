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
    private PlayerCharacter _pCur_Character = null;

    private EDir _eDir = EDir.Dir_None;

    private float _fCharacter_Move_Speed = 0f;


    public Observer_Pattern<MoveJoystickMessage> OnMove_Stick { get; private set; } = Observer_Pattern<MoveJoystickMessage>.instance;
    protected override void OnAwake()
    {
        base.OnAwake();

        if (null == _pCur_Character)
        {
            if (null != _pOriginal_PlayerCharacter)
            {
                _pCur_Character = Instantiate<PlayerCharacter>(_pOriginal_PlayerCharacter);
                _pCur_Character.transform.localPosition = Vector2.zero;

                _pCur_Character.transform.SetParent(transform);

                _pOriginal_PlayerCharacter.SetActive(false);
            }
        }

        OnMove_Stick.Subscribe += OnMove_Stick_Func;

        _fCharacter_Move_Speed = 3f;
    }

    public PlayerCharacter DoGet_Cur_Player_Character()
    {
        return _pCur_Character;
    }

    public float DoGet_Player_Move_Speed()
    {
        return _fCharacter_Move_Speed;
    }

    private void OnDestroy()
    {
        OnMove_Stick.Subscribe -= OnMove_Stick_Func;

    }

    private void OnMove_Stick_Func(MoveJoystickMessage pMessage)
    {
        Vector2 vecMoveDir = pMessage.vecMoveDir;

        if (pMessage.vecMoveDir == Vector2.zero)
        {
            _pCur_Character.DoPlay_IdleAnim();
        }
        else
        {
            _pCur_Character.DoPlay_WalkAnim();
            if (pMessage.vecMoveDir.x > 0)
            {
                _pCur_Character.DoChange_Dir(EDir.Dir_Right);
            }
            else
            {
                _pCur_Character.DoChange_Dir(EDir.Dir_Left);
            }
        }
    }
}
