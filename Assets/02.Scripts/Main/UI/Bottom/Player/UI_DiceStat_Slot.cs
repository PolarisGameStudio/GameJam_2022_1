using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_DiceStat_Slot : UI_BaseSlot<DiceStat>
{
    public Text txtStat;
    public Text txtValue;
    public GameObject OnLock;
    public GameObject OnDiable;
    public Text OnDiableText;

    public override void Init(DiceStat data)
    {
        _data = data;
        Refresh();
    }

    public void SetDisableText(string text)
    {
        OnDiableText.text = text;
    }

    public void Refresh()
    {
        if (_data.Index == -1)
        {
            txtStat.text = "";
            txtValue.text = "";
        }
        else
        {
            var diceData = TBL_UPGRADE_DICE.GetEntity(_data.Index);
            txtStat.text = $"{StringValue.GetStatName(diceData.StatType)}";
            txtValue.text = $"{diceData.MinStatValue + _data.AddValue}%";
            txtValue.color = ColorValue.GetColorByGrade(diceData.Grade);
            txtStat.color = ColorValue.GetColorByGrade(diceData.Grade);
        }
        
        OnLock.SetActive(_data.IsLock);
        OnDiable.SetActive(!_data.IsActive); 
    }

    public void OnClickSlot()
    {
        _data.ToggleLock();
        Refresh();
        
        SoundManager.Instance.PlaySound("ui_common_button");
    }
}
