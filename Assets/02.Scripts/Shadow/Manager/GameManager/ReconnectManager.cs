using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ReconnectManager : SingletonBehaviour<ReconnectManager>
{
    public int Minute { get; set; }

    private void Start()
    {
        CheckReconnect();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus == false)
        {
            CheckReconnect();
        }
    }


    public void CheckReconnect()
    {
        var reconnectDiff = DateTime.Now - DataManager.Container.LastSaveTime;

        if (reconnectDiff.TotalMinutes >= SystemValue.MINIMUM_RECONNECT_MINUTE)
        {
            Minute = Mathf.Min((int) reconnectDiff.TotalMinutes, SystemValue.MAXIMUM_RECONNECT_MINUTE);

            UI_Popup_Reconnect.Instance.Open();
        }
    }

    public double GetGoldAmount()
    {
        var data = TBL_STAGE.GetEntity(DataManager.StageData.HighestStageLevel);

        if (data == null)
        {
            return 0d;
        }

        return data.GoldPerMin * Minute;
    }

    public double GetExpAmount()
    {
        var data = TBL_STAGE.GetEntity(DataManager.StageData.HighestStageLevel);

        if (data == null)
        {
            return 0d;
        }

        return data.ExpPerMin * Minute;
    }

    public double GetStoneAmount()
    {
        var data = TBL_STAGE.GetEntity(DataManager.StageData.HighestStageLevel);

        if (data == null)
        {
            return 0d;
        }

        return data.UpgradeStonePerMin * Minute;
    }

    public void GetReward(bool isDoubleReward)
    {
        var goldAmount = GetGoldAmount();
        var expAmount = GetExpAmount();
        var stoneAmount = GetStoneAmount();

        if (isDoubleReward)
        {
            goldAmount *= 2;
            expAmount *= 2;
            stoneAmount *= 2;
        }

        var rewards = new List<Reward>();

        rewards.Add(new Reward(RewardType.Currency, (int) Enum_CurrencyType.Gold, goldAmount));
        rewards.Add(new Reward(RewardType.Currency, (int) Enum_CurrencyType.Exp, expAmount));
        rewards.Add(new Reward(RewardType.Currency, (int) Enum_CurrencyType.EquipmentStone, stoneAmount));
        
        RewardManager.Get(rewards);

        DataManager.Instance.Save(force: true);
    }
}