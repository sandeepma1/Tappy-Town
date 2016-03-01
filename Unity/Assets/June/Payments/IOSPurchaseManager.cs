#if UNITY_IOS && INAPP_ENABLED
using System;
using System.Collections;
using System.Collections.Generic;
using Prime31;

namespace June.Payments {
	/// <summary>
	/// iOS Purchase Manager.
	/// </summary>
	public class IOSPurchaseManager : PurchaseManager
	{
		public List<StoreKitProduct> m_ProductListIos = new List<StoreKitProduct> ();
		
		#region implemented abstract members of PurchaseManager

		/// <summary>
		/// Gets the product fetched count.
		/// </summary>
		/// <value>The product fetched count.</value>
		public override int ProductFetchedCount {
			get {
				return m_ProductListIos.Count;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance can make payments.
		/// </summary>
		/// <value>true</value>
		/// <c>false</c>
		public override bool CanMakePayments {
			get {
				return StoreKitBinding.canMakePayments();
			}
		}

		/// <summary>
		/// Initializes this instance.
		/// </summary>
		protected override void Init () {
			Util.Log("[IOSPurchaseManager] Init()");
			StoreKitManager.purchaseFailedEvent += PurchaseFailed;
			StoreKitManager.purchaseSuccessfulEvent += PurchaseSuccessful;
			StoreKitManager.purchaseCancelledEvent += PurchaseCancelled;
			StoreKitManager.productListReceivedEvent += ProductListReceived;
			StoreKitManager.restoreTransactionsFinishedEvent += RestorePurchasesSuccessful;
			StoreKitManager.restoreTransactionsFailedEvent += RestorePurchasesFailed;
			RequestProductData(this.ProductIdentifiers.ToArray());
		}

		/// <summary>
		/// Requests the product data from the respective stores.
		/// </summary>
		/// <param name="productIdentifiers">Product identifiers.</param>
		public override void RequestProductData (string[] productIdentifiers) {
			Util.Log("[IOSPurchaseManager] RequestProductData");
			if (ProductFetchedCount >= productIdentifiers.Length)
				return;
			
			Util.Log("[IOSPurchaseManager] Fetching Product Data");
			StoreKitBinding.requestProductData(productIdentifiers);
		}

		/// <summary>
		/// Purchases the product.
		/// </summary>
		/// <param name="productIdentifier">Product identifier.</param>
		/// <param name="callback">Callback.</param>
		public override void PurchaseProduct (string productIdentifier, Action<PurchaseStatus, string> callback) {
			Util.Log("[IOSPurchaseManager] PurchaseProduct - " + productIdentifier);

	#if UNITY_EDITOR
			callback(PurchaseStatus.Success, Store.FakeReceipt);
	#else
			_OnPurchaseCallback = callback;
			_OnRestoreItemCallback = null;
			_OnRestoreCompleteCallback = null;
			if (StoreKitBinding.canMakePayments ()) {
				StoreKitBinding.purchaseProduct (productIdentifier, 1);
			}
			else {
				if(null != callback)
					callback(PurchaseStatus.Failure, null);
			}
	#endif
		}
		
		public override void PurchaseProduct (string productIdentifier, string developerPayload, Action<PurchaseStatus, string> callback) {
			Util.Log("[IOSPurchaseManager] PurchaseProduct - " + productIdentifier + ", payLoad: " + developerPayload);
			PurchaseProduct(productIdentifier, callback);
		}
		
		/// <summary>
		/// Consumes the product.
		/// </summary>
		/// <param name="productIdentifier">Product identifier.</param>
		public override void ConsumeProduct (string productIdentifier) {
			// Not required for iOS
		}

		/// <summary>
		/// Restores the purchases.
		/// </summary>
		/// <param name="itemCallback">Item callback.</param>
		/// <param name="restoreCallback">Restore callback.</param>
		public override void RestorePurchases (Action<PurchaseStatus> restoreCompleteCallback, Action<PurchaseStatus, string, string> restoreItemCallback) {
			_OnRestoreItemCallback = restoreItemCallback;
			_OnRestoreCompleteCallback = restoreCompleteCallback;
			_OnPurchaseCallback = null;
			StoreKitBinding.restoreCompletedTransactions();
		}


		#endregion

		#region iOS Events
		/// <summary>
		/// Purchases failed event handler.
		/// </summary>
		/// <param name="obj">Object.</param>
		void PurchaseFailed (string obj) {
			Util.Log("[IOSPurchaseManager] Purchase Failed :: StoreManager" + obj);
			if(null != _OnPurchaseCallback) {
				_OnPurchaseCallback(PurchaseStatus.Failure, null);
			}
		}

		/// <summary>
		/// Purchase success event handler
		/// </summary>
		/// <param name="obj">Object.</param>
		void PurchaseSuccessful (StoreKitTransaction obj) {
//			UnityEngine.Debug.Log(string.Format("[IOSPurchaseManager] Purchase Successful :: ID : {0} receipt: \n{1}",obj.productIdentifier, obj.base64EncodedTransactionReceipt));
//			var storeItem = Store.GetStoreItemByIdentifier(obj.productIdentifier);
//			if(null != storeItem) {
//				UnityEngine.Debug.Log("[iOSPurchaseManager] PurchaseSuccessful Verifying Purchase: " + storeItem.ToString());
//				Api.ChhotaBheemApi.Purchase(
//					obj.productIdentifier, 
//					storeItem.Id, 
//					storeItem.CurrencyCode, 
//					(float)storeItem.CurrentPrice, 
//					obj.base64EncodedTransactionReceipt,
//					(status, error) => UnityEngine.Debug.Log("[iOSPurchaseManager] Purchase Make Status:" + status + " Error:" + error));
//			}
//			else {
//				UnityEngine.Debug.Log("[iOSPurchaseManager] PurchaseSuccessful Cannot Find: " + obj.productIdentifier);
//			}

			EquipPurchase(obj.productIdentifier);

			if(null != _OnPurchaseCallback) {
				_OnPurchaseCallback(PurchaseStatus.Success, obj.base64EncodedTransactionReceipt);
			}

			if(null != _OnRestoreItemCallback) {
				_OnRestoreItemCallback(PurchaseStatus.Success, obj.base64EncodedTransactionReceipt, obj.productIdentifier);
			}
		}

		/// <summary>
		/// Purchase cancelled event handler
		/// </summary>
		/// <param name="obj">Object.</param>
		void PurchaseCancelled (string obj) {
			Util.Log("[IOSPurchaseManager] Purchase Cancelled :: StoreManager" + obj);
			if(null != _OnPurchaseCallback) {
				_OnPurchaseCallback(PurchaseStatus.Cancelled, null);
			}
		}

		/// <summary>
		/// Products list received event handler.
		/// </summary>
		/// <param name="obj">Object.</param>
		void ProductListReceived (List<StoreKitProduct> obj) {
			Util.Log("[IOSPurchaseManager] Product List received");
			m_ProductListIos.Clear();
			foreach (StoreKitProduct product in obj) {
				m_ProductListIos.Add (product);
				Util.Log("[IOSPurchaseManager] Product name : " + product.title + "\nProduct identifier : " + product.productIdentifier);
			}
			Store.UpdateStoreItems(obj);
		}

		/// <summary>
		/// Restore purchases successful event handler.
		/// </summary>
		void RestorePurchasesSuccessful() {
			Util.Log("[IOSPurchaseManager] Restore Purchases Successful");
			if(null != _OnRestoreCompleteCallback) {
				_OnRestoreCompleteCallback(PurchaseStatus.Success);
				_OnRestoreCompleteCallback = null;
			}
		}

		/// <summary>
		/// Restore purchases failed event handler.
		/// </summary>
		/// <param name="error">Error.</param>
		void RestorePurchasesFailed(string error) {
			Util.Log("[IOSPurchaseManager] Restore Purchases Failed - " + error);
			if(null != _OnRestoreCompleteCallback) {
				_OnRestoreCompleteCallback(PurchaseStatus.Failure);
				_OnRestoreCompleteCallback = null;
			}
		}
		#endregion
	}
}
#endif
