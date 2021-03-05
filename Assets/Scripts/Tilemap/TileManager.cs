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
    const int const_iDefault_Count_OneSide = 25;

    private Tilemap _pTilemap = null;

    protected override void OnAwake()
    {
        base.OnAwake();

        if (null == _pTilemap)
        {
            _pTilemap = GetComponentInChildren<Tilemap>();
        }
    }

    private void Start()
    {
        SpriteAtlas pSpriteAtlas = DataManager.GetSpriteAtlas("TileSet");

        Sprite[] arrSprites = new Sprite[20];
        int iArrayCount = pSpriteAtlas.GetSprites(arrSprites);

        Tile newTile = new Tile();
        for (int i = -const_iDefault_Count_OneSide; i < const_iDefault_Count_OneSide; ++i)
        {
            for (int j = -const_iDefault_Count_OneSide; j < const_iDefault_Count_OneSide; ++j)
            {
                int iRandomIdx = Random.Range(0, iArrayCount);
                newTile.sprite = arrSprites[iRandomIdx];
                Vector3Int pos = new Vector3Int(i, j, 0);
                _pTilemap.SetTile(pos, newTile);
            }
        }
    }
}
