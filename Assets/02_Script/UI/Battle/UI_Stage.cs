
using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_Stage : SingletonBehaviour<UI_Stage> , GameEventListener<BattleEvent>
{
    [SerializeField] private Text _txtStageTitle;
    
    [SerializeField] private GameObject _btnWorld;
    [SerializeField] private GameObject _btnBossChallenge;
    [SerializeField] private GameObject _btnGiveUp;
    
    private void Awake()
    {
        this.AddGameEventListening<BattleEvent>();
    }

    public void Refresh()
    {
        SafeSetActive(true);
        
        _btnBossChallenge.gameObject.SetActive(BattleManager.Instance.CurrentBattleType == Enum_BattleType.Stage);
        _btnWorld.gameObject.SetActive(BattleManager.Instance.CurrentBattleType == Enum_BattleType.Stage);
        _btnGiveUp.gameObject.SetActive(BattleManager.Instance.CurrentBattleType != Enum_BattleType.Stage);

        _txtStageTitle.text = BattleManager.Instance.CurrentBattle.GetBattleTitle();
    }

    public void OnGameEvent(BattleEvent e)
    {
        if (e.Type == Enum_BattleEventType.BattleStart)
        {
            Refresh();
        }
    }

    public void OnClickBossChallengeBattle()
    {
        BattleManager.Instance.BattleStart(Enum_BattleType.StageBoss, DataManager.StageData.StageLevel);
    }

    public void OnClickWorldButton()
    {
        UI_Popup_World.Instance.Open();
    }
    
    public void OnClickGiveUp()
    {
        BattleManager.Instance.BattleStart(Enum_BattleType.Stage, DataManager.StageData.StageLevel);
    }
}
