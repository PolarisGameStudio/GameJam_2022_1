using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Passive_List : SingletonBehaviour<UI_Passive_List>, GameEventListener<RefreshEvent>
{
    public List<Image> OnActive;
    public List<Text> TxtRemainTimes;

    public void OnClickButton()
    {
    }

    public void Refresh()
    {
        for (int i = 0; i < (int) Enum_RuneBuffType.Count; i++)
        {
            bool isActive = DataManager.RuneData.IsRuneActivate((Enum_RuneBuffType) i);

            OnActive[i].enabled = isActive;

            if (isActive)
            {
                TxtRemainTimes[i].text = $"{DataManager.RuneData.RuneRemainTime[i] / 60}m";
            }
            else
            {
                TxtRemainTimes[i].text = "";
            }
        }
    }

    public void OnGameEvent(RefreshEvent e)
    {
        if (e.Type == Enum_RefreshEventType.Rune)
        {
            Refresh();
        }
    }
}