using System;
using System.Collections.Generic;
using UnityEngine;

public enum Enum_RuneBuffType
{
    Damage,
    Gold,
    Exp,
    Stone,

    Count,
}

public class RuneData : StatData
{
    public List<int> RuneRemainTime = new List<int>();
    public List<int> RuneLimitCount = new List<int>();

    protected override void CalculateStat()
    {
        Stat.Init();

        Stat[Enum_StatType.Damage] = IsRuneActivate(Enum_RuneBuffType.Damage) ? 2 : 1;

        StatEvent.Trigger(Enum_StatEventType.StatChange);
        RefreshEvent.Trigger(Enum_RefreshEventType.Rune);
    }

    public override void ValidCheck()
    {
        base.ValidCheck();

        var typeCount = (int) Enum_RuneBuffType.Count;

        var saveCount = RuneRemainTime.Count;

        if (typeCount > saveCount)
        {
            for (int i = saveCount; i < typeCount; i++)
            {
                RuneRemainTime.Add(0);
                RuneLimitCount.Add(0);
            }
        }

        CalculateStat();

        TimeManager.Instance.AddOnTickCallback(OnTick);
    }

    private void OnTick()
    {
        bool isRuneFinish = false;
        for (int i = 0; i < RuneRemainTime.Count; i++)
        {
            if (RuneRemainTime[i] < 0)
            {
                continue;
            }
            
            RuneRemainTime[i] -= 1;

            if (RuneRemainTime[i] <= 0)
            {
                isRuneFinish = true;
            }
        }

        if (isRuneFinish)
        {
            RefreshEvent.Trigger(Enum_RefreshEventType.Rune);
            CalculateStat();
        }
    }

    public bool IsRuneActivate(Enum_RuneBuffType type)
    {
        return RuneRemainTime[(int) type] > 0;
    }

    public bool IsOverLimit(Enum_RuneBuffType type)
    {
        return RuneLimitCount[(int) type] >= SystemValue.RUNE_DAILY_LIMIT;
    }

    private void StartRune(Enum_RuneBuffType type)
    {
        RuneLimitCount[(int) type] += 1;

        if (RuneRemainTime[(int) type] < 0)
        {
            RuneRemainTime[(int) type] = 0;
        }

        RuneRemainTime[(int) type] += SystemValue.RUNE_DURATUIN;
        
        RefreshEvent.Trigger(Enum_RefreshEventType.Rune);

        CalculateStat();
    }

    public void TryStartRune(Enum_RuneBuffType type)
    {
        if (IsOverLimit(type))
        {
            //ToastMessagePool.Instance.Show(LocalizeText.GetText("UI_Toast_DailyADReward_Disable"));
            return;
        }

        if (DataManager.ShopData.IsAdRemove)
        {
            StartRune(type);
        }
        else
        {
            AdManager.Instance.TryShowRequest(ADType.Rune, () => { StartRune(type); });
        }
    }

    public override void OnNextDay()
    {
        for (int i = 0; i < RuneLimitCount.Count; i++)
        {
            RuneLimitCount[i] = 0;
        }
    }
}