using System;
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
        ServerManager.Instance.SendCreative(5);
    }    
    
    [Category("몸종")]
    public void getfloower()
    {
        DataManager.FollowerData.AddFollower(0,10);
        DataManager.FollowerData.AddFollower(1,10);
        DataManager.FollowerData.AddFollower(2,10);
        DataManager.FollowerData.AddFollower(3,10);
        DataManager.FollowerData.AddFollower(4,10);
    }
}