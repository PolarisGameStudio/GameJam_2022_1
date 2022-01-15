using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataContainer
{
    [SerializeField] private PlayerData _playerData;
    public PlayerData PlayerData
    {
        get { return _playerData; }
    }
    
    [SerializeField] private BattleData _battleData;
    public BattleData BattleData
    {
        get { return _battleData; }
    }
    
    
    public DataContainer()
    {
        _playerData = new PlayerData();
        _battleData = new BattleData();
    }
}
