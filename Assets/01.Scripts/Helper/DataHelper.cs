using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class LanguageData : ScriptableObject, ILanguageTextData
{
    string ILanguageTextData.strLanguageKey => strLanguageKey;

    public string GetLocalText(SystemLanguage eSystemLanguage)
    {
        switch (eSystemLanguage)
        {
            case SystemLanguage.English: return strEnglish;
            case SystemLanguage.Korean: return strKorean;

            default: return $"Not Found Language Key : {strLanguageKey} // Lang : {eSystemLanguage}";
        }
    }
}

public class DataHelper : MonoBehaviour
{
    
}
