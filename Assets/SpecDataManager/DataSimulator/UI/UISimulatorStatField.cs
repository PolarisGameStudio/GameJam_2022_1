using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISimulatorStatField : MonoBehaviour
{
    [SerializeField] private Text filedName;
    [SerializeField] private InputField inputValue;
    [SerializeField] private Button btnUp;
    [SerializeField] private Button btnDown;

    private Enum_UpgradeStat _currentStat;
    
    private void Awake()
    {
        this.btnUp.onClick.AddListener(OnClickUp);
        this.btnDown.onClick.AddListener(OnClickDown);
    }

    public void SetFieldData(Enum_UpgradeStat statType)
    {
        _currentStat = statType;
        filedName.text = statType.ToString();
        inputValue.text = PlayerDataManager.PlayerDataContainer.GetStatValue(statType).ToString();
    }

    void OnClickUp()
    {
        // Debug.LogError("Click up : " + filedName.text);
        int result = PlayerDataManager.PlayerDataContainer.StatLevelUp(_currentStat);
        inputValue.text = result.ToString();
        
        RefreshEvent.Trigger(Enum_RefreshEventType.PlayerStat);
    }
    
    void OnClickDown()
    {
        // Debug.LogError("Click down : " + filedName.text);
        int result = PlayerDataManager.PlayerDataContainer.StatLevelUp(_currentStat);
        inputValue.text = result.ToString();
        
        RefreshEvent.Trigger(Enum_RefreshEventType.PlayerStat);
    }
}
