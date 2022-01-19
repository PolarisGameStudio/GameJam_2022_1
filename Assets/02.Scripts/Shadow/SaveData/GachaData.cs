using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum GachaType
{
    Weapon_L,
    Weapon_R,
    Weapon_M,
    
    Ring,
    Neckless,
    
    Count,
}

[Serializable]
public class GachaData : SaveDataBase
{
    [NonSerialized] private Dictionary<GachaType, GachaHandler<int>> _gachaHandlers;

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

        _gachaHandlers = new Dictionary<GachaType, GachaHandler<int>>();
        
        //_gachaHandlers.Add(GachaType, new Weapon_L_GachaHandler());
        //_gachaHandlers.Add(GachaType, new Weapon_R_GachaHandler());
        //_gachaHandlers.Add(GachaType, new Weapon_M_GachaHandler());
        //_gachaHandlers.Add(GachaType, new Ring_GachaHandler());
        //_gachaHandlers.Add(GachaType, new Weapon_M_GachaHandler());
        
        
        //_gachaHandlers.Add(GachaType, new Skill_GachaHandler());
        //_gachaHandlers.Add(GachaType, new Rune_GachaHandler());
    }

    public void Gacha(GachaType type, int count)
    {
        _gachaHandlers[type].Gacha(count);

        GachaCount[(int) type] += count;
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
    
    
    public int GetRandomStarCount()
    {
        int random = Random.Range(0, 100);

        if (random < 40)
        {
            return 0;
        }
        else if (random < 70)
        {
            return 1;   
        }        
        else if (random < 90)
        {
            return 2;   
        }
        else
        {
            return 3;
        }
    }
}
