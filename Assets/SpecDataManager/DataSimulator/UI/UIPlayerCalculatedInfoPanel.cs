using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerCalculatedInfoPanel : MonoBehaviour, GameEventListener<RefreshEvent>
{
    [SerializeField] private Text infoText;
    

    private PlayerGrowStat _statInfo;

    private void Awake()
    {
        this.AddGameEventListening<RefreshEvent>();
    }

    private void Start()
    {
        PlayerDataManager.PlayerDataContainer.GetGrowStat(ref _statInfo);
        
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
