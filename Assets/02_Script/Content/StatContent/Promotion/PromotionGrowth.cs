using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromotionGrowth
{
    public Enum_StatType StatType { get; }

    private int _level;
    private int _price;

    private double _value;
    private double _increaseValue;

    public double Level => _level;
    public double Value => _value;

    public PromotionGrowth(Enum_StatType type, int level, int price, double increaseValue)
    {
        StatType = type;
        _level = level;
        _price = price;
        _increaseValue = increaseValue;

        Calculate();
    }

    public bool IsEnableLevelUp()
    {
        return CurrencyManager.Instance.HaveCurrency(Enum_CurrencyType.PromotionStone, _price);
    }

    public bool TryLevelUp()
    {
        if (!CurrencyManager.Instance.TryBuyCurrency(Enum_CurrencyType.PromotionStone, _price))
        {
            return false;
        }

        _level += 1;

        Calculate();

        return true;
    }

    private void Calculate()
    {
        _value = _level * _increaseValue;
    }

    public void ResetLevel()
    {
        _level = 0;
        Calculate();
    }
}
