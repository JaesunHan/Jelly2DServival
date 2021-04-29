using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class EnemyBase : ObjectBase
{
    //[SerializeField]
    public EnemyData pEnemyData { get; private set; } = null;

    //[SerializeField]
    private SpriteRenderer _pSprite_Jelly = null;

    //private Transform _pTrasnform_Player = null;

    private Vector2 _vecPlayer_Pos = Vector2.zero;

    private Animator _pAnim = null;

    private CircleCollider2D _pCollider2d = null;
    private Rigidbody2D _pRigidbody = null;

    private float _fHP = 0f;
    private bool bIsAlive = false;

    [GetComponentInChildren]
    private HPBar _pHPBar = null;

    protected override void OnAwake()
    {
        base.OnAwake();

        if (null == _pSprite_Jelly)
        {
            var arrSprite = transform.GetComponentsInChildren<SpriteRenderer>();
            for (int i = 0; i < arrSprite.Length; ++i)
            {
                if ("Sprite_Jelly" == arrSprite[i].name)
                {
                    _pSprite_Jelly = arrSprite[i];
                    break;
                }
            }
        }

        if (null == _pAnim)
        {
            _pAnim = GetComponent<Animator>();
        }

        if (null == _pCollider2d)
        {
            _pCollider2d = GetComponent<CircleCollider2D>();
        }

        if (null == _pRigidbody)
        {
            _pRigidbody = GetComponent<Rigidbody2D>();
        }
        bIsAlive = false;
    }

    public void DoInit(EnemyData pEnemyData)
    {
        this.pEnemyData = pEnemyData;

        _fHP = this.pEnemyData.fHP;
        _pHPBar.DoAwake();
        _pHPBar.DoInit(_fHP);
        _pSprite_Jelly.sprite = pEnemyData.pFile;

        _pCollider2d.offset = new Vector2(0, this.pEnemyData.fColliderPosY);
        _pCollider2d.radius = this.pEnemyData.fColliderRadius;

        _vecPlayer_Pos = PlayerManager_HJS.instance.DoGet_Cur_Player_Character().transform.position;

        //_pRigidbody.velocity = Vector2.zero;
        Tracing_Player();
        bIsAlive = true;
    }

    public void DoKnockback(Vector2 vecBulletDir)
    {
        StopAllCoroutines();
        if(gameObject.activeSelf)
            StartCoroutine(OnCoroutine_Knockback(vecBulletDir));
    }

    public void DoCrash_With_Player(float fDamage)
    {
        _fHP -= fDamage;

        Check_HP();
    }

    private IEnumerator OnCoroutine_Knockback(Vector2 vecBulletDir)
    {
        float fProgress = 0f;
        Vector2 vecPos = transform.position;
        Vector2 vecDest = vecPos + vecBulletDir * 1.2f;
        while (fProgress <= 1f)
        {
            fProgress += Time.deltaTime;
            Vector2 vecCurPos = transform.position;

            transform.position = Vector2.Lerp(vecCurPos, vecDest, Time.deltaTime * 5f);

            yield return null;
        }

        Tracing_Player();
    }

    private void Tracing_Player()
    {
        StopAllCoroutines();
        _pAnim.SetBool("isWalk", true);
        StartCoroutine(nameof(OnCoroutine_Tracing_Player));
    }

    private IEnumerator OnCoroutine_Tracing_Player()
    {
        while (true)
        {
            Vector2 vecCurPos = transform.position;

            _vecPlayer_Pos = PlayerManager_HJS.instance.DoGet_Cur_Player_Character().transform.position;
            Vector2 vecMoveDir = (_vecPlayer_Pos - vecCurPos).normalized;

            if (_vecPlayer_Pos.x < transform.position.x)
                _pSprite_Jelly.transform.localScale = new Vector3(-1, 1, 1);
            else
                _pSprite_Jelly.transform.localScale = new Vector3(1, 1, 1);

            transform.position += (Vector3)(vecMoveDir * Time.deltaTime * (pEnemyData.fMoveSpeed) );
            //_pRigidbody.velocity = vecMoveDir * _pEnemyData.fMoveSpeed;

            yield return null;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Bullet pBullet = collision.GetComponent<Bullet>();

        if (null != pBullet)
        {
            float fDamage = pBullet.fDamage;
            _fHP -= fDamage;
            Check_HP();

            return;
        }

        SkillBase pSkill = collision.GetComponentInParent<SkillBase>();
        if (null != pSkill)
        {
            if (pSkill._pSkillData.eSkill == ESkill.Skill_Summon_Fairy)
            {
                _fHP -= 3f;
                Check_HP();
                return;
            }
        }
    }

    private void Check_HP()
    {
        if (0 >= _fHP)
        {
            bIsAlive = false;
            EnemyManager.instance.OnReturn_Enemy.DoNotify(new EnemyManager.ReturnEnemyMessage(this, bIsAlive));
            ManaManager.instance.OnGet_MP.DoNotify(new ManaManager.GetMPMessage(pEnemyData.fGetMP));
        }
        _pHPBar.DoSetHP(_fHP);
    }

    #region Legacy_Code

    private IEnumerator OnCoroutin_Awake()
    {
        if (null == DataManager.instance)
        {
            DebugLogManager.Log("데이터 매니저의 인스턴스가 없다.");
        }

        while (!DataManager.bIsLoaded_AllResource)
        {
            yield return null;
        }
    }

    

    #endregion
}
