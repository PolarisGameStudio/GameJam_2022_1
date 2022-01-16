using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UI_Currency : GameBehaviour, GameEventListener<RefreshEvent> , GameEventListener<BattleEvent>
{
    [SerializeField] private Text _goldText;

    private void Start()
    {
        Refresh();
        
        this.AddGameEventListening<RefreshEvent>();
    }

    public void OnGameEvent(RefreshEvent e)
    {
        if (e.Type == Enum_RefreshEventType.Currency)
        {
            Refresh();
        }
    }

    private void Refresh()
    {
        if(BattleManager.Instance.CurrentBattleType != Enum_BattleType.Stage)
        
        _goldText.text = $"{DataManager.CurrencyData.GetAmount(Enum_CurrencyType.Gold)}";
    }

    public void OnGameEvent(BattleEvent e)
    { 
        if (e.Type == Enum_BattleEventType.BattleStart)
        {
            Refresh();
        }
    }
}


