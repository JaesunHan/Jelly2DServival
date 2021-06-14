using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class Skill_Fairy : SkillBase
{
    const float const_fFire_Term = 3f;
    

    private Transform _pPlayer = null;

    private float _fRotSpeed = 50f; //120f

    

    private WaitForSeconds _ws_FireTerm;

    [GetComponentInChildren]
    private List< SpriteRenderer > _list_Sprite = new List<SpriteRenderer>();

    //private Vector2 _vecOriginal_pos = Vector2.zero;

    protected override void OnAwake()
    {
        base.OnAwake();

        _bIsAlive = false;

        //_vecOriginal_pos = transform.position;

        for (int i = 0; i < _list_Sprite.Count; ++i)
        {
            _list_Sprite[i].gameObject.SetActive(false);
        }
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

        //transform.position = _pPlayer.position + (Vector3)(Vector2.up * 0.4f);
        transform.position = _pPlayer.position + (Vector3)(Vector2.up * 0.661f);
        //_list_Sprite.transform.position = transform.position +(Vector3)(Vector2.up * 1.3f);
        _list_Sprite[0].gameObject.SetActive(true);
        _bIsAlive = true;

        _ws_FireTerm = new WaitForSeconds(const_fFire_Term);
        StartCoroutine(nameof(OnCoroutine_RotateAround));
        //StartCoroutine(nameof(OnCoroutine_Fire));
    }

    public void DoSet_Original_Pos()
    {
        StopAllCoroutines();
        //transform.position = _pPlayer.position + (Vector3)(vector2.up * 0.661f);
        //_list_Sprite.transform.position = transform.position + (Vector3)(Vector2.up * 1.3f);
        Invoke(nameof(SetOriginal_Pos), Time.deltaTime);
        //StartCoroutine(nameof(OnCoroutine_Fire));
    }

    private void SetOriginal_Pos()
    {
        StartCoroutine(nameof(OnCoroutine_RotateAround));
    }

    public void DoUpgrade(int iUpgradeLv)
    {
        //base.DoUpgrade();
        _iUpgradeLv = iUpgradeLv;
        if (1 == _iUpgradeLv)
        {
            _list_Sprite[0].gameObject.SetActive(true);
        }
        else if (3 == _iUpgradeLv)
        {
            _list_Sprite[1].gameObject.SetActive(true);
        }
        else if (5 == _iUpgradeLv)
        {
            _list_Sprite[2].gameObject.SetActive(true);
        }
        else if (7 == _iUpgradeLv)
        {
            _list_Sprite[3].gameObject.SetActive(true);
        }

    }

    /// <summary>
    /// 플레이어 주변을 공전한다.
    /// </summary>
    /// <returns></returns>
    private IEnumerator OnCoroutine_RotateAround()
    {
        while (_bIsAlive)
        {
            transform.position = _pPlayer.position + (Vector3)(Vector2.up * 0.661f); // + (Vector3)(Vector2.up * 0.6f)

            for (int i = 0; i < _list_Sprite.Count; ++i)
            {
                _list_Sprite[i].transform.RotateAround(transform.position, Vector3.forward, _fRotSpeed * Time.fixedDeltaTime); //+ (Vector3)(Vector2.up * 0.6f )
            }
            
            yield return new WaitForFixedUpdate();
        }
    }

    //private IEnumerator OnCoroutine_Fire()
    //{
    //    while (_bIsAlive)
    //    {

    //        yield return _ws_FireTerm;
    //    }
    //}
    

}
