using System;
using System.Collections.Generic;
using EnhancedScrollerDemos.Chat;
using NPOI.SS.Formula.Functions;
using UnityEngine;


// 아이템 종류가 추가되도 아래 순서는 건드리지말기!!!!
// RewardType 참조하는 테이블 값 순서 바뀔 수 있음.
public enum RewardType
{
    None,

    Currency,

    Skill,

    Equipment_Left,
    Equipment_Right,
    Equipment_Mouth,
    Equipment_Ring,

    Follower,

    Costume,

    Count,
}

public class Reward
{
    public RewardType RewardType;

    /// Value는 획득할 아이템이 명확한 타입일 경우 해당 타입의 인덱스(순서)값!
    /// 획득할 아이템이 랜덤일 경우 등급! ex) 0 => 노말, 1 => 레어, 2 => 에픽
    public int Value;

    /// Count는 획득할 아이템이 명확한 타입일 경우 갯수!
    /// 획득할 아이템이 랜덤일 경우 횟수!
    public double Count;

    public Reward(RewardType rewardType, int value, double count)
    {
        Init(rewardType, value, count);
    }

    public void Init(RewardType rewardType, int value, double count)
    {
        RewardType = rewardType;
        Value = value;
        Count = count;
    }
}

public static class RewardManager
{
    public static void Get(Reward reward, List<Reward> rewardsForUI)
    {
        int count = (int) reward.Count;
        int value = reward.Value;

        switch (reward.RewardType)
        {
            case RewardType.Currency:
                DataManager.CurrencyData.Add((Enum_CurrencyType) value, count);
                break;

            case RewardType.Skill:
                break;

            // case RewardType.Equipment_Left:
            //     DataManager.EquipmentData.AddEquipment(Enum_EquipmentType.Left, value, count);
            //     break;
            // case RewardType.Equipment_Right:
            //     DataManager.EquipmentData.AddEquipment(Enum_EquipmentType.Right, value, count);
            //     break;
            // case RewardType.Equipment_Mouth:
            //     DataManager.EquipmentData.AddEquipment(Enum_EquipmentType.Mouth, value, count);
            //     break;
            // case RewardType.Equipment_Ring:
            //     DataManager.EquipmentData.AddEquipment(Enum_EquipmentType.Ring, value, count);
            //     break;

            case RewardType.Follower:
                DataManager.FollowerData.AddFoloower(value, count);
                break;

            case RewardType.Costume:
                break;

            default:
                Debug.LogError($"{reward.RewardType}이 없습니다.");
                break;
        }
    }

    public static void GetWithRewardUI(List<Reward> rewards)
    {
        List<Reward> rewardsForUI = new List<Reward>();

        foreach (var reward in rewards)
        {
            Get(reward, rewardsForUI);
        }
    }
}