using System;
using System.Collections;
using System.Collections.Generic;

using Logging = UnityEngine.Debug;

namespace June.Payments {
	public abstract class PurchaseManager {
		private static PurchaseManager _Instance;
		/// <summary>
		/// Gets the instance.
		/// </summary>
		/// <value>The instance.</value>
		public static PurchaseManager Instance {
			get {
				if(null == _Instance) {
					#if UNITY_EDITOR
					_Instance = new DummyPurchaseManager();
					#elif UNITY_IOS && INAPP_ENABLED
					_Instance = new IOSPurchaseManager();
					#elif UNITY_ANDROID && !UNITY_AMAZON && INAPP_ENABLED
					_Instance = new AndroidPurchaseManager();
					#elif UNITY_ANDROID && UNITY_AMAZON && INAPP_ENABLED
					_Instance = new AmazonPurchaseManager();
					#else
					_Instance = new DummyPurchaseManager();
					#endif
				}
				return _Instance;
			}
		}

		protected List<string> _ProductIdentifiers;
		/// <summary>
		/// Gets the product identifiers.
		/// </summary>
		/// <value>The product identifiers.</value>
		public virtual List<string> ProductIdentifiers {
			get {
				if(null == _ProductIdentifiers) {
					_ProductIdentifiers = new List<string>();
					for (int i=0; i<Store.InAppItems.Count; i++) {
						_ProductIdentifiers.Add(Store.InAppItems [i].ProductId);
					}
				}
				return _ProductIdentifiers;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance can make payments.
		/// </summary>
		/// <value><c>true</c> if this instance can make payments; otherwise, <c>false</c>.</value>
		public abstract bool CanMakePayments { get; }

		protected Action<PurchaseStatus, string> _OnPurchaseCallback;
		protected Action<PurchaseStatus, string, string> _OnRestoreItemCallback;
		protected Action<PurchaseStatus> _OnRestoreCompleteCallback;

		/// <summary>
		/// Gets the product fetched count.
		/// </summary>
		/// <value>The product fetched count.</value>
		public abstract int ProductFetchedCount { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="PurchaseManager"/> class.
		/// </summary>
		public PurchaseManager() {
			Init();
		}

		/// <summary>
		/// Determines whether the product is consumable.
		/// </summary>
		/// <returns><c>true</c> if this product is consumable; otherwise, <c>false</c>.</returns>
		/// <param name="productIdentifier">Product identifier.</param>
		protected virtual bool IsConsumable(string productIdentifier) {
			if (string.IsNullOrEmpty (productIdentifier))
				return false;

			StoreItem item = Store.GetStoreItemByIdentifier(productIdentifier);
			return item.Type == StoreItemType.Consumable || item.Type == StoreItemType.Offer;
		}

		/// <summary>
		/// Initializes this instance.
		/// </summary>
		protected abstract void Init();

		/// <summary>
		/// Requests the product data from the respective stores.
		/// </summary>
		public virtual void RequestProductData () {
			RequestProductData(ProductIdentifiers.ToArray());
		}

		/// <summary>
		/// Requests the product data from the respective stores.
		/// </summary>
		/// <param name="productIdentifiers">Product identifiers.</param>
		public abstract void RequestProductData (string[] productIdentifiers);

		/// <summary>
		/// Purchases the product.
		/// </summary>
		/// <param name="productIdentifier">Product identifier.</param>
		/// <param name="callback">Callback.</param>
		public abstract void PurchaseProduct (string productIdentifier, Action<PurchaseStatus, string> callback);

		/// <summary>
		/// Purchases the product.
		/// </summary>
		/// <param name="productIdentifier">Product identifier.</param>
		/// <param name="developerPayload">Developer payload.</param>
		/// <param name="callback">Callback.</param>
		public abstract void PurchaseProduct (string productIdentifier, string developerPayload, Action<PurchaseStatus, string> callback);


		/// <summary>
		/// Consumes the product.
		/// </summary>
		/// <param name="productIdentifier">Product identifier.</param>
		public abstract void ConsumeProduct (string productIdentifier);

		/// <summary>
		/// Restores the purchases.
		/// </summary>
		/// <param name="itemCallback">Item callback.</param>
		/// <param name="restoreCallback">Restore callback.</param>
		public abstract void RestorePurchases (Action<PurchaseStatus> restoreCompleteCallback, Action<PurchaseStatus, string, string> restoreItemCallback);

		/// <summary>
		/// Equips the purchase.
		/// </summary>
		/// <param name="productidentifier">Productidentifier.</param>
		public virtual void EquipPurchase(string productIdentifier) {
			//TODO: Equip device with items related to the product that has been purchased/restored.
			Logging.Log ("[PurchaseManager] EquipPurchase - " + productIdentifier);

			if(!string.IsNullOrEmpty(productIdentifier)) {
				var storeItem = Store.GetStoreItemByIdentifier(productIdentifier);
				if(null != storeItem && storeItem.Type == StoreItemType.OneTime) {
					Logging.Log ("[PurchaseManager] Adding To Inventory - " + storeItem.Id);
					PlayerProfile.AddFeatureToInventory(storeItem.Id);
				}

				/*if(null != storeItem)
					PlayerProfile.MoneySpent += (float)storeItem.CurrentPrice;*/
				PlayerProfile.InAppPurchaseCount += 1;
			}
		}
	}

}