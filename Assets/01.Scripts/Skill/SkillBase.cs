using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBase : ObjectBase
{
    public SkillData _pSkillData { get; private set; } = null;
    /// <summary>
    /// 스킬이 활성화 되었을 때 true 이다.
    /// </summary>
    protected bool bIsSkillOn = false;

    protected override void OnAwake()
    {
        base.OnAwake();
    }

    public virtual void DoInit(SkillData pSkillData)
    {
        _pSkillData = pSkillData;
    }

    
}
