using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Equipment_List : UI_BaseContent<UI_Equipment_List, UI_Equipment_List_Slot>
{
    private Enum_EquipmentType _currentEquipmentType;

    [SerializeField] private List<Toggle> _toggles;

    private void Start()
    {
        OnToggleClick(0);
    }

    private void ExpandSlot()
    {
        var dataCount = TBL_UPGRADE_GOLD.CountEntities;
        var slotCount = m_SlotList.Count;

        if (dataCount > slotCount)
        {
            Expand(dataCount - slotCount);
        }
    }

    protected override void Refresh()
    {
        ExpandSlot();

        // var equipmentGroup = DataManager.EquipmentData.GetEquipments(_currentEquipmentType);
        //
        // for (int i = 0; i < m_SlotList.Count; i++)
        // {
        //     if (dataCount <= i)
        //     {
        //         m_SlotList[i].SafeSetActive(false);
        //     }
        //
        //     m_SlotList[i].SafeSetActive(true);
        //     m_SlotList[i].Init(new Equipment());
        // }
    }

    public void OnToggleClick(int i)
    {
        _toggles[i].isOn = true;

        _currentEquipmentType = (Enum_EquipmentType) i;

        Refresh();
    }
}