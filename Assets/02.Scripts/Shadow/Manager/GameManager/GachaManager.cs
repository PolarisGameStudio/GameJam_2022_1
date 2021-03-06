using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;


public class GachaManager : SingletonBehaviour<GachaManager>
{
    private Dictionary<GachaType, GachaHandler> _gachaHandlers;

    private GachaType _lastGachaType;
    private Enum_CurrencyType _lastCurrencyType;
    private int _lastPrice;
    private int _lastCount;

    private void Awake()
    {
        _gachaHandlers = new Dictionary<GachaType, GachaHandler>();
        _gachaHandlers.Add(GachaType.Weapon, new WeaponGachaHandler());
        _gachaHandlers.Add(GachaType.Ring, new RingGachaHandler());
        _gachaHandlers.Add(GachaType.Skill, new SkillGachaHandler());

        _gachaHandlers.Add(GachaType.Dice, new DiceGachaHandler());
    }

    public void ReGacha()
    {
        if (DataManager.CurrencyData.TryConsume(_lastCurrencyType, _lastPrice))
        {
            Gacha(_lastGachaType, _lastCurrencyType, _lastPrice, _lastCount);
        }
        else
        {
            UI_Popup_OK.Instance.OpenCurrencyNotEnough();
        }
    }

    public List<int> Gacha(GachaType type, Enum_CurrencyType currencyType, int price, int count)
    {
        if (!_gachaHandlers.ContainsKey(type))
        {
            Debug.LogError($"{type} 가챠가 없습니다.");
            return null;
        }

        _lastGachaType = type;
        _lastCurrencyType = currencyType;
        _lastPrice = price;
        _lastCount = count;

        DataManager.GachaData.AddGachaCount(type, count);
        var list = _gachaHandlers[type].Gacha(count);

        if (type != GachaType.Dice)
        {
            RefreshEvent.Trigger(Enum_RefreshEventType.Quest);
            DataManager.Instance.Save();
        }

        return list;
    }

    public int GachaByGrade(GachaType type, Enum_ItemGrade grade)
    {
        if (_gachaHandlers.ContainsKey(type))
        {
            return _gachaHandlers[type].GachaByGrade(grade);
        }

        return -1;
    }


    public int GetRandomStarCount()
    {
        int random = UnityEngine.Random.Range(0, 100);

        if (random < 40)
        {
            return 0;
        }
        else if (random < 70)
        {
            return 1;
        }
        else if (random < 90)
        {
            return 2;
        }
        else if (random < 95)
        {
            return 3;
        }
        else
        {
            return 4;
        }
    }


    public int GetRandomIndex(GachaType type)
    {
        // if (_artifactGacha.ContainsKey(type))
        // {
        //     return _artifactGacha[type].GetRandomIndex();
        // }
        // else if (_skillGacha.ContainsKey(type))
        // {
        //     return _skillGacha[type].GetRandomIndex();
        // }

        return 0;
    }

    public GachaType GetLastGachaType()
    {
        return _lastGachaType;
    }

    public int GetLastGachaPrice()
    {
        return _lastPrice;
    }
    public Enum_CurrencyType GetLastCurrency()
    {
        return _lastCurrencyType;
    }
}