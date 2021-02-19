using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoSingleton<EnemyManager>
{
    const int const_iMax_Pool_Count = 50;

    /// <summary>
    /// 현재 맵에 배치된 적들을 저장하는 리스트
    /// </summary>
    private List<EnemyData> _list_Transform_Enemy = new List<EnemyData>();

    protected override void OnAwake()
    {
        base.OnAwake();


    }

    public void DoInit()
    { 
    
    }

    private void Create_Pool()
    {
        for (int i = 0; i < const_iMax_Pool_Count; ++i)
        {
            //Transform pNewTransform = Instantiate();
        }
    }
}
