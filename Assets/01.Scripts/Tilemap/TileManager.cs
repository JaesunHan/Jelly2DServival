using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.Tilemaps;

public class TileManager : MonoSingleton<TileManager>
{
    /// <summary>
    /// 0을 기준으로 좌우로 25만큼씩 기본타일 생성
    /// </summary>
    const int const_iDefault_Count_OneSide = 500;

    private Tilemap _pTilemap = null;

    private WaitForSeconds _ws_Check_Term = null;

    private int iMaxMovePos = 0;

    protected override void OnAwake()
    {
        base.OnAwake();

        if (null == _pTilemap)
        {
            _pTilemap = GetComponentInChildren<Tilemap>();
        }

        _ws_Check_Term = new WaitForSeconds(2);
    }



    protected override IEnumerator OnEnableCoroutine()
    {
        while (!DataManager.bIsLoaded_AllResource)
        {
            yield return null;
        }

        SpriteAtlas pSpriteAtlas = DataManager.GetSpriteAtlas("TileSet");

        Sprite[] arrSprites = new Sprite[20];
        int iArrayCount = pSpriteAtlas.GetSprites(arrSprites);

        Tile newTile = new Tile();
        for (int i = -const_iDefault_Count_OneSide; i < const_iDefault_Count_OneSide; ++i)
        {
            for (int j = -const_iDefault_Count_OneSide; j < const_iDefault_Count_OneSide; ++j)
            {
                int iRandomIdx = Random.Range(0, iArrayCount);
                //DebugLogManager.Log($"arrSprites[iRandomIdx] : {arrSprites[iRandomIdx]}");
                newTile.sprite = arrSprites[iRandomIdx];
                //DebugLogManager.Log($"newTile.sprite : {newTile.sprite}");
                Vector3Int pos = new Vector3Int(i, j, 0);
                _pTilemap.SetTile(pos, newTile);
            }
        }

        iMaxMovePos = const_iDefault_Count_OneSide - 12;
    }

    ///// <summary>
    ///// 플레이어가 움직였다는 신호를 받았을 때  플레이어의 위치를 체크해서 맵을 넓힌다.
    ///// </summary>
    ///// <returns></returns>
    //private void OnMove_Joystick_Func(PlayerManager_HJS.MoveJoystickMessage pMessage)
    //{
    //    Vector2 vecMoveDir = pMessage.vecMoveDir;

    //    Vector2 vecCurPlayerPos = PlayerManager_HJS.instance.DoGet_Cur_Player_WorldPos();

    //    if(vecCurPlayerPos.x < -iMaxMovePos || vecCurPlayerPos.x > iMaxMovePos ||
    //        vecCurPlayerPos.y < - iMaxMovePos || vecCurPlayerPos.y > iMaxMovePos)
    //    {
    //        for (int i = iMaxMovePos; i < const_iDefault_Count_OneSide; ++i)
    //        {
    //            iMaxMovePos += const_iDefault_Count_OneSide;
    //            Vector3Int posLeft = new Vector3Int(i, j, 0);
    //            //_pTilemap.
    //        }

    //    }
    //}

}
