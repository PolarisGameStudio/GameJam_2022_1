using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Player_TopInfo_Level : MonoBehaviour, GameEventListener<PlayerEvent>
{
    [SerializeField] private Text _txtLevel;
    [SerializeField] private Text _txtExp;
    [SerializeField] private Text _txtExpPercent;
    [SerializeField] private Slider _sliderExp;

    private void OnEnable()
    {
        this.AddGameEventListening<PlayerEvent>();
        Refresh();
    }

    private void OnDisable()
    {
        this.RemoveGameEventListening<PlayerEvent>();
    }

    public void OnGameEvent(PlayerEvent e)
    {
        if (e.Type == Enum_PlayerEventType.Exp)
        {
            Refresh();
        }
    }

    private void Refresh()
    {
        _txtLevel.text = $"Lv.{DataManager.PlayerData.Level}";
        _txtExp.text =  $"{DataManager.PlayerData.Exp} / {DataManager.PlayerData.GetRequireExp()}";

        var expPercent = Mathf.Min(1,DataManager.PlayerData.GetExpPercents());
        
        _txtExpPercent.text = $"{expPercent * 100:N2}%";
        _sliderExp.value = expPercent;
    }

    public void OnClickLevelUp()
    {
        DataManager.PlayerData.TryLevelUp();
    }
}
