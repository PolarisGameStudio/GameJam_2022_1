using System.Collections.Generic;
using DataSimulator;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerInfoPanel : MonoBehaviour, GameEventListener<RefreshEvent>
{
    [SerializeField] private Text infoText;
    
    private PlayerUpgradeStat _statInfo;

    private void Awake()
    {
        this.AddGameEventListening<RefreshEvent>();
    }

    private void Start()
    {
        PlayerDataManager.PlayerDataContainer.GetUpgradeStat(ref _statInfo);
        
        InitPanel();
        
    }

    public void InitPanel()
    {
        infoText.text = _statInfo?.ToString();
    }


    public void OnGameEvent(RefreshEvent e)
    {
        if (e.Type == Enum_RefreshEventType.PlayerStat)
        {
            InitPanel();
        }
    }

}
