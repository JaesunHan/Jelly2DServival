using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommonData;

public class PlayerCharacter : ObjectBase
{
    const float const_fMove_Speed = 1.5f;

    //private Animator _pAnim = null;

    private Transform _pSprite_Transform = null;

    private Rigidbody2D _pRigidbody = null;

    [GetComponentInChildren]
    private SPUM_Prefabs _pSPUM_Prefabs = null;

    [GetComponentInChildren("Pos_MagicFire")]
    private Transform _pTransform_Pos_Magic_Fire = null;

    /// <summary>
    /// 마법 발사 애니메이터
    /// </summary>
    [GetComponentInChildren("MagicFire")]
    private Animator _pAnim_Magic_Fire = null;
    protected override void OnAwake()
    {
        base.OnAwake();
        //if (null == _pAnim)
        //{
        //    _pAnim = GetComponentInChildren<Animator>(true);
        //    _pSprite_Transform = _pAnim.transform;
        //}

        if (null == _pRigidbody)
        {
            _pRigidbody = GetComponentInChildren<Rigidbody2D>();
        }
    }

    public void DoPlay_WalkAnim()
    {
        //if(_pSPUM_Prefabs.ePlayerState != EPlayerState.Attack_Magic)
        _pSPUM_Prefabs.PlayAnimation((int)EPlayerState.Run);
        //if (false == IsExist_Anim())
        //    return;

        //_pAnim.SetBool("Run", true);
    }

    public void DoPlay_IdleAnim()
    {
        _pSPUM_Prefabs.PlayAnimation((int)EPlayerState.Idle);
        //if (false == IsExist_Anim())
        //    return;

        //_pAnim.SetBool("isWalk", false);
    }

    public void DoPlay_AttackMagicAnim()
    {
        _pSPUM_Prefabs.PlayAnimation((int)EPlayerState.Attack_Magic);
        
        _pAnim_Magic_Fire.SetTrigger("trigger_Magic_Fire");
    }

    public void DoChange_Dir(EDir eDir)
    {
        if(_pSPUM_Prefabs.ePlayerState == EPlayerState.Run)
            transform.localScale = new Vector3((int)eDir, 1, 1);
    }

    /// <summary>
    /// 현재 발사하려는 방향을 바라보게 한다.
    /// </summary>
    /// <param name="eDir"></param>
    public void DoLook_Fire_Dir(EDir eDir)
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

    /// <summary>
    /// 마법 공격시 발사 지점 반환
    /// </summary>
    /// <returns></returns>
    public Vector3 DoGet_Pos_Magic_Fire()
    {
        return _pTransform_Pos_Magic_Fire.position;
    }
}
