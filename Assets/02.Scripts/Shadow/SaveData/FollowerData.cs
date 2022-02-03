using System.Collections.Generic;
using UnityEngine;

public class FollowerData : StatData
{
    public List<int> Levels = new List<int>();
    public List<int> Counts = new List<int>();

    public List<int> EquippedIndex = new List<int>() {-1, -1, -1, -1};

    public List<DiceStatData> DiceDatas = new List<DiceStatData>();

    public override void ValidCheck()
    {
        base.ValidCheck();

        var maxLevel = SystemValue.FOLLOWER_MAX_LEVEL;

        var requireCount = TBL_FOLLOWER.CountEntities;
        var saveCount = Levels.Count;


        if (requireCount > saveCount)
        {
            for (int i = saveCount; i < requireCount; i++)
            {
                Levels.Add(0);
                Counts.Add(0);

                var diceStat = new DiceStatData();
                DiceDatas.Add(diceStat);
            }
        }
        
        for (var i = 0; i < DiceDatas.Count; i++)
        {
            DiceDatas[i].Init(maxLevel);
        }

        CheckDiceUnlock();

        CalculateStat();
    }

    public bool IsEnableLevelUp(int index)
    {
        var data = TBL_FOLLOWER.GetEntity(index);
        var level = Levels[index];

        if (level >= SystemValue.FOLLOWER_MAX_LEVEL)
        {
            return false;
        }

        return Counts[index] >= GetLevelUpCost(index);
    }

    public int GetLevelUpCost(int index)
    {
        var data = TBL_FOLLOWER.GetEntity(index);
        var level = Levels[index];

        return level == 0 ? data.UnlockCost : data.LevelUpCost + data.IncreaseCost * (level - 1);
    } 

    public bool TryLevelUp(int index)
    {
        if (!IsEnableLevelUp(index))
        {
            return false;
        }

        var data = TBL_FOLLOWER.GetEntity(index);
        var level = Levels[index];

        Counts[index] -= GetLevelUpCost(index);
        Levels[index] += 1;

        CheckDiceUnlock();
        
        CalculateStat();
        RefreshEvent.Trigger(Enum_RefreshEventType.Follower);

        return true;
    }

    public void TryEquip(int index, int changeSlotIndex)
    {
        var alreadyEquip = EquippedIndex.FindIndex(x => x == index);
        if (alreadyEquip != -1)
        {
            EquippedIndex[alreadyEquip] = -1;
        }
        
        EquippedIndex[changeSlotIndex] = index;

        CalculateStat();
        RefreshEvent.Trigger(Enum_RefreshEventType.Follower);
        PlayerEvent.Trigger(Enum_PlayerEventType.EquipFollower);
    }
    
    public void AddFollower(int rewardValue, int rewardCount)
    {
        if (Counts.Count > rewardValue)
        {
            Counts[rewardValue] += rewardCount;
        }
        
        RefreshEvent.Trigger(Enum_RefreshEventType.Follower);
    }

    public void TryUnEquip(int index)
    {
        var equippedIndex = EquippedIndex.FindIndex(x => x == index);

        if (equippedIndex != -1 && EquippedIndex.Count < equippedIndex)
        {
            EquippedIndex[equippedIndex] = -1;
        }

        CalculateStat();
        RefreshEvent.Trigger(Enum_RefreshEventType.Follower);
    }


    public void CheckDiceUnlock()
    {
        for (int i = 0; i < DiceDatas.Count; i++)
        {
            DiceDatas[i].ActiveDiceSlot(Levels[i]);
        }
    }

    protected override void CalculateStat()
    {
        Stat.Init();

        for (int i = 0; i < Levels.Count; i++)
        {
            var level = Levels[i];

            if (level == 0)
            {
                return;
            }

            var data = TBL_FOLLOWER.GetEntity(i);

            Stat[data.StatType1] += data.DefaultValue1 + (data.IncreaseValue1 * level);
            Stat[data.StatType2] += data.DefaultValue2 + (data.IncreaseValue2 * level);
        }

        StatEvent.Trigger(Enum_StatEventType.StatChange);
        RefreshEvent.Trigger(Enum_RefreshEventType.Follower);
    }

    public bool TryRoll(int dataIndex)
    {
        if (DiceDatas[dataIndex].TryRoll())
        {
            return true;
        }

        return false;
    }

    public double GetStatValue(Enum_StatType type)
    {
        double value = 0;

        for (var i = 0; i < EquippedIndex.Count; i++)
        {
            if (EquippedIndex[i] < 0)
            {
                continue;
            }
            
            value += DiceDatas[EquippedIndex[i]].Stat[type];
        }

        return value;
    }
}