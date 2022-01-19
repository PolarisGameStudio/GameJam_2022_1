using System.Collections.Generic;
using UnityEngine;

public class FollowerData : StatData
{
    public List<int> Levels = new List<int>();
    public List<int> Counts = new List<int>();

    public List<int> EquippedIndex = new List<int>() {-1, -1, -1, -1};

    public List<DiceStatData> DiceStatList = new List<DiceStatData>();

    public override void ValidCheck()
    {
        base.ValidCheck();

        var maxLevel = TBL_FOLLOWER.GetEntity(0).MaxLevel;

        var requireCount = TBL_FOLLOWER.CountEntities;
        var saveCount = Levels.Count;


        if (requireCount > saveCount)
        {
            for (int i = saveCount; i < requireCount; i++)
            {
                Levels.Add(0);
                Counts.Add(0);

                var diceStat = new DiceStatData();
                diceStat.Init(maxLevel);
                DiceStatList.Add(diceStat);
            }
        }

        CheckDiceUnlock();

        CalculateStat();
    }

    public bool IsEnableLevelUp(int index)
    {
        var data = TBL_FOLLOWER.GetEntity(index);
        var level = Levels[index];

        if (level >= data.MaxLevel)
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
        ;
    }

    public void TryLevelUp(int index)
    {
        if (IsEnableLevelUp(index))
        {
            return;
        }

        var data = TBL_FOLLOWER.GetEntity(index);
        var level = Levels[index];

        Counts[index] -= GetLevelUpCost(index);
        Levels[index] += 1;
        
        CalculateStat();
    }

    public void TryEquip(int index, int changeSlotIndex)
    {
        EquippedIndex[changeSlotIndex] = index;
        
        CalculateStat();
    }

    public void TryUnEquip(int index)
    {
        var equippedIndex = EquippedIndex.FindIndex(x => x == index);

        if (equippedIndex != -1 && EquippedIndex.Count < equippedIndex)
        {
            EquippedIndex[equippedIndex] = -1;
        }

        CalculateStat();
    }


    private void CheckDiceUnlock()
    {
        for (int i = 0; i < DiceStatList.Count; i++)
        {
            DiceStatList[i].ActiveDiceSlot(Levels[i]);
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

        RefreshEvent.Trigger(Enum_RefreshEventType.StatChange);
        RefreshEvent.Trigger(Enum_RefreshEventType.Follower);
    }
}