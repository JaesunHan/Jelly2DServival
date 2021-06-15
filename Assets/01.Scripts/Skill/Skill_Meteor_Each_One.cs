using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Meteor_Each_One : SkillBase
{
    const string const_str_Trigger_Meteor_Falling = "trigger_Skill_Meteor_Falling";
    const string const_str_Trigger_Crash_With_Ground = "trigger_Crash_With_Ground";

    [GetComponent]
    private Animator _pAnim = null;

    [GetComponentInChildren]
    private Rigidbody2D _pRigidbody = null;

    /// <summary>
    /// 운석이 떨어지는 방향과 크기를 저장
    /// </summary>
    private Vector2 _vec_Velocity_Dir;

    /// <summary>
    /// 첫 시작 지점
    /// </summary>
    private Vector2 _vec_Start_Pos;

    protected override void OnAwake()
    {
        base.OnAwake();

        _vec_Velocity_Dir = new Vector2(1, -1f) * 6f;
        _vec_Start_Pos = transform.position;
    }

    public override void DoInit(SkillData pSkillData)
    {
        base.DoInit(pSkillData);
    }

    /// <summary>
    /// 떨어지기 시작하면 오른쪽 아래로 움직이게 한다.
    /// </summary>
    public void DoStart_Falling()
    {
        _pAnim.SetTrigger(const_str_Trigger_Meteor_Falling);
        transform.position = (Vector3)_vec_Start_Pos + (Vector3)PlayerManager_HJS.instance.DoGet_Cur_Player_WorldPos();
        _pRigidbody.velocity = _vec_Velocity_Dir;
    }

    /// <summary>
    /// 땅과 부딪혔을 때의 애니메이션을 재생한다.
    /// </summary>
    public void DoChange_Anim_To_Crash_With_Ground()
    {
        float fTerm = Random.Range(0.7f, 2f) ;

        Invoke(nameof(Change_Anim_Term), fTerm);
    }

    private void Change_Anim_Term()
    {
        _pRigidbody.velocity = Vector2.zero;
        _pAnim.SetTrigger(const_str_Trigger_Crash_With_Ground);

        MainCameraManager.instance.OnOccur_Camera_Anim.DoNotify(new MainCameraManager.OccurCameraAnimMessage(MainCameraManager.ECameraAnimType.Crash_Meteor));
    }

}
