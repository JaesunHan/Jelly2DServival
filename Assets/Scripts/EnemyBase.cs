using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class EnemyBase : ObjectBase
{
    private EnemyData _pEnemyData = null;

    //[SerializeField]
    private SpriteRenderer _pSprite_Jelly = null;

    private Transform _pTrasnform_Player = null;

    private Vector2 _vecPlayer_Pos = Vector2.zero;

    protected override void OnAwake()
    {
        base.OnAwake();

         if (null == _pSprite_Jelly)
        {
            //var arrSprite = transform.GetComponentsInChildren<Sprite>(true);
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
    }



    public void DoInit(EnemyData pEnemyData)
    {
        StopAllCoroutines();
        _pEnemyData = pEnemyData;

        _pSprite_Jelly.sprite = pEnemyData.pFile;

        _vecPlayer_Pos = PlayerManager.instance.DoGet_Cur_Player_Character().transform.position;

        Tracing_Player();
    }

    private void Tracing_Player()
    {
        StartCoroutine(nameof(OnCoroutine_Tracing_Player));
    }

    private IEnumerator OnCoroutine_Tracing_Player()
    {
        while (gameObject.activeSelf)
        {
            Vector2 vecCurPos = transform.position;
            Vector2 vecMoveDir = (_vecPlayer_Pos - vecCurPos).normalized;

            transform.position += (Vector3)(vecMoveDir * Time.deltaTime * (_pEnemyData.iMoveSpeed) );
            
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

    private void Check_Sprite_IsNull()
    {
        if (null == _pSprite_Jelly)
        {
            //var arrSprite = transform.GetComponentsInChildren<Sprite>(true);
            var arrSprite = GetComponentsInChildren<Sprite>();
            for (int i = 0; i < arrSprite.Length; ++i)
            {
                if ("Sprite_Jelly" == arrSprite[i].name)
                {
                    //_pSprite_Jelly = arrSprite[i];
                    break;
                }
            }
        }
    }

    #endregion
}
