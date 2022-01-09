using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_UserLevel : GameBehaviour, GameEventListener<RefreshEvent>
{
    [SerializeField] private Text _txtUserLevel;
    [SerializeField] private Text _txtGaugeValue;

    [SerializeField] private Slider _sliderUserExp;

    private PlayerGrowStat playerGrowStatData;
    
    private void Awake()
    {
        this.AddGameEventListening<RefreshEvent>();
    }

    private void Start()
    {
        PlayerDataManager.PlayerDataContainer.GetGrowStat(ref playerGrowStatData);
        Init();
    }

    private void Init()
    {
        _txtUserLevel.text = $"lv{playerGrowStatData.level}";
        _sliderUserExp.maxValue = (float)playerGrowStatData.nextLevelExp;
        _sliderUserExp.value = (float)playerGrowStatData.exp;
        _txtGaugeValue.text = $"{_sliderUserExp.value} / {_sliderUserExp.maxValue}";

        // _txtUserLevel.text = $"lv{UserManager.Instance.Level}"; 
        //
        // _sliderUserExp.maxValue = UserManager.Instance.GetRequireExp(); 
        // _sliderUserExp.value =  UserManager.Instance.Exp; 
    }

    private void Update()
    {
        _txtGaugeValue.text = $"{_sliderUserExp.value} / {_sliderUserExp.maxValue}";
        _sliderUserExp.value = (float)playerGrowStatData.exp; //UserManager.Instance.Exp; 
    }

    public void OnGameEvent(RefreshEvent e)
    {
        if (e.Type == Enum_RefreshEventType.LevelUp)
        {
            Init();
        }
    }
}
