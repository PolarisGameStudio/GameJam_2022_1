using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Popup_Follower : UI_BasePopup<UI_Popup_Follower>, GameEventListener<RefreshEvent>
{
    public Text _txtGrade;
    public Text _txtName;
    public Image _imgFollower;

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
        _isDiceToggle = isOn;
        Init(_data);
        Refresh();
    }

    public void Open(TBL_FOLLOWER data)
    {
        Init(data);
        base.Open();
    }

    private void Init(TBL_FOLLOWER data)
    {
        _data = data;
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

        UI_Popup_Follower_Equip.Instance.Open(_data);
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
            DiceSlotList[i].SetDisableText($"{i+1}등급에 해금");
        }

        _txtDiceAmount.text = DataManager.CurrencyData.GetAmount(Enum_CurrencyType.Dice).ToCurrencyString();
        _txtDiceCost.text = diceStat.GetRollPrice().ToString();
    }

    public void TryRoll()
    {
        if (DataManager.FollowerData.TryRoll(_data.Index))
        {
            InitDicePanel();
        }
    }

    protected override void Refresh()
    {
        //_txtGrade.text = $"{_data}";
        _txtGrade.text = $"{DataManager.FollowerData.Levels[_data.Index]}등급";
        _txtName.text = $"{_data.name}";

        _imgFollower.sprite = AssetManager.Instance.FollowerBodyIcon[_data.Index];

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

    public void InitLevelUpPanel()
    {
        var level = DataManager.FollowerData.Levels[_data.Index];

        _txtEquipStat.text = $"{StringValue.GetStatName(_data.StatType1)}";
        _txtEquipStatValue.text =
            $"{_data.DefaultValue1 + _data.IncreaseValue1 * (level - 1)} -> {_data.DefaultValue1 + _data.IncreaseValue1 * (level)}";

        if (_data.DefaultValue2 == 0)
        {
            _txtOwnStat1.text = "";
            _txtOwnStatValue1.text = "";
        }
        else
        {
            _txtOwnStat1.text = $"{StringValue.GetStatName(_data.StatType2)}";
            _txtOwnStatValue1.text =
                $"{_data.DefaultValue2 + _data.IncreaseValue2 * (level - 1)} -> {_data.DefaultValue2 + _data.IncreaseValue2 * (level)}";
        }
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
        
        _txtPrice.text = $"({DataManager.FollowerData.Counts[_data.Index]}/{DataManager.FollowerData.GetLevelUpCost(_data.Index)})";
        _btnLevelUp.interactable = DataManager.FollowerData.IsEnableLevelUp(_data.Index);

        _btnEquip.interactable = DataManager.FollowerData.Levels[_data.Index] > 0;
    }

    public void OnGameEvent(RefreshEvent e)
    {
        if (e.Type == Enum_RefreshEventType.Follower)
        {
            Refresh();
        }
    }
}