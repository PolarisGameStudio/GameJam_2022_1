using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Player_Level : MonoBehaviour, GameEventListener<PlayerEvent>
{
    [SerializeField] private Text _txtLevel;
    [SerializeField] private Text _txtExp;
    [SerializeField] private Text _txtExpPercent;
    [SerializeField] private Slider _sliderExp;
    [SerializeField] private GameObject _onLevelUpEnable;

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
        if (e.Type == Enum_PlayerEventType.Exp || e.Type == Enum_PlayerEventType.LevelUp)
        {
            Refresh();
        }
    }

    private void Refresh()
    {
        _txtLevel.text = $"Lv.{DataManager.PlayerData.Level + 1}";

        if (DataManager.PlayerData.IsMaxLevel())
        {
            _txtExp.text =  $"Max";
        
            _txtExpPercent.text = $"";
            
            _sliderExp.value = 1;
            _onLevelUpEnable.gameObject.SetActive(false);
        }
        else
        {
            _txtExp.text =  $"{DataManager.PlayerData.Exp} / {DataManager.PlayerData.GetRequireExp()}";

            var expPercent = Mathf.Min(1,DataManager.PlayerData.GetExpPercents());
        
            _txtExpPercent.text = $"{expPercent * 100:N1}%";
            
            _sliderExp.value = expPercent;
            _onLevelUpEnable.gameObject.SetActive(expPercent >= 1);
        }
    }

    public void OnClickLevelUp()
    {
        if (DataManager.PlayerData.TryLevelUp())
        {
            SoundManager.Instance.PlaySound("ui_levelup_button");
        }
    }
}
