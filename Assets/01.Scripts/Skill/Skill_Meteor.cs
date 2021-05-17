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

    

    private float _fCrash_Term = 3.0f;


    protected override void OnAwake()
    {
        base.OnAwake();

        _bIsAlive = false;
        
        for (int i = 0; i < _list_rigidbody_Each_FireBalls.Count; ++i)
        {
            //_list_rigidbody_Each_FireBalls[i].velocity = _vec_Force_Dir;
            _list_rigidbody_Each_FireBalls[i].DoStart_Falling();
        }

        Invoke(nameof(DoChange_Anim_To_Crash_With_Ground), 1.5f);

    }

    public override void DoInit(SkillData pSkillData)
    {
        base.DoInit(pSkillData);

        _bIsAlive = true;

        //for (int i = 0; i < _list_rigidbody_Each_FireBalls.Count; ++i)
        //{
        //    _list_rigidbody_Each_FireBalls[i].AddForce(_vec_Force_Dir);
        //}

        
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

    

    ///// <summary>
    ///// 
    ///// </summary>
    ///// <returns></returns>
    //private IEnumerator OnCoroutine_Falling_Anim()
    //{
    //    while (_bIsAlive)
    //    {
    //        for (int i = 0; i < _list_Anim_Each_FireBalls.Count; ++i)
    //        {
                
    //        }
    //        yield return new WaitForFixedUpdate();
    //    }
    //}
}
