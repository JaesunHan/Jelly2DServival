using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraManager : MonoSingleton<MainCameraManager>
{
    /// <summary>
    /// 카메라 애니메이션 타입
    /// </summary>
    public enum ECameraAnimType
    {
        /// <summary>
        /// 메테오가 떨어지면 카메라 쉐이킹
        /// </summary>
        Crash_Meteor,

    }

    public struct OccurCameraAnimMessage
    {
        public ECameraAnimType eAnimType;

        public OccurCameraAnimMessage(ECameraAnimType eType)
        {
            eAnimType = eType;
        }
    }
    /// <summary>
    /// 카메라 애니메이션이 발생하는지 구도한다.
    /// 메테오 : 
    /// </summary>
    public Observer_Pattern<OccurCameraAnimMessage> OnOccur_Camera_Anim { get; private set; } = Observer_Pattern<OccurCameraAnimMessage>.instance;

    private PlayerCharacter _pPlayerCharacter = null;

    protected override void OnAwake()
    {
        base.OnAwake();

        if (null == _pPlayerCharacter)
        {
            _pPlayerCharacter = PlayerManager_HJS.instance.DoGet_Cur_Player_Character();
        }

        OnOccur_Camera_Anim.Subscribe += OnOccur_Camera_Anim_Func;
    }
    // Update is called once per frame
    void Update()
    {
        if (null != _pPlayerCharacter)
        {
            Vector3 vecPlayerPos = _pPlayerCharacter.transform.position;
            transform.position = new Vector3(vecPlayerPos.x, vecPlayerPos.y, transform.position.z);
        }
        else
        {
            _pPlayerCharacter = PlayerManager_HJS.instance.DoGet_Cur_Player_Character();
        }
    }

    private void OnOccur_Camera_Anim_Func(OccurCameraAnimMessage pMessage)
    {
        switch (pMessage.eAnimType)
        {
            case ECameraAnimType.Crash_Meteor:
                Play_Camera_Shake();

                break;
            default:
                break;
        }
    }

    private void Play_Camera_Shake()
    {
        StartCoroutine(nameof(OnCoroutine_Camera_Shake));
    }

    private IEnumerator OnCoroutine_Camera_Shake()
    {
        float fShakingTime = 0.75f;
        float fProgress = 0f;
        while (fProgress <= fShakingTime)
        {
            fProgress += Time.deltaTime;

            transform.position = transform.position + (Vector3)(Random.insideUnitCircle * 0.1f);

            yield return null;
        }


        Vector3 vecPlayerPos = _pPlayerCharacter.transform.position;
        transform.position = new Vector3(vecPlayerPos.x, vecPlayerPos.y, transform.position.z);
    }
}
