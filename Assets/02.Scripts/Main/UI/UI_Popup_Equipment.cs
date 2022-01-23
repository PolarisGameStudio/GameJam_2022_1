using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Popup_Equipment : UI_BasePopup<UI_Popup_Equipment>, GameEventListener<RefreshEvent>
{
    public UI_Equipment_List_Slot selectEquipmentSlot;
    public UI_Equipment_List_Slot nextEquipmentSlot;

    public Text _txtGrade;
    public Text _txtName;

    public Text _txtEquipStat;
    public Text _txtEquipStatValue;

    public Text _txtOwnStat1;
    public Text _txtOwnStatValue1;

    public Text _txtOwnStat2;
    public Text _txtOwnStatValue2;

    public Text _txtOwnStat3;
    public Text _txtOwnStatValue3;

    public Text _txtPrice;
    
    public Button _btnLevelUp;
    public Button _btnEquip;
    public Button _btnGradeUp;

    private bool _isGradeToggle = false;

    public GameObject OnLevelUpToggle;
    public GameObject OnGradeUpToggle;

    private TBL_EQUIPMENT _data;

    protected void OnEnable()
    {
        this.AddGameEventListening<RefreshEvent>();
    }

    protected void OnDisable()
    {
        this.RemoveGameEventListening<RefreshEvent>();
    }
    
    public void Toggle(bool isOn)
    {
        _isGradeToggle = isOn;
        Refresh();
    }
    
    public void OnNextEquipmentClick()
    {
        if (_data.Index >= TBL_EQUIPMENT.CountEntities - 1)
        {
            return;
        }

        var next = TBL_EQUIPMENT.GetEntity(_data.Index + 1);

        if (next.Type != _data.Type)
        {
            return;
        }
        
        _data = next;
    }

    public void OnPreEquipmentClick()
    {
        if (_data.Index <= 0)
        {
            return;
        }

        var next = TBL_EQUIPMENT.GetEntity(_data.Index - 1);

        if (next.Type != _data.Type)
        {
            return;
        }
        
        _data = next;
    }

    public void Open(TBL_EQUIPMENT data)
    {
        _data = data;
        base.Open();
    }

    public void OnClickLevelUp()
    {
        if (DataManager.EquipmentData.TryLevelUp(_data.Index))
        {
            selectEquipmentSlot.Init(_data);
            InitLevelUpPanel();
        }
    }

    public void OnClickTryEquip()
    {
        if (DataManager.EquipmentData.Levels[_data.Index] <= 0)
        {
            return;
        }

        DataManager.EquipmentData.TryEquip((int) _data.Type, _data.Index);
    }

    public void OnClickGradeUp()
    {
        if (DataManager.EquipmentData.TryGradeUp(_data.Index))
        {
            selectEquipmentSlot.Init(_data);
            InitGradeUpPanel();
        }
    }

    public void InitGradeUpPanel()
    {
        var next = DataManager.EquipmentData.GetNextEquipment(_data.Index);
        if (next == null)
        {
            nextEquipmentSlot.SafeSetActive(false);
        }
        else
        {
            nextEquipmentSlot.SafeSetActive(true);
            nextEquipmentSlot.Init(next);
        }
        _btnGradeUp.interactable = DataManager.EquipmentData.IsEnableGradeUp(_data.Index);
    }

    public void InitLevelUpPanel()
    {
        var level = DataManager.EquipmentData.Levels[_data.Index];

        _txtEquipStat.text = $"{_data.OnEquipStat}";
        _txtEquipStatValue.text =
            $"{_data.OnEquipVaue + _data.OnEquipIncreaseValue * (level - 1)} -> {_data.OnEquipVaue + _data.OnEquipIncreaseValue * (level)}";

        if (_data.OnOwnValue1 == 0)
        {
            _txtOwnStat1.text = "";
            _txtOwnStatValue1.text = "";
        }
        else
        {
            _txtOwnStat1.text = $"{_data.OnOwnStat1}";
            _txtOwnStatValue1.text =
                $"{_data.OnOwnValue1 + _data.OnOwnIncreaseValue1 * (level - 1)} -> {_data.OnOwnValue1 + _data.OnOwnIncreaseValue1 * (level)}";
        }

        if (_data.OnOwnValue2 == 0)
        {
            _txtOwnStat2.text = "";
            _txtOwnStatValue2.text = "";
        }
        else
        {
            _txtOwnStat2.text = $"{_data.OnOwnStat2}";
            _txtOwnStatValue2.text =
                $"{_data.OnOwnValue2 + _data.OnOwnIncreaseValue2 * (level - 1)} -> {_data.OnOwnValue2 + _data.OnOwnIncreaseValue2 * (level)}";
        }

        if (_data.OnOwnValue3 == 0)
        {
            _txtOwnStat3.text = "";
            _txtOwnStatValue3.text = "";
        }
        else
        {
            _txtOwnStat3.text = $"{_data.OnOwnStat3}";
            _txtOwnStatValue3.text =
                $"{_data.OnOwnValue3 + _data.OnOwnIncreaseValue3 * (level - 1)} -> {_data.OnOwnValue3 + _data.OnOwnIncreaseValue3 * (level)}";
        }

        var price = DataManager.EquipmentData.GetCost(_data.Index);
        
        _txtPrice.text = price.ToCurrencyString();
        _btnLevelUp.interactable = DataManager.EquipmentData.IsEnableLevelUp(_data.Index);
        
        _btnEquip.interactable = DataManager.EquipmentData.Levels[_data.Index] > 0;
    }
    

    protected override void Refresh()
    {
        selectEquipmentSlot.Init(_data);

        _txtGrade.text = StringValue.GetGradeName(_data.Grade);
        _txtName.text = $"{_data.name}";

        OnLevelUpToggle.SetActive(!_isGradeToggle);
        OnGradeUpToggle.SetActive(_isGradeToggle);

        if (!_isGradeToggle)
        {
            InitLevelUpPanel();
        }
        else
        {
            InitGradeUpPanel();
        }
    }

    public void OnGameEvent(RefreshEvent e)
    {
        if (e.Type == Enum_RefreshEventType.Equipment)
        {
            Refresh();
        }
    }
}