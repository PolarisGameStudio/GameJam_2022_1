using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

[SerializeField]
public class DataManager : SingletonBehaviour<DataManager>, GameEventListener<StatEvent>
{
    private DataContainer _container;
    public static DataContainer Container => Instance._container;

    public static PlayerData PlayerData => Container.PlayerData;
    public static StageData StageData => Container.StageData;
    public static CurrencyData CurrencyData => Container.CurrencyData;
    public static GoldGrowthData GoldGrowthData => Container.GoldGrowthData;
    public static StatGrowthData StatGrowthData => Container.StatGrowthData;
    public static EquipmentData EquipmentData => Container.EquipmentData;
    public static PromotionData PromotionData => Container.PromotionData;
    public static GachaData GachaData => Container.GachaData;
    public static FollowerData FollowerData => Container.FollowerData;
    public static DungeonData DungeonData => Container.DungeonData;
    public static RuneData RuneData => Container.RuneData;
    public static ShopData ShopData => Container.ShopData;
    public static AchievementData AchievementData => Container.AchievementData;
    public static OptionData OptionData => Container.OptionData;
    public static QuestData QuestData => Container.QuestData;
    public static SkillData SkillData => Container.SkillData;

    public bool IsReady { get; set; }

    private float _saveTimer = 0;


    protected override void Awake()
    {
        base.Awake();

        this.AddGameEventListening<StatEvent>();

        Load();
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    private const string saveKey = "UserData";

    private void Update()
    {
        _saveTimer += Time.unscaledTime;
    }

    public void Save(bool force = false)
    {
        if ( !force && (!IsReady || _saveTimer <= SystemValue.MINIMUM_SAVE_PERIOD))
        {
            return;
        }
        
        ES3.Save(saveKey, _container);
        _saveTimer = 0;
        
        _container.LastSaveTime = DateTime.Now;
    }

    public void Load()
    {
        _container = ES3.Load(saveKey, new DataContainer());

        _container.ValidCheck();
        _container.CalculateStat();

        IsReady = true;
    }

    public void OnGameEvent(StatEvent e)
    {
        if (e.Type == Enum_StatEventType.StatChange)
        {
            _container.CalculateStat();
            StatEvent.Trigger(Enum_StatEventType.StatCalculate);
        }
    }

    public void OnNextDay()
    {
        Container.OnNextDay();
        Save();
    }
}