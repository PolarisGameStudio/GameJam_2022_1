using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GoldGrowthData : StatData
{
    [SerializeField] private List<int> _levels = new List<int>();
    public override void ValidCheck()
    {
        base.ValidCheck();

        var statCount = TBL_UPGRADE_GOLD.CountEntities;

        var saveCount = _levels.Count;

        if (statCount > saveCount)
        {
            for (int i = saveCount; i < statCount; i++)
            {
                _levels.Add(0);
            }
        }

        CalculateStat();
    }
    
    public void LevelUp(TBL_UPGRADE_GOLD type)
    {
        _levels[type.Index]++;

        CalculateStat();
    }
    

    protected override void InitStat()
    {
       // throw new System.NotImplementedException();
    }

    protected override void CalculateStat()
    {
        Stat.Init();
        
        for (int i = 0; i < _levels.Count; i++)
        {
            var data = TBL_UPGRADE_GOLD.GetEntity(i);

            Stat[data.StatType] += _levels[i] * data.IncreaseValue;
        }

        StatEvent.Trigger(Enum_StatEventType.StatChange);
        RefreshEvent.Trigger(Enum_RefreshEventType.GoldGrowth);
    }

    
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///
    /// 


    public double GetPrice(TBL_UPGRADE_GOLD type)
    {
        var level = _levels[type.Index];

        return level * type.IncreaseGoldCost + type.GoldCost;
    }    
        
    public int GetLevel(TBL_UPGRADE_GOLD type)
    {
        return _levels[type.Index];
    }    
    
    public double GetValue(TBL_UPGRADE_GOLD type)
    {
        var level = _levels[type.Index];

        return level * type.IncreaseValue;
    } 
    
    public double GetNextValue(TBL_UPGRADE_GOLD type)
    {
        var level = _levels[type.Index];

        return (level + 1) * type.IncreaseValue;
    }

}
