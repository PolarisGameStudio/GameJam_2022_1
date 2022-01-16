using System;
using UnityEngine;

[Serializable]
public class DataContainer
{
    [SerializeField] public PlayerLevelData PlayerLevelData { get; set; }
    [SerializeField] public BattleData BattleData { get; set; }
    [SerializeField] public CurrencyData CurrencyData { get; set; }
    [SerializeField] public GoldGrowthData GoldGrowthData { get; set; }
    [SerializeField] public StatGrowthData StatGrowthData { get; set; }
    [SerializeField] public EquipmentData EquipmentData { get; set; }

    public void ValidCheck()
    {
        PlayerLevelData ??= new PlayerLevelData();
        BattleData ??= new BattleData();
        CurrencyData ??= new CurrencyData();
        GoldGrowthData ??= new GoldGrowthData();
        StatGrowthData ??= new StatGrowthData();
        EquipmentData ??= new EquipmentData();
        
        PlayerLevelData.ValidCheck();
        BattleData.ValidCheck();
        CurrencyData.ValidCheck();
        GoldGrowthData.ValidCheck();
        StatGrowthData.ValidCheck();
        EquipmentData.ValidCheck();
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// 스탯/////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public Stat Stat = new Stat();

    public void CalculateStat()
    {
        
    }

    public void GetDamage()
    {
    }
    
    
    public void GetHealth()
    {
        
    }
}
