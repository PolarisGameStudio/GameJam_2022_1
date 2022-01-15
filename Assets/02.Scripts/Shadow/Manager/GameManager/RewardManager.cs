﻿using System;
using System.Collections.Generic;
using UnityEngine;


// 아이템 종류가 추가되도 아래 순서는 건드리지말기!!!!
// RewardType 참조하는 테이블 값 순서 바뀔 수 있음.
public enum RewardType
{
    None,
    
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
    public static RewardType StringToRewardType(string typeString)
    {
        return (RewardType) Enum.Parse(typeof(RewardType), typeString);
    }
    
    public static Sprite GetSprite(RewardType type, int value)
    {
        switch (type)
        {
     

            default:
                return null;
        }
    }

    public static void Get(List<Reward> rewards)
    {
        List<Reward> rewardsForUI = new List<Reward>();
        foreach (var reward in rewards)
        {
            rewardsForUI.Add(reward);
            Get(reward, rewardsForUI);
        }
    }  
    public static void Get(Reward reward)
    {
        List<Reward> rewardsForUI = new List<Reward>();
        
        rewardsForUI.Add(reward);
        Get(reward, rewardsForUI);
    }

    public static void Get(Reward reward, List<Reward> rewardsForUI)
    {
        int count = (int)reward.Count;
        int value = reward.Value;

        switch (reward.RewardType)
        {
   
        }
    }

    public static void GetWithRewardUI(Reward reward)
    {
        List<Reward> rewardsForUI = new List<Reward>(1);

        rewardsForUI.Add(reward);

        Get(reward, rewardsForUI);
    }

    public static void GetWithRewardUI(List<Reward> rewards)
    {
        List<Reward> rewardsForUI = new List<Reward>();

    }
}