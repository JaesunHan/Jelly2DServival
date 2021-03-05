using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommonData;

public class PlayerCharacter : ObjectBase
{
    const float const_fMove_Speed = 1.5f;

    private Animator _pAnim = null;

    private Transform _pSprite_Transform = null;

    private Rigidbody2D _pRigidbody = null;
    protected override void OnAwake()
    {
        base.OnAwake();
        if (null == _pAnim)
        {
            _pAnim = GetComponentInChildren<Animator>(true);
            _pSprite_Transform = _pAnim.transform;
        }

        if (null == _pRigidbody)
        {
            _pRigidbody = GetComponentInChildren<Rigidbody2D>();
        }
    }

    public void DoPlay_WalkAnim()
    {
        if (false == IsExist_Anim())
            return;

        _pAnim.SetBool("isWalk", true);
    }

    public void DoPlay_IdleAnim()
    {
        if (false == IsExist_Anim())
            return;

        _pAnim.SetBool("isWalk", false);
    }

    public void DoChange_Dir(EDir eDir)
    {
        transform.localScale = new Vector3((int)eDir, 1, 1);
    }

    public void DoMove(Vector2 vecMoveDir)
    {
        _pRigidbody.velocity = vecMoveDir.normalized * const_fMove_Speed;
    }

    public void DoStop()
    {
        _pRigidbody.velocity = Vector2.zero;
    }

    private bool IsExist_Anim()
    {
        if (null != _pAnim)
            return true;
        else
        {
            DebugLogManager.LogError("There's no Animator Component");
            return false;
        }
    }

    

}
