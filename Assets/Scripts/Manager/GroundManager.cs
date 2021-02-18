using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundManager : ObjectBase
{
    const int const_iMax_Pool_Count = 30;

    const float const_fDot_Move_Speed = 2f;

    const float const_fCreate_Boundary = 4f;


    ///// <summary>
    ///// 땅에 얼룩 같은 자국들을 미리 리스트에 저장해둔다. Sprite 로 저장해둔다.
    ///// </summary>
    //[SerializeField]
    //private List<Sprite> _list_Ground_Obj = new List<Sprite>();

    [SerializeField]
    private List<Transform> _list_Ground_Obj = new List<Transform>();

    /// <summary>
    /// 땅의 얼룩 자국들을 미리생성해서 리스트에 저장해둔다.(오브젝트풀)
    /// </summary>
    private Queue<Transform> _queue_Pool_Ground_Obj = new Queue<Transform>();

    [SerializeField]
    private Transform _pTransform_DotGroup = null;


    private Transform _pTransfrom_LeftTop = null;
    private Transform _pTransform_RightBottom = null;

    /// <summary>
    /// 움직이는 중인 방향을 저장한다.
    /// </summary>
    private Vector2 _vecMoveDir = Vector2.zero;

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

        PlayerManager.instance.OnMove_Stick.Subscribe += OnMove_Joystick_Func;
    }

    private void OnDestroy()
    {
        PlayerManager.instance.OnMove_Stick.Subscribe -= OnMove_Joystick_Func;
    }

    private void Start()
    {
        DoInit();
    }

    public void DoInit()
    {

        for (int i = 0; i < 2; ++i)
        {
            var pObj1 = _queue_Pool_Ground_Obj.Dequeue();
            pObj1.localPosition = new Vector2(Random.Range(-7, -5), Random.Range(-3, -1));
            pObj1.SetParent(_pTransform_DotGroup);
            pObj1.gameObject.SetActive(true);

            var pObj2 = _queue_Pool_Ground_Obj.Dequeue();
            pObj2.localPosition = new Vector2(Random.Range(5, 7), Random.Range(1, 3));
            pObj2.SetParent(_pTransform_DotGroup);
            pObj2.gameObject.SetActive(true);
        }
    }


    private void OnMove_Joystick_Func(PlayerManager.MoveJoystickMessage pMessage)
    {
        _vecMoveDir = pMessage.vecMoveDir * -1f;
        
        Vector3 vecResult_LocalPos = _pTransform_DotGroup.transform.localPosition;

        Vector2 vecPos = _vecMoveDir * Time.deltaTime* const_fDot_Move_Speed;

        vecResult_LocalPos = new Vector3(vecResult_LocalPos.x + vecPos.x, vecResult_LocalPos.y + vecPos.y, vecResult_LocalPos.z);

        _pTransform_DotGroup.transform.localPosition = vecResult_LocalPos;
    }

    private Transform Create_New_Object_By_Random()
    {
        Transform pNewObj = Instantiate(_list_Ground_Obj[Random.Range(0, _list_Ground_Obj.Count)]);
        pNewObj.localPosition = Vector2.zero;

        return pNewObj;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var pCollisionTransform = collision.transform;

        pCollisionTransform.gameObject.SetActive(false);
        _queue_Pool_Ground_Obj.Enqueue(pCollisionTransform);

        bool bRemoveSucce = _list_Ground_Obj.Remove(pCollisionTransform);
        if (bRemoveSucce)
            Debug.Log("삭제 성공");
        else
            Debug.Log("삭제 실패");


        //for (int i = 0; i < _list_Ground_Obj.Count; ++i)
        //{ 
        //    if(pCollisionTransform.name == _list_Ground_Obj[i].name)
        //}

        
    }
}
