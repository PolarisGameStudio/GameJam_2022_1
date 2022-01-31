using System;
using System.Collections.Generic;
using UnityEngine;


// 아이템 종류가 추가되도 아래 순서는 건드리지말기!!!!
// RewardType 참조하는 테이블 값 순서 바뀔 수 있음.
public enum RewardType
{
    None,

    Currency,

    Skill,

    Equipment,
    Equipment_Left,
    Equipment_Right,
    Equipment_Mouth,
    Equipment_Ring,

    Follower,


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
                if (value == (int) Enum_CurrencyType.Exp)
                {
                    DataManager.PlayerData.AddExp(count);
                    break;
                }
                
                DataManager.CurrencyData.Add((Enum_CurrencyType) value, count);
                break;

            case RewardType.Skill:
                DataManager.SkillData.AddSkill(value, count);
                break;

            case RewardType.Equipment:
                DataManager.EquipmentData.AddEquipment(value, count);
                break;

            case RewardType.Follower:
                DataManager.FollowerData.AddFollower(value, count);
                break;

            default:
                Debug.LogError($"{reward.RewardType}이 없습니다.");
                break;
        }
    }

    public static void Get(List<Reward> rewards, bool showPopup = false)
    {
        List<Reward> rewardsForUI = new List<Reward>();

        foreach (var reward in rewards)
        {
            Get(reward, rewardsForUI);
        }

        if (showPopup)
        {
            UI_Popup_Reward.Instance.Open(rewards);
        }
    }
}