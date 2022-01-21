using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum GachaType
{
    Weapon,
    Ring,

    Skill,

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

    public int GetPreRequireExp(GachaType type)
    {
        var level = GetGachaLevel(type);

        if (level <= 0)
        {
            return 0;
        }

        switch (type)
        {
            case GachaType.Weapon:
            case GachaType.Ring:
                return TBL_GACHA_EQUIPMENT.GetEntity(level - 1).RequireCount;

            case GachaType.Skill:
                return TBL_GACHA_SKILL.GetEntity(level - 1).RequireCount;
        }

        return 0;
    }

    public int GetNextRequireExp(GachaType type)
    {
        var level = GetGachaLevel(type);

        switch (type)
        {
            case GachaType.Weapon:
            case GachaType.Ring:
                if (level >= TBL_GACHA_EQUIPMENT.CountEntities - 1)
                {
                    return int.MaxValue;
                }

                return TBL_GACHA_EQUIPMENT.GetEntity(level).RequireCount;

            case GachaType.Skill:
                if (level >= TBL_GACHA_SKILL.CountEntities - 1)
                {
                    return int.MaxValue;
                }

                return TBL_GACHA_SKILL.GetEntity(level).RequireCount;
        }

        return 0;
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

        RefreshEvent.Trigger(Enum_RefreshEventType.Gacha);
    }
}