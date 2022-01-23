using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Passive : MonoBehaviour
{
    public GameObject _onDisableDamage;
    public GameObject _onDisableGold;
    public GameObject _onDisableExp;
    public GameObject _onDisableStone;

    public Text _txtDamageRemainTime;
    public Text _txtGoldRemainTime;
    public Text _txtExpRemainTime;
    public Text _txtStoneRemainTime;

    private void Start()
    {
        TimeManager.Instance.AddOnTickCallback(Refresh);
    }

    protected void Refresh()
    {
        var damage = DataManager.RuneData.RuneRemainTime[0];
        _onDisableDamage.SetActive(damage >= 0);
        _txtDamageRemainTime.text = $"{damage / 60:D2}:{damage % 60:D2}";

        var gold = DataManager.RuneData.RuneRemainTime[1];
        _onDisableGold.SetActive(gold >= 0);
        _txtGoldRemainTime.text = $"{gold / 60:D2}:{gold % 60:D2}";

        var exp = DataManager.RuneData.RuneRemainTime[2];
        _onDisableExp.SetActive(exp >= 0);
        _txtExpRemainTime.text = $"{exp / 60:D2}:{exp % 60:D2}";

        var stone = DataManager.RuneData.RuneRemainTime[3];
        _onDisableStone.SetActive(stone >= 0);
        _txtStoneRemainTime.text = $"{stone / 60:D2}:{stone % 60:D2}";
    }
}
