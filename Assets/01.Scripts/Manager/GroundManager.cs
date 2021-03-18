using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundManager : ObjectBase
{
    const int const_iMax_Pool_Count = 50;

    const float const_fDot_Move_Speed = 2f;

    const float const_fCreate_Boundary = 4f;


    ///// <summary>
    ///// 땅에 얼룩 같은 자국들을 미리 리스트에 저장해둔다. Sprite 로 저장해둔다.
    ///// </summary>
    //[SerializeField]
    //private List<Sprite> _list_Ground_Obj = new List<Sprite>();

    /// <summary>
    /// 그라운드 오브젝트들의 프리팹을 저장해두는 리스트이다. (프리팹 원본들로 구성된 리스트)
    /// </summary>
    [SerializeField]
    private List<Transform> _list_Original_Ground_Obj = new List<Transform>();

    /// <summary>
    /// 땅의 얼룩 자국들을 미리생성해서 리스트에 저장해둔다.(오브젝트풀)
    /// </summary>
    private Queue<Transform> _queue_Pool_Ground_Obj = new Queue<Transform>();

    /// <summary>
    /// 현재 화면에 보여지고 있는 오브젝트들의 리스트이다.
    /// </summary>
    private List<Transform> _list_Cur_Seeing_Obj = new List<Transform>();

    [SerializeField]
    private Transform _pTransform_DotGroup = null;


    private Transform _pTransfrom_LeftTop = null;
    private Transform _pTransform_RightBottom = null;

    private WaitForSeconds _ws_Check_OutOfRange;

    /// <summary>
    /// 조이스틱이 향한 방향을 저장한다.
    /// </summary>
    private Vector2 _vecJoystic_Move_Dir = Vector2.zero;

    protected override void OnAwake()
    {
        base.OnAwake();

        var arrTransform = GetComponentsInChildren<Transform>();
        if (null == _pTransfrom_LeftTop)
        {
            for (int i = 0; i < arrTransform.Length; ++i)
            {
                if ("LeftTop" == arrTransform[i].name)
                {
                    _pTransfrom_LeftTop = arrTransform[i];
                }
                else if ("RightBottom" == arrTransform[i].name)
                {
                    _pTransform_RightBottom = arrTransform[i];
                }
            }
        }

        if (_queue_Pool_Ground_Obj.Count <= 0)
        {
            for (int i = 0; i < const_iMax_Pool_Count; ++i)
            {
                var pNewObj = Create_New_Object_By_Random();
                pNewObj.gameObject.SetActive(false);
                _queue_Pool_Ground_Obj.Enqueue(pNewObj);
            }
        }

        _ws_Check_OutOfRange = new WaitForSeconds(5f);

        PlayerManager_HJS.instance.OnMove_Stick.Subscribe += OnMove_Joystick_Func;
    }

    private void OnDestroy()
    {
        //PlayerManager_HJS.instance.OnMove_Stick.Subscribe -= OnMove_Joystick_Func;
    }

    private void Start()
    {
        DoInit();
    }

    public void DoInit()
    {
        StopAllCoroutines();
        for(int i = 0; i< 5; ++i)
        {
            var pObj1 = _queue_Pool_Ground_Obj.Dequeue();
            pObj1.localPosition = new Vector2(Random.Range(-15, -1), Random.Range(-8, -1));
            pObj1.SetParent(_pTransform_DotGroup);
            pObj1.gameObject.SetActive(true);
            _list_Cur_Seeing_Obj.Add(pObj1);

            var pObj2 = _queue_Pool_Ground_Obj.Dequeue();
            pObj2.localPosition = new Vector2(Random.Range(1, 15), Random.Range(1, 8));
            pObj2.SetParent(_pTransform_DotGroup);
            pObj2.gameObject.SetActive(true);
            _list_Cur_Seeing_Obj.Add(pObj2);

            var pObj3 = _queue_Pool_Ground_Obj.Dequeue();
            pObj3.localPosition = new Vector2(Random.Range(-15, 15), Random.Range(-8, 8));
            pObj3.SetParent(_pTransform_DotGroup);
            pObj3.gameObject.SetActive(true);
            _list_Cur_Seeing_Obj.Add(pObj3);
        }

        StartCoroutine(nameof(OnCoroutine_Check_OutOf_Range));
    }

    private void OnMove_Joystick_Func(PlayerManager_HJS.MoveJoystickMessage pMessage)
    {
        _vecJoystic_Move_Dir = pMessage.vecMoveDir;

        Vector2 vecMoveDir = pMessage.vecMoveDir * -1f;
        
        Vector3 vecResult_LocalPos = _pTransform_DotGroup.transform.localPosition;

        Vector2 vecPos = vecMoveDir * Time.deltaTime* const_fDot_Move_Speed;

        vecResult_LocalPos = new Vector3(vecResult_LocalPos.x + vecPos.x, vecResult_LocalPos.y + vecPos.y, vecResult_LocalPos.z);

        _pTransform_DotGroup.transform.localPosition = vecResult_LocalPos;
    }

    private Transform Create_New_Object_By_Random()
    {
        Transform pNewObj = Instantiate(_list_Original_Ground_Obj[Random.Range(0, _list_Original_Ground_Obj.Count)]);
        pNewObj.localPosition = Vector2.zero;

        return pNewObj;
    }

    /// <summary>
    /// 주기적으로 일정 영역 박으로 나간 dot 오브젝트는 다시 재배치한다.
    /// </summary>
    /// <returns></returns>
    private IEnumerator OnCoroutine_Check_OutOf_Range()
    {
        while (true)
        {
            //DebugLogManager.Log($"_list_Cur_Seeing_Obj[0].position : {_list_Cur_Seeing_Obj[0].position}");
            for (int i = 0; i < _list_Cur_Seeing_Obj.Count; ++i)
            {
                var pTarnsform = _list_Cur_Seeing_Obj[i];
                Vector2 vecCurPos = pTarnsform.transform.position;

                if (vecCurPos.x < -15f || 
                    vecCurPos.y < -15f || 
                    vecCurPos.x > 15f || 
                    vecCurPos.y > 15f)   
                {
                    Recycle_Object(pTarnsform);
                }
            }

            yield return _ws_Check_OutOfRange;
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        var pCollisionTransform = collision.transform;

        Recycle_Object(pCollisionTransform);

        //pCollisionTransform.gameObject.SetActive(false);
        //_queue_Pool_Ground_Obj.Enqueue(pCollisionTransform);
        //pCollisionTransform.SetParent(null);

        //for (int i = 0; i < _list_Cur_Seeing_Obj.Count; ++i)
        //{
        //    if (pCollisionTransform.name == _list_Cur_Seeing_Obj[i].name)
        //    {
        //        _list_Cur_Seeing_Obj.Remove(_list_Cur_Seeing_Obj[i]);
        //        DebugLogManager.Log($"리스트에서 삭제. 남은 개수 : {_list_Cur_Seeing_Obj.Count}");
        //        break;
        //    }
        //}

        //var pObj1 = _queue_Pool_Ground_Obj.Dequeue();

        //Vector2 vecRandRange = new Vector2(Random.Range(0, 0.8f), Random.Range(0, 0.8f));

        //float fCreateRange = 12f;
        ////가로로 이동중이므로 생성 위치값을 길게 설정
        //if (_vecJoystic_Move_Dir.x > _vecJoystic_Move_Dir.y)
        //{
        //    fCreateRange = 11f;
        //}
        ////세로로 이동중이므로 생성 위치값을 짤게 설정한다.
        //else
        //{
        //    fCreateRange = 7.5f;
        //}

        //float fRandDistRange = Random.Range(fCreateRange, fCreateRange+1);

        //pObj1.localPosition = (_vecJoystic_Move_Dir + vecRandRange).normalized * fRandDistRange;
        //pObj1.SetParent(_pTransform_DotGroup);
        //pObj1.gameObject.SetActive(true);
        //_list_Cur_Seeing_Obj.Add(pObj1);
    }


    private void Recycle_Object(Transform pTransform_Obj)
    {
        pTransform_Obj.gameObject.SetActive(false);
        _queue_Pool_Ground_Obj.Enqueue(pTransform_Obj);
        pTransform_Obj.SetParent(null);

        for (int i = 0; i < _list_Cur_Seeing_Obj.Count; ++i)
        {
            if (pTransform_Obj.name == _list_Cur_Seeing_Obj[i].name)
            {
                _list_Cur_Seeing_Obj.Remove(_list_Cur_Seeing_Obj[i]);
                //DebugLogManager.Log($"리스트에서 삭제. 남은 개수 : {_list_Cur_Seeing_Obj.Count}");
                break;
            }
        }

        var pObj1 = _queue_Pool_Ground_Obj.Dequeue();

        Vector2 vecRandRange = new Vector2(Random.Range(0, 0.9f), Random.Range(0, 0.9f));

        float fCreateRange = 12f;
        //가로로 이동중이므로 생성 위치값을 길게 설정
        if (_vecJoystic_Move_Dir.x > _vecJoystic_Move_Dir.y)
        {
            fCreateRange = 11f;
        }
        //세로로 이동중이므로 생성 위치값을 짤게 설정한다.
        else
        {
            fCreateRange = 7.5f;
        }

        float fRandDistRange = Random.Range(fCreateRange, fCreateRange + 1);

        pObj1.position = (_vecJoystic_Move_Dir + vecRandRange).normalized * fRandDistRange;
        pObj1.SetParent(_pTransform_DotGroup);
        pObj1.gameObject.SetActive(true);
        _list_Cur_Seeing_Obj.Add(pObj1);
    }
}
