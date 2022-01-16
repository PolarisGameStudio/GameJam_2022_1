using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DataContainer
{
    [SerializeField] public PlayerData PlayerData { get; set; }
    
    [SerializeField] public BattleData BattleData { get; set; }

    [SerializeField] public CurrencyData CurrencyData { get; set; }

    public DataContainer()
    {
        PlayerData = new PlayerData();
        BattleData = new BattleData();
        CurrencyData = new CurrencyData();
    }

    public void ValidCheck()
    {
        PlayerData.ValidCheck();
        BattleData.ValidCheck();
        CurrencyData.ValidCheck();
    }
}
