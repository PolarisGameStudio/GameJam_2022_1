using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UI_Popup_Achievement : UI_BaseContent<UI_Popup_Achievement, UI_Popup_Achievement_Slot>, GameEventListener<RefreshEvent>
{
    private Enum_AchivementType _type;
    
    protected override void Awake()
    {
        base.Awake();
        _type = Enum_AchivementType.Daily;
        SafeSetActive(false);
    }

    public void OnToggleLoopType(bool isOn)
    {
        _type = isOn ? Enum_AchivementType.Loop : Enum_AchivementType.Daily;
        Refresh();
    }

    private void SetAchievementSlot()
    {
        var list = TBL_ACHIEVEMENT.GetEntitiesByKeyWithType(_type);

        var dataCount = list.Count;
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
            m_SlotList[i].Init(list[i]);
        }
    }

    protected override void Refresh()
    { 
        SetAchievementSlot();
        
        for (int i = 0; i < m_SlotList.Count; i++)
        {
            m_SlotList[i].Refresh();
        }
    }
    

    public void OnGameEvent(RefreshEvent e)
    {
        if (e.Type == Enum_RefreshEventType.Acheieve)
        {
            Refresh();
        }
    }
    
    

    private void OnEnable()
    {
        this.AddGameEventListening<RefreshEvent>();
    }

    private void OnDisable()
    {
        this.RemoveGameEventListening<RefreshEvent>();
    }

    public override void Close()
    {
        base.Close();
        
        UIManager.Instance.Pop(UIType.Popup_Achieve);
    }

    public override void Open()
    {
        base.Open();
        
        UIManager.Instance.Push(UIType.Popup_Achieve);
    }
}