using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommonData;

public class PlayerCharacter : ObjectBase
{
    const float const_fDefault_HP = 100;
    const float const_fMove_Speed = 2.0f;

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

    /// <summary>
    /// 플레이어의 HP
    /// </summary>
    private float _fHP = 0;

    [GetComponentInChildren]
    private HPBar _pHPBar = null;

    protected override void OnAwake()
    {
        base.OnAwake();

        if (null == _pRigidbody)
        {
            _pRigidbody = GetComponentInChildren<Rigidbody2D>();
        }
    }

    /// <summary>
    /// 캐릭터가 처음 맵에 배치될 때 초기화
    /// </summary>
    public void DoInit()
    {
        //기본 체력 초기화
        _fHP = const_fDefault_HP;

        _pHPBar.DoInit(_fHP);
    }

    public void DoPlay_WalkAnim()
    {
        _pSPUM_Prefabs.PlayAnimation((int)EPlayerState.Run);
    }

    public void DoPlay_IdleAnim()
    {
        _pSPUM_Prefabs.PlayAnimation((int)EPlayerState.Idle);
    }

    public void DoPlay_AttackMagicAnim()
    {
        _pSPUM_Prefabs.PlayAnimation((int)EPlayerState.Attack_Magic);
        
        _pAnim_Magic_Fire.SetTrigger("trigger_Magic_Fire");
    }

    public void DoChange_Dir(EDir eDir)
    {
        if (_pSPUM_Prefabs.ePlayerState == EPlayerState.Run)
            _pSPUM_Prefabs.transform.localScale = new Vector3((int)eDir*-1, 1, 1);
            //transform.localScale = new Vector3((int)eDir, 1, 1);
    }

    /// <summary>
    /// 현재 발사하려는 방향을 바라보게 한다.
    /// </summary>
    /// <param name="eDir"></param>
    public void DoLook_Fire_Dir(EDir eDir)
    {
        //transform.localScale = new Vector3((int)eDir, 1, 1);
        _pSPUM_Prefabs.transform.localScale = new Vector3((int)eDir*-1, 1, 1);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var pEnemy = collision.GetComponentInChildren<EnemyBase>();

        if (null != pEnemy)
        {
            pEnemy.DoCrash_With_Player(const_fDefault_HP * 0.02f);

            _fHP -= pEnemy.pEnemyData.fCrashDamage;
            DebugLogManager.Log($"남은 체력 : {_fHP}");
            if (0 >= _fHP)
            {
                _fHP = 0;
                DebugLogManager.Log("플레이어가 죽었다.");
            }
            _pHPBar.DoSetHP(_fHP);
        }
    }
}
