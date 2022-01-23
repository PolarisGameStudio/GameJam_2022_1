using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Dungeon : SingletonBehaviour<UI_Dungeon>
{
    public void Close()
    {
        SafeSetActive(false);
    }

    public void Open()
    {
        SafeSetActive(true);
    }

    public void OnClickTreasureDungeon()
    {
        UI_Popup_Dungeon_Treasure.Instance.Open();
    }
    public void OnClickBossDungeon()
    {
        UI_Popup_Dungeon_Boss.Instance.Open();
    }
    public void OnClickSmithDungeon()
    {
        UI_Popup_Dungeon_Smith.Instance.Open();
    }
}
