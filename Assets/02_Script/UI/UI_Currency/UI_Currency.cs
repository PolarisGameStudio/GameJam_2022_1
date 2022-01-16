using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UI_Currency : GameBehaviour, GameEventListener<RefreshEvent>
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
        _goldText.text = $"{DataManager.CurrencyData.GetAmount(Enum_CurrencyType.Gold)}";
    }
}


