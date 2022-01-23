using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UI_Popup_Passive : UI_BasePopup<UI_Popup_Passive>
{
    public GameObject _onDisableDamage;
    public GameObject _onDisableGold;
    public GameObject _onDisableExp;
    public GameObject _onDisableStone;

    public Text _txtDamageRemainTime;
    public Text _txtGoldRemainTime;
    public Text _txtExpRemainTime;
    public Text _txtStoneRemainTime;

    public Text _txtDamageLimitCount;
    public Text _txtGoldimitCount;
    public Text _txtExpLimitCount;
    public Text _txtStoneLimitCount;

    private void OnEnable()
    {
        TimeManager.Instance.AddOnTickCallback(Refresh);
    }

    private void OnDisable()
    {
        TimeManager.Instance.RemoveOnTickCallback(Refresh);
    }

    protected override void Refresh()
    {
        var damage = DataManager.RuneData.RuneRemainTime[0];
        _onDisableDamage.SetActive(damage >= 0);
        _txtDamageRemainTime.text = $"{damage / 60:D2}:{damage % 60:D2}";
        _txtDamageLimitCount.text = $"일일횟수 ({DataManager.RuneData.RuneLimitCount[0]}/{SystemValue.RUNE_DAILY_LIMIT})";

        var gold = DataManager.RuneData.RuneRemainTime[1];
        _onDisableGold.SetActive(gold >= 0);
        _txtGoldRemainTime.text = $"{gold / 60:D2}:{gold % 60:D2}";
        _txtGoldimitCount.text = $"일일횟수 ({DataManager.RuneData.RuneLimitCount[1]}/{SystemValue.RUNE_DAILY_LIMIT})";

        var exp = DataManager.RuneData.RuneRemainTime[2];
        _onDisableExp.SetActive(exp >= 0);
        _txtExpRemainTime.text = $"{exp / 60:D2}:{exp % 60:D2}";
        _txtExpLimitCount.text = $"일일횟수 ({DataManager.RuneData.RuneLimitCount[2]}/{SystemValue.RUNE_DAILY_LIMIT})";

        var stone = DataManager.RuneData.RuneRemainTime[3];
        _onDisableStone.SetActive(stone >= 0);
        _txtStoneRemainTime.text = $"{stone / 60:D2}:{stone % 60:D2}";
        _txtStoneLimitCount.text = $"일일횟수 ({DataManager.RuneData.RuneLimitCount[3]}/{SystemValue.RUNE_DAILY_LIMIT})";
    }

    public void OnClickAd(int index)
    {
        DataManager.RuneData.TryStartRune((Enum_RuneBuffType) index);
    }
}