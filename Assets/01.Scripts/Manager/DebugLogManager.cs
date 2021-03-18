using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DebugLogManager : 특정 플랫폼에서는 Debug Mode 에 따라 로그를 출력한다.
/// (Debug Mode 가 true 일 때만 로그 출력)
/// - Log Function 
/// - LogError Function 
/// </summary>
public class DebugLogManager : Singleton<DebugLogManager>
{
    private static bool _bIsDebugMode = false;

    public static bool bIsDebugMode 
    {
        get { return _bIsDebugMode; }
        set { _bIsDebugMode = value; }
    }

    /// <summary>
    /// 생성자가 호출 되면 기본적으로 _bIsDebugMode 값을 false 로 지정한다.
    /// </summary>
    public DebugLogManager()
    {
        _bIsDebugMode = false;
    }


    public static void Log(string strMessage)
    {
#if UNITY_EDITOR
        Debug.Log(strMessage);
#elif UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE_WIN
        if (_bIsDebugMode)
        {
            Debug.Log(strMessage);
        }
#endif
    }

    public static void LogError(string strMessage)
    {
#if UNITY_EDITOR
        Debug.LogError(strMessage);
#elif UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE_WIN
        Debug.LogError(strMessage);
#endif
    }
}
