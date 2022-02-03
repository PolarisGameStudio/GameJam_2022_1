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
        Stat[Enum_StatType.CriticalDamage] = GetStatValue(Enum_StatType.CriticalDamage);
        Stat[Enum_StatType.MoveSpeed] = GetStatValue(Enum_StatType.MoveSpeed);
        Stat[Enum_StatType.AttackSpeed] = GetStatValue(Enum_StatType.AttackSpeed);
        Stat[Enum_StatType.SuperCriticalChance] = GetSuperCriticalChance();
        Stat[Enum_StatType.SuperCriticalDamage] = GetStatValue(Enum_StatType.SuperCriticalDamage);
        Stat[Enum_StatType.AttackRange] = 2;
        Stat[Enum_StatType.DetectRange] = 2;
        Stat[Enum_StatType.MoreGold] = GetStatValue(Enum_StatType.MoreGold);
        Stat[Enum_StatType.MoreExp] = GetStatValue(Enum_StatType.MoreExp);
    }

    public double GetDamage()
    {
        double damage = 0;

        damage = PlayerData.Stat[Enum_StatType.Damage] +
                 GoldGrowthData.Stat[Enum_StatType.Damage] +
                 StatGrowthData.Stat[Enum_StatType.Damage];

        damage *= (100 + EquipmentData.Stat[Enum_StatType.Damage]) / 100f;

        var diceStatDamage = PromotionData.DiceStatData.Stat[Enum_StatType.Damage] +
                             FollowerData.GetStatValue(Enum_StatType.Damage);
        
        damage *= (100 + (FollowerData.Stat[Enum_StatType.Damage] + diceStatDamage))  / 100f;

        damage *= PromotionData.Stat[Enum_StatType.Damage];
        
        damage *= RuneData.Stat[Enum_StatType.Damage];

        return damage;
    }

    public double GetHealth()
    {
        double health = 0;

        health = PlayerData.Stat[Enum_StatType.MaxHealth] + 
                 GoldGrowthData.Stat[Enum_StatType.MaxHealth] +
                 StatGrowthData.Stat[Enum_StatType.MaxHealth];

        health *= (100 + EquipmentData.Stat[Enum_StatType.MaxHealth]) / 100f;
        
        var diceStatHealth = PromotionData.DiceStatData.Stat[Enum_StatType.MaxHealth] +
                             FollowerData.GetStatValue(Enum_StatType.MaxHealth);
        
        health *= (100 + (FollowerData.Stat[Enum_StatType.MaxHealth] + diceStatHealth))  / 100f;

        health *= PromotionData.Stat[Enum_StatType.MaxHealth];

        return health;
    }

    public double GetCriticalChance()
    {
        var type = Enum_StatType.CriticalChance;
        
        var value = PlayerData.Stat[type];

        double addValue = GoldGrowthData.Stat[type] +
                          StatGrowthData.Stat[type] +
                          FollowerData.Stat[type] +
                          EquipmentData.Stat[type] +
                          PromotionData.DiceStatData.Stat[type] +
                          FollowerData.GetStatValue(type);

        value += addValue;

        return value;
    }
    
    private double GetSuperCriticalChance()
    {
        var type = Enum_StatType.SuperCriticalChance;
        
        var value = PlayerData.Stat[type];

        double addValue = GoldGrowthData.Stat[type] +
                          StatGrowthData.Stat[type] +
                          FollowerData.Stat[type] +
                          EquipmentData.Stat[type] +
                          PromotionData.DiceStatData.Stat[type] +
                          FollowerData.GetStatValue(type);

        value += addValue;

        return value;
    }

    public double GetStatValue(Enum_StatType type)
    {
        var value = PlayerData.Stat[type];

        double addValue = GoldGrowthData.Stat[type] +
                          StatGrowthData.Stat[type] +
                          FollowerData.Stat[type] +
                          EquipmentData.Stat[type] +
                          PromotionData.DiceStatData.Stat[type] +
                          FollowerData.GetStatValue(type);

        value *= (100 + addValue) / 100f;

        return value;
    }
}