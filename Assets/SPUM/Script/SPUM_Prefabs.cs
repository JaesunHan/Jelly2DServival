using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommonData;

public class SPUM_Prefabs : MonoBehaviour
{
    public float _version;
    public SPUM_SpriteList _spriteOBj;
    public bool EditChk;
    public string _code;
    public Animator _anim;
    public EPlayerState ePlayerState { get; private set; }

    public void PlayAnimation (int num)
    {
        switch(num)
        {
            case 0: //Idle
                ePlayerState = EPlayerState.Idle;
            _anim.SetFloat("RunState",0f);
            break;

            case 1: //Run
                ePlayerState = EPlayerState.Run;
                _anim.SetFloat("RunState",0.5f);
            break;

            case 2: //Death
                ePlayerState = EPlayerState.Death;
                _anim.SetTrigger("Die");
            _anim.SetBool("EditChk",EditChk);
            break;

            case 3: //Stun
                ePlayerState = EPlayerState.Stun;
                _anim.SetFloat("RunState",1.0f);
            break;

            case 4: //Attack Sword
                ePlayerState = EPlayerState.Attack_Sword;
                _anim.SetTrigger("Attack");
            _anim.SetFloat("AttackState",0.0f);
            _anim.SetFloat("NormalState",0.0f);
            break;

            case 5: //Attack Bow
                ePlayerState = EPlayerState.Attack_Bow;
                _anim.SetTrigger("Attack");
            _anim.SetFloat("AttackState",0.0f);
            _anim.SetFloat("NormalState",0.5f);
            break;

            case 6: //Attack Magic
                ePlayerState = EPlayerState.Attack_Magic;
                _anim.SetFloat("AttackSpeed", 0.5f);
            _anim.SetTrigger("Attack");
            _anim.SetFloat("AttackState",0.0f);
            _anim.SetFloat("NormalState",1.0f);
            break;

            case 7: //Skill Sword
                ePlayerState = EPlayerState.Skill_Sword;
                _anim.SetTrigger("Attack");
            _anim.SetFloat("AttackState",1.0f);
            _anim.SetFloat("SkillState",0.0f);
            break;

            case 8: //Skill Bow
                ePlayerState = EPlayerState.Skill_Bow;
                _anim.SetTrigger("Attack");
            _anim.SetFloat("AttackState",1.0f);
            _anim.SetFloat("SkillState",0.5f);
            break;

            case 9: //Skill Magic
                ePlayerState = EPlayerState.Skill_Magic;
                _anim.SetTrigger("Attack");
            _anim.SetFloat("AttackState",1.0f);
            _anim.SetFloat("SkillState",1.0f);
            break;
        }
    }
}
