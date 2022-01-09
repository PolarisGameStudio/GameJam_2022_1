using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public sealed class CurrencyManager : SingletonBehaviour<CurrencyManager>
{
    //private CurrencyInfo _currencyInfo;
    
    // 가진 재화량
    private List<double> _amounts = new List<double>(new double[(int) Enum_CurrencyType.Count]);

    public double Gold => _amounts[(int) Enum_CurrencyType.Gold];

    protected override void Awake()
    {
        base.Awake();
        
        Load();
    }


    /********************************** 추가 **********************************/
    public void AddGold(double addAmount)
    {
        AddCurrency(Enum_CurrencyType.Gold, addAmount);
    }
    
    public void AddCurrency(Enum_CurrencyType currencyType, double addAmount)
    {
        _amounts[(int) currencyType] += addAmount;
        
        RefreshEvent.Trigger(Enum_RefreshEventType.Currency);
    }
    
    /********************************** 조회 **********************************/
    public bool HaveGold(double amount) => HaveCurrency(Enum_CurrencyType.Gold, amount);
    
    public bool HaveCurrency(Enum_CurrencyType currencyType, double amount)
    {
        return _amounts[(int) currencyType] >= amount;
    }
    
    
    /********************************** 소모 **********************************/
    public bool TryBuyGold(double amount) => TryBuyCurrency(Enum_CurrencyType.Gold, amount);
    public bool TryBuyCurrency(Enum_CurrencyType currencyType, double amount)
    {
        if (!HaveCurrency(currencyType, amount))
        {
            return false;
        }

        _amounts[(int) currencyType] -= amount;
        
        RefreshEvent.Trigger(Enum_RefreshEventType.Currency);

        return true;
    }
    
    /********************************** 초기화 **********************************/
    
    public void SetCurrency(Enum_CurrencyType currencyType, double addAmount)
    {
        _amounts[(int) currencyType] = addAmount;
        
        RefreshEvent.Trigger(Enum_RefreshEventType.Currency);
    }
    
    ////////////////////////////////////////////////////////////////////////////////

    private const string _saveKey = "currencyInfo";

    public void Save()
    {
        ES3.Save(_saveKey, _amounts);
    }

    private void Load()
    {
        _amounts = ES3.Load<List<double>>(_saveKey, new List<double>(new double[(int) Enum_CurrencyType.Count]));
    }
}
