
using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_Stage : SingletonBehaviour<UI_Stage> , GameEventListener<RefreshEvent>
{
    [SerializeField] private Text _txtStageTitle;

    private StageBattle _stage;
    
    private void Awake()
    {
        _stage = FindObjectOfType<StageBattle>();
        this.AddGameEventListening<RefreshEvent>();
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

    public void OnGameEvent(RefreshEvent e)
    {
        if (e.Type == Enum_RefreshEventType.Battle)
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
