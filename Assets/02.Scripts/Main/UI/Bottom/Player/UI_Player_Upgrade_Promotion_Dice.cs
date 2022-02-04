using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Player_Upgrade_Promotion_Dice : UI_BaseContent<UI_Player_Upgrade_Promotion_Dice,UI_DiceStat_Slot> , GameEventListener<RefreshEvent>
{
    public Text _txtDiceAmount;
    public Text _txtDiceCost;
    
    protected void OnEnable()
    {
        this.AddGameEventListening<RefreshEvent>();
    }

    protected void OnDisable()
    {
        this.RemoveGameEventListening<RefreshEvent>();
    }
    
    public void Open()
    {
        SafeSetActive(true);
        Refresh();
    }
    
    public void Close()
    {
        SafeSetActive(false);
    }

    protected override void Refresh()
    {
        var diceStat = DataManager.PromotionData.DiceStatData;

        var slotCount = m_SlotList.Count;
        var diceCount = diceStat.DiceSlotList.Count;

        if (diceCount - slotCount > 0)
        {
            Expand(diceCount - slotCount);
        }

        for (int i = 0; i < m_SlotList.Count; i++)
        {
            m_SlotList[i].Init(diceStat.DiceSlotList[i]);

            if (i == 0)
            {
                continue;
            }
            m_SlotList[i].SetDisableText($"{TBL_PROMOTION.GetEntity(i - 1).name}에 해금");
        }

        _txtDiceAmount.text = DataManager.CurrencyData.GetAmount(Enum_CurrencyType.Dice).ToCurrencyString();
        _txtDiceCost.text = diceStat.GetRollPrice().ToString();
    }

    public void TryRoll()
    {
        if (DataManager.PromotionData.DiceStatData.TryRoll())
        {
            Refresh();
            SoundManager.Instance.PlaySound("ui_dice_button");
        }
    }

    public void OnGameEvent(RefreshEvent e)
    {
        if (e.Type == Enum_RefreshEventType.Promotion)
        {
            Refresh();
        }
    }
}
