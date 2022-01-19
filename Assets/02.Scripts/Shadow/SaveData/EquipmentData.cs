using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentData : StatData
{
    public List<EquipmentGroup> EquipmentGroups = new List<EquipmentGroup>();


    private Dictionary<Enum_EquipmentType, List<TBL_EQUIPMENT>> _dataDict =
        new Dictionary<Enum_EquipmentType, List<TBL_EQUIPMENT>>();

    public override void ValidCheck()
    {
        base.ValidCheck();

        var typeCount = (int) Enum_EquipmentType.Count;

        var saveCount = EquipmentGroups.Count;

        if (typeCount > saveCount)
        {
            for (int i = saveCount; i < typeCount; i++)
            {
                var group = new EquipmentGroup();
                group.InitEquipmentGroup((Enum_EquipmentType) i);
                EquipmentGroups.Add(group);
            }
        }

        for (int i = 0; i < typeCount; i++)
        {
            var type = (Enum_EquipmentType) i;

            _dataDict.Add(type, TBL_EQUIPMENT.GetEntitiesByKeyWithEquipmentType(type));
        }


        CalculateStat();
    }

    protected override void InitStat()
    {
    }

    protected override void CalculateStat()
    {
        var typeCount = (int) Enum_EquipmentType.Count;

        Stat.Init();

        for (int i = 0; i < typeCount; i++)
        {
            var type = (Enum_EquipmentType) i;

            var group = GetGroupByType(type);

            var dataList = _dataDict[type];

            for (int j = 0; i < group.Levels.Count; i++)
            {
                var level = group.Levels[i];

                if (level <= 0)
                {
                    continue;
                }
                
                Stat[dataList[i].OnOwnStat1] = dataList[i].OnOwnValue1;
                Stat[dataList[i].OnOwnStat2] = dataList[i].OnOwnValue2;
                Stat[dataList[i].OnOwnStat3] = dataList[i].OnOwnValue3;

                if (group.EquippedIndex == i)
                {
                    Stat[dataList[i].OnEquipStat] += dataList[i].OnEquipVaue;
                }
            }
        }

        RefreshEvent.Trigger(Enum_RefreshEventType.StatChange);        
        RefreshEvent.Trigger(Enum_RefreshEventType.Equipment);
    }

    public void AddEquipment(Enum_EquipmentType type, int index, int count = 1)
    {
        var group = GetGroupByType(type);
        
        if (group == null || index >= group.Levels.Count)
        {
            return;
        }

        group.Counts[index] += count;

        if (group.Levels[index] == 0)
        {
            group.Levels[index] = 1;
        }

        if (group.EquippedIndex == -1)
        {
            TryEquip(type, index);
        }
        else
        {
            CalculateStat();   
        }
    }

    public EquipmentGroup GetGroupByType(Enum_EquipmentType type)
    {
        var group = EquipmentGroups.Find(x => x.Type == type);

        return group;
    }

    public void TryEquip(Enum_EquipmentType type, int index)
    {
        var group = GetGroupByType(type);

        if (group == null || index >= group.Levels.Count || group.Levels[index] <= 0)
        {
            return;
        }

        group.EquippedIndex = index;

        CalculateStat();
    }

    public bool TryGradeUp(Enum_EquipmentType type, int index)
    {
        var group = GetGroupByType(type);

        if (group == null || index >= group.Levels.Count)
        {
            return false;
        }

        if (!IsEnableGradeUp(type, index))
        {
            return false;
        }

        var dataList = _dataDict[type];

        group.Counts[index] -= dataList[index].GradeUpCost;
        
        AddEquipment(type,index + 1, 1);

        return true;
    }

    public bool TryLevelUp(Enum_EquipmentType type, int index)
    {
        var group = GetGroupByType(type);

        if (group == null || index >= group.Levels.Count)
        {
            return false;
        }

        if (!IsEnableLevelUp(type, index))
        {
            return false;
        }

        var dataList = _dataDict[type];

        var cost = dataList[index].LevelUpCost + dataList[index].LevelUpIncreaseCost * group.Levels[index];

        if (DataManager.CurrencyData.TryConsume(Enum_CurrencyType.EquipmentStone, cost))
        {
            group.Levels[index] += 1;
            CalculateStat();

            return true;
        }
        else
        {
            return false;
        }
    }


    public bool IsEnableLevelUp(Enum_EquipmentType type, int index)
    {
        var group = GetGroupByType(type);

        if (group == null || index >= group.Levels.Count)
        {
            return false;
        }

        var dataList = _dataDict[type];

        var cost = dataList[index].LevelUpCost + dataList[index].LevelUpIncreaseCost * group.Levels[index];

        return DataManager.CurrencyData.IsEnough(Enum_CurrencyType.EquipmentStone, cost);
    }

    public bool IsEnableGradeUp(Enum_EquipmentType type, int index)
    {
        var group = GetGroupByType(type);

        if (group == null || index >= group.Levels.Count)
        {
            return false;
        }

        if (group.Levels.Count <= index - 1)
        {
            return false;
        }

        var dataList = _dataDict[type];

        return group.Counts[index] >= dataList[index].GradeUpCost;
    }
}


public class EquipmentGroup
{
    public Enum_EquipmentType Type;

    public List<int> Levels = new List<int>();
    public List<int> Counts = new List<int>();

    public int EquippedIndex = -1;

    public void InitEquipmentGroup(Enum_EquipmentType type)
    {
        var equipmentList = TBL_EQUIPMENT.GetEntitiesByKeyWithEquipmentType(type);
        if (equipmentList == null || equipmentList.Count == 0)
        {
            return;
        }
        
        var equipmentCount = equipmentList.Count;

        var saveCount = Levels.Count;

        if (equipmentCount > saveCount)
        {
            for (int i = saveCount; i < equipmentCount; i++)
            {
                Levels.Add(0);
                Counts.Add(0);
            }
        }
    }
}