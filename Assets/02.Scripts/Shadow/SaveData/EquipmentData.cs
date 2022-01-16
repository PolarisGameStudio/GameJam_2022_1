using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentData : StatData
{
    public List<EquipmentGroup> EquipmentGroups = new List<EquipmentGroup>();

    public override void ValidCheck()
    {
        base.ValidCheck();
        
        var statCount = (int)Enum_EquipmentType.Count;

        var saveCount = EquipmentGroups.Count;

        if (statCount > saveCount)
        {
            for (int i = saveCount; i < statCount; i++)
            {
                EquipmentGroups.Add(new EquipmentGroup((Enum_EquipmentType)i));
            }
        }

        CalculateStat();
    }

    protected override void InitStat()
    {
        throw new System.NotImplementedException();
    }

    protected override void CalculateStat()
    {
        RefreshEvent.Trigger(Enum_RefreshEventType.StatChange);
    }
}


public class EquipmentGroup
{
    public Enum_EquipmentType Type;

    public List<int> Levels = new List<int>();

    public int EquippedIndex = -1;

    public EquipmentGroup(Enum_EquipmentType type)
    {
        Type = type;
        
        
    }
}

public class Equipment
{
    
}
