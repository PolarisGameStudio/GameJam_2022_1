using System.ComponentModel;

public partial class SROptions
{
    [Category("광고재생")]
    public void ShowAd()
    {
        AdManager.Instance.TryShowRequest(ADType.None, () => { }, () => { });
    }
}