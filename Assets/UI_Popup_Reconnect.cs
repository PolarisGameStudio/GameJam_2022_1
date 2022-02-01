using UnityEngine.UI;

public class UI_Popup_Reconnect : UI_BasePopup<UI_Popup_Reconnect>
{
    public Text _txtReconnectTime;

    public Text _txtGoldAmound;
    public Text _txtExpAmound;
    public Text _txtStoneAmound;

    protected override void Refresh()
    {
        var minute = ReconnectManager.Instance.Minute;
        _txtReconnectTime.text = $"<color=yellow>{(minute / 60):D2}h {(minute % 60):D2}m</color> / 12h 00m";

        _txtGoldAmound.text = ReconnectManager.Instance.GetGoldAmount().ToCurrencyString();
        _txtExpAmound.text = ReconnectManager.Instance.GetExpAmount().ToCurrencyString();
        _txtStoneAmound.text = ReconnectManager.Instance.GetStoneAmount().ToCurrencyString();
    }

    public void OnClickGetButton()
    {
        ReconnectManager.Instance.GetReward(false);
        Close();
    }

    public void OnClickGetDoubleButton()
    {
        AdManager.Instance.TryShowRequest(ADType.Reconnect, () =>
        {
            ReconnectManager.Instance.GetReward(true);
            Close();
        });
    }
}