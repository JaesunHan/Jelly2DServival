using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneLoadManager
{
    const string const_strIngameSceneName = "IngameScene";
    //const string const_strCommonSystemSceneName = "CommonSystem";
    const string const_strLobbySceneName = "LobbyScene";
    const string const_strTitleSceneName = "TitleScene";

    const string const_strSystemMessageSceneName = "SystemMessageScene";

    const string const_strLoadingSceneName = "LoadingScene";

    public enum EScene_Where
    {
        None,

        //[RegistSubString(const_strIngameSceneName)]
        InGameScene,

        //[RegistSubString(const_strCommonSystemSceneName)]
        //CommonSystem,

        //[RegistSubString(const_strTitleSceneName)]
        TitleScene,

        LobbyScene,

        SystemMessageScene,

        LoadingScene,
    }

    public static EScene_Where eSceneWhere { get; private set; } = EScene_Where.None;
    public static string strSceneName { get; private set; } = "";

    public static Observer_Pattern<EScene_Where> OnLoadScene { get; private set; } = new Observer_Pattern<EScene_Where>();

    public static void Event_SetSceneWhere(EScene_Where eScene)
    {
        eSceneWhere = eScene;
    }

    public static void DoChangeScene(EScene_Where eSecen_Where)
    {
        switch (eSecen_Where)
        {
            case EScene_Where.InGameScene:
                eSceneWhere = eSecen_Where;
                strSceneName = const_strIngameSceneName;
                ////임시 코드
                //SceneManager.LoadScene(const_strSystemMessageSceneName, LoadSceneMode.Additive);

                SceneManager.LoadScene(const_strLoadingSceneName);
                //SceneManager.LoadScene(const_strIngameSceneName);

                break;

            case EScene_Where.LobbyScene:
                eSceneWhere = eSecen_Where;
                strSceneName = const_strLobbySceneName;
                //SceneManager.LoadScene(const_strLoadingSceneName);
                SceneManager.LoadScene(const_strLoadingSceneName);

                //SceneManager.LoadScene(const_strSystemMessageSceneName, LoadSceneMode.Additive);
                break;

            case EScene_Where.TitleScene:
                eSceneWhere = eSecen_Where;
                strSceneName = const_strTitleSceneName;
                SceneManager.LoadScene(const_strTitleSceneName);
                break;

            default:
                break;
        }

        

    }


    public static void LoadScene()
    {
        //SoundManager.instance.DoStopAllSound();
        //EffectManager.DoStopAllEffect();

    }

    public static void DoLoadSceneAddtive(EScene_Where eScene)
    {
        switch (eScene)
        {
            //case EScene_Where.CommonSystem:
            //    SceneManager.LoadScene(const_strCommonSystemSceneName, LoadSceneMode.Additive);
            //    break;

            case EScene_Where.SystemMessageScene:
                SceneManager.LoadScene(const_strSystemMessageSceneName, LoadSceneMode.Additive);
                break;
            default:
                break;
        }
    }

    private IEnumerator OnCoroutine_LoadSceneAsync()
    {
        yield return null;

        AsyncOperation pAsyncOperation = SceneManager.LoadSceneAsync(strSceneName);

        //작업의 완료 유무를 boolean 형으로 반환합니다.
        bool bIsDone = pAsyncOperation.isDone;

        //진행도를 float 타입으로 반환하는데, 0, 1 을 반환합니다. (0 : 진행중, 1 : 진행완료)
        float fProgress = pAsyncOperation.progress;
        //true 면 로딩이 완료됐을 때 바로 씬을 넘기는 거고, false  면 Progress 가 0.9f 에서 멈춥니다.  이 값을 다시 true 로 해야 불러운 Scene 으로 넘어갈 수 있다.
        bool bAllowSceneActivation = pAsyncOperation.allowSceneActivation;

        //로딩이 끝나도 멈추게 하기
        pAsyncOperation.allowSceneActivation = false;

        Debug.Log($"Progress : {pAsyncOperation.progress}");

        //아직 전부 로딩하지 못했을 때 는 아래 반복문을 처리한다.
        while (!pAsyncOperation.isDone)
        {
            yield return null;

            //if (progressbar.value < 1f)
            //{
            //    if (null != progressbar)
            //        progressbar.value = Mathf.MoveTowards(progressbar.value, 1f, Time.deltaTime);
            //}
            //else
            //{
            //    if (null != loadText)
            //        loadText.text = "Touch the Screen";
            //}

            //if (progressbar.value >= 1f)
            //{
            //    if (Input.GetMouseButtonDown(0))
            //    {
            //        pAsyncOperation.allowSceneActivation = true;
            //    }
            //}
        }
    }
}
