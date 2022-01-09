using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class DataSimulatorManager : MonoBehaviour
{
    [SerializeField]
    private UIPlayerInfoPanel _playerInfoPanel;
    
    
    [SerializeField]
    private GameObject statUpgradePanel;
    
    [SerializeField]
    private GameObject prefUpgradeField;
    [SerializeField]
    private GameObject prefAddValueField;
    [SerializeField]
    private GameObject prefToggleField;

    
    
    
    
    private List<UISimulatorStatField> _listUpgradeFileds = new List<UISimulatorStatField>();
    
    void Awake()
    {
        Application.targetFrameRate = 60;
        Application.runInBackground = true;

        GenerateStatButtons();
        GenerateAddExpButton();
        GenerateAddBerserkerButton();
    }

    
    
    
    /// <summary>
    /// enum 파일 기준으로 버튼 생성한다.
    /// reflection : 라이브 프로젝트 사용 불가.
    /// </summary>
    void GenerateStatButtons()
    {
        FieldInfo[] fields = typeof(PlayerUpgradeStat).GetFields();
        
        foreach (var fieldInfo in fields)
        {
            try
            {
                Enum_UpgradeStat type = (Enum_UpgradeStat)Enum.Parse(typeof(Enum_UpgradeStat), fieldInfo.Name);
                UISimulatorStatField sc = Instantiate(prefUpgradeField, statUpgradePanel.transform).GetComponent<UISimulatorStatField>();
                sc.SetFieldData(type);
                _listUpgradeFileds.Add(sc);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }

    void GenerateAddExpButton()
    {
        UISimulatorAddValueField sc = Instantiate(prefAddValueField, statUpgradePanel.transform).GetComponent<UISimulatorAddValueField>();
        sc.SetFieldData("add exp", d =>
        {
            PlayerDataManager.PlayerDataContainer.AddExp(d);
            RefreshEvent.Trigger(Enum_RefreshEventType.PlayerStat);
        });
    }

    void GenerateAddBerserkerButton()
    {
        UISimulatorToggleField sc = Instantiate(prefToggleField, statUpgradePanel.transform).GetComponent<UISimulatorToggleField>();
        sc.SetFieldData("berserker", d =>
        {
            BerserkerStat ssc = null;
            PlayerDataManager.PlayerDataContainer.GetBerserkerStat(ref ssc);
            ssc.SetBerserkerState(d);
            RefreshEvent.Trigger(Enum_RefreshEventType.PlayerStat);
        });
    }
    
}
