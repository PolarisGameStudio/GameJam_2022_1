using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Player_Upgrade_Stat_Slot : UI_BaseSlot<TBL_UPGRADE_STAT>, GameEventListener<RefreshEvent>
{
    [SerializeField] private Image _imgStatIcon;
    
    [SerializeField] private Text _txtStatName;
    //[SerializeField] private Text _txtStatMaxLevel;
    [SerializeField] private Text _txtStatValue;
    [SerializeField] private Text _txtStatCurrenctLevel;
    //[SerializeField] private Text _txtStatPrice;

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

    public override void Init(TBL_UPGRADE_STAT data)
    {
        _data = data;

        Refresh();
    }

    public void Refresh()
    {
        if (_data == null)
        {
            return;
        }
        _imgStatIcon.sprite = AssetManager.Instance.StatIcon[(int)_data.StatType];
        _txtStatName.text = $"{StringValue.GetStatName(_data.StatType)} Max Lv.{_data.MaxLevel}";

        if (_data.StatType == Enum_StatType.CriticalChance || _data.StatType == Enum_StatType.SuperCriticalChance)
        {
            _txtStatValue.text =
                $"{DataManager.StatGrowthData.GetValue(_data):N2}% -> {DataManager.StatGrowthData.GetNextValue(_data):N2}%";
        }
        else if(_data.StatType == Enum_StatType.Damage || _data.StatType == Enum_StatType.MaxHealth)
        {
            _txtStatValue.text =
                $"{DataManager.StatGrowthData.GetValue(_data)} -> {DataManager.StatGrowthData.GetNextValue(_data)}";
        }
        else
        {
            _txtStatValue.text =
                $"{DataManager.StatGrowthData.GetValue(_data):N1}% -> {DataManager.StatGrowthData.GetNextValue(_data):N1}%";
        }

        _txtStatCurrenctLevel.text = $"Lv.{DataManager.StatGrowthData.GetLevel(_data)}";
      //  _txtStatPrice.text = $"{_data.Price}";

        CheckEnableLevelUp();
    }

    public void CheckEnableLevelUp()
    {
        _btnLevelUp.interactable = DataManager.StatGrowthData.GetRemainPoint() >= _data.Price;
    }
    
    public void OnGameEvent(RefreshEvent e)
    {
        if (e.Type == Enum_RefreshEventType.StatGrowth)
        {
            Refresh();
        }
    }

    public void OnClickLevelUp()
    {
        if (DataManager.StatGrowthData.GetRemainPoint() >= _data.Price)
        {
            DataManager.StatGrowthData.LevelUp(_data);
            Refresh();
            SoundManager.Instance.PlaySound("ui_levelup_button");
        }
    }
}