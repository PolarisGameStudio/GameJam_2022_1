using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Equipment : UI_BaseContent<UI_Equipment, UI_Equipment_List_Slot>
{
    private Enum_EquipmentType _currentEquipmentType;

    [SerializeField] private List<Toggle> _toggles;

    private void Start()
    {
        OnToggleClick(0);
    }
    protected override void Refresh()
    { 
        var equipments = DataManager.EquipmentData.GetEquipmentListByType(_currentEquipmentType);
        
        var dataCount = equipments.Count;
        var slotCount = m_SlotList.Count;

        if (dataCount > slotCount)
        {
            Expand(dataCount - slotCount);
        }
        
        for (int i = 0; i < m_SlotList.Count; i++)
        {
            if (m_SlotList.Count >= i)
            {
                m_SlotList[i].SafeSetActive(false);
            }
        
            m_SlotList[i].SafeSetActive(true);
            m_SlotList[i].Init(equipments[i]);
        }
    }

    public void OnToggleClick(int i)
    {
        _toggles[i].isOn = true;

        _currentEquipmentType = (Enum_EquipmentType) i;

        Refresh();
    }
}
