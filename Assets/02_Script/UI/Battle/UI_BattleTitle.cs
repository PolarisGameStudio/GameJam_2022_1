using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_BattleTitle : GameBehaviour, GameEventListener<RefreshEvent>
{
    [SerializeField] private Text _stageText;
    [SerializeField] private Text _waveText;
    
    private void OnEnable()
    {
        this.AddGameEventListening<RefreshEvent>();
    }

    public void OnGameEvent(RefreshEvent e)
    {
        if (e.Type == Enum_RefreshEventType.Battle)
        {
            Refresh();
        }
    }

    private void Refresh()
    {
        switch (BattleManager.Instance.CurrentBattleType)
        {
            case Enum_BattleType.Stage:
            {
                RefreshStage();
                break;
            }
                
        }
    }

    private void RefreshStage()
    {
        var stageBattle = BattleManager.Instance.GetBattle<StageBattle>();
        _stageText.text = $"Stage {stageBattle.Level + 1}";
        _waveText.text = $"Wave {stageBattle.WaveLevel + 1}";
    }
}
