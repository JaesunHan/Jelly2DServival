using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Meteor : SkillBase
{
    
    /// <summary>
    /// 떨어지는 운석 하나에 리지드바디 하나가 있음.
    /// 각 운석을 참조하여 리스트에 추가.
    /// </summary>
    [GetComponentInChildren]
    private List<Skill_Meteor_Each_One> _list_rigidbody_Each_FireBalls = new List<Skill_Meteor_Each_One>();

    private float _fCrash_Term = 10.0f;

    private WaitForSeconds _ws_Crash_Term;


    protected override void OnAwake()
    {
        base.OnAwake();

        _bIsAlive = false;

        //for (int i = 0; i < _list_rigidbody_Each_FireBalls.Count; ++i)
        //{
        //    //_list_rigidbody_Each_FireBalls[i].velocity = _vec_Force_Dir;
        //    _list_rigidbody_Each_FireBalls[i].DoAwake();
        //    _list_rigidbody_Each_FireBalls[i].DoStart_Falling();
        //}

        //Invoke(nameof(DoChange_Anim_To_Crash_With_Ground), 1.5f);
        _ws_Crash_Term = new WaitForSeconds(_fCrash_Term);
    }

    public override void DoInit(SkillData pSkillData)
    {
        base.DoInit(pSkillData);

        StopAllCoroutines();
        _iUpgradeLv = 0;

        var pPlayer = PlayerManager_HJS.instance.DoGet_Cur_Player_Character().transform;

        transform.position = pPlayer.position;

        _bIsAlive = true;

        //for (int i = 0; i < _list_rigidbody_Each_FireBalls.Count; ++i)
        //{
        //    //_list_rigidbody_Each_FireBalls[i].velocity = _vec_Force_Dir;
        //    _list_rigidbody_Each_FireBalls[i].DoStart_Falling();
        //}

        //Invoke(nameof(DoChange_Anim_To_Crash_With_Ground), 1.5f);
        StartCoroutine(nameof(OnCoroutine_Fall_Meteor));
    }

    public void DoUpgrade(int iUpgradeLv)
    {
        _iUpgradeLv = iUpgradeLv;
    }

    public void DoChange_Anim_To_Crash_With_Ground()
    {
        for (int i = 0; i < _list_rigidbody_Each_FireBalls.Count; ++i)
        {
            var pSkill_Meteor_Each_One = _list_rigidbody_Each_FireBalls[i].GetComponent<Skill_Meteor_Each_One>();
            if (null != pSkill_Meteor_Each_One)
            {
                pSkill_Meteor_Each_One.DoChange_Anim_To_Crash_With_Ground();
            }
        }
    }

    

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private IEnumerator OnCoroutine_Fall_Meteor()
    {
        while (_bIsAlive)
        {
            for (int i = 0; i < _list_rigidbody_Each_FireBalls.Count; ++i)
            {
                //_list_rigidbody_Each_FireBalls[i].velocity = _vec_Force_Dir;
                _list_rigidbody_Each_FireBalls[i].DoStart_Falling();
            }

            Invoke(nameof(DoChange_Anim_To_Crash_With_Ground), 1.5f);

            yield return _ws_Crash_Term;
        }
    }
}
