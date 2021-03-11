using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaPotionBase : ObjectBase
{
    private ManaPotionData _pManaPotionData = null;

    [GetComponentInChildren("Sprite_Potion")]
    private SpriteRenderer _pSpriteRenderer = null;

    private bool bIsAlive = false;

    protected override void OnAwake()
    {
        base.OnAwake();


    }

    public void DoInit(ManaPotionData pData)
    {
        _pManaPotionData = pData;

        bIsAlive = true;

        _pSpriteRenderer.sprite = _pManaPotionData.pFile;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!bIsAlive)
            return;

        var pPlayerCharacter = collision.GetComponent<PlayerCharacter>();

        if (null != pPlayerCharacter)
        {
            bIsAlive = false;

            //DebugLogManager.Log("플레이어가 포션을 먹었다.");
            PlayerManager_HJS.instance.OnGet_MP.DoNotify(new PlayerManager_HJS.GetMPMessage(_pManaPotionData.fGetMP));
            ManaManager.instance.OnReturn_ManaPotion.DoNotify(new ManaManager.ReturnManaPotionMessage(this, bIsAlive));
        }
    }

}
