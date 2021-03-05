using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Focus_Player : ObjectBase
{
    private PlayerCharacter _pPlayerCharacter = null;

    protected override void OnAwake()
    {
        base.OnAwake();

        if (null == _pPlayerCharacter)
        {
            _pPlayerCharacter = PlayerManager.instance.DoGet_Cur_Player_Character();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (null != _pPlayerCharacter)
        {
            Vector3 vecPlayerPos = _pPlayerCharacter.transform.position;
            transform.position = new Vector3(vecPlayerPos.x, vecPlayerPos.y, transform.position.z);
        }
        else
        {
            _pPlayerCharacter = PlayerManager.instance.DoGet_Cur_Player_Character();
        }
    }
}
