using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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
                GetRewardTreasureDungeon(level);
                break;
            case Enum_BattleType.SmithDungeon:
                SmithDungeonHighLevel = Mathf.Max(SmithDungeonHighLevel, level + 1);
                DataManager.CurrencyData.TryConsume(Enum_CurrencyType.Ticket_Smith, 1);
                GetRewardSmithDungeon(level);
                break;
            case Enum_BattleType.BossDungeon:
                BossDungeonHighLevel = Mathf.Max(BossDungeonHighLevel, level);
                DataManager.CurrencyData.TryConsume(Enum_CurrencyType.Ticket_Boss, 1);
                GetRewardBossDungeon(level);
                break;

            default:
                Debug.LogError("던전 아니면 안됨");
                return;
        }

        DataManager.AchievementData.ProgressAchievement(Enum_AchivementMission.Daily_EnterDungeon, 1);
        RefreshEvent.Trigger(Enum_RefreshEventType.Quest);
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
                if (TreasureDungeonHighLevel == 0)
                {
                    return false;
                }

                DataManager.CurrencyData.TryConsume(Enum_CurrencyType.Ticket_Treasure, count);
                GetRewardTreasureDungeon(TreasureDungeonHighLevel);
                break;

            case Enum_BattleType.SmithDungeon:
                if (SmithDungeonHighLevel == 0)
                {
                    return false;
                }

                DataManager.CurrencyData.TryConsume(Enum_CurrencyType.Ticket_Smith, count);
                GetRewardSmithDungeon(SmithDungeonHighLevel);
                break;

            case Enum_BattleType.BossDungeon:
                if (BossDungeonHighLevel == 0)
                {
                    return false;
                }

                DataManager.CurrencyData.TryConsume(Enum_CurrencyType.Ticket_Boss, count);
                GetRewardBossDungeon(BossDungeonHighLevel);
                break;
            
            default:
                return false;
        }

        DataManager.AchievementData.ProgressAchievement(Enum_AchivementMission.Daily_EnterDungeon, 1);
        return true;
    }

    public bool TryChallenge(Enum_BattleType dungeonBattleType)
    {
        if (BattleManager.Instance.CurrentBattle.BattleType != Enum_BattleType.Stage)
        { 
            return false;
        }
        
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

    public void GetRewardTreasureDungeon(int level)
    {
        List<Reward> rewards = new List<Reward>();

        double gold = 0;
        double gem = 0;
        double stone = 0;

        for (int i = 0; i < level; i++)
        {
            var data = TBL_DUNGEON_TREASURE.GetEntity(i);

            gem += data.RewardCount1;
            gold += data.RewardCount2;
            stone += data.RewardCount3;
        }
        
        rewards.Add(new Reward(RewardType.Currency, (int) Enum_CurrencyType.Gem, gem));
        rewards.Add(new Reward(RewardType.Currency, (int) Enum_CurrencyType.Gold, gold));
        rewards.Add(new Reward(RewardType.Currency, (int) Enum_CurrencyType.EquipmentStone, stone));

        RewardManager.Get(rewards, true);
    }

    public void GetRewardSmithDungeon(int level)
    {
        List<Reward> rewards = new List<Reward>();

        var data = TBL_DUNGEON_SMITH.GetEntity(level);

        int randomAmount = Random.Range(data.EquipmentMinCount1, data.EquipmentMaxCount1 + 1);
        
        for (int i = 0; i < randomAmount; i++)
        {
            int type = Random.Range(0, 4);
            int index = 0;

            if (type == 3)
            {
                index = GachaManager.Instance.GachaByGrade(GachaType.Ring, data.EquipmentGrade1);
            }
            else
            {
                index = GachaManager.Instance.GachaByGrade(GachaType.Weapon, data.EquipmentGrade1);
            }

            var target = rewards.Find(x => x.Value == index);
        
            if (target == null)
            {
                rewards.Add(new Reward(RewardType.Equipment, index, 1));
            }
            else
            {
                target.Count += 1;
            }
        } 
        
        int randomAmount2 = Random.Range(data.EquipmentMinCount2, data.EquipmentMaxCount2 + 1);
        
        for (int i = 0; i < randomAmount2; i++)
        {
            int type = Random.Range(0, 4);
            int index = 0;

            if (type == 3)
            {
                index = GachaManager.Instance.GachaByGrade(GachaType.Ring, data.EquipmentGrade2);
            }
            else
            {
                index = GachaManager.Instance.GachaByGrade(GachaType.Weapon, data.EquipmentGrade2);
            }

            var target = rewards.Find(x => x.Value == index);
        
            if (target == null)
            {
                rewards.Add(new Reward(RewardType.Equipment, index, 1));
            }
            else
            {
                target.Count += 1;
            }
        }
        
        rewards.Add(new Reward(RewardType.Currency, (int) Enum_CurrencyType.EquipmentStone, data.EquipmentStonCount));

        RewardManager.Get(rewards, true);
    }

    public void GetRewardBossDungeon(int level)
    {
        List<Reward> rewards = new List<Reward>();
        
        double diceAmount = 0;

        List<int> followers = new List<int>(new int[TBL_FOLLOWER.CountEntities]);

        for (int i = 0; i < level; i++)
        {
            var data = TBL_DUNGEON_BOSS.GetEntity(i);
            diceAmount += data.DiceCount;

            var count1 = Random.Range(data.FollowerMinCount1, data.FollowerMaxCount1 + 1);
            int randomIndex1 = Random.Range(0, TBL_FOLLOWER.CountEntities);
            followers[randomIndex1] += count1;           
            
            var count2 = Random.Range(data.FollowerMinCount1, data.FollowerMaxCount1 + 1);
            int randomIndex2 = Random.Range(0, TBL_FOLLOWER.CountEntities);
            followers[randomIndex2] += count2;
        }

        for (var i = 0; i < followers.Count; i++)
        {
            if (followers[i] == 0)
            {
                continue;
            }

            rewards.Add(new Reward(RewardType.Follower, i, followers[i]));
        }

        rewards.Add(new Reward(RewardType.Currency, (int) Enum_CurrencyType.Dice, diceAmount));

        RewardManager.Get(rewards, true);
    }
}