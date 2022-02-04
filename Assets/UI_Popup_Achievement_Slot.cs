using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Popup_Achievement_Slot : UI_BaseSlot<TBL_ACHIEVEMENT> 
{
    public Text _txtTitle;
    public Text _txtProgress;
    public Text _txtReward;

    public GameObject _onClear;
    public Button _btnClaim;

    public override void Init(TBL_ACHIEVEMENT data)
    {
        _data = data;
        _txtTitle.text = _data.name;
        Refresh();
    }

    public void Refresh()
    {
        _txtTitle.text = _data.name;
        
        var isClear = DataManager.AchievementData.IsClear[_data.Index];
        _onClear.SetActive(isClear);
        _btnClaim.gameObject.SetActive(!isClear);

        _btnClaim.interactable = DataManager.AchievementData.IsEnableClear(_data.AchievementMission);

        _txtProgress.text = isClear ? "" : $"{DataManager.AchievementData.Progress[_data.Index]} / {_data.CompleteCount}";

        _txtReward.text = _data.RewardCount.ToString();
    }

    public void OnClickClaim()
    {
        if (DataManager.AchievementData.TryClearAchieve(_data.AchievementMission))
        {
            Refresh();
            SoundManager.Instance.PlaySound("sfx_coinDrop");
        }
    }
}
