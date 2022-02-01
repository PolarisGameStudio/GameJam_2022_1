using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopData : SaveDataBase
{
    public List<int> PurchaseCounts = new List<int>();
    public List<int> DailyLimit = new List<int>();
    
    public bool IsAdRemove;

    [NonSerialized] private TBL_PACKAGE _package;

    public override void ValidCheck()
    {
        base.ValidCheck();
        
        var typeCount = TBL_PACKAGE.CountEntities;

        var saveCount = PurchaseCounts.Count;

        if (typeCount > saveCount)
        {
            for (int i = saveCount; i < typeCount; i++)
            {
                PurchaseCounts.Add(0);
                DailyLimit.Add(0);
            }
        }

        var vipPackage = TBL_PACKAGE.FindEntity(x => x.IAP_ID.Contains("vip"));

        if (vipPackage != null)
        {
            if (PurchaseCounts[vipPackage.Index] > 0)
            {
                IsAdRemove = true;
            }
        }

    }

    public void TryPurchase(string id)
    {
        _package = TBL_PACKAGE.FindEntity(x => x.IAP_ID == id);
        
        if(_package == null)
        {
            return;
        }
        
        UI_LoadingBlocker.Instance.Open();
        
        IAPManager.Instance.PurchaseItem(id, (success) =>
            {
                UI_LoadingBlocker.Instance.Close();
                GetReward();
                
                ShopEvent.Trigger();
                DataManager.Instance.Save(force:true);
            }, (fail) =>
            {
                UI_LoadingBlocker.Instance.Close();
            }
        );
    }

    private void GetReward()
    {
        if (_package == null)
        {
            return;
        }

        if (_package.IAP_ID.ToLower().Contains("vip"))
        {
            IsAdRemove = true;
        }
        
        List<Reward>  rewards = new List<Reward>(4)
        {
            new Reward(_package.Item_1_Type, _package.Item_1_Index, _package.Item_1_Count),
            new Reward(_package.Item_2_Type, _package.Item_2_Index, _package.Item_2_Count),
            new Reward(_package.Item_3_Type, _package.Item_3_Index, _package.Item_3_Count),
            new Reward(_package.Item_4_Type, _package.Item_4_Index, _package.Item_4_Count),
            new Reward(_package.Item_5_Type, _package.Item_5_Index, _package.Item_5_Count),
        };
        
        RewardManager.Get(rewards);

        PurchaseCounts[_package.Index]++;
        DailyLimit[_package.Index]++;
        
        _package = null;
    }

    public override void OnNextDay()
    {
        base.OnNextDay();

        for (var i = 0; i < DailyLimit.Count; i++)
        {
            DailyLimit[i] = 0;
        }
    }
}
