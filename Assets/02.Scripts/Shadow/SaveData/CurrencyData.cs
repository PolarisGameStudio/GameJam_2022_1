using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CurrencyData : SaveDataBase
{
    [SerializeField] public List<Currency> CurrencyList { get; set; }

    public override void ValidCheck()
    {
        var currencyCount = (int) Enum_CurrencyType.Count;
        
        if (CurrencyList == null)
        {
            CurrencyList = new List<Currency>(currencyCount);
        }

        var lackCount = currencyCount - CurrencyList.Count;

        if (lackCount > 0)
        {
            for (int i = CurrencyList.Count; i < currencyCount; i++)
            {
                Enum_CurrencyType type = (Enum_CurrencyType) i;

                var currency = new Currency(type, 0);
                //currency.Init(type, 0);
                CurrencyList.Add(currency);
            }
        }
    }
    
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private Currency GetCurrency(Enum_CurrencyType type)
    {
        return CurrencyList[(int) type];
        //return CurrencyList.Find(x => x.CurrencyType == type);
    }

    public double GetAmount(Enum_CurrencyType type)
    {
        return GetCurrency(type).Amount;
    }

    public bool IsEnough(Enum_CurrencyType type, double amount)
    {
        return GetCurrency(type).IsEnough(amount);
    }    
    
    public bool TryConsume(Enum_CurrencyType type, double amount)
    {
        if (!IsEnough(type, amount))
        {
            return false;
        }
        
        GetCurrency(type).Consume(amount);

        return true;
    }
     
    public void Add(Enum_CurrencyType type, double amount)
    {
        GetCurrency(type).Add(amount);
    }
    
}
