using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Skill : UI_BaseContent<UI_Skill,UI_Skill_Slot> , GameEventListener<RefreshEvent>
{
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
        var skills = TBL_SKILL.CountEntities;

        var dataCount = skills;
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
            m_SlotList[i].Init(TBL_SKILL.GetEntity(i));
        }
    }

    public void OnGameEvent(RefreshEvent e)
    {
        if (e.Type == Enum_RefreshEventType.Skill)
        {
            Refresh();   
        }
    }
}