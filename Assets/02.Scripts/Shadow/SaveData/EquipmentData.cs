using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentData : StatData
{
     public List<int> Levels = new List<int>();
     public List<int> Counts = new List<int>();

     public List<int> EquippedIndex = new List<int>() {-1, -1, -1, -1};

     public List<TBL_EQUIPMENT> GetEquipmentListByType(Enum_EquipmentType type)
     {
         return TBL_EQUIPMENT.GetEntitiesByKeyWithEquipmentType(type);
     }

    public override void ValidCheck()
    {
        base.ValidCheck();

        var typeCount = (int) Enum_EquipmentType.Count;

        var saveCount = Levels.Count;

        if (typeCount > saveCount)
        {
            for (int i = saveCount; i < typeCount; i++)
            {
                Levels.Add(0);
                Counts.Add(0);
            }
        }
        
        CalculateStat();
    }

    protected override void InitStat()
    {
    }

    protected override void CalculateStat()
    {
        Stat.Init();

        for (int i = 0; i < Levels.Count; i++)
        {
            var level = Levels[i];
            var data = TBL_EQUIPMENT.GetEntity(i);
            if (level <= 0)
            {
                continue;
            }

            Stat[data.OnOwnStat1] += data.OnOwnValue1 + data.OnOwnIncreaseValue1 * (level - 1);
            Stat[data.OnOwnStat2] += data.OnOwnValue2 + data.OnOwnIncreaseValue2 * (level - 1);
            Stat[data.OnOwnStat3] += data.OnOwnValue3 + data.OnOwnIncreaseValue3 * (level - 1);

            if (i == EquippedIndex[(int) data.Type])
            {
                Stat[data.OnEquipStat] += data.OnEquipVaue + data.OnEquipIncreaseValue * (level - 1);
            }
        }

        StatEvent.Trigger(Enum_StatEventType.StatChange);
        RefreshEvent.Trigger(Enum_RefreshEventType.Equipment);
    }

    public void AddEquipment(int index, int count = 1)
    {
        Counts[index] += count;

        if (Levels[index] == 0)
        {
            Levels[index] = 1;
        }

        var typeIndex = (int)TBL_EQUIPMENT.GetEntity(index).Type;

        if (EquippedIndex[typeIndex] == -1)
        {
            TryEquip(typeIndex, index);
        }
        else
        {
            CalculateStat();
        }
    }
    
    public void AddEquipmentList(List<TBL_EQUIPMENT> list)
    {
        CalculateStat();
    }

    public void TryEquip(int typeIndex, int index)
    {
        if (Levels[index] <= 0)
        {
            return;
        }
        
        EquippedIndex[typeIndex] = index;

        CalculateStat();
    }

    public bool TryGradeUp(int index)
    {
        if (!IsEnableGradeUp(index))
        {
            return false;
        }

        var data = TBL_EQUIPMENT.GetEntity(index);
        
        var gradeUpCount = (int) (Counts[index] / data.GradeUpCost);

        Counts[index] -= data.GradeUpCost * gradeUpCount;

        AddEquipment(GetNextEquipment(index).Index, gradeUpCount);

        DataManager.AcheievmentData.ProgressAchievement(Enum_AchivementMission.Daily_MergeEquipment, gradeUpCount);
        DataManager.AcheievmentData.ProgressAchievement(Enum_AchivementMission.Loop_MergeEquipment, gradeUpCount);

        return true;
    }

    public bool TryLevelUp(int index)
    {
        if (!IsEnableLevelUp(index))
        {
            return false;
        }

        if (DataManager.CurrencyData.TryConsume(Enum_CurrencyType.EquipmentStone, GetCost(index)))
        {
            Levels[index] += 1;
            CalculateStat();

            DataManager.AcheievmentData.ProgressAchievement(Enum_AchivementMission.Loop_LevelUpEquipment, 1);
            return true;
        }
        else
        {
            return false;
        }
    }

    public double GetCost(int index)
    {
        var data = TBL_EQUIPMENT.GetEntity(index);

        var cost = data.LevelUpCost + data.LevelUpIncreaseCost * (Levels[index] - 1);
        
        return cost;
    }

    public bool IsEnableLevelUp(int index)
    {  
        if (Levels.Count <= index || index < 0)
        {
            return false;
        }

        return DataManager.CurrencyData.IsEnough(Enum_CurrencyType.EquipmentStone, GetCost(index));
    }

    public bool IsEnableGradeUp(int index)
    {
        if (Levels.Count <= index || index < 0)
        {
            return false;
        }

        if (Counts[index] < 5)
        {
            return false;
        }

        if (GetNextEquipment(index) == null)
        {
            return false;
        }

        return true;
    }

    public TBL_EQUIPMENT GetNextEquipment(int index)
    {
        var data = TBL_EQUIPMENT.GetEntity(index);

        var typeList = TBL_EQUIPMENT.GetEntitiesByKeyWithEquipmentType(data.Type);
        
        Enum_ItemGrade targetGrade = Enum_ItemGrade.Common;
        int targetStar = 0;
        
        if (data.Star == 4)
        {
            targetGrade = (data.Grade + 1);
            targetStar = 0;
        }
        else
        {
            targetGrade = data.Grade;
            targetStar = data.Star + 1;
        }

        var targetEquipment = typeList.Find(x => x.Grade == targetGrade && x.Star == targetStar);

        return targetEquipment;
    }
}

//
// public class EquipmentGroup
// {
//     public Enum_EquipmentType Type;
//
//     public List<int> Levels = new List<int>();
//     public List<int> Counts = new List<int>();
//
//     public int EquippedIndex = -1;
//
//     public void InitEquipmentGroup(Enum_EquipmentType type)
//     {
//         var equipmentList = TBL_EQUIPMENT.GetEntitiesByKeyWithEquipmentType(type);
//         if (equipmentList == null || equipmentList.Count == 0)
//         {
//             return;
//         }
//
//         var equipmentCount = equipmentList.Count;
//
//         var saveCount = Levels.Count;
//
//         if (equipmentCount > saveCount)
//         {
//             for (int i = saveCount; i < equipmentCount; i++)
//             {
//                 Levels.Add(0);
//                 Counts.Add(0);
//             }
//         }
//     }
// }