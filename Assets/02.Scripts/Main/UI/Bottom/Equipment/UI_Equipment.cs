using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Equipment : UI_BaseContent<UI_Equipment, UI_Equipment_List_Slot>, GameEventListener<RefreshEvent>
{
    private Enum_EquipmentType _currentEquipmentType;

    [SerializeField] private List<Toggle> _toggles;

    protected override void OnEnable()
    {
        base.OnEnable();

        this.AddGameEventListening<RefreshEvent>();
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        this.RemoveGameEventListening<RefreshEvent>();
    }

    protected override void Refresh()
    {
        var equipments = TBL_EQUIPMENT.GetEntitiesByKeyWithEquipmentType(_currentEquipmentType);

        var dataCount = equipments.Count;
        var slotCount = m_SlotList.Count;

        if (dataCount > slotCount)
        {
            Expand(dataCount - slotCount);
        }

        for (int i = 0; i < m_SlotList.Count; i++)
        {
            if (i >= dataCount)
            {
                m_SlotList[i].SafeSetActive(false);
                continue;
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

    public void OnGameEvent(RefreshEvent e)
    {
        if (e.Type == Enum_RefreshEventType.Equipment)
        {
            Refresh();
        }
    }
    
    
    public void OnClickGradeAll()
    {
        if (DataManager.EquipmentData.TryGradeAll())
        {
            Refresh();
        }
    }
}