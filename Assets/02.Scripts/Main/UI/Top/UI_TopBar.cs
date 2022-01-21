using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_TopBar : MonoBehaviour, GameEventListener<RefreshEvent>, GameEventListener<PlayerEvent>
{
    public Text TxtGemAmount;
    public Text TxtStoneAmount;
    public Text TxtNickname;
    
    public Text TxtPlayerLevel;
    public Text TxtPlayerExp;
    public Slider SliderPlayerExp;

    private void Awake()
    {
        this.AddGameEventListening<RefreshEvent>();
        this.AddGameEventListening<PlayerEvent>();
    }

    private void Start()
    {
        RefreshCurrency();
        RefreshPlayer();
    }

    public void OnGameEvent(RefreshEvent e)
    {
        if (e.Type == Enum_RefreshEventType.Currency)
        {
            RefreshCurrency();
        }
    }

    private void RefreshCurrency()
    {
        TxtGemAmount.text = DataManager.CurrencyData.GetAmount(Enum_CurrencyType.Gem).ToPriceString();
        TxtStoneAmount.text = DataManager.CurrencyData.GetAmount(Enum_CurrencyType.EquipmentStone).ToPriceString();
        TxtNickname.text = "승급명";
    }

    public void OnGameEvent(PlayerEvent e)
    {
        if (e.Type == Enum_PlayerEventType.LevelUp || e.Type == Enum_PlayerEventType.Exp)
        {
            RefreshPlayer();
        }
    }

    private void RefreshPlayer()
    {
        TxtPlayerLevel.text = $"Lv. {DataManager.PlayerData.Level + 1}";
        TxtPlayerExp.text = $"{DataManager.PlayerData.GetExpPercents() * 100:N0}%";
        SliderPlayerExp.value = DataManager.PlayerData.GetExpPercents();
    }


    public void OnClickOption()
    {
        
    }

    public void OnClickQuest()
    {
        
    }

    public void OnClickAchieve()
    {
        
    }   
    public void OnClickPackage()
    {
        
    }
}
