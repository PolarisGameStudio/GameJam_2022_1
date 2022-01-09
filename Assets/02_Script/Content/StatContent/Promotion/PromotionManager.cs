using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromotionInfo
{
    public int PromotionLevel;

    public List<int> Levels;
}

public class PromotionManager : StatManager<PromotionManager>
{
    private PromotionInfo _promotionInfo;

    private Promotion _currentPromotion;

    private List<PromotionGrowth> _lists;

    private int _currentMaxLevel;

    public int PromotionLevel => _promotionInfo.PromotionLevel;

    protected override void Awake()
    {
        base.Awake();

        Load();
        
        Init();
    }

    private void Init()
    {
        _lists = new List<PromotionGrowth>(4);

        _lists.Add(new PromotionGrowth(Enum_StatType.Damage, _promotionInfo.Levels[0], 1, 10));
        //_lists.Add(new PromotionGrowth(Enum_StatType.Health, _promotionInfo.Levels[1], 1, 10));
        _lists.Add(new PromotionGrowth(Enum_StatType.CriticalChance, _promotionInfo.Levels[2], 1, 10));
        _lists.Add(new PromotionGrowth(Enum_StatType.CriticalDamage, _promotionInfo.Levels[3], 1, 10));

        _currentPromotion = new Promotion(new Stat(), 10);

        _currentMaxLevel = (_promotionInfo.PromotionLevel + 1) * 10;
    }

    public void TryLevelUp(int index)
    {
        if (_lists[index].TryLevelUp())
        {
            RefreshEvent.Trigger(Enum_RefreshEventType.Currency);

            CalculateStat();
        }
    }

    protected override void InitStat()
    {
        Stat.Reset();
    }

    protected override void CalculateStat()
    {
        InitStat();

        foreach (var promotion in _lists)
        {
            Stat[promotion.StatType] += promotion.Value;
        }

        RefreshEvent.Trigger(Enum_RefreshEventType.Stat);
    }

    public void TryPromotion()
    {
        if (IsEnablePromotion())
        {
            _promotionInfo.PromotionLevel += 1;
            
            
            //todo: Promotion 다음레벨로

            //_currentPromotion = PromotionList[_promotionInfo.PromotionLevel];
        }
    }

    public bool IsEnablePromotion()
    {
        var notMaxLevel = _lists.Find(x => x.Level < _currentPromotion.MaxLevel);

        return notMaxLevel != null;
    }


    private const string _saveKey = "promotionGrowthInfo";

    public void Save()
    {
        ES3.Save(_saveKey, _promotionInfo);
    }

    private void Load()
    {
        _promotionInfo = ES3.Load<PromotionInfo>(_saveKey, new PromotionInfo());
    }
}