using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StatGrowthData : StatData
{    
    [SerializeField] private List<int> _levels = new List<int>();

    private int _remainPoint;
    public int RemainPoint => _remainPoint;
    
    public override void ValidCheck()
    {
        base.ValidCheck();

        var statCount = TBL_UPGRADE_STAT.CountEntities;

        var saveCount = _levels.Count;

        if (statCount > saveCount)
        {
            for (int i = saveCount; i < statCount; i++)
            {
                _levels.Add(0);
            }
        }

        CalculateStat();
        CalculateStatPoint();
    }
    
    public void LevelUp(TBL_UPGRADE_STAT type)
    {
        _levels[type.Index]++;

        CalculateStat();
        CalculateStatPoint();
    }
    protected override void InitStat()
    {
    }

    protected override void CalculateStat()
    {
        Stat.Init();
        
        for (int i = 0; i < _levels.Count; i++)
        {
            var data = TBL_UPGRADE_STAT.GetEntity(i);

            Stat[data.StatType] += _levels[i] * data.IncreaseValue;
        }
        
        StatEvent.Trigger(Enum_StatEventType.StatChange);
        RefreshEvent.Trigger(Enum_RefreshEventType.StatGrowth);
    }

    public void CalculateStatPoint()
    {
        var usedPoint = 0;

        for (int i = 0; i < _levels.Count; i++)
        {
            usedPoint += _levels[i] * TBL_UPGRADE_STAT.GetEntity(i).Price;
        }
        
        _remainPoint = DataManager.PlayerData.Level - usedPoint;
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///
    

    public int GetRemainPoint()
    {
        return _remainPoint;
    }

    
    public int GetLevel(TBL_UPGRADE_STAT type)
    {
        return _levels[type.Index];
    }    
    
    public double GetValue(TBL_UPGRADE_STAT type)
    {
        var level = _levels[type.Index];

        return level * type.IncreaseValue;
    } 
    
    public double GetNextValue(TBL_UPGRADE_STAT type)
    {
        var level = _levels[type.Index];

        return (level + 1) * type.IncreaseValue;
    }

    public void TryRollback()
    {
        for (var i = 0; i < _levels.Count; i++)
        {
            _levels[i] = 0;
        }
        
        CalculateStat();
        CalculateStatPoint();
    }
}
