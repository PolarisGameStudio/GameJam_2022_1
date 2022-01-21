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
        if (!IsEnableLevelUp(type))
        {
            return;
        }
        
        _levels[type.Index]++;
        
        DataManager.AcheievmentData.ProgressAchievement(Enum_AchivementMission.Loop_LevelUpGoldGrowth);

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
        RefreshEvent.Trigger(Enum_RefreshEventType.Quest);
    }

    
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///
    /// 

    // 레벨 N이 되기 위해 필요한 골드 량 데미지~ 크뎀
    // IncreaseGoldCost*(N*(N-1)/2) +GoldCost
    //
    // 회심의 일격 레벨 N이 되기 위해 필요한 골드 량
    // N^4
    
    public double GetPrice(TBL_UPGRADE_GOLD type)
    {
        var level = _levels[type.Index];

        switch (type.StatType)
        {
            case Enum_StatType.Damage:
            case Enum_StatType.MaxHealth:
            case Enum_StatType.CriticalChance:
            case Enum_StatType.CriticalDamage:
                return type.IncreaseGoldCost * (level * (level - 1) / 2) + type.GoldCost;
                break;
            
            
            case Enum_StatType.SuperCriticalChance:
            case Enum_StatType.SuperCriticalDamage:
                return level * level * level * level;
                break;
            
            default:
                Debug.LogError($"{type.StatType} 얘는 공식 없음");
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
        
        var price = GetPrice(data);

        return DataManager.CurrencyData.IsEnough(Enum_CurrencyType.Gold, price);
    }
}
