﻿#region Header
/*	============================================
 *	Aurthor 			    : Strix
 *	Initial Creation Date 	: 2020-02-10
 *	Summary 		        : 
 *  Template 		        : For Unity Editor V1
   ============================================ */
#endregion Header

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


/// <summary>
/// �� Text�� Text�� �Ҵ��ϴ� ������Ʈ.
/// </summary>
[ExecuteInEditMode]
public class LanguageText : ObjectBase
{
    /* const & readonly declaration             */

    /* enum & struct declaration                */

    /* public - Field declaration               */

    [SerializeField]
    string strLanguageKey = "";

    [Header("없으면 GetComponentInChildren 합니다")]
    public Text pText;

    /* protected & private - Field declaration  */

    object[] _arrObject;
    bool _bUseStringFormat = false;

    // ========================================================================== //

    /* public - [Do~Somthing] Function 	        */

    public void DoSetText(string strLanguageKey)
    {
        this.strLanguageKey = strLanguageKey;
        _bUseStringFormat = false;

        UpdateText();
    }

    public void DoSetText_Format(string strLanguageKey, params object[] arrObject)
    {
        this.strLanguageKey = strLanguageKey;
        _bUseStringFormat = true;
        _arrObject = arrObject;

        UpdateText();
    }

    // ========================================================================== //

    /* protected - [Override & Unity API]       */

#if UNITY_EDITOR
    private void Reset()
    {
        if (Application.isPlaying == false)
            UpdateInEditor();
    }

    private void Update()
    {
        if (Application.isPlaying == false)
            UpdateInEditor();
    }

    private void UpdateInEditor()
    {
        LanguageText[] arrSameComponent = GetComponents<LanguageText>();
        if (arrSameComponent.Length > 1)
        {
            for (int i = 1; i < arrSameComponent.Length; i++)
                DestroyImmediate(arrSameComponent[i]);
        }

        if (pText == null)
            pText = GetComponentInChildren<Text>();

        pText.text = "Key: " + strLanguageKey;
    }
#endif

    protected override void OnAwake()
    {
        base.OnAwake();

        if (pText == null)
            pText = GetComponentInChildren<Text>();

        if (null == LanguageManager.instance)
        {
            Debug.Log("! LanguageManager is null !");
        }
        else
        {
            Debug.Log("LanguageManager is Exist");
        }
    }

    protected override void OnEnableObject()
    {
        base.OnEnableObject();

#if UNITY_EDITOR
        if (Application.isEditor && Application.isPlaying == false)
            return;
#endif

        LanguageManager.instance.OnSetLanguage.Subscribe += OnSetLanguage_Subscribe_And_Listen_CurrentData; //Subscribe_And_Listen_CurrentData
    }

    protected override void OnDisableObject(bool bIsQuit_Application)
    {
        base.OnDisableObject(bIsQuit_Application);

        if (bIsQuit_Application)
            return;

#if UNITY_EDITOR
        if (Application.isEditor && Application.isPlaying == false)
            return;
#endif

        //LanguageManager.instance.OnSetLanguage.DoRemove_Observer(OnSetLanguage_Subscribe_And_Listen_CurrentData);
    }

    private void OnSetLanguage_Subscribe_And_Listen_CurrentData(SystemLanguage pMessage)
    {
        if (LanguageManager.instance.bIsInit == false)
            return;

        UpdateText();
    }

    private void UpdateText()
    {
        if (string.IsNullOrEmpty(strLanguageKey))
            return;

        string strText;
        bool bResult = _bUseStringFormat ? LanguageManager.instance.GetTryText_Format(strLanguageKey, out strText, _arrObject) : LanguageManager.instance.GetTryText(strLanguageKey, out strText);

        if (bResult == false)
        {
            strText = $"{name} - Not Found LanguageKey : \"{strLanguageKey}\"";
            Debug.LogError(strText, this);
        }

        pText.text = strText;
    }

    /* protected - [abstract & virtual]         */


    // ========================================================================== //

    #region Private

    #endregion Private
}
