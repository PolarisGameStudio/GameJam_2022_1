using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Player_Upgrade_Promotion : UI_BaseContent<UI_Player_Upgrade_Promotion, UI_Player_Upgrade_Promotion_Slot> ,GameEventListener<RefreshEvent>
{
    public Image _imgCurrentPormo;
    public Text _txtCurrentPromo;

    
    private void OnEnable()
    {
        this.AddGameEventListening<RefreshEvent>();        
        Refresh();

    }

    private void OnDisable()
    {
        this.RemoveGameEventListening<RefreshEvent>();
    }

    
    protected override void Refresh()
    {
        var dataCount = TBL_PROMOTION.CountEntities;
        var slotCount = m_SlotList.Count;

        if (dataCount > slotCount)
        {
            Expand(dataCount - slotCount);
        }

        for (int i = 0; i < dataCount; i++)
        {
            m_SlotList[i].Init(TBL_PROMOTION.GetEntity(i));
        }

        var data =TBL_PROMOTION.GetEntity(DataManager.PromotionData.CurrentPromotionIndex);

        _imgCurrentPormo.sprite = AssetManager.Instance.PromotionIcon[data.Index];
        _txtCurrentPromo.text = $"{data.name}";
    }

    public void OnGameEvent(RefreshEvent e)
    {
        if (e.Type == Enum_RefreshEventType.Battle)
        {
            Refresh();
        }
    }

}
