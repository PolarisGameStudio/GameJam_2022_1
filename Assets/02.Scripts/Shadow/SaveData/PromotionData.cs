using System;
using UnityEngine;

public class PromotionData : StatData
{
    [SerializeField] private int _currentPromotionIndex;
    public int CurrentPromotionIndex => _currentPromotionIndex;

    [SerializeField] public DiceStatData DiceStatData;
    
    public PromotionData()
    {
        DiceStatData = new DiceStatData();
    }

    public override void ValidCheck()
    {
        base.ValidCheck();

        if (DiceStatData == null)
        {
            DiceStatData = new DiceStatData();
        }
        DiceStatData.Init(TBL_PROMOTION.CountEntities + 1);

        CheckDiceUnlock();
        CalculateStat();

        _currentPromotionIndex = Mathf.Clamp(_currentPromotionIndex, 0, TBL_PROMOTION.CountEntities - 1);
    }
    
    private void CheckDiceUnlock()
    {
        DiceStatData.ActiveDiceSlot(_currentPromotionIndex + 1);
    }

    public void OnClearPromotionBattle(int level)
    {
        _currentPromotionIndex = Mathf.Max(_currentPromotionIndex, level);
        
        CheckDiceUnlock();
        CalculateStat();
        
        RefreshEvent.Trigger(Enum_RefreshEventType.Quest);
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

        StatEvent.Trigger(Enum_StatEventType.StatChange);
        RefreshEvent.Trigger(Enum_RefreshEventType.Promotion);
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