using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : ObjectBase
{
    private float _fMoveSpeed = 0f;

    private Vector2 _vecMoveDir = Vector2.zero;
    private Vector2 _vecTargetPos = Vector2.zero;

    private Rigidbody2D _pRigidyBody = null;

    public bool bIsAlive { get; private set; } = false;

    /// <summary>
    /// 탄환의 데미지 
    /// </summary>
    public float fDamage { get; private set; } = 1f;

    protected override void OnAwake()
    {
        base.OnAwake();

        if (null == _pRigidyBody)
        {
            _pRigidyBody = GetComponent<Rigidbody2D>();
        }

        _fMoveSpeed = 30;
    }

    /// <summary>
    /// 발사 시점에 타게팅한 적의 위치를 매개변수로 받는다.
    /// </summary>
    /// <param name="vecDestPos"></param>
    public void DoFire(Vector2 vecDestPos)
    {
        StopAllCoroutines();

        bIsAlive = true;
        _vecTargetPos = vecDestPos;

        Vector2 vecPlayerPos = PlayerManager.instance.DoGet_Cur_Player_WorldPos();

        _vecMoveDir = (vecDestPos - vecPlayerPos).normalized;
        Vector2 vecForce = _vecMoveDir * _fMoveSpeed;
        
        _pRigidyBody.velocity = vecForce;

        StartCoroutine(nameof(OnCoroutine_Move_Bullet));
    }

    private IEnumerator OnCoroutine_Move_Bullet()
    {
        float fMovingTime = 0;

        //일정 시간동안 총알이 정해진 방향으로 날아간다.
        while (fMovingTime <= 0.3f)
        {
            fMovingTime += Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();
        }

        bIsAlive = false;

        _pRigidyBody.velocity = Vector2.zero;

        PlayerManager.instance.OnReturn_Bullet.DoNotify(new PlayerManager.ReturnBulletMessage(this, bIsAlive));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var pColTarget = collision.GetComponent<EnemyBase>();

        if (null != pColTarget)
        {
            bIsAlive = false;

            _pRigidyBody.velocity = Vector2.zero;
            pColTarget.DoKnockback(_vecMoveDir);

            PlayerManager.instance.OnReturn_Bullet.DoNotify(new PlayerManager.ReturnBulletMessage(this, bIsAlive));
        }
    }

    
}
