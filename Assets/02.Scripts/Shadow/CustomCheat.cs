using System.ComponentModel;

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
}