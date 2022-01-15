using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataContainer
{
    private PlayerData _playerData;
    public PlayerData PlayerData => _playerData;
    
    private BattleData _battleData;
    public BattleData BattleData => _battleData;

    public DataContainer()
    {
        _playerData = new PlayerData();
        _battleData = new BattleData();
    }
}
