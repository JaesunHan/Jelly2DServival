using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class EnemyBase : ObjectBase
{
    [SerializeField]
    private EnemyData _pEnemyData = null;

    //[SerializeField]
    private SpriteRenderer _pSprite_Jelly = null;

    //private Transform _pTrasnform_Player = null;

    private Vector2 _vecPlayer_Pos = Vector2.zero;

    private Animator _pAnim = null;

    private CircleCollider2D _pCollider2d = null;
    private Rigidbody2D _pRigidbody = null;

    protected override void OnAwake()
    {
        base.OnAwake();

        if (null == _pSprite_Jelly)
        {
            var arrSprite = transform.GetComponentsInChildren<SpriteRenderer>();
            for (int i = 0; i < arrSprite.Length; ++i)
            {
                if ("Sprite_Jelly" == arrSprite[i].name)
                {
                    _pSprite_Jelly = arrSprite[i];
                    break;
                }
            }
        }

        if (null == _pAnim)
        {
            _pAnim = GetComponent<Animator>();
        }

        if (null == _pCollider2d)
        {
            _pCollider2d = GetComponent<CircleCollider2D>();
        }

        if (null == _pRigidbody)
        {
            _pRigidbody = GetComponent<Rigidbody2D>();
        }
    }



    public void DoInit(EnemyData pEnemyData)
    {
        _pEnemyData = pEnemyData;

        _pSprite_Jelly.sprite = pEnemyData.pFile;

        _pCollider2d.offset = new Vector2(0, _pEnemyData.fColliderPosY);
        _pCollider2d.radius = _pEnemyData.fColliderRadius;

        _vecPlayer_Pos = PlayerManager.instance.DoGet_Cur_Player_Character().transform.position;

        //_pRigidbody.velocity = Vector2.zero;
        Tracing_Player();
    }

    private void Tracing_Player()
    {
        StopAllCoroutines();
        _pAnim.SetBool("isWalk", true);
        StartCoroutine(nameof(OnCoroutine_Tracing_Player));
    }

    private IEnumerator OnCoroutine_Tracing_Player()
    {
        while (true)
        {
            Vector2 vecCurPos = transform.position;
            Vector2 vecMoveDir = (_vecPlayer_Pos - vecCurPos).normalized;

            transform.position += (Vector3)(vecMoveDir * Time.deltaTime * (_pEnemyData.fMoveSpeed) );
            //_pRigidbody.velocity = vecMoveDir * _pEnemyData.fMoveSpeed;

            yield return null;
        }
    }


    #region Legacy_Code

    private IEnumerator OnCoroutin_Awake()
    {
        if (null == DataManager.instance)
        {
            DebugLogManager.Log("데이터 매니저의 인스턴스가 없다.");
        }

        while (!DataManager.bIsLoaded_AllResource)
        {
            yield return null;
        }
    }

    //private void Check_Sprite_IsNull()
    //{
    //    if (null == _pSprite_Jelly)
    //    {
    //        //var arrSprite = transform.GetComponentsInChildren<Sprite>(true);
    //        var arrSprite = GetComponentsInChildren<Sprite>();
    //        for (int i = 0; i < arrSprite.Length; ++i)
    //        {
    //            if ("Sprite_Jelly" == arrSprite[i].name)
    //            {
    //                //_pSprite_Jelly = arrSprite[i];
    //                break;
    //            }
    //        }
    //    }
    //}

    #endregion
}
