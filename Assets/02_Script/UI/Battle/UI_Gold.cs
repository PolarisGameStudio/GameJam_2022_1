using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Gold : SingletonBehaviour<UI_Gold>, GameEventListener<RefreshEvent> , GameEventListener<BattleEvent>
{
    [SerializeField] private Text _goldText;

    private void Start()
    {
        Refresh();
        
        this.AddGameEventListening<RefreshEvent>();
        this.AddGameEventListening<BattleEvent>();
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
        if (BattleManager.Instance.CurrentBattleType != Enum_BattleType.Stage)
        {
            SafeSetActive(false);
            return;
        }
        
        SafeSetActive(true);
        _goldText.text = $"{DataManager.CurrencyData.GetAmount(Enum_CurrencyType.Gold).ToCurrencyString()}";
    }

    public void OnGameEvent(BattleEvent e)
    { 
        if (e.Type == Enum_BattleEventType.BattleStart)
        {
            Refresh();
        }
    }
}
