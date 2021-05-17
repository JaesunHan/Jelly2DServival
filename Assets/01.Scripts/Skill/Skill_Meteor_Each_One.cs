using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Meteor_Each_One : ObjectBase
{
    const string const_str_Trigger_Crash_With_Ground = "trigger_Crash_With_Ground";

    [GetComponent]
    private Animator _pAnim = null;

    [GetComponentInChildren]
    private Rigidbody2D _pRigidbody = null;

    /// <summary>
    /// 운석이 떨어지는 방향과 크기를 저장
    /// </summary>
    private Vector2 _vec_Force_Dir;

    protected override void OnAwake()
    {
        base.OnAwake();

        _vec_Force_Dir = new Vector2(1, -1f) * 5f;
    }

    /// <summary>
    /// 떨어지기 시작하면 오른쪽 아래로 움직이게 한다.
    /// </summary>
    public void DoStart_Falling()
    {
        _pRigidbody.velocity = _vec_Force_Dir;
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
