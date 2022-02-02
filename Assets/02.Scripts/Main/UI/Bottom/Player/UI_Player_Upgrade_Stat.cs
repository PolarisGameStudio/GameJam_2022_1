using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Player_Upgrade_Stat : UI_BaseContent<UI_Player_Upgrade_Stat,UI_Player_Upgrade_Stat_Slot>, GameEventListener<PlayerEvent> , GameEventListener<RefreshEvent>
{   

    [SerializeField] private Text _txtRemainPoint;
    

    protected override void OnEnable()
    {
        base.OnEnable();

        this.AddGameEventListening<RefreshEvent>();
        this.AddGameEventListening<PlayerEvent>();
        
        InitSlot();
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        this.RemoveGameEventListening<RefreshEvent>();
        this.RemoveGameEventListening<PlayerEvent>();
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
        _txtRemainPoint.text = $"남은 포인트 : {DataManager.StatGrowthData.GetRemainPoint()}";
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

    public void OnClickResetStat()
    {
        UI_Popup_Buy.Instance.Open("성장 초기화", "성장 스탯을 되돌립니다." , Enum_CurrencyType.Gem, SystemValue.STAT_RESET_PRICE, () =>
        {
            if (DataManager.CurrencyData.TryConsume(Enum_CurrencyType.Gem, SystemValue.STAT_RESET_PRICE))
            {
                DataManager.StatGrowthData.ResetStatPoint();
            }
            else
            {
                UI_Popup_OK.Instance.OpenCurrencyNotEnough();
            }
        });
    }
}
