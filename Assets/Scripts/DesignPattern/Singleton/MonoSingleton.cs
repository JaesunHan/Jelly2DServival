using UnityEngine;

/// <summary>
/// 하이어라키에 존재하는 게임 오브젝트에 붙일 싱글톤이다.
/// 참고 링크
/// https://debuglog.tistory.com/35
/// </summary>
/// <typeparam name="T"></typeparam>
public class MonoSingleton<T> : ObjectBase where T : MonoBehaviour
{
    protected static T _instance = null;

    public static T instance
    {
        get 
        {
            _instance = FindObjectOfType(typeof(T)) as T;

            if (null == _instance)
            {
                //새 게임오브젝트를 생성하고, 컴포넌트로 이 싱글톤을 추가한다.
                _instance = new GameObject($"@{typeof(T).ToString()}", typeof(T)).AddComponent<T>();

                DontDestroyOnLoad(_instance);
            }

            return _instance;
        }
    }
}
