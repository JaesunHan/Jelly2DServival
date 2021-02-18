using UnityEngine;

/// <summary>
/// 하이어라키에 존재하지 않는 일반 싱글톤이다.
/// 참고 링크
/// https://debuglog.tistory.com/35
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class Singleton<T> where T : class
{
    protected static T _instance = null;

    public static T instance
    {
        get
        {
            if (null == _instance)
                _instance = System.Activator.CreateInstance(typeof(T)) as T;

            return _instance;
        }
    }
}
