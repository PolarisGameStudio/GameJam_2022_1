using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_StageProcess : GameBehaviour, GameEventListener<RefreshEvent> 
{
    [SerializeField] private Slider _processGauge;

    private StageBattle _stageBattle;

    public void Start()
    {
        _stageBattle = BattleManager.Instance.GetBattle<StageBattle>();
        
        this.AddGameEventListening<RefreshEvent>();
        
        Refresh();
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
        if (BattleManager.Instance.CurrentBattleType != Enum_BattleType.Stage)
        {
            SafeSetActive(false);
            return;
        }

        SafeSetActive(true);

        _processGauge.value = _stageBattle.StageProcess;
    }
}
