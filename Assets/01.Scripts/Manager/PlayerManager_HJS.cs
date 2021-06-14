using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommonData;

public class PlayerManager_HJS : MonoSingleton<PlayerManager_HJS>
{
    const float const_fFire_Bullet_Term = 2f;

    public struct MoveJoystickMessage
    {
        /// <summary>
        /// 조이스틱이 바라보는 방향
        /// </summary>
        public Vector2 vecMoveDir;

        public MoveJoystickMessage(Vector2 vecMoveDir)
        {
            this.vecMoveDir = vecMoveDir;
        }
    }

    /// <summary>
    /// 더이상 사용하지 않는 불렛을 반환한다.
    /// </summary>
    public struct ReturnBulletMessage
    {
        public Bullet pBullet;

        public bool bIsAlive;

        public ReturnBulletMessage(Bullet pBullet, bool bIsAlive)
        {
            this.pBullet = pBullet;
            this.bIsAlive = bIsAlive;
        }
    }

    /// <summary>
    /// 선택한 스크롤 정보를 받아서 해당 스크롤의 스킬을 적용한다.
    /// 스크롤 선택 패널에서 선택한 스크롤의 ESkill 값을 전달한다.
    /// </summary>
    public struct ApplyScrollMessage
    {
        public ESkill eSkill;

        public ApplyScrollMessage(ESkill eSkill)
        {
            this.eSkill = eSkill;
        }
    }


    [SerializeField]
    private PlayerCharacter _pOriginal_PlayerCharacter = null;

    [SerializeField]
    private PlayerCharacter _pCur_Character = null;
    private Pooling_Component<PlayerCharacter> _pPool_Character = Pooling_Component<PlayerCharacter>.instance;

    //private EDir _eDir = EDir.Dir_None;

    private float _fCharacter_Move_Speed = 0f;

    private WaitForSeconds _ws_Fire_Bullet_Term;

    [GetComponentInChildren]
    private Bullet _pOriginal_Bullet;

    private Pooling_Component<Bullet> _pPool_Bullet = Pooling_Component<Bullet>.instance;
    private List<Bullet> _list_Cur_Using_Bullet = new List<Bullet>();

    /// <summary>
    /// 현재 보유중인 스킬과 스킬레벨을 저장하는 딕셔너리
    /// </summary>
    private Dictionary<ESkill, int> _map_Cur_Get_Skill = new Dictionary<ESkill, int>();

    //private Pooling_Component<Transform> _pPool_Skill = Pooling_Component<Transform>.instance;
    //private Pooling_Component<SkillBase> _pPool_Skill = Pooling_Component<SkillBase>.instance;

    [GetComponentInChildren]
    private Skill_Fairy _pSkill_Fairy = null;

    [GetComponentInChildren]
    private Skill_Meteor _pSkill_Meteor = null;

    /// <summary>
    /// 현재 타겟의 위치를 멤버 변수에 저장한다.
    /// </summary>
    private Vector2 _vec_CurTarget = Vector2.zero;

    public Observer_Pattern<MoveJoystickMessage> OnMove_Stick { get; private set; } = Observer_Pattern<MoveJoystickMessage>.instance;

    public Observer_Pattern<ReturnBulletMessage> OnReturn_Bullet { get; private set; } = Observer_Pattern<ReturnBulletMessage>.instance;

    public Observer_Pattern<ApplyScrollMessage> OnApply_Scroll { get; private set; } = Observer_Pattern<ApplyScrollMessage>.instance;



    //protected override void OnAwake()
    //{
    //    if (null == _list_Skill_Fairy)
    //    {
    //        DebugLogManager.Log("PlayerManager_HJS - _list_Skill_Fairy 가 널이다");
    //        _list_Skill_Fairy = new List<Skill_Fairy>();
    //    }

    //    if (0 >= _list_Skill_Fairy.Count)
    //    {
    //        DebugLogManager.Log("PlayerManager_HJS - _list_Skill_Fairy 의 카운트가 0보다 작거나 같다");

    //        return;
    //    }
        
    //}


    protected override IEnumerator OnEnableCoroutine()
    {
        yield return base.OnEnableCoroutine();

        while (!DataManager.bIsLoaded_AllResource)
        {
            yield return null;
        }


        if (null == _list_Cur_Using_Bullet)
        {
            _list_Cur_Using_Bullet = new List<Bullet>();
        }

        if (null != _pOriginal_Bullet)
        {
            //_pOriginal_Bullet = GetComponentInChildren<Bullet>();
            _pOriginal_Bullet.SetActive(false);
        }

        //_ws_Fire_Bullet_Term = new WaitForSeconds(const_fFire_Bullet_Term);
        

        OnMove_Stick.Subscribe += OnMove_Stick_Func;
        OnReturn_Bullet.Subscribe += OnReturn_Bullet_Message;

        OnApply_Scroll.Subscribe += OnApply_Scroll_Func;
    }

    public PlayerCharacter DoGet_Cur_Player_Character()
    {
        return _pCur_Character;
    }

    public Vector2 DoGet_Cur_Player_WorldPos()
    {
        return _pCur_Character.transform.position;
    }

    public float DoGet_Player_Move_Speed()
    {
        return _fCharacter_Move_Speed;
    }

    public Vector3 DoGet_Pos_Magic_Fire()
    {
        return _pCur_Character.DoGet_Pos_Magic_Fire();
    }

    private void OnDestroy()
    {
        OnMove_Stick.DoRemove_All_Observer();
        OnReturn_Bullet.DoRemove_All_Observer();
        OnApply_Scroll.DoRemove_All_Observer();
    }

    public void DoInit()
    {
        _fCharacter_Move_Speed = 4f;

        _pPool_Character.DoInit_Pool(_pOriginal_PlayerCharacter);
        if (null == _pCur_Character)
        {
            if (null != _pOriginal_PlayerCharacter)
            {
                //_pCur_Character = Instantiate<PlayerCharacter>(_pOriginal_PlayerCharacter);
                _pCur_Character = _pPool_Character.DoPop();
                _pCur_Character.SetActive(true);
                _pCur_Character.transform.localPosition = Vector2.zero;

                _pCur_Character.transform.SetParent(transform);

                _pOriginal_PlayerCharacter.SetActive(false);
            }
        }

        _pCur_Character.DoInit();

        _pPool_Bullet.DoInit_Pool(_pOriginal_Bullet);
        //SkillBase pOriginal_Skill = ESkill.Skill_Summon_Fairy.GetSkillData().pFilePrefab.GetComponentInChildren<SkillBase>();
        //_pPool_Skill.DoInit_Pool(pOriginal_Skill);

        if (_map_Cur_Get_Skill.Count <= 0)
        {
            for (ESkill eSkill = ESkill.Skill_Summon_Fairy; eSkill <= ESkill.Skill_Shield; ++eSkill)
            {
                _map_Cur_Get_Skill.Add(eSkill, 0);
            }
        }
        else
        {
            for (ESkill eSkill = ESkill.Skill_Summon_Fairy; eSkill <= ESkill.Skill_Shield; ++eSkill)
            {
                _map_Cur_Get_Skill[eSkill] = 0;
            }
        }

        _pSkill_Fairy.DoAwake();
        _pSkill_Fairy.SetActive(false);

        _pSkill_Meteor.DoAwake();
        _pSkill_Meteor.SetActive(false);

        _ws_Fire_Bullet_Term = new WaitForSeconds(EGlobalKey_float.불렛_기본_발사_속도.Getfloat());

        StartCoroutine(nameof(OnCoroutine_Fire_Bullet));
    }

    private void OnMove_Stick_Func(MoveJoystickMessage pMessage)
    {
        Vector2 vecMoveDir = pMessage.vecMoveDir;

        if (pMessage.vecMoveDir == Vector2.zero)
        {
            _pCur_Character.DoPlay_IdleAnim();
            _pCur_Character.DoStop();
        }
        else
        {
            _pCur_Character.DoPlay_WalkAnim();
            if (pMessage.vecMoveDir.x > 0)
            {
                _pCur_Character.DoChange_Dir(EDir.Dir_Right);
            }
            else
            {
                _pCur_Character.DoChange_Dir(EDir.Dir_Left);
            }
            _pCur_Character.DoMove(vecMoveDir);
        }
    }

    /// <summary>
    /// 주기적으로 불렛을 발사하는 코루틴 함수
    /// </summary>
    /// <returns></returns>
    private IEnumerator OnCoroutine_Fire_Bullet()
    {
        while (true)
        {
            yield return _ws_Fire_Bullet_Term;

            var pNewBullet = _pPool_Bullet.DoPop(_pOriginal_Bullet);
            pNewBullet.SetActive(true);
            pNewBullet.transform.SetParent(transform);
            //pNewBullet.transform.position = _pCur_Character.transform.position + new Vector3(0, 0.5f, 0);

            _list_Cur_Using_Bullet.Add(pNewBullet);

            //Vector2 vecTargetPos = Vector2.one;
            //if (null == EnemyManager.instance.DoGet_Enemy_Near_By_Player())
            //{
            //    vecTargetPos = new Vector2(Random.Range(-2f, 2f), Random.Range(-2f, 2f)).normalized * 12f;
            //}
            //else
            //{
            //    vecTargetPos = EnemyManager.instance.DoGet_Enemy_Near_By_Player().transform.position;
            //}
            //DebugLogManager.Log($"Target Pos : {vecTargetPos}");
            
            _pCur_Character.DoPlay_AttackMagicAnim();

            pNewBullet.DoFire();

            //if (vecTargetPos.x > _pCur_Character.transform.position.x)
            //    _pCur_Character.DoLook_Fire_Dir(EDir.Dir_Right);
            //else
            //    _pCur_Character.DoLook_Fire_Dir(EDir.Dir_Left);
        }
    }

    public void DoLook_Fire_Dir()
    {
        Vector2 vecTargetPos = EnemyManager.instance.DoGet_Enemy_Near_By_Player().transform.position;

        if (vecTargetPos.x > _pCur_Character.transform.position.x)
            _pCur_Character.DoLook_Fire_Dir(EDir.Dir_Right);
        else
            _pCur_Character.DoLook_Fire_Dir(EDir.Dir_Left);
    }

    public int DoGet_Skill_Lv(ESkill eSkill)
    {
        return _map_Cur_Get_Skill[eSkill];
    }


    private void OnReturn_Bullet_Message(ReturnBulletMessage pMessage)
    {
        if (!pMessage.bIsAlive)
        {
            var pReturnBullet = pMessage.pBullet;

            for (int i = 0; i < _list_Cur_Using_Bullet.Count; ++i)
            {
                if (_list_Cur_Using_Bullet[i].transform == pReturnBullet.transform)
                {
                    _list_Cur_Using_Bullet[i].SetActive(false);
                    
                    _pPool_Bullet.DoPush(_list_Cur_Using_Bullet[i]);
                    _list_Cur_Using_Bullet.Remove(_list_Cur_Using_Bullet[i]);
                    break;
                }
            }
        }
    }


    private void OnApply_Scroll_Func(ApplyScrollMessage pMessage)
    {
        ESkill eSkill = pMessage.eSkill;
        SkillData pSkillData = eSkill.GetSkillData();

        //스킬 레벨 업
        _map_Cur_Get_Skill[eSkill]++;
        if (_map_Cur_Get_Skill[eSkill] >= EGlobalKey_int.스킬_최대_레벨.Getint())
        {
            _map_Cur_Get_Skill[eSkill] = EGlobalKey_int.스킬_최대_레벨.Getint();
        }

        if (null != pSkillData)
        { 
            switch (eSkill)
            {
                case ESkill.Skill_Summon_Fairy:
                    if (1 == _map_Cur_Get_Skill[eSkill])
                    {
                        _pSkill_Fairy.SetActive(true);
                        _pSkill_Fairy.DoInit(pSkillData);
                    }
                    else
                    {
                        _pSkill_Fairy.DoUpgrade(_map_Cur_Get_Skill[eSkill]);
                    }
                    break;

                case ESkill.Skill_Meteor:
                    if (1 == _map_Cur_Get_Skill[eSkill])
                    {
                        _pSkill_Meteor.SetActive(true);
                        _pSkill_Meteor.DoInit(pSkillData);
                    }
                    else
                    {
                        _pSkill_Meteor.DoUpgrade(_map_Cur_Get_Skill[eSkill]);
                    }
                    break;

                default:
                    break;
            }
        }
        /*
        bool bIsExist = _map_Cur_Get_Skill.ContainsKey(pMessage.eSkill);
        if (!bIsExist)
        {
            SkillData pSkillData = pMessage.eSkill.GetSkillData();
            if (null != pSkillData)
            {
                
                Transform pTransform_Skill = pSkillData.pFilePrefab;
                if (null != pTransform_Skill)
                {
                    var pNewSkill = _pPool_Skill.DoPop();
                    //pNewTransform = pSkillData.pFilePrefab;
                    pNewSkill.transform.SetParent(transform);

                    //Skill_Fairy pSkillFairy = pNewTransform.GetComponentInChildren<Skill_Fairy>();
                    //if (null != pSkillFairy)
                    //{
                    //    pSkillFairy.SetActive(true);
                    //    pSkillFairy.DoInit(pSkillData);
                    //}

                    SkillBase pSkillBase = pNewSkill.GetComponentInChildren<SkillBase>();
                    if (null != pSkillBase)
                    {
                        pSkillBase.SetActive(true);
                        pSkillBase.DoAwake();
                        pSkillBase.DoInit(pSkillData);
                    }
                }
                
            }

        }
        */
    }
}
