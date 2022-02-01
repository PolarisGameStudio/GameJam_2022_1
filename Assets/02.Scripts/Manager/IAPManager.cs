using System;
using System.Globalization;
using System.Linq;
using IAPModule;
using UnityEngine.Purchasing;


public class IAPManager : SingletonBehaviour<IAPManager>
{
    protected IAPManager()
    {
    }

    private bool isReady = false;

    public bool IsReady
    {
        get { return isReady; }
    }

    protected Action<IAPModule.IAPSuccessResult> OnIapSuccessed = null;
    protected Action<IAPModule.IAPFailResult> OnIapFailed = null;
    //
    // protected Action OnIapComplete = null;
    // protected Action OnIapFailed = null;

    private IAPModule.IAPModule iapModule = null;

    // 초기화중 재진입 방지용.
    private bool isInitializing = false;

    public bool IsInitializing
    {
        get { return isInitializing; }
    }

    private string currencySymbol = null;

    //  private string[] productList = null;

    private Action<bool> _OnRetryInitComplete;

    // 인앱 관리자 초기화 
    //public void Initialize(string [] productList)
    public void Initialize()
    {
        //  this.productList = productList;
        if (iapModule != null && IsReady) // 이미 초기화 된 경우
        {
            return;
        }

        if (isInitializing) return;

        isInitializing = true;
        iapModule = new IAPModule.IAPUnity();
        // iapModule.Initialize(productList, OnInitializeResult);
        iapModule.Initialize(OnInitializeResult);
    }

    public void ReInitialize(Action<bool> OnRetryInitComplete_)
    {
        _OnRetryInitComplete = OnRetryInitComplete_;

        if (isInitializing) return;

        isReady = false;
        isInitializing = true;
        if (iapModule == null)
        {
            iapModule = new IAPModule.IAPUnity();
        }

        iapModule.Initialize(OnInitializeResult);
    }

    public void RetryToInit(Action<bool> OnRetryInitComplete_)
    {
        _OnRetryInitComplete = OnRetryInitComplete_;

        Initialize();
    }

    // 인앱 관리자 초기화 결과 
    private void OnInitializeResult(bool result)
    {
        isInitializing = false;
        Debug.Log("IAPManager init result : " + result);
        isReady = result;

        if (_OnRetryInitComplete != null)
        {
            _OnRetryInitComplete(isReady);
            _OnRetryInitComplete = null;
        }

        CheckRestoreTransactions(); //unconsumed item check
    }

    // 상품의 가격 문자 가져오기 
    public string GetProductMoneyString(string idString)
    {
        if (iapModule == null)
            return "none";

        return iapModule.GetProductPriceString(idString);
    }

    // 상품의 가격 숫자만 가져오기 
    public decimal GetProductPrice(string idString)
    {
        if (iapModule == null)
            return 0m;

        return iapModule.GetProductPrice(idString);
    }

    // 상품의 통화 코드 가져오기 
    public string GetProductCurrencyCode(string idString)
    {
        if (iapModule == null)
            return "";

        return iapModule.GetProductCurrencyCode(idString);
    }

    public string GetProductTitle(string idString)
    {
        if (iapModule == null)
            return "Title";

        return iapModule.GetProductTitle(idString);
    }

    public string GetProductCurrencySymbol(string idString)
    {
        if (iapModule == null)
            return "";
        if (currencySymbol != null)
        {
            return currencySymbol;
        }
        else if (TryGetCurrencySymbol(GetProductCurrencyCode(idString), out currencySymbol))
        {
            return currencySymbol;
        }
        else
        {
            return "";
        }
    }

    private bool TryGetCurrencySymbol(string ISOCurrencySymbol, out string symbol)
    {
        symbol = CultureInfo
            .GetCultures(CultureTypes.AllCultures)
            .Where(c => !c.IsNeutralCulture)
            .Select(culture =>
            {
                try
                {
                    return new RegionInfo(culture.Name);
                }
                catch
                {
                    return null;
                }
            })
            .Where(ri => ri != null && ri.ISOCurrencySymbol == ISOCurrencySymbol)
            .Select(ri => ri.CurrencySymbol)
            .FirstOrDefault();
        return symbol != null;
    }

    /*
    public void PurchaseItem(EnumProductList id, Action<IAPModule.IAPSuccessResult> OnPurchaseSuccess, Action<IAPModule.IAPFailResult> OnPurchaseFail = null)
    {
        PurchaseItem(id.ToString(), OnPurchaseSuccess, OnPurchaseFail);
    }
    */

    // 아이템 구입하기.
//     public void PurchaseItem(string productID, Action OnPurchaseSuccess,
//         Action OnPurchaseFail = null)
//     {
//         UI_LoadingBlocker.Instance.Open();
//         
//         //AppEventManager.AppEvent_PURCHASE_STEP(productID, IAP_PURCHASE_STEP.IAP_REQUEST);   // IAP 결제 요청 
//
//         OnIapComplete = OnPurchaseSuccess;
//         OnIapFailed = OnPurchaseFail;
//
// // #if UNITY_EDITOR
// //         OnIapComplete?.Invoke();
// //         return;
// // #endif
//
//         iapModule.PurchaseItem(productID, OnPurchaseItemSuccess,
//             (result) => { OnPurchaseItemFail(productID, result); });
//     }
//
//     private void OnPurchaseItemSuccess(IAPModule.IAPSuccessResult result)
//     {
//         //Adjust 매출 추적 및 매출 중복 제거
//         AdjustEvent adjustEvent = new AdjustEvent(PCAdJustDefine.RevenueEventToken);
//         adjustEvent.setRevenue(Convert.ToDouble(result.GetLocalizedPrice()), result.GetIsoCurrencyCode());
//         adjustEvent.setTransactionId(result.GetReceipt());
//         Adjust.trackEvent(adjustEvent);
//
//         //AppEventManager.AppEvent_PURCHASE_STEP(result.GetProductID(), IAP_PURCHASE_STEP.IAP_SUCCES);   // IAP 결제 성공
//
//         ServerManager.Instance.VerifyReceiptServer(result, (result) =>
//         {
//             if (result)
//             {
//                 OnIapComplete?.Invoke();
//             }
//             else
//             {
//                 OnIapFailed?.Invoke();
//             }
//         });
//     }  
    
    //아이템 구입하기.
    public void PurchaseItem(string productID, Action<IAPModule.IAPSuccessResult> OnPurchaseSuccess,
        Action<IAPModule.IAPFailResult> OnPurchaseFail = null)
    {
        //AppEventManager.AppEvent_PURCHASE_STEP(productID, IAP_PURCHASE_STEP.IAP_REQUEST);   // IAP 결제 요청 

#if UNITY_EDITOR
        OnPurchaseSuccess?.Invoke(null);
        return;
#endif
        
        OnIapSuccessed = OnPurchaseSuccess;
        OnIapFailed = OnPurchaseFail;
    
        iapModule.PurchaseItem(productID, OnPurchaseItemSuccess,
            (result) => { OnPurchaseItemFail(productID, result); });
    }
    
    private void OnPurchaseItemSuccess(IAPModule.IAPSuccessResult result)
    {
        //AppEventManager.AppEvent_PURCHASE_STEP(result.GetProductID(), IAP_PURCHASE_STEP.IAP_SUCCES);   // IAP 결제 성공
        
        if (OnIapSuccessed != null)
            OnIapSuccessed(result);
    }

    private void OnPurchaseItemFail(string id, IAPModule.IAPFailResult result)
    {
        //AppEventManager.AppEvent_PURCHASE_STEP(id, IAP_PURCHASE_STEP.IAP_FAIL);   // IAP 결제 실패
        
        if (OnIapFailed != null)
            OnIapFailed(result);
        
        Debug.LogErrorDev(result.GetReason());
    }

    public void UpdateOnPurchase(IAPModule.IAPSuccessResult result, int usdPrice, int amount)
    {
        //  DataManager.MyInfo.UpdatePurchaseInfo(usdPrice);
        // AppEventManager.AppEvent_PURCHASES(usdPrice, amount, type, result.GetProductID());
        // AppEventForSingular.SendPurchaseEvent(result);
    }

    #region Restore Purchase

    public void ConsumePurchase(Product p)
    {
        iapModule.ConsumePurchase(p);
    }

    public void ConsumePurchase(string productId)
    {
        iapModule.ConsumePurchase(productId);
    }

    public void CheckRestoreTransactions()
    {
        iapModule.RegCallbackEvents(OnRestoreSuccess, OnRestoreFailed);
        iapModule.RestorePurchase((restoreSuccess) => { Debug.Log($"RestorePurchase result : {restoreSuccess}"); });
    }

    private void OnRestoreSuccess(IAPSuccessResult result)
    {
        string productId = result.GetProductID();

        Debug.LogErrorDev($"unconsumed product : {productId}");

        // ServerManager.Instance.VerifyReceiptServer(result, (success) =>
        // {
        //     if (success)
        //     {
        //         RestorePackagesWithID(productId);    
        //     }
        // });

        //AppEventManager.AppEvent_PURCHASE_STEP(productId, IAP_PURCHASE_STEP.RESTORE_REQUEST); // 복구 시도
//         Net_PurchaseIAP.Send((isSuccess) =>
//         {
// #if !UNITY_EDITOR
//             //AppEventManager.AppEvent_PURCHASE_STEP(productId, IAP_PURCHASE_STEP.RESTORE_SUCCES); // 복구 성공 
// #endif
//         }, result.GetReceipt(), result.GetIsoCurrencyCode(), result.GetLocalizedPrice().ToString());
    }

    private void OnRestoreFailed(IAPFailResult result)
    {
        Debug.LogErrorDev($"restore consume failed : {result}");
    }

    private void RestorePackagesWithID(string productId)
    {
        // var petPackage = TBL_PET.FindEntity(x => x.IAP_ID == productId);
        //
        // if (petPackage != null)
        // {
        //     switch (productId)
        //     {
        //         case "pet_warrior":
        //             PetManager.Instance.ForcePurchase(0);
        //             break;
        //         
        //         case "pet_magician":
        //             PetManager.Instance.ForcePurchase(1);
        //             break;
        //         
        //         case "pet_tanker":
        //             PetManager.Instance.ForcePurchase(2);
        //             break;
        //     }
        //     return;
        // }        
        //
        // var gemPackage = TBL_SHOP.FindEntity(x => x.IAP_ID == productId);
        //
        // if (gemPackage != null)
        // {
        //     RewardManager.GetWithRewardUI(new Reward(RewardType.Gem, 0, gemPackage.Amount));
        //     return;
        // }
        //
        // if (productId == "shadowlevel100")
        // {
        //     int subIndex = 0;
        //
        //     subIndex = ShopManager.Instance.ShopInfo.PurchasedSubIndex.Count > 0
        //         ? ShopManager.Instance.ShopInfo.PurchasedSubIndex[0]
        //         : 0;
        //     
        //     var package = TBL_PACKAGE.FindEntity(x => x.IAP_ID == productId && x.Item_1_Index == subIndex);
        //
        //     // 담부턴 비소모품으로 등록하지말자!
        //     //구매했으면 재설치 구매복구 무시시키기, 중복보상 방지
        //     if (ShopManager.Instance.PackageList[package.Index].Purchased)
        //     {
        //         return;
        //     }
        //     
        //     if (package != null)
        //     {
        //         ShopManager.Instance.ShopInfo.PurchasedSubIndex.Remove(subIndex);
        //         ShopManager.Instance.ForcePurchase(package.Index);
        //         return;
        //     }
        // }
        // else
        // {
        //     var package = TBL_PACKAGE.FindEntity(x => x.IAP_ID == productId);
        //
        //     // 담부턴 비소모품으로 등록하지말자!
        //     //구매했으면 재설치 구매복구 무시시키기, 중복보상 방지
        //     if (ShopManager.Instance.PackageList[package.Index].Purchased)
        //     {
        //         return;
        //     }
        //     
        //     if (package != null)
        //     {
        //         ShopManager.Instance.ForcePurchase(package.Index);
        //         return;
        //     }
        // }
    }

    #endregion
}