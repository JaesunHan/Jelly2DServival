using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoSingleton<EnemyManager>
{
    const int const_iDefault_EnemyCount = 10;

    /// <summary>
    /// 기본적으로 2초에 하나씩 적이 생성된다.
    /// </summary>
    const float const_fDefault_Respawn_Term_Time = 1.5f;

    public struct ReturnEnemyMessage
    {
        public EnemyBase pEnemyBase;

        public bool bIsAlive;

        public ReturnEnemyMessage(EnemyBase pEnemyBase, bool bIsAlive)
        {
            this.pEnemyBase = pEnemyBase;
            this.bIsAlive = bIsAlive;
        }
    }

    public Observer_Pattern<ReturnEnemyMessage> OnReturn_Enemy { get; private set; } = Observer_Pattern<ReturnEnemyMessage>.instance;

    /// <summary>
    /// 오브젝트 풀
    /// </summary>
    private Pooling_Component<EnemyBase> _pPool_EnemyBase = Pooling_Component<EnemyBase>.instance;
    [SerializeField]
    private EnemyBase _pEnemyBase_Original = null;
    /// <summary>
    /// 현재 맵에 배치된 적들을 저장하는 리스트
    /// </summary>
    private List<EnemyBase> _list_Enemy = new List<EnemyBase>();

    private int iCurWave = 0;

    //[SerializeField]
    private List<Transform> _list_Respawn_Point = new List<Transform>();

    ///// <summary>
    ///// 플레이어가 움직이는 방향의 반대방향으로 적들을 이동시키기 위한 부모 트랜스폼
    ///// </summary>
    //[SerializeField]
    //private Transform _pTransform_MovingGroup = null;

    private bool bIsPlaying = false;

    private WaitForSeconds _ws_Respawn_Term;

    private Vector2 _vecJoystic_Move_Dir = Vector2.zero;

    //private float _fPlayer_Move_Speed = 0f;
    

    protected override void OnAwake()
    {
        base.OnAwake();

        iCurWave = 0;
        if(null != _pEnemyBase_Original)
            _pEnemyBase_Original.SetActive(false);

        StartCoroutine(nameof(OnCoroutine_Awake));
    }

    private IEnumerator OnCoroutine_Awake()
    {
        if (null == DataManager.instance)
        {
            DebugLogManager.Log("데이터 매니저의 인스턴스가 널이다.");
        }

        while (!DataManager.bIsLoaded_AllResource)
        {
            yield return null;
        }

        DebugLogManager.Log("리소스 로드 완료");
        if (null == _pEnemyBase_Original)
        {
            var pLoad = BundleLoadManager.instance.DoLoad<Transform>("Prefabs", "Enemy/Enemy_Jelly.prefab");
            if (null != pLoad)
            {
                _pEnemyBase_Original = pLoad.GetComponent<EnemyBase>();
            }
        }
        
        _pPool_EnemyBase.DoInit_Pool(_pEnemyBase_Original);

        if (_list_Respawn_Point.Count <= 0)
        {
            var arrRespawnPoint = GetComponentsInChildren<Transform>();
            for (int i = 0; i < arrRespawnPoint.Length; ++i)
            {
                if(arrRespawnPoint[i].name.Contains("RespawnPoint"))
                    _list_Respawn_Point.Add(arrRespawnPoint[i]);
            }
        }

        //DoInit();
    }

    public void DoInit()
    {
        StopAllCoroutines();
        _ws_Respawn_Term = new WaitForSeconds(EGlobalKey_float.적_리스폰_주기.Getfloat());//const_fDefault_Respawn_Term_Time

        //처음 시작할 때 웨이브는 0부터 시작한다. 
        iCurWave = 0;

        //임시로 Init과 동시에 true 로 변경한다.  
        bIsPlaying = true;

        //_fPlayer_Move_Speed = PlayerManager_HJS.instance.DoGet_Player_Move_Speed();

        StartGame();
        AddSubScribe();

        StageManager.instance.OnUpgrade_Wave.Subscribe += OnUpgrade_Wave_Func;
    }

    /// <summary>
    /// 현재 살아있는 에너미중에서 플레이어 캐릭터랑 가장 가까이 있는 적의 정보를 반환한다.
    /// </summary>
    /// <returns></returns>
    public EnemyBase DoGet_Enemy_Near_By_Player()
    {
        EnemyBase pEnemyBase = null;

        float fMinDist = 100f;
        Vector2 vecPlayerPos = PlayerManager_HJS.instance.DoGet_Cur_Player_WorldPos();

        for (int i = 0; i < _list_Enemy.Count; ++i)
        {
            if (_list_Enemy[i].gameObject.activeSelf)
            {
                Vector2 vecPos = _list_Enemy[i].transform.position;
                float fDist = (vecPlayerPos - vecPos).magnitude;

                if (fDist < fMinDist)
                {
                    fMinDist = fDist;
                    //DebugLogManager.Log($"fMinDist : {fMinDist} / fDist : {fDist}");
                    pEnemyBase = _list_Enemy[i];
                    //DebugLogManager.Log($"이름 : {pEnemyBase.name}");
                }
            }
        }

        return pEnemyBase;
    }

    private void AddSubScribe()
    {
        //PlayerManager.instance.OnMove_Stick.Subscribe += OnMove_Stick_Func;
        OnReturn_Enemy.Subscribe += OnReturn_Enemy_Func;
        PlayerManager_HJS.instance.OnMove_Stick.Subscribe += OnJoystick_Move_Func;
    }

    private void OnDestroy()
    {
        OnReturn_Enemy.DoRemove_All_Observer();
    }

    private void StartGame()
    {
        StartCoroutine(nameof(OnCoroutine_Spawn_Enemy));
    }

    private IEnumerator OnCoroutine_Spawn_Enemy()
    {
        while (bIsPlaying)
        {
            //int iRandomIdx = Random.Range(0, _list_Respawn_Point.Count);
            //Transform pTransform_Random_Point = _list_Respawn_Point[iRandomIdx];

            //Vector3 vecRespawnPosition = PlayerManager.instance.DoGet_Cur_Player_WorldPos() +  _vecJoystic_Move_Dir.normalized * 50f;
            if (_vecJoystic_Move_Dir == Vector2.zero)
                _vecJoystic_Move_Dir = Vector2.one;

            int iRandomRangeX = Random.Range(5, 13); // 10, 17
            int iRandomRangeY = Random.Range(8, 10); // 9, 11

            int iRandomDir = Random.Range(-10, 10);
            if (0 >= iRandomDir)
                iRandomDir = -1;
            else
                iRandomDir = 1;
            Vector3 vecRespawnPosition = PlayerManager_HJS.instance.DoGet_Cur_Player_WorldPos() + new Vector2(_vecJoystic_Move_Dir.x *iRandomRangeX* iRandomDir, _vecJoystic_Move_Dir.y* iRandomDir * iRandomRangeY);

            
            EnemyBase pEnemyBase_Spawn = GetEnemyBase_In_Pool();
            pEnemyBase_Spawn.DoAwake();
            //현재 사용하고 있지 않은 에너미 베이스를 랜덤 리스폰 포인트에 배치한다.
            pEnemyBase_Spawn.transform.position = vecRespawnPosition;
            pEnemyBase_Spawn.SetActive(true);

            //배치되는 에너미의 정보값을 세팅한다. (현재 웨이브에서 등장할 수 있는 에너미들 중에서 랜덤으로 선택한다.)
            var pEnemyData = DataManager.DoGet_Random_EnemyDatas_ByStageWave(iCurWave);
            pEnemyBase_Spawn.DoInit(pEnemyData);

            yield return _ws_Respawn_Term;
        }
    }

    /// <summary>
    /// 현재 _list_Transform_Enemy 에 있는 에너미 베이스 중에서 액티브가 꺼져있는 것을 반환한다.
    /// 없으면 Pool 에서 꺼내서 반환한다.
    /// </summary>
    /// <returns></returns>
    private EnemyBase GetEnemyBase_In_Pool()
    {
        var pEnemyBaseClone = _pPool_EnemyBase.DoPop();
        pEnemyBaseClone.SetActive(false);
        pEnemyBaseClone.transform.SetParent(transform); //_pTransform_MovingGroup

        _list_Enemy.Add(pEnemyBaseClone);

        return pEnemyBaseClone;
    }

    private void OnReturn_Enemy_Func(ReturnEnemyMessage pMessage)
    {
        if (!pMessage.bIsAlive)
        {
            var pReturnEnemy = pMessage.pEnemyBase;

            for (int i = 0; i < _list_Enemy.Count; ++i)
            {
                if (_list_Enemy[i].transform == pReturnEnemy.transform)
                {
                    //_list_Enemy[i].SetActive(false);
                    //_list_Enemy[i].DoDisappear();
                    
                    _pPool_EnemyBase.DoPush(_list_Enemy[i]);
                    _list_Enemy.Remove(_list_Enemy[i]);
                    break;
                }
            }
        }
    }

    private void OnJoystick_Move_Func(PlayerManager_HJS.MoveJoystickMessage pMessage)
    {
        _vecJoystic_Move_Dir = pMessage.vecMoveDir;
    }

    private void OnUpgrade_Wave_Func(StageManager.UpgradeWaveMessage pMessage)
    {
        iCurWave = pMessage.iCurWave;

        float fTerm = EGlobalKey_float.적_리스폰_주기.Getfloat() - iCurWave * 0.2f;

        if (fTerm <= 0.8f)
            fTerm = 0.8f;

        _ws_Respawn_Term = new WaitForSeconds(fTerm);
    }
    
}
