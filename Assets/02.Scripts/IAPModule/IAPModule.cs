using System;
using UnityEngine.Purchasing;

namespace IAPModule
{
	public class IAPModule
	{
		protected Action<bool> handlerInitCompleted = null;
		protected Action<bool> handlerPurchaseResult = null;

		protected Action<IAPSuccessResult> callbackIAPSuccess = null;
		protected Action<IAPFailResult> callbackIAPFail = null;

		//public virtual void Initialize(string [] productList, Action<bool> OnInitCompleted) { if (OnInitCompleted != null) OnInitCompleted(true); }
		public virtual void Initialize(Action<bool> OnInitCompleted) { if (OnInitCompleted != null) OnInitCompleted(true); }

		public virtual string GetProductPriceString(string id) { return "none"; }

		public virtual decimal GetProductPrice(string id) { return 0m; }

		public virtual string GetProductCurrencyCode(string id) { return ""; }

		public virtual string GetProductTitle(string id) { return "Title"; }

		public virtual void PurchaseItem(string productID, Action<IAPSuccessResult> OnSuccessed, Action<IAPFailResult> OnFailed) { if (OnSuccessed != null) OnSuccessed(null); }
		
		
		
		public virtual void ConsumePurchase(Product p)
		{
		}
		public virtual void ConsumePurchase(string productId)
		{
		}

		//구매복구
		public virtual void RestorePurchase(Action<bool> callback = null)
		{
		}
		
		// callback 등록 : 초반 구매 복구에 사용
		public virtual void RegCallbackEvents(Action<IAPSuccessResult> onSucceed, Action<IAPFailResult> onFailed)
		{
			callbackIAPSuccess = onSucceed;
			callbackIAPFail = onFailed;
		}
	}

	// 인앱결제 성공 정보 
	public interface IAPSuccessResult
	{
		string GetReceipt();
		string GetIsoCurrencyCode();
		decimal GetLocalizedPrice();
		string GetProductID();
        
		Product GetProduct();

		string GetDescription();

		string GetTransactionId();
	}

	public interface IAPFailResult
	{
		string GetReason();
	}
}