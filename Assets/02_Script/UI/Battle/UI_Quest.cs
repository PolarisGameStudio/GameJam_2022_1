using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Quest : MonoBehaviour , GameEventListener<RefreshEvent>
{
    [SerializeField] private Text _txtTitle;
    [SerializeField] private Text _txtRewardAmount;

    [SerializeField] private GameObject _objOnClear;

    private void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        var quest = DataManager.QuestData.CurrentQuest;
        var progress = DataManager.QuestData.GetProgress();

        _txtTitle.text = $"{quest.QuestType} ({progress}/{quest.CompleteCount})";
        _txtRewardAmount.text = $"{quest.RewardCount}";

        _objOnClear.SetActive(DataManager.QuestData.IsEnableClear());
    }

    public void OnGameEvent(RefreshEvent e)
    {
        if (e.Type == Enum_RefreshEventType.Quest)
        {
            Refresh();
        }
    }

    public void OnClickClaim()
    {
        if (DataManager.QuestData.TryClearQuest())
        {
            Refresh();
        }
    }
}
