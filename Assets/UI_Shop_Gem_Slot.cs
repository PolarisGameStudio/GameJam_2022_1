using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Shop_Gem_Slot : MonoBehaviour
{
    public Text _txtAmount;
    public Text _txtPrice;

    public TBL_SHOP Data;

    public void Init(TBL_SHOP data)
    {
        Data = data;

        _txtAmount.text = $"보석 {Data.Amount}개";

        _txtPrice.text = IAPManager.Instance.GetProductMoneyString(Data.IAP_ID);
    }

    public void OnClickPurchase()
    {
        UI_LoadingBlocker.Instance.Open();
        
        IAPManager.Instance.PurchaseItem(Data.IAP_ID, (success) =>
            {
                UI_LoadingBlocker.Instance.Close();
                
                RewardManager.Get(new Reward(RewardType.Currency, (int) Enum_CurrencyType.Gem, Data.Amount));
                
                ShopEvent.Trigger();
                DataManager.Instance.Save(force:true);
                SoundManager.Instance.PlaySound("sfx_coinDrop");
            }, (fail) =>
            {
                UI_LoadingBlocker.Instance.Close();
            }
        );
    }
}