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
        DataManager.DungeonData.TryChallenge(Enum_BattleType.TreasureDungeon);
    }
    public void OnClickBossDungeon()
    {
        DataManager.DungeonData.TryChallenge(Enum_BattleType.BossDungeon);
    }
    public void OnClickSmithDungeon()
    {
        DataManager.DungeonData.TryChallenge(Enum_BattleType.SmithDungeon);
    }
}
