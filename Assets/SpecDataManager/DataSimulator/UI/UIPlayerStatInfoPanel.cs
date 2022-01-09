using System;
using System.Collections;
using System.Collections.Generic;
using DataSimulator;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerStatInfoPanel : MonoBehaviour, GameEventListener<RefreshEvent>
{
    [SerializeField] private Text infoText;
    
    private void Awake()
    {
        this.AddGameEventListening<RefreshEvent>();
    }

    private void Start()
    {
        InitPanel();
    }

    public void InitPanel()
    {
        infoText.text = PlayerDataManager.PlayerDataContainer.ToString();
    }


    public void OnGameEvent(RefreshEvent e)
    {
        if (e.Type == Enum_RefreshEventType.PlayerStat)
        {
            InitPanel();
        }
    }

}
