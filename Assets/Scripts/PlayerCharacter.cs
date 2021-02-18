using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommonData;

public class PlayerCharacter : ObjectBase
{
    private Animator _pAnim = null;

    private Transform _pSprite_Transform = null;


    protected override void OnAwake()
    {
        base.OnAwake();
        if (null == _pAnim)
        {
            _pAnim = GetComponentInChildren<Animator>(true);
            _pSprite_Transform = _pAnim.transform;
        }
    }

    public void DoPlay_WalkAnim()
    {
        if (false == IsExist_Anim())
            return;

        _pAnim.SetBool("isWalk", true);
    }

    public void DoPlay_IdleAnim()
    {
        if (false == IsExist_Anim())
            return;

        _pAnim.SetBool("isWalk", false);
    }

    public void DoChange_Dir(EDir eDir)
    {
        //DebugLogManager.Log($"Local scale X : {transform.localScale.x}");
        transform.localScale = new Vector3((int)eDir, 1, 1);
    }

    private bool IsExist_Anim()
    {
        if (null != _pAnim)
            return true;
        else
        {
            DebugLogManager.LogError("There's no Animator Component");
            return false;
        }
    }

    

}
