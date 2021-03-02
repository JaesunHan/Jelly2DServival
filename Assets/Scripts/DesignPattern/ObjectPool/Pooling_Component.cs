using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pooling_Component<CLASS_POOL_TARGER> : Singleton<Pooling_Component<CLASS_POOL_TARGER>>
    where CLASS_POOL_TARGER : Component
{
    const int const_iPool_Default_Count = 30;

    private Queue<GameObject> _qPool = new Queue<GameObject>();

    private GameObject _objRoot = null;


    public void DoInit_Pool(CLASS_POOL_TARGER pOriginalObjectTarget)
    {
        _objRoot = new GameObject();

        Prepare_Objec_Pool(pOriginalObjectTarget);

        _objRoot.name = $"{pOriginalObjectTarget.name} 풀 생성";
    }

    public CLASS_POOL_TARGER DoPop(CLASS_POOL_TARGER pObjectOriginal, bool bDefaultActive = false) //Component
    {
        GameObject pDequeueObj = null;
        if (_qPool.Count <= 0)
        {
            var pObj = GameObject.Instantiate(pObjectOriginal.gameObject);
            pObj.hideFlags = HideFlags.HideInHierarchy;
            if (null == _objRoot)
                pObj.transform.SetParent(null);
            else
                pObj.transform.SetParent(_objRoot.transform);

            _qPool.Enqueue(pObj);
        }

        pDequeueObj = _qPool.Dequeue();
        pDequeueObj.hideFlags = HideFlags.None;

        pDequeueObj.gameObject.SetActive(bDefaultActive);

        return pDequeueObj.GetComponent<CLASS_POOL_TARGER>();
    }

    public void DoPush(CLASS_POOL_TARGER pObject)
    {
        _qPool.Enqueue(pObject.gameObject);
        pObject.hideFlags = HideFlags.HideInHierarchy;
        pObject.transform.SetParent(null); //_objRoot.transform
    }

    /// <summary>
    /// 오브젝트 풀 준비
    /// </summary>
    private void Prepare_Objec_Pool(CLASS_POOL_TARGER pObjectOriginal)
    {
        for (int i = 0; i < const_iPool_Default_Count; ++i)
        {
            var pObj = GameObject.Instantiate(pObjectOriginal.gameObject);
            pObj.hideFlags = HideFlags.HideInHierarchy;
            pObj.transform.SetParent(_objRoot.transform);
            pObj.SetActive(false);

            _qPool.Enqueue(pObj);
        }
    }
}
