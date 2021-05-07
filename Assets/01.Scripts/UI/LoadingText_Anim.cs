using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingText_Anim : MonoBehaviour
{

    WaitForSeconds _wsTerm;

    Text _pText_Loading = null;

    int iDot = 0;

    void OnEnable()
    {
        _pText_Loading = GetComponentInChildren<Text>();
        _wsTerm = new WaitForSeconds(0.5f);

        StopAllCoroutines();
        //StartCoroutine(nameof(OnCoroutine_Text_Anim));
    }

    public void DoStopAllCoroutine()
    {
        StopAllCoroutines();
    }

    private IEnumerator OnCoroutine_Text_Anim()
    {
        while (isActiveAndEnabled)
        {
            iDot++;

            string strDot = ".";
            switch (iDot % 3)
            {
                case 0:
                    strDot = ".";
                    break;
                case 1:
                    strDot = "..";
                    break;
                case 2:
                    strDot = "...";
                    break;
            }

            _pText_Loading.text = "Loading" + strDot;

            yield return _wsTerm;
        }
    }
}
