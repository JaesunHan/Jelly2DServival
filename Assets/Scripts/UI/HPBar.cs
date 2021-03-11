using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : ObjectBase
{
    const float const_fYellow_Zone = 0.7f;
    const float const_fOrange_Zone = 0.5f;
    const float const_fRed_Zone = 0.3f;

    [GetComponentInChildren]
    private Slider _pSlider_HPBar = null;

    private float _fDefaultHP = 0;
    private float _fCurHP = 0;

    [GetComponentInChildren("Fill")]
    private Image _pImage_Fill = null;

    private Color _color_Green = new Color(0.2403933f, 1, 0);
    private Color _color_Yellow = new Color(0.9743208f, 1, 0.03137255f);
    private Color _color_Orange = new Color(1, 0.5547292f, 0.03137255f);
    private Color _color_Red = new Color(1, 0, 0.01486254f);

    protected override void OnAwake()
    {
        base.OnAwake();
    }


    public void DoInit(float fDefaultHP)
    {
        _fDefaultHP = fDefaultHP;
        _fCurHP = fDefaultHP;

        _pSlider_HPBar.interactable = false;
        _pSlider_HPBar.maxValue = 1f;
        _pSlider_HPBar.minValue = 0f;
        _pSlider_HPBar.value = 1f;
        _pSlider_HPBar.gameObject.SetActive(false);

        _pImage_Fill.color = _color_Green;
    }

    /// <summary>
    /// 변경된 HP 값을 HP 바에 적용시킨다.
    /// </summary>
    /// <param name="fHP">현재 HP 값이다.</param>
    public void DoSetHP(float fHP)
    {
        _fCurHP = fHP;
        float fValue = _fCurHP / _fDefaultHP;

        if (_fCurHP < _fDefaultHP)
        {
            _pSlider_HPBar.gameObject.SetActive(true);

            if (_fCurHP >= _fDefaultHP * const_fYellow_Zone)
            {//Green : 70퍼 이상은 초록색 바
                _pImage_Fill.color = _color_Green;
            }
            else if (_fCurHP >= _fDefaultHP * const_fOrange_Zone)
            {//Yellow : 50퍼 이상은 노란색
                _pImage_Fill.color = _color_Yellow;
            }
            else if (_fCurHP >= _fDefaultHP * const_fRed_Zone)
            {//Orange : 30퍼 이상은 주황색
                _pImage_Fill.color = _color_Orange;
            }
            else
            {//Red : 30퍼 미만은 빨강색
                _pImage_Fill.color = _color_Red;
            }
        }
        else
        {
            _pSlider_HPBar.gameObject.SetActive(false);
        }


        _pSlider_HPBar.value = fValue;
    }
}
