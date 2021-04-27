using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Canvas_System_Message : PanelBase
{
    /// <summary>
    /// 메시지를 보여주는 시간
    /// </summary>
    const float const_fShow_Time = 2f;

    [GetComponentInChildren]
    private Image _pImage_Message_Back = null;

    [GetComponentInChildren]
    private Text _pText_Message = null;

    //[GetComponentInChildren]
    //private Animation _pAnim = null;

    protected override void OnAwake()
    {
        base.OnAwake();
        if (null != _pImage_Message_Back)
            _pImage_Message_Back.gameObject.SetActive(false);
    }

    ///// <summary>
    ///// 메시지를 약 1.5초간 보여줬다가 끄는 애니메이션을 재생한다.
    ///// </summary>
    ///// <param name="strMessageKey">보여줄 메시지의 언어 키를 문자열 형태로 전달</param>
    //public void DoShow_Message_Anim(string strMessageKey)
    //{
    //    _pImage_Message_Back.gameObject.SetActive(true);
    //    _pText_Message.text = DataManager.GetLocalText(strMessageKey);

    //    Invoke(nameof(MessageOff), const_fShow_Time);
    //}

    /// <summary>
    /// 메시지를 약 1.5초간 보여줬다가 끄는 애니메이션을 재생한다.
    /// </summary>
    /// <param name="eMessageKey">보여줄 메시지의 언어 키로 전달</param>
    public void DoShow_Message_Anim(ELanguageKey eMessageKey)
    {
        StopAllCoroutines();
        _pImage_Message_Back.gameObject.SetActive(true);
        _pText_Message.text = DataManager.GetLocalText(eMessageKey);

        //_pAnim.Play("ShowMessage");

        //Invoke(nameof(MessageOff), const_fShow_Time);
        StartCoroutine(nameof(OnCoroutine_OffImage));
    }

    private IEnumerator OnCoroutine_OffImage()
    {
        float fProgress = 0;
        while (fProgress <= const_fShow_Time)
        {
            fProgress += 0.016667f;
            yield return null;
        }

        _pImage_Message_Back.gameObject.SetActive(false);
    }


    //private void MessageOff()
    //{
    //    _pImage_Message_Back.gameObject.SetActive(false);
    //}
}
