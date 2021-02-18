using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    /// <summary>
    /// 조이스틱이 움직일 수 있는 반경
    /// </summary>
    const float const_fMax_Stick_Arange = 5f;

    const float const_fStick_Move_Dist = 0.5f;

    [SerializeField]
    private RectTransform _pRectTransform_RoundBack = null;
    [SerializeField]
    private RectTransform _pRectTransform_Stick = null;

    private Vector2 _vecStick_Original_Pos = Vector2.zero;

    private Vector2 _vecDrag_Dir = Vector2.zero;

    private Vector2 _vecPressPos = Vector2.zero;

    private void Awake()
    {
        if (null == _pRectTransform_RoundBack)
        {
            DebugLogManager.Log("조이스틱 RoundBack 의 Transform 이 널이다");
            _pRectTransform_RoundBack = GetComponent<RectTransform>();
        }

        if (null != _pRectTransform_Stick)
        {
            _pRectTransform_Stick.anchoredPosition = Vector3.zero;
        }
        else 
        {
            DebugLogManager.LogError("조이스틱 Sitck 의 Transform 이 널이다.");
        }

        _vecStick_Original_Pos = new Vector2(_pRectTransform_Stick.transform.localPosition.x, _pRectTransform_Stick.transform.localPosition.y);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _vecPressPos = eventData.pressPosition;
        StartCoroutine(nameof(OnCoroutine_Dragging_Process));
    }

    public void OnDrag(PointerEventData eventData)
    {
        var pTransformStick = _pRectTransform_Stick.transform;

        Vector2 vecInputDir = eventData.position - _vecPressPos;  //eventData.position - _pRectTransform_RoundBack.anchoredPosition;
        Vector2 vecClampedDir = vecInputDir.magnitude > const_fMax_Stick_Arange ? vecInputDir.normalized * const_fMax_Stick_Arange : vecInputDir;
        _pRectTransform_Stick.anchoredPosition = vecClampedDir;

        Vector2 vecCurLocalPos = new Vector2(_pRectTransform_Stick.transform.localPosition.x, _pRectTransform_Stick.transform.localPosition.y);
        //_vecDrag_Dir = (vecCurLocalPos - _vecStick_Original_Pos).normalized * -1f;
        _vecDrag_Dir = vecInputDir.normalized;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _vecDrag_Dir = Vector2.zero;
        StopAllCoroutines();
        _pRectTransform_Stick.anchoredPosition = Vector2.zero;
        PlayerManager.instance.OnMove_Stick.DoNotify(new PlayerManager.MoveJoystickMessage(_vecDrag_Dir));
    }


    private IEnumerator OnCoroutine_Dragging_Process()
    {
        while (true)
        {
            //_pRectTransform_Stick.anchoredPosition = _vecDrag_Dir * Time.deltaTime * 1000f;
            //DebugLogManager.Log($"SendMessage / vecDragDir : {_vecDrag_Dir}");
            PlayerManager.instance.OnMove_Stick.DoNotify(new PlayerManager.MoveJoystickMessage(_vecDrag_Dir));

            yield return null;
        }
    }
}
