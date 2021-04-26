using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canvas_Lobby_Top : PanelBase
{
    const int const_iCount_MoneyType = 2;
    public override void DoShow()
    {
        base.DoShow();

        DoInit();
    }

    [GetComponentInChildren]
    List<Widget_Money_Button> _list_Widget_MoneyBtn = new List<Widget_Money_Button>();
    
    public void DoInit()
    {
        if (_list_Widget_MoneyBtn.Count == const_iCount_MoneyType)
        {
            for (int i = 0; i < _list_Widget_MoneyBtn.Count; ++i)
            {
                EMoneyType eMoneyType = (EMoneyType)i;
                _list_Widget_MoneyBtn[i].DoInit(eMoneyType);
            }
        }
    }




}
