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

    public void ValidCheck()
    {
        PlayerData ??= new PlayerData();
        StageData ??= new StageData();
        CurrencyData ??= new CurrencyData();
        GoldGrowthData ??= new GoldGrowthData();
        StatGrowthData ??= new StatGrowthData();
        EquipmentData ??= new EquipmentData();
        
        PlayerData.ValidCheck();
        StageData.ValidCheck();
        CurrencyData.ValidCheck();
        GoldGrowthData.ValidCheck();
        StatGrowthData.ValidCheck();
        EquipmentData.ValidCheck();
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
        Stat[Enum_StatType.CriticalChance] = 0;
        Stat[Enum_StatType.CriticalDamage] = GetDamage();
        Stat[Enum_StatType.MoveSpeed] = 2;
        Stat[Enum_StatType.AttackSpeed] = 2;
        Stat[Enum_StatType.SuperCriticalChance] = 0;
        Stat[Enum_StatType.SuperCriticalDamage] = 0;
        Stat[Enum_StatType.AttackRange] = 2;
        Stat[Enum_StatType.DetectRange] = 2;
    }

    public double GetDamage()
    {
        double damage = 0;

        damage = PlayerData.Stat[Enum_StatType.Damage] + GoldGrowthData.Stat[Enum_StatType.Damage] +
                 StatGrowthData.Stat[Enum_StatType.Damage];

        damage *= (1 + EquipmentData.Stat[Enum_StatType.Damage]);

        return damage;
    }
    
    
    public double GetHealth()
    {
        return 1000;
    }
}
