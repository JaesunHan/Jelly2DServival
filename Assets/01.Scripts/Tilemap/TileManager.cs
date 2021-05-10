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

        Sprite[] arrSprites = new Sprite[5];
        int iArrayCount = pSpriteAtlas.GetSprites(arrSprites);

        Tile newTile = new Tile();

        for (int i = 0; i < 1000; ++i)
        {
            int iSpriteRandomIdx = Random.Range(0, iArrayCount-1);

            int iRandomIdx1 = Random.Range(-280, 280);
            int iRandomIdx2 = Random.Range(-280, 280);

            newTile.sprite = arrSprites[iSpriteRandomIdx];

            Vector3Int pos = new Vector3Int(iRandomIdx1, iRandomIdx2, 0);
            _pTilemap.SetTile(pos, newTile);
        }

        //for (int i = -const_iDefault_Count_OneSide; i < const_iDefault_Count_OneSide; ++i)
        //{
        //    for (int j = -const_iDefault_Count_OneSide; j < const_iDefault_Count_OneSide; ++j)
        //    {
        //        int iRandomIdx = Random.Range(0, iArrayCount);
        //        //DebugLogManager.Log($"arrSprites[iRandomIdx] : {arrSprites[iRandomIdx]}");
        //        newTile.sprite = arrSprites[iRandomIdx];
        //        //DebugLogManager.Log($"newTile.sprite : {newTile.sprite}");
        //        Vector3Int pos = new Vector3Int(i, j, 0);
        //        _pTilemap.SetTile(pos, newTile);
        //    }
        //}

        iMaxMovePos = const_iDefault_Count_OneSide - 12;
    }

}
