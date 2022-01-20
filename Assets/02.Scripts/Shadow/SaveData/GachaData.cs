using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum GachaType
{
    Weapon,
    Ring,
    
    Dice,
    
    Count,
}

[Serializable]
public class GachaData : SaveDataBase
{
    [NonSerialized] private Dictionary<GachaType, GachaHandler> _gachaHandlers;

    public List<int> GachaCount = new List<int>();

    public override void ValidCheck()
    {
        base.ValidCheck();

        var gachaTypeCount = (int) GachaType.Count;

        var saveCount = GachaCount.Count;

        if (gachaTypeCount > saveCount)
        {
            for (int i = saveCount; i < gachaTypeCount; i++)
            {
                GachaCount.Add(0);
            }
        }

        _gachaHandlers = new Dictionary<GachaType, GachaHandler>();
        
        _gachaHandlers.Add(GachaType.Dice,new DiceGachaHandler());
        
        _gachaHandlers.Add(GachaType.Weapon, new WeaponGachaHandler());
        _gachaHandlers.Add(GachaType.Ring, new RingGachaHandler());
    }
    
    

    public int GetGachaCount(GachaType type)
    {
        return GachaCount[(int)type];
    }

    public int GetGachaLevel(GachaType type)
    {
        List<int> levelConditions = new List<int>();

        TBL_GACHA_EQUIPMENT.ForEachEntity(data => levelConditions.Add(data.RequireCount));

        var currenctGachaCount = GetGachaCount(type);

        int level = 0;

        for (level = 0; level < levelConditions.Count; level++)
        {
            if (currenctGachaCount < levelConditions[level])
            {
                break;
            }
        }

        level = Mathf.Min(level, levelConditions.Count - 1);

        return level;
    }

    public void AddGachaCount(GachaType type, int count)
    {
        int index = (int) type;
        
        if (GachaCount.Count > index && index >= 0)
        {
            GachaCount[index] += count;
        }
    }
}
