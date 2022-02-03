using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Shop_Package_Slot_Daily : UI_Shop_Package_Slot
{
    public Text _txtDailyLimit;


    public override void Init(TBL_PACKAGE data)
    {
        base.Init(data);

        _txtDailyLimit.text = $"일일구매제한 {DataManager.ShopData.DailyLimit[data.Index]}/{SystemValue.TICKET_PACKAGE_DAILY_LIMIT}";
    }
    
    

    public override void OnClickPurchase()
    {
        if (DataManager.ShopData.DailyLimit[Data.Index] >= SystemValue.TICKET_PACKAGE_DAILY_LIMIT)
        {
            return;
        }

        DataManager.ShopData.TryPurchase(Data.IAP_ID);
    }
}
