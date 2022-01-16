using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Player_Upgrade_Gold_Slot : UI_BaseSlot<TBL_UPGRADE_GOLD> ,GameEventListener<RefreshEvent>
{
    [SerializeField] private Image _imgStatIcon;

    [SerializeField] private Text _txtStatName;
    [SerializeField] private Text _txtStatMaxLevel;
    [SerializeField] private Text _txtStatValue;
    [SerializeField] private Text _txtStatCurrenctLevel;
    [SerializeField] private Text _txtStatPrice;

    [SerializeField]  private Button _btnLevelUp;

    private void OnEnable()
    {
        this.AddGameEventListening<RefreshEvent>();

        if (_data != null)
        {
            CheckEnableLevelUp();
        }
    }

    private void OnDisable()
    {
        this.RemoveGameEventListening<RefreshEvent>();
    }

    public override void Init(TBL_UPGRADE_GOLD data)
    {
        _data = data;

        Refresh();
    }

    private void Refresh()
    {
        _imgStatIcon.sprite = null;
        
        _txtStatName.text = $"{_data.StatType}";
        _txtStatMaxLevel.text = $"Max Lv.{_data.MaxLevel}";

        if (_data.StatType == Enum_StatType.CriticalChance || _data.StatType == Enum_StatType.SuperCriticalChance)
        {
            _txtStatValue.text =
                $"{DataManager.GoldGrowthData.GetValue(_data):N1} -> {DataManager.GoldGrowthData.GetNextValue(_data):N1}";    
        }
        else
        {
            _txtStatValue.text =
                $"{DataManager.GoldGrowthData.GetValue(_data)} -> {DataManager.GoldGrowthData.GetNextValue(_data)}";    
        }
        
        _txtStatCurrenctLevel.text = $"Lv.{DataManager.GoldGrowthData.GetLevel(_data)}";
        _txtStatPrice.text = $"{DataManager.GoldGrowthData.GetPrice(_data)}";

        CheckEnableLevelUp();
    }

    public void CheckEnableLevelUp()
    {
        var price = DataManager.GoldGrowthData.GetPrice(_data);
        
        _btnLevelUp.interactable = DataManager.CurrencyData.IsEnough(Enum_CurrencyType.Gold, price);
    }

    public void OnGameEvent(RefreshEvent e)
    {
        if (e.Type == Enum_RefreshEventType.Currency)
        {
            CheckEnableLevelUp();
        }
    }

    public void OnClickLevelUp()
    {
        var price = DataManager.GoldGrowthData.GetPrice(_data);
        
        if(DataManager.CurrencyData.TryConsume(Enum_CurrencyType.Gold, price))
        {
            DataManager.GoldGrowthData.LevelUp(_data);
            Refresh();
        }
    }
}