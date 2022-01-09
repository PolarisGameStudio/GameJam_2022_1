using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Player_StatSlot : MonoBehaviour
{
    [SerializeField] private Enum_UpgradeStat _upgradeType;

    [SerializeField] private Text _txtStatName;
    [SerializeField] private Text _txtStatValue;
    [SerializeField] private Text _txtStatNextValue;
    [SerializeField] private Text _txtPrice;

    [SerializeField] private Button _btnLevelup;

    private double _price;

    private void OnEnable()
    {
        Refresh();
    }


    public void Refresh()
    {
        _txtStatName.text = _upgradeType.ToString();

        var statValue = GoldGrowthManager.Instance.GetStatValue(_upgradeType);
        var nextValue = GoldGrowthManager.Instance.GetNextStatValue(_upgradeType);

        if (_upgradeType == Enum_UpgradeStat.criticalChanceLevel ||
            _upgradeType == Enum_UpgradeStat.criticalDamageLevel)
        {
            _txtStatValue.text = $"{statValue * 100:N2}%";
            _txtStatNextValue.text = $"{nextValue * 100:N2}%";
        }
        else
        {
            _txtStatValue.text = $"{statValue}";
            _txtStatNextValue.text = $"{nextValue}";
        }

        _price = GoldGrowthManager.Instance.GetPrice(_upgradeType);
        _txtPrice.text = _price.ToString();

        // _btnLevelup.interactable = CurrencyManager.Instance.HaveGold(_price);

        //_txtPrice.text = _statType.ToString();
    }

    public void OnClickButtonLevelUp()
    {
        // GoldGrowthManager.Instance.TryLevelUp(_upgradeType);
        // Refresh();
        if (CurrencyManager.Instance.TryBuyGold(_price))
        {
            GoldGrowthManager.Instance.TryLevelUp(_upgradeType);
            Refresh();
        }
    }
}