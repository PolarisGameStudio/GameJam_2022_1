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

        if (DataManager.ShopData.PurchaseCounts[Data.Index] > 0 && Data.IsLimited)
        {
            gameObject.SetActive(false);
        }

        _txtPrice.text = IAPManager.Instance.GetProductMoneyString(Data.IAP_ID);
    }

    public virtual void OnClickPurchase()
    {
        DataManager.ShopData.TryPurchase(Data.IAP_ID);
    }
}
