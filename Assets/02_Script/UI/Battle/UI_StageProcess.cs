using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_StageProcess : GameBehaviour, GameEventListener<BattleEvent> , GameEventListener<RefreshEvent>
{
    [SerializeField] private Slider _processGauge;

    public void Start()
    {
        this.AddGameEventListening<RefreshEvent>();
        this.AddGameEventListening<BattleEvent>();
        
        Refresh();
    }
    
    public void OnGameEvent(BattleEvent e)
    {
        if (e.Type == Enum_BattleEventType.BattleStart)
        {
            Refresh();
        }
    }

    private void Refresh()
    {
        if (BattleManager.Instance.CurrentBattleType != Enum_BattleType.Stage &&
            BattleManager.Instance.CurrentBattleType != Enum_BattleType.SmithDungeon)
        {
            SafeSetActive(false);
            return;
        }

        SafeSetActive(true);

        _processGauge.value = BattleManager.Instance.CurrentBattle.GetProgress();
    }

    public void OnGameEvent(RefreshEvent e)
    {
        if (e.Type == Enum_RefreshEventType.Battle)
        {
            Refresh();
        }
    }
}
