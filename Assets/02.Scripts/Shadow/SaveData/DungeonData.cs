using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonData : SaveDataBase
{
    public int TreasureDungeonKillCount = 0;
    public double BossDungeonHighestDamage = 0;


    public int TreasureDungeonHighLevel = 0;
    public int SmithDungeonHighLevel = 0;
    public int BossDungeonHighLevel = 0;

    public void OnDungeonBattleEnd(Enum_BattleType dungeonBattleType, int level)
    {
        switch (dungeonBattleType)
        {
            case Enum_BattleType.TreasureDungeon:
                TreasureDungeonHighLevel = Mathf.Max(TreasureDungeonHighLevel, level);
                break;
            case Enum_BattleType.SmithDungeon:
                SmithDungeonHighLevel = Mathf.Max(SmithDungeonHighLevel, level);
                break;
            case Enum_BattleType.BossDungeon:
                BossDungeonHighLevel = Mathf.Max(BossDungeonHighLevel, level);
                break;

            default:
                Debug.LogError("던전 아니면 안됨");
                return;
        }
    }

    public void RecordDungeonScore(Enum_BattleType dungeonBattleType, double score)
    {
        switch (dungeonBattleType)
        {
            case Enum_BattleType.TreasureDungeon:
                int scr = (int) score;
                TreasureDungeonKillCount = TreasureDungeonKillCount > scr ? TreasureDungeonKillCount : scr;
                break;
            case Enum_BattleType.BossDungeon:
                BossDungeonHighestDamage = BossDungeonHighestDamage > score ? BossDungeonHighestDamage : score;
                break;

            default:
                Debug.LogError("던전 아니면 안됨");
                return;
        }
    }

    public void GetDungeonReward(Enum_BattleType dungeonBattleType, int count = 1)
    {
        switch (dungeonBattleType)
        {
            case Enum_BattleType.TreasureDungeon:
                break;
            case Enum_BattleType.SmithDungeon:
                break;
            case Enum_BattleType.BossDungeon:
                break;

            default:
                break;
        }
    }

    public void TryChallenge(Enum_BattleType dungeonBattleType)
    {
        Enum_CurrencyType ticket = Enum_CurrencyType.Count;
        int level = 0;
        switch (dungeonBattleType)
        {
            case Enum_BattleType.TreasureDungeon:
                ticket = Enum_CurrencyType.Ticket_Treasure;
                break;
            case Enum_BattleType.SmithDungeon:
                ticket = Enum_CurrencyType.Ticket_Smith;
                level = SmithDungeonHighLevel;
                break;
            case Enum_BattleType.BossDungeon:
                ticket = Enum_CurrencyType.Ticket_Boss;
                break;

            default:
                Debug.LogError("던전 배틀타입만처리 가능");
                return;
        }

        if (DataManager.CurrencyData.IsEnough(ticket, 1))
        {
            BattleManager.Instance.BattleStart(dungeonBattleType, level);
        }
    }

    public void GetRewardTreasureDungeon(int count)
    {
        List<Reward> rewards = new List<Reward>();

        while (count > 1)
        {
            count--;
        }

        RewardManager.GetWithRewardUI(rewards);
    }

    public void GetRewardSmithDungeon(int count)
    {
        List<Reward> rewards = new List<Reward>();

        while (count > 1)
        {
            count--;
        }

        RewardManager.GetWithRewardUI(rewards);
    }

    public void GetRewardBossDungeon(int count)
    {
        List<Reward> rewards = new List<Reward>();

        while (count > 1)
        {
            count--;
        }

        RewardManager.GetWithRewardUI(rewards);
    }
}