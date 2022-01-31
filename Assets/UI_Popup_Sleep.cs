using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Popup_Sleep : UI_BasePopup<UI_Popup_Sleep>
{
    public Text _txtCurStage;
    public Slider _sliderClose;
    
    public override void Open()
    {
        base.Open();

        SleepEvent.Trigger(Enum_SleepEventType.SleepOn);
    }

    public override void Close()
    {
        base.Close();

        SleepEvent.Trigger(Enum_SleepEventType.SleepOff);
    }

    protected override void Refresh()
    {
        _sliderClose.value = 0;
        _txtCurStage.text = BattleManager.Instance.CurrentBattle.GetBattleTitle();
    }

    public void OnValueChange(float value)
    {
        if (value >= 1)
        {
            Close();
        }
    }

    public void OnPointerUp()
    {
        _sliderClose.value = 0;
    }
}