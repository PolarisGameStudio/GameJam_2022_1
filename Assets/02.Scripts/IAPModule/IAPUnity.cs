

//#define RECEIPT_VALIDATION
//
// using System;
// using UnityEngine;
//
// using UnityEngine.Purchasing;

using System;
using System.Threading;
using UnityEngine;
using UnityEngine.Purchasing;

#if RECEIPT_VALIDATION
using UnityEngine.Purchasing.Security;
#endif

namespace IAPModule
{

    public class IAPUnity : IAPModule, IStoreListener
    {
        private IStoreController m_Controller;
        private IAppleExtensions m_AppleExtensions;

#if RECEIPT_VALIDATION
        private CrossPlatformValidator validator;
#endif

        #region 초기화 
        /// <summary>
        /// 초기화 
        /// </summary>
        // public override void Initialize(string [] productList, Action<bool> OnInitCompleted)
        public override void Initialize(Action<bool> OnInitCompleted)
        {
            handlerInitCompleted = OnInitCompleted;

            var module = StandardPurchasingModule.Instance();
            var builder = ConfigurationBuilder.Instance(module);
// #if UNITY_ANDROID && !UNITY_ANDROID_AMAZON
//             builder.Configure<IGooglePlayConfiguration>().SetPublicKey(
//                 "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA34oj6xsyBd4/OURNXVOu5kiDxf5jHQF+VmgfmEkfgM8Y49nZb1BIk0UM0+smoyXN3WGcwTYPW26LDQVehyvxkPDQHfWlS5wiyLiOE48Ll6iENqZlHV0F/7J91gpdGK2uTFCtuCkoBSv1vdVz8wOLrCxZEkQme2MNfLX/7aC0vgf6iVBYEpEHk3h5Pd/wL9DEhfuOkdgmzvAt7cFCJKO9x4+uJI2d23XLQDua7V4d2qRFLRCA4jRcVy1A2W7rGidJPMim7KJGHtqfmzqNsy71CVRk3ItG7fwBhtV2XVH7A3dXZ7aOPPw6mMdxGfgz0Y6QVuI4GwwQSy8FkllZvO41bQIDAQAB");
// #endif
            // foreach (string item in productList)
            // {
            //     builder.AddProduct(item, ProductType.Consumable, null);
            // }
            //
            TBL_PACKAGE.ForEachEntity(x => builder.AddProduct(x.IAP_ID, ProductType.Consumable, null));
            
            TBL_SHOP.ForEachEntity(x =>
            {
                if (string.IsNullOrEmpty(x.IAP_ID))
                {
                    return;
                }
                builder.AddProduct(x.IAP_ID, ProductType.Consumable, null);
            });
            
            UnityPurchasing.Initialize(this, builder);

#if RECEIPT_VALIDATION
            string appIdentifier;
            appIdentifier = Application.identifier;
            validator = new CrossPlatformValidator(GooglePlayTangle.Data(), AppleTangle.Data(), UnityChannelTangle.Data(), appIdentifier);
#endif
        }

        /// <summary>
        /// Unity IAP 초기화 성공 콜백
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="extensions"></param>
        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            m_Controller = controller;
            m_AppleExtensions = extensions.GetExtension<IAppleExtensions>();

            // On Apple platforms we need to handle deferred purchases caused by Apple's Ask to Buy feature.
            // On non-Apple platforms this will have no effect; OnDeferred will never be called.
            m_AppleExtensions.RegisterPurchaseDeferredListener(OnDeferred);

            foreach (var item in controller.products.all)
            {
                if (item.availableToPurchase)
                {
                    Debug.Log(string.Join(" - ",
                        new[] {
                            item.metadata.localizedTitle,
                            item.metadata.localizedDescription,
                            item.metadata.isoCurrencyCode,
                            item.metadata.localizedPrice.ToString (),
                            item.metadata.localizedPriceString
                        }));
                }
            }

            if (handlerInitCompleted != null)
                handlerInitCompleted(true);
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            Debug.LogError("Billing failed to initialize!");
            switch (error)
            {
                case InitializationFailureReason.AppNotKnown:
                    Debug.LogError("Is your App correctly uploaded on the relevant publisher console?");
                    break;
                case InitializationFailureReason.PurchasingUnavailable:
                    // Ask the user if billing is disabled in device settings.
                    Debug.Log("Billing disabled!");
                    break;
                case InitializationFailureReason.NoProductsAvailable:
                    // Developer configuration error; check product metadata.
                    Debug.Log("No products available for purchase!");
                    break;
            }

            if (handlerInitCompleted != null)
                handlerInitCompleted(false);
        }

        private void OnDeferred(Product item)
        {
            Debug.Log("Purchase deferred: " + item.definition.id);
        }

        #endregion

        /// <summary>
        /// 상품의 로컬라이즈 된 가격을 통화 기호와 함께 가져온다.
        /// </summary>
        public override string GetProductPriceString(string id)
        {
            #if UNITY_EDITOR
            return "EditorPrice";
            #endif
            
            if (m_Controller == null)
                return "none";
            
            return m_Controller.products.WithStoreSpecificID(id).metadata.localizedPriceString;
        }

        /// <summary>
        /// 상품의 로컬라이즈 된 가격의 숫자만 가져온다 
        /// </summary>
        public override decimal GetProductPrice(string id)
        {
            if (m_Controller == null)
                return 0m;

            return m_Controller.products.WithStoreSpecificID(id).metadata.localizedPrice;
        }

        /// <summary>
        /// 상품의 통화 코드를 가져온다. 
        /// </summary>
        public override string GetProductCurrencyCode(string id)
        {
            if (m_Controller == null)
                return "";

            return m_Controller.products.WithStoreSpecificID(id).metadata.isoCurrencyCode;
        }

        /// <summary>
        /// 상품의 타이틀을 얻어온다.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override string GetProductTitle(string id)
        {
            if (m_Controller == null)
                return "";

            return m_Controller.products.WithStoreSpecificID(id).metadata.localizedTitle;
            /* 스토어 정보를 안쓸듯 하다.( 괄호 안에 앱 이름이 같이 옴)
            string str = m_Controller.products.WithStoreSpecificID(id).metadata.localizedTitle;
            if (string.IsNullOrEmpty(str))
                return "";

            int index = str.IndexOf('(') - 1;
            int length = str.Length;

            if (index <= 0)
                return str;

            string ex2 = str.Remove(length - index);
            return ex2;
            */
        }


        public override void PurchaseItem(string productID, Action<IAPSuccessResult> OnSuccessed, Action<IAPFailResult> OnFailed)
        {
            callbackIAPSuccess = OnSuccessed;
            callbackIAPFail = OnFailed;

            m_Controller.InitiatePurchase(m_Controller.products.WithID(productID));
        }

        /// <summary>
        /// Unity IAP Callback
        /// 인앱 상품 구입 성공 콜백 
        /// </summary>
		public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
        {
#if RECEIPT_VALIDATION
            bool validationResult = PurcahseReceiptValidation(e);
            Debug.LogError("구매 검증 결과 : " + validationResult);
#endif
            #if __DEV
            Debug.LogError("구매 ProcessPurchase called");
            #endif
            
            callbackIAPSuccess?.Invoke(new UnityIAPSuccessInfo(e));
            
            return PurchaseProcessingResult.Complete;    
            
            if(consumePurchase)
            {
                Debug.LogError("구매 consume 성공");
                consumePurchase = false;
                return PurchaseProcessingResult.Complete;    
            }
            else
            {
                return PurchaseProcessingResult.Pending;
            }
                
        }

        private bool consumePurchase = false;
        // 
        public override void RestorePurchase(Action<bool> callback = null)
        {
            if (m_AppleExtensions == null)
            {
                return;
            }
        
            m_AppleExtensions.RestoreTransactions(result =>
            {
                if (result)
                {
                    consumePurchase = true;
                    Debug.Log("Succeed restoration.");
                }
                else
                    Debug.Log("failed restoration.");
        
                callback?.Invoke(result);
            });
        }
        
        public override void ConsumePurchase(Product p)
        {
            Debug.Log("Confirming purchase of " + p.definition.id);
            consumePurchase = true;
            m_Controller.ConfirmPendingPurchase(p);
        }
        
        public override void ConsumePurchase(string productId)
        {
            Debug.Log("Confirming purchase of " + productId);
            consumePurchase = true;
            Product p = m_Controller.products.WithID(productId);
            m_Controller.ConfirmPendingPurchase(p);
        }

        /// <summary>
        /// Unity IAP Callback
        /// 인앱 상품 결제 실패 콜백
        /// </summary>
        public void OnPurchaseFailed(Product item, PurchaseFailureReason r)
        {
            if (callbackIAPFail != null)
            {
                callbackIAPFail(new UnityIAPFailInfo(item, r));
            }
        }


#if RECEIPT_VALIDATION
        
        public bool PurcahseReceiptValidation(PurchaseEventArgs e)
        {
            try
            {
                var result = validator.Validate(e.purchasedProduct.receipt);

                Debug.Log("Receipt is valid. Contents: " + result);

                foreach (IPurchaseReceipt productReceipt in result)
                {
#if __DEV
                    Debug.Log(productReceipt.productID);
                    Debug.Log(productReceipt.purchaseDate);
                    Debug.Log(productReceipt.transactionID);

                    GooglePlayReceipt google = productReceipt as GooglePlayReceipt;
                    if (null != google)
                    {
                        Debug.Log(google.purchaseState);
                        Debug.Log(google.purchaseToken);
                    }

                    UnityChannelReceipt unityChannel = productReceipt as UnityChannelReceipt;
                    if (null != unityChannel)
                    {
                        Debug.Log(unityChannel.productID);
                        Debug.Log(unityChannel.purchaseDate);
                        Debug.Log(unityChannel.transactionID);
                    }

                    AppleInAppPurchaseReceipt apple = productReceipt as AppleInAppPurchaseReceipt;
                    if (null != apple)
                    {
                        Debug.Log(apple.originalTransactionIdentifier);
                        Debug.Log(apple.subscriptionExpirationDate);
                        Debug.Log(apple.cancellationDate);
                        Debug.Log(apple.quantity);
                    }
#endif
                    // For improved security, consider comparing the signed
                    // IPurchaseReceipt.productId, IPurchaseReceipt.transactionID, and other data
                    // embedded in the signed receipt objects to the data which the game is using
                    // to make this purchase.
                }
            }
            catch (IAPSecurityException ex)
            {
                Debug.Log("Invalid receipt, not unlocking content. " + ex);
                return false;
            }
            return true;
        }
#endif

    }

    /// <summary>
    /// Unity IAP 용 결제 성공 이벤트 
    /// </summary>
    public class UnityIAPSuccessInfo : IAPSuccessResult
    {
        public PurchaseEventArgs args;

        public UnityIAPSuccessInfo(PurchaseEventArgs e)
        {
            args = e;
        }

        public string GetReceipt()
        {
            return args.purchasedProduct.receipt;
        }

        public string GetIsoCurrencyCode()
        {
            return args.purchasedProduct.metadata.isoCurrencyCode;
        }

        public decimal GetLocalizedPrice()
        {
            return args.purchasedProduct.metadata.localizedPrice;
        }

        public string GetProductID()
        {
            return args.purchasedProduct.definition.id;
        }
        public Product GetProduct()
        {
            return args.purchasedProduct;
        }

        public string GetDescription()
        {
            return args.purchasedProduct.metadata.localizedDescription;
        }
        
        public string GetTransactionId()
        {
            return args.purchasedProduct.transactionID;
        }
    }

    public class UnityIAPFailInfo : IAPFailResult
    {
        public Product product;
        public PurchaseFailureReason reason;

        public UnityIAPFailInfo(Product item, PurchaseFailureReason r)
        {
            product = item;
            reason = r;
        }

        public string GetReason()
        {
            return reason.ToString();
        }
    }
}