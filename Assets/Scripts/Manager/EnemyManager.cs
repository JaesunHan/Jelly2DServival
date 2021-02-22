using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoSingleton<EnemyManager>
{
    const int const_iDefault_EnemCount = 10;

    /// <summary>
    /// 기본적으로 2초에 하나씩 적이 생성된다.
    /// </summary>
    const float const_fDefault_Respawn_Term_Time = 2f;

    /// <summary>
    /// 오브젝트 풀
    /// </summary>
    private PoolingManager_Component<EnemyBase> _pPool_EnemyData = PoolingManager_Component<EnemyBase>.instance;
    [SerializeField]
    private EnemyBase _pEnemyBase_Original = null;
    /// <summary>
    /// 현재 맵에 배치된 적들을 저장하는 리스트
    /// </summary>
    private List<EnemyBase> _list_Transform_Enemy = new List<EnemyBase>();

    private int iCurWave = 0;

    [SerializeField]
    private List<Transform> _list_Respawn_Point = new List<Transform>();

    /// <summary>
    /// 플레이어가 움직이는 방향의 반대방향으로 적들을 이동시키기 위한 부모 트랜스폼
    /// </summary>
    [SerializeField]
    private Transform _pTransform_MovingGroup = null;

    private bool bIsPlaying = false;

    private WaitForSeconds _ws_Respawn_Term;

    private Vector2 _vecJoystic_Move_Dir = Vector2.zero;

    private float _fPlayer_Move_Speed = 0f;

    protected override void OnAwake()
    {
        base.OnAwake();

        iCurWave = 0;
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
        
        _pPool_EnemyData.DoInit_Pool(_pEnemyBase_Original);

        if (_list_Respawn_Point.Count <= 0)
        {
            var arrRespawnPoint = GetComponentsInChildren<Transform>();
            for (int i = 0; i < arrRespawnPoint.Length; ++i)
            {
                if(arrRespawnPoint[i].name.Contains("RespawnPoint"))
                    _list_Respawn_Point.Add(arrRespawnPoint[i]);
            }
        }

        DoInit();
    }

    public void DoInit()
    {
        StopAllCoroutines();
        _ws_Respawn_Term = new WaitForSeconds(const_fDefault_Respawn_Term_Time);

        //처음 시작할 때 웨이브는 0부터 시작한다. 
        iCurWave = 0;

        if (_list_Transform_Enemy.Count <= 0)
        {
            Create_List();
        }
        //임시로 Init과 동시에 true 로 변경한다.  
        bIsPlaying = true;

        _fPlayer_Move_Speed = PlayerManager.instance.DoGet_Player_Move_Speed();

        StartGame();
        AddSubScribe();
    }

    private void AddSubScribe()
    {
        PlayerManager.instance.OnMove_Stick.Subscribe += OnMove_Stick_Func;
    }

    private void OnDestroy()
    {
        PlayerManager.instance.OnMove_Stick.Subscribe -= OnMove_Stick_Func;
    }

    private void Create_List()
    {
        if (null == _pEnemyBase_Original)
        {
            DebugLogManager.Log("복제할 오리지널 EnemyBase가 null 이므로 새로 세팅한다.");
        }

        for (int i = 0; i < const_iDefault_EnemCount; ++i)
        {
            //var pEnemyBaseClone = (EnemyBase)_pPool_EnemyData.DoPop(_pEnemyBase_Original);
            //pEnemyBaseClone.SetActive(false);
            //pEnemyBaseClone.transform.SetParent(transform);

            //_list_Transform_Enemy.Add(pEnemyBaseClone);
            Pop_EnemyBase_In_Pool();
        }
    }

    private void StartGame()
    {
        StartCoroutine(nameof(OnCoroutine_Spawn_Enemy));
    }

    private IEnumerator OnCoroutine_Spawn_Enemy()
    {
        while (bIsPlaying)
        {
            int iRandomIdx = Random.Range(0, _list_Respawn_Point.Count);
            Transform pTransform_Random_Point = _list_Respawn_Point[iRandomIdx];

            //현재 사용하고 있지 않은 에너미 베이스를 랜덤 리스폰 포인트에 배치한다.
            EnemyBase pEnemyBase_Spawn = GetNot_Using_EnemyBase();
            pEnemyBase_Spawn.transform.position = pTransform_Random_Point.position;
            pEnemyBase_Spawn.SetActive(true);

            //배치되는 에너미의 정보값을 세팅한다. (현재 웨이브에서 등장할 수 있는 에너미들 중에서 랜덤으로 선택한다.)
            var pEnemyData = DataManager.DoGet_Random_EnemyData_ByStageWave(iCurWave);
            pEnemyBase_Spawn.DoInit(pEnemyData);

            yield return _ws_Respawn_Term;
        }
    }

    /// <summary>
    /// 현재 _list_Transform_Enemy 에 있는 에너미 베이스 중에서 액티브가 꺼져있는 것을 반환한다.
    /// 없으면 Pool 에서 꺼내서 반환한다.
    /// </summary>
    /// <returns></returns>
    private EnemyBase GetNot_Using_EnemyBase()
    {
        EnemyBase pEnemyBase = null;

        for (int i = 0; i < _list_Transform_Enemy.Count; ++i)
        {
            if (!_list_Transform_Enemy[i].gameObject.activeSelf)
            {
                pEnemyBase = _list_Transform_Enemy[i];
                break;
            }
        }

        if (null == pEnemyBase)
        {
            pEnemyBase = Pop_EnemyBase_In_Pool();
        }

        return pEnemyBase;
    }

    /// <summary>
    /// 풀에서 EnemyBase 를 꺼내고, 리스트에 저장하는 코드이다.
    /// </summary>
    /// <returns></returns>
    private EnemyBase Pop_EnemyBase_In_Pool()
    {
        var pEnemyBaseClone = (EnemyBase)_pPool_EnemyData.DoPop(_pEnemyBase_Original);
        pEnemyBaseClone.SetActive(false);
        pEnemyBaseClone.transform.SetParent(_pTransform_MovingGroup);

        _list_Transform_Enemy.Add(pEnemyBaseClone);

        return pEnemyBaseClone;
    }

    private void OnMove_Stick_Func(PlayerManager.MoveJoystickMessage pMessage)
    {
        _vecJoystic_Move_Dir = pMessage.vecMoveDir;

        Vector2 vecMoveDir = pMessage.vecMoveDir * -1f;

        Vector3 vecResult_LocalPos = _pTransform_MovingGroup.transform.localPosition;

        Vector2 vecPos = vecMoveDir * Time.deltaTime * _fPlayer_Move_Speed;

        vecResult_LocalPos = new Vector3(vecResult_LocalPos.x + vecPos.x, vecResult_LocalPos.y + vecPos.y, vecResult_LocalPos.z);

        _pTransform_MovingGroup.transform.localPosition = vecResult_LocalPos;
    }
}
