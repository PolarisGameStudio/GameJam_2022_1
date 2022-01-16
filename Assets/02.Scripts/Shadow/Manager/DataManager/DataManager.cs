using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class DataManager : SingletonBehaviour<DataManager>
{
    [SerializeField] private DataContainer _container;
    public static DataContainer Container => Instance._container;

    public static PlayerData PlayerData => Container.PlayerData;
    public static BattleData BattleData => Container.BattleData;
    public static CurrencyData CurrencyData => Container.CurrencyData;

    protected override void Awake()
    {
        base.Awake();
        
        Load();
    }

    private void OnApplicationQuit()
    {
        Save();
    }
    
    private const string saveKey = "UserData";

    public void Save()
    {
        ES3.Save(saveKey, _container);
    }

    public void Load()
    {
        _container = ES3.Load(saveKey, new DataContainer());

        _container.ValidCheck();
    }
}
