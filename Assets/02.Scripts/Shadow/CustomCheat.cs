using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public partial class SROptions
{
    
    [Category("광고재생")]
    public void ShowAd()
    {
        AdManager.Instance.TryShowRequest(ADType.None, () => { }, () => { });
    }

    [Category("크리에이티브 발사")]
    public void SendCreative()
    {
        UI_Popup_Review.Instance.Open();
    }

    [Category("몸종")]
    public void getfloower()
    {
        List<Reward> rewards = new List<Reward>();
        
        rewards.Add(new Reward(RewardType.Follower,0,10));
        rewards.Add(new Reward(RewardType.Follower,1,10));
        rewards.Add(new Reward(RewardType.Follower,2,10));
        rewards.Add(new Reward(RewardType.Follower,3,10));
        
        RewardManager.Get(rewards);
        
        for (var i = 0; i < DataManager.FollowerData.Levels.Count; i++)
        {
            DataManager.FollowerData.Levels[i] += 1;
        }
    }

    [Category("스킬")]

    public void saf()
    {
        // List<Reward> rewards = new List<Reward>();
        //
        // rewards.Add(new Reward(RewardType.Skill,0,10));
        // rewards.Add(new Reward(RewardType.Skill,1,10));
        // rewards.Add(new Reward(RewardType.Skill,2,10));
        // rewards.Add(new Reward(RewardType.Skill,3,10));
        // rewards.Add(new Reward(RewardType.Skill,4,10));
        // rewards.Add(new Reward(RewardType.Skill,5,10));
        //
        // UI_Popup_Reward.Instance.Open(rewards);

        for (var i = 0; i < DataManager.SkillData.Levels.Count; i++)
        {
            DataManager.SkillData.Levels[i] += 1;
        }
    }
}