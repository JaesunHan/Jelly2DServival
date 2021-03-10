using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommonData;

public class ManaManager : MonoSingleton<ManaManager>
{
    private int iCurWave = 0;

    public struct ReturnManaPotionMessage
    {
        public ManaPotionBase pManaPotionBase;

        public bool bIsAlive;

        public ReturnManaPotionMessage(ManaPotionBase pManaPotionBase, bool bIsAlive)
        {
            this.pManaPotionBase = pManaPotionBase;
            this.bIsAlive = bIsAlive;
        }
    }

    /// <summary>
    /// 복제할 오리지널 마나 프리팹
    /// </summary>
    [GetComponentInChildren]
    private ManaPotionBase _pOriginal_ManaPotionBase = null;

    /// <summary>
    /// 현재 맵에 배치되어 있는 마나 포션들의 리스트
    /// </summary>
    List<ManaPotionBase> _list_Alive_ManaPotion = new List<ManaPotionBase>();

    /// <summary>
    /// 마나포션의 오브젝트 풀
    /// </summary>
    Pooling_Component<ManaPotionBase> _pPool_ManaPotion = Pooling_Component<ManaPotionBase>.instance;

    public Observer_Pattern<ReturnManaPotionMessage> OnReturn_ManaPotion { get; private set; } = Observer_Pattern<ReturnManaPotionMessage>.instance;

    protected override void OnAwake()
    {
        base.OnAwake();
    }

    private void OnDestroy()
    {
        OnReturn_ManaPotion.Subscribe -= OnReturn_ManaPotion_Func;
    }

    public void DoInit()
    {
        iCurWave = 0;

        _pOriginal_ManaPotionBase.SetActive(false);
        _pPool_ManaPotion.DoInit_Pool(_pOriginal_ManaPotionBase);

        for (int i = 0; i < _list_Alive_ManaPotion.Count; ++i)
        {
            _list_Alive_ManaPotion[i].SetActive(false);

            _pPool_ManaPotion.DoPush(_list_Alive_ManaPotion[i]);
        }

        _list_Alive_ManaPotion.Clear();

        OnReturn_ManaPotion.Subscribe += OnReturn_ManaPotion_Func;

        StartCoroutine(nameof(OnCoroutine_Respawn_ManaPotion));
    }

    /// <summary>
    /// 주기적으로 마나 포션을 리스폰한다.
    /// </summary>
    /// <returns></returns>
    private IEnumerator OnCoroutine_Respawn_ManaPotion()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);

            iCurWave = StageManager.instance.iCurWave;

            for (int i = 0; i < 2; ++i)
            {
                int iRandom = Random.Range(0, 2);
                EDir eDir = EDir.Dir_Left;
                if (iRandom == 0) eDir = EDir.Dir_Left; 
                else eDir = EDir.Dir_Right;


                ManaPotionData pNewMPData = DataManager.DoGet_Random_ManaPotionData_ByStageWave(iCurWave);
                var pNewPotion = _pPool_ManaPotion.DoPop(_pOriginal_ManaPotionBase);
                pNewPotion.DoAwake();
                pNewPotion.DoInit(pNewMPData);
                pNewPotion.SetActive(true);
                pNewPotion.transform.SetParent(transform);
                
                Vector3 vecSpawnPos = PlayerManager_HJS.instance.DoGet_Cur_Player_WorldPos();
                vecSpawnPos.x += Random.Range(14f * (int)eDir, 8f * (int)eDir);
                vecSpawnPos.y += Random.Range(12f * (int)eDir, 6f * (int)eDir);

                pNewPotion.transform.position = vecSpawnPos;

                _list_Alive_ManaPotion.Add(pNewPotion);
            }
            
        }
    }

    private void OnReturn_ManaPotion_Func(ReturnManaPotionMessage pMessage)
    {
        if (!pMessage.bIsAlive)
        {
            var pReturnManaPotion = pMessage.pManaPotionBase;

            for (int i = 0; i < _list_Alive_ManaPotion.Count; ++i)
            {
                if (_list_Alive_ManaPotion[i].transform == pReturnManaPotion.transform)
                {
                    _list_Alive_ManaPotion[i].SetActive(false);

                    _pPool_ManaPotion.DoPush(_list_Alive_ManaPotion[i]);
                    _list_Alive_ManaPotion.Remove(_list_Alive_ManaPotion[i]);
                }
            }
        }
    }
}
