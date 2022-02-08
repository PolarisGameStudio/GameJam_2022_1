using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Player_Upgrade_Gold_Slot : UI_BaseSlot<TBL_UPGRADE_GOLD>, GameEventListener<RefreshEvent>
{
    [SerializeField] private Image _imgStatIcon;

    [SerializeField] private Text _txtStatName;

    //[SerializeField] private Text _txtStatMaxLevel;
    [SerializeField] private Text _txtStatValue;
    [SerializeField] private Text _txtStatCurrenctLevel;
    [SerializeField] private Text _txtStatPrice;


    [SerializeField] private GameObject _onLockObject;
    [SerializeField] private Text _txtUnlockCondition;

    [SerializeField] private Button _btnLevelUp;

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
        _imgStatIcon.sprite = AssetManager.Instance.StatIcon[(int)_data.StatType];

        _txtStatName.text = $"{StringValue.GetStatName(_data.StatType)} Max Lv.{_data.MaxLevel}";
        //_txtStatMaxLevel.text = $"Max Lv.{_data.MaxLevel}";

        if (_data.StatType == Enum_StatType.CriticalChance || _data.StatType == Enum_StatType.SuperCriticalChance)
        {
            _txtStatValue.text =
                $"{DataManager.GoldGrowthData.GetValue(_data):N2}% -> {DataManager.GoldGrowthData.GetNextValue(_data):N2}%";
        }
        else if(_data.StatType == Enum_StatType.Damage || _data.StatType == Enum_StatType.MaxHealth)
        {
            _txtStatValue.text =
                $"{DataManager.GoldGrowthData.GetValue(_data)} -> {DataManager.GoldGrowthData.GetNextValue(_data)}";
        }
        else
        {
            _txtStatValue.text =
                $"{DataManager.GoldGrowthData.GetValue(_data):N1}% -> {DataManager.GoldGrowthData.GetNextValue(_data):N1}%";
        }

        _txtStatCurrenctLevel.text = $"Lv.{DataManager.GoldGrowthData.GetLevel(_data)}";

        var price = DataManager.GoldGrowthData.GetPrice(_data);
        _txtStatPrice.text = $"{price.ToCurrencyString()}";

        CheckEnableLevelUp();
    }

    public void CheckEnableLevelUp()
    {
        bool isEnable = DataManager.GoldGrowthData.IsEnableLevelUp(_data);
        _btnLevelUp.interactable = isEnable;
        _txtStatPrice.color = isEnable ? ColorValue.ENABLE_TEXT_COLOR :ColorValue.DISABLE_TEXT_COLOR;
        
        if (_data.IsLock)
        {
            var targetData = TBL_UPGRADE_GOLD.GetEntity(_data.ConditionIndex);
            var isLock = DataManager.Container.GoldGrowthData.GetLevel(targetData) < targetData.MaxLevel;
            
            _onLockObject.gameObject.SetActive(isLock);
            _txtUnlockCondition.text = $"{StringValue.GetStatName(targetData.StatType)} {targetData.MaxLevel}레벨 달성 시 해금"; 
        }
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
        if (DataManager.GoldGrowthData.TryLevelUp(_data))
        {
            Refresh();
            SoundManager.Instance.PlaySound("ui_levelup_button");
        }
    }
}