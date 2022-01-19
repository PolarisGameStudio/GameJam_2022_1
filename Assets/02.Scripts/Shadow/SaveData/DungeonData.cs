using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonData : SaveDataBase
{
    public int TresureDungeonKillCount = 0;
    public int SmithDungeonLevel = 0;
    public double BossDungeonHighestDamage = 0;

    public override void ValidCheck()
    {
        base.ValidCheck();
    }

    public void GetDungeonReward(Enum_BattleType _dungeonBattleType, int count = 1)
    {
        switch (_dungeonBattleType)
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

    public void TryChallenge(Enum_BattleType _dungeonBattleType)
    {
        Enum_CurrencyType ticket = Enum_CurrencyType.Count;
        int level = 0;
        switch (_dungeonBattleType)
        {
            case Enum_BattleType.TreasureDungeon:
                ticket = Enum_CurrencyType.Ticket_Treasure;
                break;
            case Enum_BattleType.SmithDungeon:
                ticket = Enum_CurrencyType.Ticket_Smith;
                level = SmithDungeonLevel;
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
            BattleManager.Instance.BattleStart(_dungeonBattleType, level);
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
