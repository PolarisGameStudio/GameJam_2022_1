using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Follower : UI_BaseContent<UI_Follower,UI_Follower_Slot>
{
    public List<UI_Follower_Slot> EquippedSlots;

    private void Start()
    {
        Refresh();
    }

    protected override void Refresh()
    {
        var followers = TBL_FOLLOWER.CountEntities;

        var dataCount = followers;
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
            m_SlotList[i].Init(TBL_FOLLOWER.GetEntity(i));
        }

        var equipped = DataManager.FollowerData.EquippedIndex;
        
        for (var i = 0; i < equipped.Count; i++)
        {
            if (equipped[i] == -1)
            {
                EquippedSlots[i].Init(null);
                continue;
            }
            
            EquippedSlots[i].Init(TBL_FOLLOWER.GetEntity(equipped[i]));
        }
    }
}
