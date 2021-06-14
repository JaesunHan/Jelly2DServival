using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Canvas_Loading : PanelBase
{
    [GetComponentInChildren]
    private Slider _pSlider = null;

    [GetComponentInChildren("Loading_Text")]
    private Text _pText_Loading = null;

    [GetComponentInChildren]
    private LoadingText_Anim _pLoadingTextAnim = null;

    private string strSceneName = "";

    protected override void OnAwake()
    {
        base.OnAwake();


    }

    private void Start()
    {
        SceneLoadManager.EScene_Where eScene_Where = SceneLoadManager.eSceneWhere;


        switch (eScene_Where)
        {
            case SceneLoadManager.EScene_Where.InGameScene:
                strSceneName = "IngameScene";
                break;
            case SceneLoadManager.EScene_Where.LobbyScene:
                strSceneName = "LobbyScene";
                break;
            case SceneLoadManager.EScene_Where.TitleScene:
                strSceneName = "TitleScene";
                break;
            default:
                break;
        }

        StartCoroutine(nameof(OnCoroutine_LoadSceneAsync));
    }

    public override void DoShow()
    {
        base.DoShow();

        DoInit();
    }

    public void DoInit()
    { 
    
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

            if (_pSlider.value < 1f)
            {
                if (null != _pSlider)
                    _pSlider.value = Mathf.MoveTowards(_pSlider.value, 1f, Time.deltaTime);
            }
            else
            {
                if (null != _pText_Loading)
                    _pText_Loading.text = "Touch the Screen";
                _pLoadingTextAnim.DoStopAllCoroutine();
            }

            if (_pSlider.value >= 1f)
            {
                pAsyncOperation.allowSceneActivation = true;
                if (SceneLoadManager.eSceneWhere == SceneLoadManager.EScene_Where.LobbyScene ||
                    SceneLoadManager.eSceneWhere == SceneLoadManager.EScene_Where.InGameScene)
                {
                    SceneLoadManager.DoLoadSceneAddtive(SceneLoadManager.EScene_Where.SystemMessageScene);
                }
                //if (Input.GetMouseButtonDown(0))
                //{
                //    pAsyncOperation.allowSceneActivation = true;
                //}
            }
        }
    }
}
