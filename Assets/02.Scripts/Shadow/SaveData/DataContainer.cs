using System;
using UnityEngine;

[Serializable]
public class DataContainer
{
    [SerializeField] public PlayerData PlayerData { get; set; }
    [SerializeField] public StageData StageData { get; set; }
    [SerializeField] public CurrencyData CurrencyData { get; set; }
    [SerializeField] public GoldGrowthData GoldGrowthData { get; set; }
    [SerializeField] public StatGrowthData StatGrowthData { get; set; }
    [SerializeField] public EquipmentData EquipmentData { get; set; }
    [SerializeField] public PromotionData PromotionData { get; set; }
    [SerializeField] public GachaData GachaData { get; set; }
    [SerializeField] public FollowerData FollowerData { get; set; }
    [SerializeField] public DungeonData DungeonData { get; set; }
    [SerializeField] public RuneData RuneData { get; set; }
    [SerializeField] public ShopData ShopData { get; set; }
    [SerializeField] public AchievementData AchievementData { get; set; }
    [SerializeField] public OptionData OptionData { get; set; }
    [SerializeField] public QuestData QuestData { get; set; }
    [SerializeField] public SkillData SkillData { get; set; }

    public DateTime LastDateTime = DateTime.Today;
    public DateTime LastSaveTime = DateTime.Now;

    public void ValidCheck()
    {
        PlayerData ??= new PlayerData();
        StageData ??= new StageData();
        CurrencyData ??= new CurrencyData();
        GoldGrowthData ??= new GoldGrowthData();
        StatGrowthData ??= new StatGrowthData();
        EquipmentData ??= new EquipmentData();
        PromotionData ??= new PromotionData();
        GachaData ??= new GachaData();
        FollowerData ??= new FollowerData();
        DungeonData ??= new DungeonData();
        RuneData ??= new RuneData();
        ShopData ??= new ShopData();
        AchievementData ??= new AchievementData();
        OptionData ??= new OptionData();
        QuestData ??= new QuestData();
        SkillData ??= new SkillData();

        PlayerData.ValidCheck();
        StageData.ValidCheck();
        CurrencyData.ValidCheck();
        GoldGrowthData.ValidCheck();
        StatGrowthData.ValidCheck();
        EquipmentData.ValidCheck();
        PromotionData.ValidCheck();
        GachaData.ValidCheck();
        FollowerData.ValidCheck();
        DungeonData.ValidCheck();
        RuneData.ValidCheck();
        ShopData.ValidCheck();
        AchievementData.ValidCheck();
        OptionData.ValidCheck();
        QuestData.ValidCheck();
        SkillData.ValidCheck();
    }
    
    public void OnNextDay()
    {
        PlayerData.OnNextDay();
        StageData.OnNextDay();
        CurrencyData.OnNextDay();
        GoldGrowthData.OnNextDay();
        StatGrowthData.OnNextDay();
        EquipmentData.OnNextDay();
        PromotionData.OnNextDay();
        GachaData.OnNextDay();
        FollowerData.OnNextDay();
        DungeonData.OnNextDay();
        RuneData.OnNextDay();
        ShopData.OnNextDay();
        AchievementData.OnNextDay();
        OptionData.OnNextDay();
        QuestData.OnNextDay();
        SkillData.OnNextDay();
        
        LastDateTime = DateTime.Today;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// 스탯/////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    [NonSerialized] public Stat Stat = new Stat();

    public void CalculateStat()
    {
        Stat.Init();

        Stat[Enum_StatType.Damage] = GetDamage();
        Stat[Enum_StatType.MaxHealth] = GetHealth();
        Stat[Enum_StatType.CriticalChance] = GetCriticalChance();
        Stat[Enum_StatType.CriticalDamage] = GetCriticalDamage();
        Stat[Enum_StatType.MoveSpeed] = GetMovementSpeed();
        Stat[Enum_StatType.AttackSpeed] = GetAttackSpeed();
        Stat[Enum_StatType.SuperCriticalChance] = GetSuperCriticalChance();
        Stat[Enum_StatType.SuperCriticalDamage] = GetSuperCriticalDamage();
        Stat[Enum_StatType.AttackRange] = 2;
        Stat[Enum_StatType.DetectRange] = 2;
        Stat[Enum_StatType.MoreGold] = GetMoreGold();
        Stat[Enum_StatType.MoreExp] = GetMoreExp();
    }

    private double GetMoreGold()
    {
        return 100 + StatGrowthData.Stat[Enum_StatType.MoreGold];
    }

    private double GetMoreExp()
    {
        return 100 + StatGrowthData.Stat[Enum_StatType.MoreExp];
    }

    public double GetDamage()
    {
        double damage = 0;

        damage = PlayerData.Stat[Enum_StatType.Damage] + GoldGrowthData.Stat[Enum_StatType.Damage] +
                 StatGrowthData.Stat[Enum_StatType.Damage];

        damage *= (100 + EquipmentData.Stat[Enum_StatType.Damage]) / 100f;

        damage *= PromotionData.Stat[Enum_StatType.Damage];
        
        damage *= RuneData.Stat[Enum_StatType.Damage];

        return damage;
    }


    public double GetHealth()
    {
        double health = 0;

        health = PlayerData.Stat[Enum_StatType.MaxHealth] + GoldGrowthData.Stat[Enum_StatType.MaxHealth] +
                 StatGrowthData.Stat[Enum_StatType.MaxHealth];

        health *= (1 + EquipmentData.Stat[Enum_StatType.MaxHealth]);
        
        health *= PromotionData.Stat[Enum_StatType.MaxHealth];

        return health;
    }

    public double GetCriticalChance()
    {
        return GoldGrowthData.Stat[Enum_StatType.CriticalChance];
    }

    public double GetCriticalDamage()
    {
        return  PlayerData.Stat[Enum_StatType.CriticalDamage] +GoldGrowthData.Stat[Enum_StatType.CriticalDamage];
    }

    public double GetMovementSpeed()
    {
        return PlayerData.Stat[Enum_StatType.MoveSpeed];
    }

    public double GetAttackSpeed()
    {
        return PlayerData.Stat[Enum_StatType.AttackSpeed];
    }

    public double GetSuperCriticalChance()
    {
        return GoldGrowthData.Stat[Enum_StatType.SuperCriticalChance];
    }

    public double GetSuperCriticalDamage()
    {
        return PlayerData.Stat[Enum_StatType.SuperCriticalDamage] + GoldGrowthData.Stat[Enum_StatType.SuperCriticalDamage];
    }
}