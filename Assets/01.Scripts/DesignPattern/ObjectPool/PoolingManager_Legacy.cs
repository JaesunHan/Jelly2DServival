using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PoolingManager : MonoSingleton<PoolingManager>
{

    protected Queue<GameObject> _listUnUsed = new Queue<GameObject>();
    protected Queue<GameObject> _listUsed = new Queue<GameObject>();

    private GameObject pTargetObject = null;


    public void DoPrepare_Pool(GameObject pObejctCopyTarget, int iCount)
    {
        

        pTargetObject = pObejctCopyTarget;

        if (_listUnUsed.Count <= 0)
        {
            for (int i = 0; i < iCount; ++i)
            {
                OnCreate_WhenEmptyPool(pTargetObject);
            }
        }
    }

    public GameObject DoPop()
    {
        GameObject pObj_Copy = null;

        if (null == pTargetObject)
        {
            //DebugLogManager.Log("복사하려는 게임 오브젝트 타겟이 NULL 이다.");
            return null;
        }
       
        if (_listUnUsed.Count <= 0)
        {
            pObj_Copy = OnCreate_WhenEmptyPool(pTargetObject);
        }
        else
        {
            pObj_Copy = _listUnUsed.Dequeue();
            _listUsed.Enqueue(pObj_Copy);
        }

        return pObj_Copy;
    }

    private GameObject OnCreate_WhenEmptyPool(GameObject pObjectCopyTarget)
    {
        GameObject obj_UnUsed = null;

        obj_UnUsed = GameObject.Instantiate(pObjectCopyTarget);
        obj_UnUsed.name = string.Format("{0}_{1}", pObjectCopyTarget.name, _listUnUsed.Count + _listUsed.Count);
        obj_UnUsed.transform.SetParent(transform);

        _listUnUsed.Enqueue(obj_UnUsed);

        return obj_UnUsed;
    }
}
