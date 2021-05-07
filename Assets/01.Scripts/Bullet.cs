using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : ObjectBase
{
    private float _fMoveSpeed = 0f;

    private Vector2 _vecMoveDir = Vector2.zero;
    private Vector2 _vecTargetPos = Vector2.zero;

    private Rigidbody2D _pRigidyBody = null;
    [GetComponent]
    private SpriteRenderer _pSpriteRender = null;

    public bool bIsAlive { get; private set; } = false;

    /// <summary>
    /// 탄환의 데미지 
    /// </summary>
    public float fDamage { get; private set; } = 3f;

    private Vector2 _vecDestPos = Vector2.zero;

    protected override void OnAwake()
    {
        base.OnAwake();

        if (null == _pRigidyBody)
        {
            _pRigidyBody = GetComponent<Rigidbody2D>();
        }

        _fMoveSpeed = 7;

        fDamage = 5f;

        _pSpriteRender.enabled = false;
    }

    /// <summary>
    /// 발사 시점에 타게팅한 적의 위치를 매개변수로 받는다.
    /// </summary>
    /// <param name="vecDestPos"></param>
    public void DoFire(Vector2 vecDestPos)
    {
        StopAllCoroutines();

        _pSpriteRender.enabled = false;
        _vecTargetPos = vecDestPos;

        Vector2 vecFireStartPos = PlayerManager_HJS.instance.DoGet_Pos_Magic_Fire();
        transform.position = vecFireStartPos;

        Invoke(nameof(Fire_Term), 0.7f);
    }

    private void Fire_Term()
    {
        bIsAlive = true;
        _pSpriteRender.enabled = true;
        
        Vector2 vecFireStartPos = PlayerManager_HJS.instance.DoGet_Pos_Magic_Fire();

        _vecMoveDir = (_vecTargetPos - vecFireStartPos).normalized;
        Vector2 vecForce = _vecMoveDir * _fMoveSpeed;

        transform.position = vecFireStartPos;
        _pRigidyBody.velocity = vecForce;

        StartCoroutine(nameof(OnCoroutine_Move_Bullet));
    }

    private IEnumerator OnCoroutine_Move_Bullet()
    {
        float fMovingTime = 0;

        //일정 시간동안 총알이 정해진 방향으로 날아간다.
        transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        while (fMovingTime <= 1.5f)
        {
            fMovingTime += Time.fixedDeltaTime;
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * 0.5f, Time.fixedDeltaTime * 2f);
            yield return new WaitForFixedUpdate();
        }

        bIsAlive = false;

        _pRigidyBody.velocity = Vector2.zero;

        PlayerManager_HJS.instance.OnReturn_Bullet.DoNotify(new PlayerManager_HJS.ReturnBulletMessage(this, bIsAlive));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!bIsAlive)
            return;

        var pColTarget = collision.GetComponent<EnemyBase>();

        if (null != pColTarget)
        {
            bIsAlive = false;

            _pRigidyBody.velocity = Vector2.zero;
            pColTarget.DoKnockback(_vecMoveDir);

            PlayerManager_HJS.instance.OnReturn_Bullet.DoNotify(new PlayerManager_HJS.ReturnBulletMessage(this, bIsAlive));
        }
    }

    
}
