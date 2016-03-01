#if (UNITY_ANDROID && !UNITY_AMAZON) && INAPP_ENABLED
using System;
using System.Collections;
using System.Collections.Generic;
using Prime31;

using Logging = UnityEngine.Debug;

namespace June.Payments {
	/// <summary>
	/// Android purchase manager.
	/// </summary>
	public class AndroidPurchaseManager : PurchaseManager {

		public static bool IsRestoringPurchases = false;

		public List<GoogleSkuInfo> m_ProductListAndroid = new List<GoogleSkuInfo>();

		// June Public Key
		public string m_sPublicInAppKeyAndroid = GameConfig.InAppKey;

		// Backflip Public Key
		//public string m_sPublicInAppKeyAndroid = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAnIHlK8p0TQflzYz0NCsRkVOA4rLrFuJ2AScPEGT9NG24ucqKfTu6aFmprhrb7s6QGbkkSFagD59qekGSuoEJRG5yM0bOlANx05AE8CurFecRGOJb1lnP6UsqtjXfO2aT77o0RxUmp67E+gO+7uEwaD+/hp4bxcnp6yrhj58Z6T8+tMnFJqlGFMDBoK+QZ2/bCaf3yiL6BBdOKu7ugArkO6/u+MG4bkfsF956MA/pawcsp0ajH5lZmwI4SxnMOnuB/mSck8mX/aSZyqhVJQx/C7jRSF4ZuQokdMnxVGHE5syKXkXdHLmzFQ+bnydT1Fky5h8shK7M/kzb8Eu5B0Y3oQIDAQAB";
		private List<GooglePurchase> m_ProductsToConsume = new List<GooglePurchase>();

		public bool isBillingSupported = false;

		#region implemented abstract members of PurchaseManager
		/// <summary>
		/// Gets a value indicating whether this instance can make payments.
		/// </summary>
		/// <value>true</value>
		/// <c>false</c>
		public override bool CanMakePayments {
			get {
				return true;
			}
		}

		/// <summary>
		/// Gets the product fetched count.
		/// </summary>
		/// <value>The product fetched count.</value>
		public override int ProductFetchedCount {
			get {
				return m_ProductListAndroid.Count;
			}
		}

		/// <summary>
		/// Initializes this instance.
		/// </summary>
		protected override void Init () {
			Logging.Log("[AndroidPurchaseManager] Init()");
			GoogleIABManager.billingSupportedEvent += BillingSupported;
			GoogleIABManager.billingNotSupportedEvent += BillingNotSupported;
			GoogleIABManager.purchaseSucceededEvent += PurchaseSuccessful;
			GoogleIABManager.purchaseFailedEvent += PurchaseFailed;
			GoogleIABManager.queryInventorySucceededEvent += ProductListReceived;
			GoogleIABManager.consumePurchaseSucceededEvent += ConsumeSuccessful;		
			GoogleIABManager.consumePurchaseFailedEvent += ConsumeFailed;
			
			GoogleIAB.enableLogging(true);
			GoogleIAB.setAutoVerifySignatures(true);
			GoogleIAB.init(m_sPublicInAppKeyAndroid);

			//RequestProductData(ProductIdentifiers.ToArray());
		}

		/// <summary>
		/// Requests the product data from the respective stores.
		/// </summary>
		/// <param name="productIdentifiers">Product identifiers.</param>
		public override void RequestProductData (string[] productIdentifiers) {
			Logging.Log("[AndroidPurchaseManager] RequestProductData : " + string.Join(",", productIdentifiers));
			if(isBillingSupported && (null == m_ProductListAndroid || m_ProductListAndroid.Count == 0))
				GoogleIAB.queryInventory (productIdentifiers);
			else if(false == isBillingSupported)
				GoogleIAB.init(m_sPublicInAppKeyAndroid);
		}

		/// <summary>
		/// Purchases the product.
		/// </summary>
		/// <param name="productIdentifier">Product identifier.</param>
		/// <param name="callback">Callback.</param>
		public override void PurchaseProduct (string productIdentifier, Action<PurchaseStatus, string> callback) {
			Logging.Log ("[AndroidPurchaseManager] PurchaseProduct - " + productIdentifier);
			_OnPurchaseCallback = callback;
			_OnRestoreItemCallback = null;
			_OnRestoreCompleteCallback = null;
			IsRestoringPurchases = false;
			//_OnRestoreItemCallback = null;
			GoogleIAB.purchaseProduct (productIdentifier);
		}

		/// <summary>
		/// Purchases the product.
		/// </summary>
		/// <param name="productIdentifier">Product identifier.</param>
		/// <param name="developerPayload">Developer payload.</param>
		/// <param name="callback">Callback.</param>
		public override void PurchaseProduct (string productIdentifier, string developerPayload, Action<PurchaseStatus, string> callback) {
			Logging.Log ("[AndroidPurchaseManager] PurchaseProduct - " + productIdentifier + " payload:" + developerPayload);
			_OnPurchaseCallback = callback;
			_OnRestoreItemCallback = null;
			_OnRestoreCompleteCallback = null;
			IsRestoringPurchases = false;
			//_OnRestoreItemCallback = null;
			GoogleIAB.purchaseProduct (productIdentifier, developerPayload);
		}

		/// <summary>
		/// Consumes the product.
		/// </summary>
		/// <param name="productIdentifier">Product identifier.</param>
		public override void ConsumeProduct (string productIdentifier) {
			Logging.Log ("[AndroidPurchaseManager] ConsumeProduct - " + productIdentifier);
			GoogleIAB.consumeProduct(productIdentifier);
		}

		/// <summary>
		/// Restores the purchases.
		/// </summary>
		/// <param name="itemCallback">Item callback.</param>
		/// <param name="restoreCallback">Restore callback.</param>
		public override void RestorePurchases (Action<PurchaseStatus> restoreCompleteCallback, Action<PurchaseStatus, string, string> restoreItemCallback) {
			//NOT REQUIRED
		}

		/// <summary>
		/// Equips the purchase.
		/// </summary>
		/// <param name="productidentifier">Productidentifier.</param>
		/// <param name="product">Product.</param>
		public void EquipPurchase(GooglePurchase product) {
			if(null == product) {
				Logging.Log ("[AndroidPurchaseManager] EquipPurchase - NULL");
				return;
			}
			
			Logging.Log ("[AndroidPurchaseManager] EquipPurchase - " + product.ToString());
			StoreItem item = Store.GetStoreItemByIdentifier(product.productId);
			Logging.Log ("[AndroidPurchaseManager] EquipPurchase StoreItem - " + (null != item ? item.ToString() : "<null>"));
			if(null != item && null != product && !string.IsNullOrEmpty(product.developerPayload)) {
				Logging.Log ("[AndroidPurchaseManager] EquipPurchase MakePurchase");
			}	
		}

		#endregion

		#region Google IAB Event Handlers

		/// <summary>
		/// Billings the supported.
		/// </summary>
		void BillingSupported () {
			isBillingSupported = true;
			Logging.Log("[AndroidPurchaseManager] BillingSupported");
			RequestProductData(ProductIdentifiers.ToArray());
		}

		/// <summary>
		/// Billings the not supported.
		/// </summary>
		/// <param name="obj">Object.</param>
		void BillingNotSupported (string obj) {
			isBillingSupported = false;
			Logging.Log("[AndroidPurchaseManager] BillingNotSupported");
		}

		/// <summary>
		/// Purchases successful event handler.
		/// </summary>
		/// <param name="product">Product.</param>
		void PurchaseSuccessful (GooglePurchase product) {
			Logging.Log("\n[AndroidPurchaseManager] Purchase Successful : " + product.productId);
	        Logging.Log("\n[AndroidPurchaseManager] Purchase JSON : " + product.originalJson);

			var storeItem = Store.GetStoreItemByIdentifier(product.productId);
			if(null != storeItem) {
				Logging.Log("[AndroidPurchaseManager] PurchaseSuccessful Verifying Purchase: " + storeItem.ToString());
				Api.ChhotaBheemApi.Purchase(
					product.productId, 
					storeItem.Id, 
					storeItem.CurrencyCode, 
					(float)storeItem.CurrentPrice, 
					product.originalJson,
					(status, error) => { 
						Logging.Log("[AndroidPurchaseManager] Purchase Make Status:" + status + " Error:" + error);
						PurchaseStatus pStatus = status ? PurchaseStatus.Success : PurchaseStatus.Failure;

						if(IsConsumable(product.productId)) {
							ConsumeProduct(product.productId);
						}
						else {
							EquipPurchase(product.productId);
							if(null != _OnPurchaseCallback) {
								_OnPurchaseCallback(pStatus, error);
							}
						}
					});
			}
			else {
				Logging.Log("[AndroidPurchaseManager] PurchaseSuccessful Cannot Find: " + product.productId);
				if(null != _OnPurchaseCallback) {
					_OnPurchaseCallback(PurchaseStatus.Failure, "Cannot find item in store.");
				}
			}
		}

		/// <summary>
		/// Purchases failed event handler.
		/// </summary>
		/// <param name="obj">Object.</param>
		void PurchaseFailed (string obj, int code) {
			Logging.Log("\n[AndroidPurchaseManager] Purchase Failed : " + obj + " Code: " + code);
			if(null != _OnPurchaseCallback) {
				_OnPurchaseCallback(PurchaseStatus.Failure, string.Empty);
			}
		}

		/// <summary>
		/// Consumes successful event handler.
		/// </summary>
		/// <param name="product">Product.</param>
		void ConsumeSuccessful (GooglePurchase product) {
			Logging.Log ("\n[AndroidPurchaseManager] Consume Successful : " + product.productId);
	        Logging.Log("\n[AndroidPurchaseManager] Consume JSON : " + product.originalJson);
			if(IsRestoringPurchases) {
				EquipPurchase(product);
			}
			if (null != m_ProductsToConsume && m_ProductsToConsume.Count > 0) {
				string productId = m_ProductsToConsume [0].productId;
				m_ProductsToConsume.RemoveAt (0);
				ConsumeProduct(productId);
			}
			else if(null != _OnPurchaseCallback && false == IsRestoringPurchases) {
				_OnPurchaseCallback(PurchaseStatus.Success, product.originalJson);
			}
			IsRestoringPurchases = false;
		}

		/// <summary>
		/// Consumes failed event handler.
		/// </summary>
		/// <param name="obj">Object.</param>
		void ConsumeFailed(string obj) {
			Logging.Log("\n[AndroidPurchaseManager] Consume Failed : " + obj);
			if(null != _OnPurchaseCallback) {
				_OnPurchaseCallback(PurchaseStatus.Failure, string.Empty);
			}
		}

		/// <summary>
		/// Products list received event handler.
		/// </summary>
		/// <param name="purchaseHistory">Purchase history.</param>
		/// <param name="productList">Product list.</param>
		void ProductListReceived (List<GooglePurchase> purchaseHistory, List<GoogleSkuInfo> productList) {
			Logging.Log("\n[AndroidPurchaseManager] ProductListReceived, purchaseHistory=" + purchaseHistory.Count + " productList=" + productList.Count);
			m_ProductListAndroid = productList;

			foreach(var product in productList) {
				Logging.Log ("[AndroidPurchaseManager] Product Name: " + product.title + ", Product Identifier: " + product.productId);
			}

			Store.UpdateStoreItems(productList);

			m_ProductsToConsume.Clear();
			foreach (GooglePurchase purchasedProduct in purchaseHistory) {
				Logging.Log("[AndroidPurchaseManager] Restoring Purchase - " + purchasedProduct.productId);
				if (purchasedProduct.purchaseState == GooglePurchase.GooglePurchaseState.Purchased) {
					if (null != purchasedProduct && !string.IsNullOrEmpty(purchasedProduct.productId) && IsConsumable(purchasedProduct.productId)) {
						m_ProductsToConsume.Add (purchasedProduct);
					}
					else {
						EquipPurchase(purchasedProduct.productId);

						// Remove Ads
						var removeAdsItem = Store.GetRemoveAdsItem ();
						
						if (0 == string.Compare(purchasedProduct.productId, removeAdsItem.ProductId, true)) {
							Logging.Log ("[AndroidPurchaseManager] Setting RemoveAdItem: " + purchasedProduct.ToString());
							Store.RemoveAdRestoreItem = purchasedProduct;
						}
					}
				}
			}
			
			if (m_ProductsToConsume.Count > 0) {
				string productId = m_ProductsToConsume[0].productId;
				m_ProductsToConsume.RemoveAt(0);
				IsRestoringPurchases = true;
				ConsumeProduct (productId);
			}
		}
		
		#endregion
	}
}
	#endif
