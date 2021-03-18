using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public partial class DataManager : MonoSingleton<DataManager>
{
    public enum EBundleName
    {
        Font,
        SO,
        Sprite,
        Animation,
        material,
        Prefabs,
        texture,
        Shader, 
        Sounds,

        MAX,
    }

    public BundleLoadManager.EBundleLoadLogic eLoadLogic_OnEditor = BundleLoadManager.EBundleLoadLogic.Editor;

    public Observer_Pattern<bool> OnLoadAllResource { get; private set; } = new Observer_Pattern<bool>();

    /// <summary>
    /// 리소스를 모두 불러왔는지 체크
    /// </summary>
    public static bool bIsLoaded_AllResource { get; private set; }

    #region Reference Functions

    /// <summary>
    /// 현재 웨이브에 등장할 수 있는 적들을 리스트로 반환한다.
    /// </summary>
    /// <param name="iWave"></param>
    /// <returns></returns>
    public static List<EnemyData> DoGet_EnemyData_ByStageWave(int iWave)
    {
        ///현재 웨이브에서 등장 가능한 적들의 리스트
        List<EnemyData> list_CurWave_Enemy = new List<EnemyData>();
        var listData = EnemyData_Container.instance.listData;
        for (int i = 0; i < listData.Count; ++i)
        {
            if (listData[i].iAppearWave <= iWave)
            {
                list_CurWave_Enemy.Add(listData[i]);
            }
        }

        return list_CurWave_Enemy;
    }

    /// <summary>
    /// 현재 웨이브에 등장할 수 있는 적들 중에서 하나를 랜덤으로 선택해서 반환한다.
    /// </summary>
    /// <param name="iWave"></param>
    /// <returns></returns>
    public static EnemyData DoGet_Random_EnemyDatas_ByStageWave(int iWave)
    {
        EnemyData pEnemyData = null;

        var list = DoGet_EnemyData_ByStageWave(iWave);

        int iRandomIdx = Random.Range(0, list.Count);

        pEnemyData = list[iRandomIdx];

        return pEnemyData;
    }

    /// <summary>
    /// 현재 웨이브에서 등장할 수 있는 마나포션 데이터들을 리스트로 반환한다
    /// </summary>
    /// <param name="iWave"></param>
    /// <returns></returns>
    public static List<ManaPotionData> DoGet_ManaPotionDatas_ByStageWave(int iWave)
    {
        List<ManaPotionData> list_CurWave_MP = new List<ManaPotionData>();
        var listData = ManaPotionData_Container.instance.listData;
        for (int i = 0; i < listData.Count; ++i)
        {
            if (listData[i].iAppearWave <= iWave)
            {
                list_CurWave_MP.Add(listData[i]);
            }
        }

        return list_CurWave_MP;
    }

    /// <summary>
    /// 현재 웨이브에 등장할 수 있는 마나 포션 중에서 하나를 랜덤으로 선택해서 반환한다.
    /// </summary>
    /// <param name="iWave"></param>
    /// <returns></returns>
    public static ManaPotionData DoGet_Random_ManaPotionData_ByStageWave(int iWave)
    {
        ManaPotionData pMPData = null;

        var list = DoGet_ManaPotionDatas_ByStageWave(iWave);

        int iRandomIdx = Random.Range(0, list.Count);

        pMPData = list[iRandomIdx];

        return pMPData;
    }


    #endregion

    protected override void OnAwake()
    {
        base.OnAwake();

        //DontDestroyOnLoad(gameObject);

#if UNITY_EDITOR
        BundleLoadManager.instance.DoInit(eLoadLogic_OnEditor);
#else
        BundleLoadManager.instance.DoInit(BundleLoadManager.EBundleLoadLogic.StreamingAssets);
#endif

        StartCoroutine(nameof(ResourceLoadAll_Coroutine));
    }

    IEnumerator ResourceLoadAll_Coroutine()
    {
        bIsLoaded_AllResource = false;

        List<Coroutine> listCoroutine = new List<Coroutine>();
        for (int i = 0; i < (int)EBundleName.MAX; i++)
            listCoroutine.Add(StartCoroutine(LoadBundle_Coroutine(((EBundleName)i).ToString())));
        yield return listCoroutine.GetEnumerator();

        yield return LoadBundleData_Coroutine();
        //bIsLoaded_AllSODataResource = true;
        yield return LoadLocalData_Coroutine();

        bIsLoaded_AllResource = true;

        OnLoadAllResource.DoNotify(bIsLoaded_AllResource);
    }

    /// <summary>
    /// 번들 다운받는 코루틴
    /// </summary>
    /// <param name="strBundleName"></param>
    /// <returns></returns>
    private IEnumerator LoadBundle_Coroutine(string strBundleName)
    {
        bool bLoadResult = false;

        yield return BundleLoadManager.instance.DoPreLoad_Bundle(strBundleName,
            (strBundleNameParam, bResult) => {
                bLoadResult = bResult;
            });

        if (bLoadResult)
        {
            Debug.Log($"LoadBundle Success - {strBundleName}");
        }
        else
        {
            Debug.LogError($"LoadBundle Fail - {strBundleName}");
        }
    }

    IEnumerator LoadBundleData_Coroutine()
    {
        bool bIsUpdateChildAsset = eLoadLogic_OnEditor == BundleLoadManager.EBundleLoadLogic.Editor && Application.isPlaying;

        LanguageData_Container.DoInit(LoadData<LanguageData_Container>(), bIsUpdateChildAsset);
        EnemyData_Container.DoInit(LoadData<EnemyData_Container>(), bIsUpdateChildAsset);
        ManaPotionData_Container.DoInit(LoadData<ManaPotionData_Container>(), bIsUpdateChildAsset);
        SkillData_Container.DoInit(LoadData<SkillData_Container>(), bIsUpdateChildAsset);


        GlobalData_Container.DoInit(LoadData<GlobalData_Container>(), bIsUpdateChildAsset);

        LanguageManager.instance.DoInit_LanguageData(LanguageData_Container.instance.listData.ToArray());

        //Font 
        Font pFont = BundleLoadManager.instance.DoLoad<Font>("Font", "DungGeunMo.ttf");
        FontDataDefault[] arrFontData = { new FontDataDefault(SystemLanguage.Korean, pFont) };
        LanguageManager.instance.DoInit_FontData(arrFontData);
        LanguageManager.instance.DoSetLanguage(SystemLanguage.Korean);


        yield break;
    }


    private T LoadData<T>()
    where T : ScriptableObject
    {
        return BundleLoadManager.instance.DoLoad<T>("SO", $"{typeof(T).Name}.asset", false);
    }

    #region GetSprite

    public static Sprite GetSprite_InAtlas(string strAtlasName, string strSpriteName)
    {
        Sprite pSprite = null;

        pSprite = BundleLoadManager.instance.DoLoadSprite_InAtlas(nameof(EBundleName.Sprite), strAtlasName, strSpriteName);
        
        return pSprite;
    }

    public static SpriteAtlas GetSpriteAtlas(string strAtlasName)
    {
        return BundleLoadManager.instance.DoLoadSpriteAtlas(nameof(EBundleName.Sprite), strAtlasName);
    }

    #endregion GetSprite
}
