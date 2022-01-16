using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Player_Upgrade_Gold : UI_BaseContent<UI_Player_Upgrade_Gold,UI_Player_Upgrade_Gold_Slot>
{
    private void Start()
    {
        InitSlot();
    }

    private void InitSlot()
    {
        var dataCount = TBL_UPGRADE_GOLD.CountEntities;
        var slotCount = m_SlotList.Count;

        if (dataCount > slotCount)
        {
            Expand(dataCount - slotCount);
        }

        for (int i = 0; i < dataCount; i++)
        {
            m_SlotList[i].Init(TBL_UPGRADE_GOLD.GetEntity(i));
        }
    }

    protected override void Refresh()
    {
    }

    public void CheckEnableLevelUpSlot()
    {
        foreach (var slot in m_SlotList)
        {
            slot.CheckEnableLevelUp();
        }
    }
}
