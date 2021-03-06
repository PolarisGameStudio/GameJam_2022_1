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
    
    public bool TryLevelUp(TBL_UPGRADE_GOLD type)
    {
        if (!IsEnableLevelUp(type))
        {
            return false;
        }

        if (DataManager.CurrencyData.TryConsume(Enum_CurrencyType.Gold, GetPrice(type)))
        {
            _levels[type.Index]++;
        
            DataManager.AchievementData.ProgressAchievement(Enum_AchivementMission.Loop_LevelUpGoldGrowth);

            CalculateStat();

            return true;
        }
        
        return false;
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
        RefreshEvent.Trigger(Enum_RefreshEventType.Quest);
    }

    
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///
    /// 

    // ?????? N??? ?????? ?????? ????????? ?????? ??? ?????????~ ??????
    // IncreaseGoldCost*(N*(N-1)/2) +GoldCost
    //
    // ????????? ?????? ?????? N??? ?????? ?????? ????????? ?????? ???
    // N^4
    
    public double GetPrice(TBL_UPGRADE_GOLD type)
    {
        double level = _levels[type.Index];

        switch (type.StatType)
        {
            case Enum_StatType.Damage:
            case Enum_StatType.MaxHealth:
            case Enum_StatType.CriticalChance:
            case Enum_StatType.CriticalDamage:
                double increseCost = type.IncreaseGoldCost;
                double cost = type.GoldCost;
                
                return increseCost * (level * (level - 1) / 2) + cost;
                break;
            
            
            case Enum_StatType.SuperCriticalChance:
            case Enum_StatType.SuperCriticalDamage:
                return level * level * level * level;
                break;
            
            default:
                Debug.LogError($"{type.StatType} ?????? ?????? ??????");
                return double.MaxValue;
                break;
        }
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

    public bool IsEnableLevelUp(TBL_UPGRADE_GOLD data)
    {
        if (data.IsLock)
        {
            var conditionData = TBL_UPGRADE_GOLD.GetEntity(data.ConditionIndex);
                
            if (GetLevel(conditionData) < conditionData.MaxLevel)
            {
                return false;
            }
        }

        if (data.MaxLevel <= GetLevel(data))
        {
            return false;
        }
        
        var price = GetPrice(data);

        return DataManager.CurrencyData.IsEnough(Enum_CurrencyType.Gold, price);
    }
}
