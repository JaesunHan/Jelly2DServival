using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommonData;

public class ManaManager : MonoSingleton<ManaManager>
{
    private int iCurWave = 0;

    /// <summary>
    /// 게임을 처을 시작하면 최대 마나 게이지는 100으로 시작한다.
    /// 100 다 채우고 나면 다음은 200 -> 400 -> 800 -> 1600 -> ... 으로 늘어난다.
    /// </summary>
    const float const_fDefault_MP = 100;

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
    /// 마나를 획득했을 때 발생하는 메시지이다.
    /// </summary>
    public struct GetMPMessage
    {
        public float fGetMP;

        public GetMPMessage(float fGetMP)
        {
            this.fGetMP = fGetMP;
        }
    }

    /// <summary>
    /// 최종 MP 가 변경되면, 현재 MP와 Maximum MP 값을 메시지로 전송한다.
    /// </summary>
    public struct ChangeMPMessage
    {
        public float fCurMP;

        public float fMaxMP;

        public ChangeMPMessage(float fCurMP, float fMaxMP)
        {
            this.fCurMP = fCurMP;
            this.fMaxMP = fMaxMP;
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

    private float _fMax_MP = 0;
    private float _fCur_MP = 0;

    public float fCur_MP
    {
        get { return _fCur_MP; }
    }

    public Observer_Pattern<ReturnManaPotionMessage> OnReturn_ManaPotion { get; private set; } = Observer_Pattern<ReturnManaPotionMessage>.instance;
    /// <summary>
    /// 마나를 획득했을 때의 옵저버패턴이다.
    /// </summary>
    public Observer_Pattern<GetMPMessage> OnGet_MP { get; private set; } = Observer_Pattern<GetMPMessage>.instance;
    /// <summary>
    /// 마나 포인트 값이 변경되었는지를 체크하는 옵저버패턴이다.
    /// </summary>
    public Observer_Pattern<ChangeMPMessage> OnChange_MP { get; private set; } = Observer_Pattern<ChangeMPMessage>.instance;

    protected override void OnAwake()
    {
        base.OnAwake();
        
    }

    private void OnDestroy()
    {
        OnGet_MP.DoRemove_All_Observer();
        OnReturn_ManaPotion.DoRemove_All_Observer();
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

        _fCur_MP = 0;
        _fMax_MP = EGlobalKey_float.최대_MP_기본값.Getfloat();

        OnReturn_ManaPotion.Subscribe += OnReturn_ManaPotion_Func;
        OnGet_MP.Subscribe += OnGet_MP_Func;

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
                
                Vector3 vecSpawnPos = PlayerManager_HJS.instance.DoGet_Cur_Player_WorldPos();
                vecSpawnPos.x += Random.Range(14f * (int)eDir, 8f * (int)eDir);
                vecSpawnPos.y += Random.Range(12f * (int)eDir, 6f * (int)eDir);

                pNewPotion.transform.position = vecSpawnPos;

                pNewPotion.transform.SetParent(transform);

                _list_Alive_ManaPotion.Add(pNewPotion);
                DebugLogManager.Log($"마나 포션 생성 위치 : {vecSpawnPos}");
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


    private void OnGet_MP_Func(GetMPMessage pMessage)
    {
        _fCur_MP = _fCur_MP + pMessage.fGetMP;

        if (_fCur_MP >= _fMax_MP)
        {
            _fCur_MP = _fCur_MP - _fMax_MP;
            if (0 == _fCur_MP)
                _fCur_MP = 1;

            _fMax_MP = _fMax_MP * EGlobalKey_float.최대_MP_값_증가_비율.Getfloat();

            //스킬 선택 패널 띄우기
            IngameUIManager.instance.DoShowPanel(IngameUIManager.EPanel.Panel_Select_Scroll);
        }

        OnChange_MP.DoNotify(new ChangeMPMessage(_fCur_MP, _fMax_MP));
    }
}
