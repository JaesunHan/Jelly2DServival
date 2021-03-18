using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 스테이지 넘버와 웨이브 넘버를 관리한다.
/// </summary>
public class StageManager : MonoSingleton<StageManager>
{
    /// <summary>
    /// 플탐 60초 : 0 wave -> 1 wave
    /// 플탐 120초 : 1 wave -> 2 wave
    /// 플탐 180초 : 2 wave -> 3 wave
    /// </summary>
    const float const_fUpdate_Wave_Term = 60f;
    public int iCurWave { get; private set; } = 0;

    private float _fWave_Update_Term = 0f;

    /// <summary>
    /// 한 스테이지당 최대 wave 수
    /// </summary>
    private int _iMax_Wave_Num = 0;

    public void DoInit()
    {
        iCurWave = 0;

        _fWave_Update_Term = EGlobalKey_float.웨이브_갱신_주기.Getfloat();

        _iMax_Wave_Num = EGlobalKey_int.스테이지당_최대_웨이브_수.Getint();

        StartCoroutine(nameof(OnCoroutine_Update_Wave));
    }

    /// <summary>
    /// 주기적으로 웨이브 숫자를 올린다.
    /// </summary>
    /// <returns></returns>
    private IEnumerator OnCoroutine_Update_Wave()
    {
        while (true)
        {
            //const_fUpdate_Wave_Term
            yield return new WaitForSeconds(_fWave_Update_Term * (iCurWave +1));

            ++iCurWave;
        }
    }
}
