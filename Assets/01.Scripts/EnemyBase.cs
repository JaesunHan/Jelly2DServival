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
    private bool _bIsAlive = false;

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
        _bIsAlive = false;
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
        _pSprite_Jelly.color = Color.white;
        _bIsAlive = true;
    }

    /// <summary>
    /// 사라지는 연출
    /// </summary>
    public void DoDisappear()
    {
        //StopAllCoroutines();
        StopCoroutine(nameof(OnCoroutine_Tracing_Player));
        if (!gameObject.activeSelf)
            return;
        StartCoroutine(nameof(OnCoroutine_Disappear));
    }
    private IEnumerator OnCoroutine_Disappear()
    {
        float fProgress = 0f;
        Color colorDest = new Color(1, 1, 1, 0);
        while (fProgress <= 0.7f)
        {
            fProgress += Time.deltaTime;

            Color colorCur = _pSprite_Jelly.color;
            _pSprite_Jelly.color = Color.Lerp(colorCur, colorDest, Time.deltaTime * 1.5f);

            yield return null;
        }

        _pSprite_Jelly.color = colorDest;
        yield return null;
        yield return null;

        gameObject.SetActive(false);
    }

    public void DoKnockback(Vector2 vecBulletDir)
    {
        if (!_bIsAlive)
            return;

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

        if(_fHP > 0)
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
        if (!_bIsAlive)
            return;

        Bullet pBullet = collision.GetComponent<Bullet>();

        if (null != pBullet)
        {
            StartCoroutine(OnCoroutine_Damage_Bullet(pBullet));

            //float fDamage = pBullet.fDamage;
            //_fHP -= fDamage;
            //Check_HP();

            return;
        }

        SkillBase pSkill = collision.GetComponentInParent<SkillBase>();
        if (null != pSkill)
        {
            StartCoroutine(OnCoroutine_Damage_Skill(pSkill));
            //if (pSkill._pSkillData.eSkill == ESkill.Skill_Summon_Fairy)
            //{
            //    float fDamage = ESkill.Skill_Summon_Fairy.GetSkillData().fStatAmount;
            //    _fHP -= fDamage;
            //    Check_HP();
            //    return;
            //}
            return;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Bullet pBullet = collision.GetComponent<Bullet>();

        if (null != pBullet)
        {
            StopCoroutine(OnCoroutine_Damage_Bullet(pBullet));

            return;
        }

        SkillBase pSkill = collision.GetComponentInChildren<SkillBase>();
        if (null != pSkill)
        {
            StopCoroutine(OnCoroutine_Damage_Skill(pSkill));
            return;
        }

        Check_HP();
    }

    private bool Check_HP()
    {
        if (0 >= _fHP)
        {
            _bIsAlive = false;

            DoDisappear();

            EnemyManager.instance.OnReturn_Enemy.DoNotify(new EnemyManager.ReturnEnemyMessage(this, _bIsAlive));
            ManaManager.instance.OnGet_MP.DoNotify(new ManaManager.GetMPMessage(pEnemyData.fGetMP));
        }
        _pHPBar.DoSetHP(_fHP);

        return _bIsAlive;
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

    /// <summary>
    /// 데미지 입히는 코루틴. 적이랑 닿아 있는 동안 호출함
    /// </summary>
    /// <returns></returns>
    private IEnumerator OnCoroutine_Damage_Bullet(Bullet pBullet)
    {
        while (_bIsAlive)
        {
            float x = (pBullet.transform.position.x + transform.position.x) * 0.51f;
            float y = (pBullet.transform.position.y + transform.position.y) * 0.51f;
            Vector2 vecShowPos = new Vector2(x, y);
            EffectManager.instance.OnShowEffect.DoNotify(new EffectManager.ShowEffectMessag(EEffectName.Default_Damage, (Vector2)transform.position));

            float fDamage = pBullet.fDamage;
            _fHP -= fDamage;
            Check_HP();

            yield return new WaitForSeconds(0.7f);
        }
    }

    private IEnumerator OnCoroutine_Damage_Skill(SkillBase pSkill)
    {
        while (_bIsAlive)
        {
            if (pSkill._pSkillData.eSkill == ESkill.Skill_Summon_Fairy)
            {
                //float x = (pSkill.transform.position.x + transform.position.x) * 0.5f;
                //float y = (pSkill.transform.position.y + transform.position.y) * 0.5f;
                float x = transform.position.x;
                float y = transform.position.y;
                Vector2 vecShowPos = new Vector2(x, y);
                EffectManager.instance.OnShowEffect.DoNotify(new EffectManager.ShowEffectMessag(EEffectName.Summon_Fairy_Damage, vecShowPos));

                int iSkillLv = PlayerManager_HJS.instance.DoGet_Skill_Lv(pSkill._pSkillData.eSkill);
                float fDamage = pSkill._pSkillData.fStatAmount * iSkillLv;
                _fHP -= fDamage;
                Check_HP();
                break;
            }
            else if (pSkill._pSkillData.eSkill == ESkill.Skill_Meteor)
            {
                int iSkillLv_Meteor = PlayerManager_HJS.instance.DoGet_Skill_Lv(pSkill._pSkillData.eSkill);
                if (0 == iSkillLv_Meteor)
                    iSkillLv_Meteor = 1;
                float fDamage = pSkill._pSkillData.fStatAmount * iSkillLv_Meteor;
                _fHP -= fDamage;
                Check_HP();
                break;
            }

            yield return new WaitForSeconds(0.5f);
        }
    }


    #endregion
}
