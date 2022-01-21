using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_DiceStat_Slot : UI_BaseSlot<DiceStat>
{
    public Text txtStat;
    public GameObject OnLock;
    public GameObject OnDiable;

    public override void Init(DiceStat data)
    {
        _data = data;
        Refresh();
    }

    public void Refresh()
    {
        if (_data.Index == -1)
        {
            txtStat.text = "";
        }
        else
        {
            var diceData = TBL_UPGRADE_DICE.GetEntity(_data.Index);
            txtStat.text = $"{diceData.StatType} {diceData.MinStatValue + _data.AddValue}%";
            txtStat.color = ColorValue.GetColorByGrade(diceData.Grade);
        }
        
        OnLock.SetActive(_data.IsLock);
        OnDiable.SetActive(!_data.IsActive); 
    }

    public void OnClickSlot()
    {
        _data.ToggleLock();
        Refresh();
    }
}
