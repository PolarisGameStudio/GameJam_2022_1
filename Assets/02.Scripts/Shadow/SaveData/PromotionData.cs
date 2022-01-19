using System;
using UnityEngine;

public class PromotionData : StatData
{
    [SerializeField] private int _currentPromotionIndex;

    [SerializeField] private DiceStatData _diceStatData;
    
    public PromotionData()
    {
        _diceStatData = new DiceStatData();
    }

    public override void ValidCheck()
    {
        base.ValidCheck();

        if (_diceStatData == null)
        {
            _diceStatData = new DiceStatData();
        }
        _diceStatData.Init(TBL_PROMOTION.CountEntities + 1);

        CheckDiceUnlock();
        CalculateStat();

        _currentPromotionIndex = Mathf.Clamp(_currentPromotionIndex, 0, TBL_PROMOTION.CountEntities);
    }
    
    private void CheckDiceUnlock()
    {
        _diceStatData.ActiveDiceSlot(_currentPromotionIndex);
    }

    public void OnClearPromotionBattle()
    {
        _currentPromotionIndex++;
        
        CheckDiceUnlock();
        CalculateStat();
    }

    protected override void CalculateStat()
    {
        Stat.Init();

        Stat[Enum_StatType.Damage] = 1;
        Stat[Enum_StatType.MaxHealth] = 1;

        for (int i = 0; i <= _currentPromotionIndex; i++)
        {
            if (i >= TBL_PROMOTION.CountEntities)
            {
                break;
            }

            var data = TBL_PROMOTION.GetEntity(i);

            if (data.DamageMultipleValue >= 1)
            {
                Stat[Enum_StatType.Damage] *= data.DamageMultipleValue;
            }

            if (data.HealthMultipleValue >= 1)
            {
                Stat[Enum_StatType.MaxHealth] *= data.HealthMultipleValue;
            }
        }

        RefreshEvent.Trigger(Enum_RefreshEventType.StatChange);
    }

    public bool IsEnableChallenge(int index)
    {
        return _currentPromotionIndex + 1 == index;
    }

    public bool IsAlreadyClear(int index)
    {
        return _currentPromotionIndex >= index;
    }

    public bool TryChallenge(int index)
    {
        if (!IsEnableChallenge(index))
        {
            return false;
        }
        
        BattleManager.Instance.BattleStart(Enum_BattleType.PromotionBattle, index);

        return true;
    }
}