using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Player_Upgrade_Stat : UI_BaseContent<UI_Player_Upgrade_Stat,UI_Player_Upgrade_Stat_Slot>, GameEventListener<PlayerEvent> , GameEventListener<RefreshEvent>
{   

    [SerializeField] private Text _txtRemainPoint;
    
    private void Start()
    {
        InitSlot();

        this.AddGameEventListening<RefreshEvent>();
        this.AddGameEventListening<PlayerEvent>();
    }

    private void InitSlot()
    {
        var dataCount = TBL_UPGRADE_STAT.CountEntities;
        var slotCount = m_SlotList.Count;

        if (dataCount > slotCount)
        {
            Expand(dataCount - slotCount);
        }

        for (int i = 0; i < dataCount; i++)
        {
            m_SlotList[i].Init(TBL_UPGRADE_STAT.GetEntity(i));
        }

        Refresh();
    }

    protected override void Refresh()
    {
        _txtRemainPoint.text = $"남은 포인트 : {DataManager.StatGrowthData.RemainPoint}";
    }

    public void CheckEnableLevelUpSlot()
    {
        foreach (var slot in m_SlotList)
        {
            slot.CheckEnableLevelUp();
        }
    }

    public void OnGameEvent(PlayerEvent e)
    {
        if (e.Type == Enum_PlayerEventType.LevelUp)
        {
            Refresh();
        }
    }

    public void OnGameEvent(RefreshEvent e)
    {
        if (e.Type == Enum_RefreshEventType.StatGrowth)
        {
            Refresh();
        }
    }
}
