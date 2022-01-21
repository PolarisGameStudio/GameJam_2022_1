using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Popup_Follower : UI_BasePopup<UI_Popup_Follower>
{
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

    private bool _isDiceToggle = false;

    public GameObject OnLevelUpToggle;
    public GameObject OnDiceToggle;

    public List<UI_DiceStat_Slot> DiceSlotList;

    private TBL_FOLLOWER _data;

    public void Toggle(bool isOn)
    {
        _isDiceToggle = isOn;
        Init(_data);
    }

    protected override void Awake()
    {
        base.Awake();
        gameObject.SetActive(false);
    }
    
    public void Open(TBL_FOLLOWER data)
    {
        SafeSetActive(true);
        Init(data);
    }

    public void Close()
    {
        SafeSetActive(false);
    }

    private void Init(TBL_FOLLOWER data)
    {
        _data = data;

        //_txtGrade.text = $"{_data}";
        _txtGrade.text = "";
        _txtName.text = $"{_data.name}";

        OnLevelUpToggle.SetActive(!_isDiceToggle);
        OnDiceToggle.SetActive(_isDiceToggle);

        if (!_isDiceToggle)
        {
            InitLevelUpPanel();
        }
        else
        {
            InitDicePanel();
        }
    }

    public void OnClickLevelUp()
    {
        if (DataManager.FollowerData.TryLevelUp(_data.Index))
        {
            InitLevelUpPanel();
        }
    }

    public void OnClickTryEquip()
    {
        if (DataManager.FollowerData.Levels[_data.Index] <= 0)
        {
            return;
        }

        // equipUI 추가
        DataManager.FollowerData.TryEquip(_data.Index, 0);
    }
    
    
    
    public Text _txtDiceAmount;
    public Text _txtDiceCost;


    public void InitDicePanel()
    {
        var diceStat = DataManager.FollowerData.DiceDatas[_data.Index];

        var diceCount = diceStat.DiceSlotList.Count;

        for (int i = 0; i < DiceSlotList.Count; i++)
        {
            if (i >= diceCount)
            {
                DiceSlotList[i].gameObject.SetActive(false);
                continue;
            }
            
            DiceSlotList[i].gameObject.SetActive(true);
            DiceSlotList[i].Init(diceStat.DiceSlotList[i]);
        }

        _txtDiceAmount.text = DataManager.CurrencyData.GetAmount(Enum_CurrencyType.Dice).ToCurrencyString();
        _txtDiceCost.text = diceStat.GetRollPrice().ToString();
    }
    
    public void TryRoll()
    {
        if (DataManager.FollowerData.TryRoll(_data.Index))
        {
            Refresh();
        }
    }

    public void InitLevelUpPanel()
    {
        var level = DataManager.EquipmentData.Levels[_data.Index];
        //
        // _txtEquipStat.text = $"{_data.Stat}";
        // _txtEquipStatValue.text =
        //     $"{_data.OnEquipVaue + _data.OnEquipIncreaseValue * (level - 1)} -> {_data.OnEquipVaue + _data.OnEquipIncreaseValue * (level)}";
        //
        // if (_data.OnOwnValue1 == 0)
        // {
        //     _txtOwnStat1.text = "";
        //     _txtOwnStatValue1.text = "";
        // }
        // else
        // {
        //     _txtOwnStat1.text = $"{_data.OnOwnStat1}";
        //     _txtOwnStatValue1.text =
        //         $"{_data.OnOwnValue1 + _data.OnOwnIncreaseValue1 * (level - 1)} -> {_data.OnOwnValue1 + _data.OnOwnIncreaseValue1 * (level)}";
        // }
        //
        // if (_data.OnOwnValue2 == 0)
        // {
        //     _txtOwnStat2.text = "";
        //     _txtOwnStatValue2.text = "";
        // }
        // else
        // {
        //     _txtOwnStat2.text = $"{_data.OnOwnStat2}";
        //     _txtOwnStatValue2.text =
        //         $"{_data.OnOwnValue2 + _data.OnOwnIncreaseValue2 * (level - 1)} -> {_data.OnOwnValue2 + _data.OnOwnIncreaseValue2 * (level)}";
        // }
        //
        // if (_data.OnOwnValue3 == 0)
        // {
        //     _txtOwnStat3.text = "";
        //     _txtOwnStatValue3.text = "";
        // }
        // else
        // {
        //     _txtOwnStat3.text = $"{_data.OnOwnStat3}";
        //     _txtOwnStatValue3.text =
        //         $"{_data.OnOwnValue3 + _data.OnOwnIncreaseValue3 * (level - 1)} -> {_data.OnOwnValue3 + _data.OnOwnIncreaseValue3 * (level)}";
        // }

        var price = DataManager.FollowerData.GetLevelUpCost(_data.Index);
        
        _txtPrice.text = price.ToString();
        _btnLevelUp.interactable = DataManager.EquipmentData.IsEnableLevelUp(_data.Index);
        
        _btnEquip.interactable = DataManager.EquipmentData.Levels[_data.Index] > 0;
    }
    

    protected override void Refresh()
    {
        throw new System.NotImplementedException();
    }
}