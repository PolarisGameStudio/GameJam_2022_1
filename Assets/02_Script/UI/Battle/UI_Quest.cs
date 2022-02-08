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
        this.AddGameEventListening<RefreshEvent>();
        Refresh();
    }

    public void Refresh()
    {
        if (DataManager.QuestData.IsAllQuestFinish)
        {
            gameObject.SetActive(false);
            return;
        }
        
        var quest = DataManager.QuestData.CurrentQuest;
        var progress = DataManager.QuestData.GetProgress();

        _txtTitle.text = $"{quest.name} ({progress}/{quest.CompleteCount})";
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
            SoundManager.Instance.PlaySound("sfx_coinDrop");
        }
    }
}
