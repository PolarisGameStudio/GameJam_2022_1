using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum GachaType
{
    Weapon,
    Ring,

    Skill,
    
    Costume,

    Dice,

    Count,
}

[Serializable]
public class GachaData : SaveDataBase
{
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
    }


    public int GetGachaCount(GachaType type)
    {
        return GachaCount[(int) type];
    }

    public int GetGachaLevel(GachaType type)
    {
        List<int> levelConditions = new List<int>();

        switch (type)
        {
            case GachaType.Weapon:
            case GachaType.Ring:
                TBL_GACHA_EQUIPMENT.ForEachEntity(data => levelConditions.Add(data.RequireCount));
                break;

            case GachaType.Skill:
                TBL_GACHA_SKILL.ForEachEntity(data => levelConditions.Add(data.RequireCount));
                break;
        }

        if (levelConditions.Count == 0)
        {
            return 0;
        }

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