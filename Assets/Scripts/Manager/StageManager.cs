using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoSingleton<StageManager>
{
    /// <summary>
    /// 플탐 15초 : 0 wave -> 1 wave
    /// 플탐 45초 : 1 wave -> 2 wave
    /// 플탐 90초 : 2 wave -> 3 wave
    /// </summary>
    const float const_fUpdate_Wave_Term = 15f;
    public int iCurWave { get; private set; } = 0;


    public void DoInit()
    {
        iCurWave = 0;

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
            yield return new WaitForSeconds(const_fUpdate_Wave_Term * (iCurWave +1));

            ++iCurWave;
        }
    }
}
