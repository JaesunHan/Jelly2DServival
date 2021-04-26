using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : ObjectBase
{
    protected override void OnAwake()
    {
        base.OnAwake();

        if (null == StageManager.instance)
        {
            DebugLogManager.Log("StageManager 가 null 이다");
        }

        if (null == PlayerManager_HJS.instance)
        {
            DebugLogManager.Log("PlayerManager_HJS 가 null 이다");
        }

        if (null == EnemyManager.instance)
        {
            DebugLogManager.Log("EnemyManager 가 null 이다");
        }

        if (null == ManaManager.instance)
        {
            DebugLogManager.Log("ManaManager 가 null 이다");
        }


    }

    private void Start()
    {
        Init();
    }

    /// <summary>
    /// 모든 싱글톤 매니저들을 초기화한다. 
    /// 주의 : 초기화 순서는 절대로 변경하지 말것!
    /// </summary>
    private void Init()
    {
        DataManager.instance.DoAwake();

        StartCoroutine(nameof(OnCoroutine_Wait_DataManager_Init));
    }

    /// <summary>
    /// 데이터 매니저가 초기화 될때까지 기다린다.
    /// </summary>
    /// <returns></returns>
    private IEnumerator OnCoroutine_Wait_DataManager_Init()
    {
        while (!DataManager.bIsLoaded_AllResource)
        {
            //DebugLogManager.Log("데이터 매니저 초기화가 완료될때까지 기다린다.");

            yield return null;
        }

        StageManager.instance.DoAwake();
        StageManager.instance.DoInit();

        PlayerManager_HJS.instance.DoAwake();
        PlayerManager_HJS.instance.DoInit();

        EnemyManager.instance.DoAwake();
        EnemyManager.instance.DoInit();

        ManaManager.instance.DoAwake();
        ManaManager.instance.DoInit();
    }
}
