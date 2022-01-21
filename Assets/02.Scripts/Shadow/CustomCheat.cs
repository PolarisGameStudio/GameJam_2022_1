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
        
    [Category("크sadsa리에이티브 발사")]
    public void SendCrsdaeative()
    {
        DateTime t1 = DateTime.Now;
        Debug.Log(Mathf.Pow(1.02f, 100));
        DateTime t2 = DateTime.Now;
        Debug.Log((t2 -t1).Milliseconds);
        Debug.Log(Mathf.Pow(1.02f, 1000));
        DateTime t3 = DateTime.Now;
        Debug.Log((t3 -t2).Milliseconds);
        Debug.Log(Mathf.Pow(1.02f, 10000));
        DateTime t4 = DateTime.Now;
        Debug.Log((t4 -t3).Milliseconds);
    }
}