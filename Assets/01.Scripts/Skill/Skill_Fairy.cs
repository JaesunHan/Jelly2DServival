using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class Skill_Fairy : SkillBase
{
    const float const_fFire_Term = 3f;
    /// <summary>
    /// 이 스킬의 업그레이드 레벨
    /// </summary>
    private int _iUpgradeLv = 0;

    private Transform _pPlayer = null;

    private float _fRotSpeed = 50f; //120f

    

    private WaitForSeconds _ws_FireTerm;

    [GetComponentInChildren("Sprite_Fairy")]
    private SpriteRenderer _pSprite = null;

    protected override void OnAwake()
    {
        base.OnAwake();

        _bIsAlive = false;
        
    }

    public override void DoInit(SkillData pSkillData)
    {
        base.DoInit(pSkillData);

        StopAllCoroutines();

        _iUpgradeLv = 0;

        if (null == _pPlayer)
        {
            _pPlayer = PlayerManager_HJS.instance.DoGet_Cur_Player_Character().transform;
        }

        //transform.position = _pPlayer.position + (Vector3)(Vector2.up * 1.2f);
        transform.position = _pPlayer.position + (Vector3)(Vector2.up * 0.4f);
        _pSprite.transform.position = transform.position +(Vector3)(Vector2.up * 1.3f);

        _bIsAlive = true;

        _ws_FireTerm = new WaitForSeconds(const_fFire_Term);
        StartCoroutine(nameof(OnCoroutine_RotateAround));
        StartCoroutine(nameof(OnCoroutine_Fire));
    }

    public void DoUpgrade()
    {
        _iUpgradeLv++;


    }

    /// <summary>
    /// 플레이어 주변을 공전한다.
    /// </summary>
    /// <returns></returns>
    private IEnumerator OnCoroutine_RotateAround()
    {
        while (_bIsAlive)
        {
            transform.position = _pPlayer.position + (Vector3)(Vector2.up * 0.4f);

            _pSprite.transform.RotateAround(_pPlayer.position + (Vector3)(Vector2.up * 0.4f), Vector3.forward, _fRotSpeed * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator OnCoroutine_Fire()
    {
        while (_bIsAlive)
        {

            yield return _ws_FireTerm;
        }
    }
    

}
