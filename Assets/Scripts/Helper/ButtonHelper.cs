using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

/// <summary>
/// <see cref="Button"/>을 관리해주는 Interface입니다
/// </summary>
/// <typeparam name="Enum_ButtonName">버튼 이름이 담긴 Enum 타입</typeparam>
public interface IHas_UIButton<Enum_ButtonName>
{
    void IHas_UIButton_OnClickButton(UIButtonMessage<Enum_ButtonName> sButtonMsg);
}

/// <summary>
/// UI Button
/// </summary>
/// <typeparam name="Enum_ButtonName">버튼 이름이 담긴 Enum 타입</typeparam>
public struct UIButtonMessage<Enum_ButtonName>
{
    /// <summary>
    /// 클릭한 버튼의 이름입니다.
    /// </summary>
    public Enum_ButtonName eButtonName { get; private set; }

    /// <summary>
    /// 클릭한 버튼의 인스턴스입니다. Null이 될 수 있습니다.
    /// </summary>
    public Button pButtonInstance_OrNull { get; private set; }

    public UIButtonMessage(Enum_ButtonName eButtonName, Button pButton = null)
    {
        this.eButtonName = eButtonName; this.pButtonInstance_OrNull = pButton;
    }
}

public static class ButtonHelper
{
    public static void DoInit_HasUIElement(MonoBehaviour pUIElementOwner)
    {
        if (null == pUIElementOwner)
        {
            Debug.LogError("Error - pUIElementOwner == null");
            return;
        }

        System.Type[] arrInterfaceType = pUIElementOwner.GetType().GetInterfaces();
        if (null == arrInterfaceType || 0 == arrInterfaceType.Length)
            return;

        Init_HasButton(pUIElementOwner, arrInterfaceType);
    }

    private static void Init_HasButton(MonoBehaviour pOwner, Type[] arrInterfacesType)
    {
        System.Type pType_InterfaceHasButton = arrInterfacesType.FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IHas_UIButton<>));
        if (pType_InterfaceHasButton == null)
            return;

        System.Type pType_EnumButtonName = pType_InterfaceHasButton.GetGenericArguments()[0];
        HashSet<string> setEnumName = new HashSet<string>(System.Enum.GetNames(pType_EnumButtonName));
        var arrButton = pOwner.GetComponentsInChildren<Button>(true).Where(p => setEnumName.Contains(p.name));
        foreach (Button pButton in arrButton)
        {
            if (TryParsing_NameToEnum(pType_EnumButtonName, pButton, out var pEnumValue) == false)
                continue;

            UnityEngine.Events.UnityAction pAction = () =>
            {
                System.Type pButtonMessageGenericType = typeof(UIButtonMessage<>).MakeGenericType(pType_EnumButtonName);
                object pButtonMsg = System.Activator.CreateInstance(pButtonMessageGenericType, pEnumValue, pButton);
                pOwner.SendMessage(nameof(IHas_UIButton<object>.IHas_UIButton_OnClickButton), pButtonMsg, SendMessageOptions.DontRequireReceiver);
            };

            pButton.onClick.AddListener(pAction);
        }
    }

    private static bool TryParsing_NameToEnum(System.Type pType_EnumButtonName, Component pButton, out object pEnum)
    {
        bool bResult = true;
        pEnum = null;
        if (pType_EnumButtonName.IsEnum)
        {
            try
            {
                pEnum = System.Enum.Parse(pType_EnumButtonName, pButton.name);
            }
            catch
            {
                bResult = false;
            }
        }
        else
            pEnum = pButton.name;

        return bResult;
    }
}

