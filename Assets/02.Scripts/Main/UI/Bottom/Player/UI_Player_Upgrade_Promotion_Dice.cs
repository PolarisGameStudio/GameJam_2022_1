using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Player_Upgrade_Promotion_Dice : UI_BaseContent<UI_Player_Upgrade_Promotion_Dice,UI_DiceStat_Slot>
{
    public Text _txtDiceAmount;
    public Text _txtDiceCost;

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
        }

        _txtDiceAmount.text = DataManager.CurrencyData.GetAmount(Enum_CurrencyType.Dice).ToCurrencyString();
        _txtDiceCost.text = diceStat.GetRollPrice().ToString();
    }

    public void TryRoll()
    {
        if (DataManager.PromotionData.DiceStatData.TryRoll())
        {
            Refresh();
        }
    }
}