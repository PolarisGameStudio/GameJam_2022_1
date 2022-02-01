using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Shop_Package_Slot : MonoBehaviour
{
    public Text _txtPrice;

    public TBL_PACKAGE Data;

    public virtual void Init(TBL_PACKAGE data)
    {
        Data = data;

        _txtPrice.text = IAPManager.Instance.GetProductMoneyString(Data.IAP_ID);
    }

    public void OnClickPurchase()
    {
        UI_LoadingBlocker.Instance.Open();
        
        IAPManager.Instance.PurchaseItem(Data.IAP_ID, (success) =>
            {
                UI_LoadingBlocker.Instance.Close();
                DataManager.ShopData.TryPurchase(Data.IAP_ID);
            }, (fail) =>
            {
                UI_LoadingBlocker.Instance.Close();
            }
        );
    }
}
