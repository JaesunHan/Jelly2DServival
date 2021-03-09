using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommonData;

public class PlayerManager_HJS : MonoSingleton<PlayerManager_HJS>
{
    const float const_fFire_Bullet_Term = 1f;

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

    [SerializeField]
    private PlayerCharacter _pOriginal_PlayerCharacter = null;

    [SerializeField]
    private PlayerCharacter _pCur_Character = null;

    //private EDir _eDir = EDir.Dir_None;

    private float _fCharacter_Move_Speed = 0f;

    private WaitForSeconds _ws_Fire_Bullet_Term;

    private Bullet _pOriginal_Bullet = null;

    private Pooling_Component<Bullet> _pPool_Bullet = Pooling_Component<Bullet>.instance;
    private List<Bullet> _list_Cur_Using_Bullet = new List<Bullet>();

    public Observer_Pattern<MoveJoystickMessage> OnMove_Stick { get; private set; } = Observer_Pattern<MoveJoystickMessage>.instance;

    public Observer_Pattern<ReturnBulletMessage> OnReturn_Bullet { get; private set; } = Observer_Pattern<ReturnBulletMessage>.instance;
    //public Observer_Pattern<>
    protected override void OnAwake()
    {
        base.OnAwake();

        if (null == _pCur_Character)
        {
            if (null != _pOriginal_PlayerCharacter)
            {
                _pCur_Character = Instantiate<PlayerCharacter>(_pOriginal_PlayerCharacter);
                _pCur_Character.transform.localPosition = Vector2.zero;

                _pCur_Character.transform.SetParent(transform);

                _pOriginal_PlayerCharacter.SetActive(false);
            }
        }

        if (null == _list_Cur_Using_Bullet)
        {
            _list_Cur_Using_Bullet = new List<Bullet>();
        }
        

        OnMove_Stick.Subscribe += OnMove_Stick_Func;
        OnReturn_Bullet.Subscribe += OnReturn_Bullet_Message;


        _fCharacter_Move_Speed = 3f;

        _ws_Fire_Bullet_Term = new WaitForSeconds(const_fFire_Bullet_Term);

        if (null == _pOriginal_Bullet)
        {
            _pOriginal_Bullet = GetComponentInChildren<Bullet>();
            _pOriginal_Bullet.SetActive(false);
        }

        _pPool_Bullet.DoInit_Pool(_pOriginal_Bullet);
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
        //OnMove_Stick.Subscribe -= OnMove_Stick_Func;
        //OnReturn_Bullet.Subscribe -= OnReturn_Bullet_Message;

        OnMove_Stick.DoRemove_All_Observer();
        OnReturn_Bullet.DoRemove_All_Observer();
    }

    private void Start()
    {
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
            var pNewBullet = _pPool_Bullet.DoPop(_pOriginal_Bullet);
            pNewBullet.SetActive(true);
            pNewBullet.transform.SetParent(transform);
            //pNewBullet.transform.position = _pCur_Character.transform.position + new Vector3(0, 0.5f, 0);

            _list_Cur_Using_Bullet.Add(pNewBullet);

            Vector2 vecTargetPos = Vector2.one;
            if (null == EnemyManager.instance.DoGet_Enemy_Near_By_Player())
            {
                vecTargetPos = new Vector2(Random.Range(-2f, 2f), Random.Range(-2f, 2f)).normalized * 12f;
            }
            else
            {
                vecTargetPos = EnemyManager.instance.DoGet_Enemy_Near_By_Player().transform.position;
            }
            //DebugLogManager.Log($"Target Pos : {vecTargetPos}");
            
            _pCur_Character.DoPlay_AttackMagicAnim();

            pNewBullet.DoFire(vecTargetPos);

            //if (vecTargetPos.x > _pCur_Character.transform.position.x)
            //    _pCur_Character.DoChange_Dir(EDir.Dir_Right);
            //else
            //    _pCur_Character.DoChange_Dir(EDir.Dir_Left);

            yield return _ws_Fire_Bullet_Term;
        }
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

}
