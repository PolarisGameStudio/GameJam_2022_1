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
                DataManager.CurrencyData.TryConsume(Enum_CurrencyType.Ticket_Treasure, 1);
                break;
            case Enum_BattleType.SmithDungeon:
                SmithDungeonHighLevel = Mathf.Max(SmithDungeonHighLevel, level);
                DataManager.CurrencyData.TryConsume(Enum_CurrencyType.Ticket_Smith, 1);
                break;
            case Enum_BattleType.BossDungeon:
                BossDungeonHighLevel = Mathf.Max(BossDungeonHighLevel, level);
                DataManager.CurrencyData.TryConsume(Enum_CurrencyType.Ticket_Boss, 1);
                break;

            default:
                Debug.LogError("던전 아니면 안됨");
                return;
        }
        DataManager.AchievementData.ProgressAchievement(Enum_AchivementMission.Daily_EnterDungeon, 1);
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

    public bool TrySkipDungeon(Enum_BattleType dungeonBattleType, int count = 1)
    {
        switch (dungeonBattleType)
        {
            case Enum_BattleType.TreasureDungeon:
                DataManager.CurrencyData.TryConsume(Enum_CurrencyType.Ticket_Treasure, count);
                return true;
            
            case Enum_BattleType.SmithDungeon:
                DataManager.CurrencyData.TryConsume(Enum_CurrencyType.Ticket_Smith, count);
                return true;
            
            case Enum_BattleType.BossDungeon:
                DataManager.CurrencyData.TryConsume(Enum_CurrencyType.Ticket_Boss, count);
                return true;

            default:
                return false;
        }
        
    }

    public bool TryChallenge(Enum_BattleType dungeonBattleType)
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
                return false;
        }

        if (DataManager.CurrencyData.IsEnough(ticket, 0))
        {
            BattleManager.Instance.BattleStart(dungeonBattleType, level);

            return true;
        }

        return false;
    }

    public void GetRewardTreasureDungeon(int count)
    {
        List<Reward> rewards = new List<Reward>();

        while (count > 1)
        {
            count--;
        }

        RewardManager.Get(rewards, true);
    }

    public void GetRewardSmithDungeon(int count)
    {
        List<Reward> rewards = new List<Reward>();

        while (count > 1)
        {
            count--;
        }

        RewardManager.Get(rewards, true);
    }

    public void GetRewardBossDungeon(int count)
    {
        List<Reward> rewards = new List<Reward>();

        while (count > 1)
        {
            count--;
        }

        RewardManager.Get(rewards, true);
    }
}