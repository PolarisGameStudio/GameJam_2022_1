using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrowth
{
    public Enum_StatType StatType { get; }

    private int _level;
    private int _price;

    private double _value;
    private double _increaseValue;

    public double Value => _value;

    public LevelGrowth(Enum_StatType type, int level, int price, double increaseValue)
    {
        StatType = type;
        _level = level;
        _price = price;
        _increaseValue = increaseValue;

        Calculate();
    }

    public bool IsEnableLevelUp()
    {
        return CurrencyManager.Instance.HaveCurrency(Enum_CurrencyType.LevelPoint, _price);
    }

    public bool TryLevelUp()
    {
        if (!CurrencyManager.Instance.TryBuyCurrency(Enum_CurrencyType.LevelPoint, _price))
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