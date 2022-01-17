using System;
using UnityEngine;

public class PromotionData : StatData
{
    [SerializeField] private int _currentPromotionIndex;

    [NonSerialized] public bool IsMaxPromotion;

    public override void ValidCheck()
    {
        base.ValidCheck();

        CalculateStat();

        _currentPromotionIndex = Mathf.Max(0, _currentPromotionIndex);
    }

    public void OnClearPromotionBattle()
    {
        _currentPromotionIndex++;

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