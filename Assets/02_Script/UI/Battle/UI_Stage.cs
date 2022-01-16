
using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_Stage : SingletonBehaviour<UI_Stage> , GameEventListener<BattleEvent>
{
    [SerializeField] private Text _txtStageTitle;

    private StageBattle _stage;
    
    private void Start()
    {
        _stage = FindObjectOfType<StageBattle>();
        this.AddGameEventListening<BattleEvent>();
    }

    public void Refresh()
    {
        if (BattleManager.Instance.CurrentBattleType != Enum_BattleType.Stage)
        {
            SafeSetActive(false);
            return;
        }
        
        SafeSetActive(true);

        _txtStageTitle.text = _stage.StageTitle;
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
        throw new Exception("UI 없슴");
    }
    
    
}
