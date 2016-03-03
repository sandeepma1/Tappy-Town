using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using Prime31;
using June;

public class Store {
	public const string FakeReceipt = "SKIP_INAPP_VERIFICATION_PUHLEESE!!!";

	public static string CoinDoubler = "GFA-002";
	public static string RemoveAds = "GFA-001";
	private static List<StoreItem> _Items;

#if UNITY_IPHONE && INAPP_ENABLED

	public static List<StoreKitProduct> _Products;

#elif UNITY_ANDROID && INAPP_ENABLED

	public static GooglePurchase RemoveAdRestoreItem;
	public static List<GoogleSkuInfo> _Products;

#endif

	/// <summary>
	/// Gets the store items.
	/// </summary>
	/// <value>
	/// The items.
	/// </value>
	public static List<StoreItem> Items {
		get {
			if (null == _Items) {
				Load ();
			}
			return _Items;
		}
	}

	public static List<StoreItem> StoreItems {
		get {
			var items = Util.FilterList(Items, i => i.IsStoreVisible);
			items.Sort(Util.GetComparisonAscending<StoreItem, int>(s => s.SortOrder));
			return items;
		}
	}

	public static List<StoreItem> SpecialItems {
		get {
			var items = Util.FilterList(Items, i => 0 == string.Compare(i.Id, RemoveAds, true) || 0 == string.Compare(i.Id, CoinDoubler, true));
			items.Sort(Util.GetComparisonAscending<StoreItem, int>(s => s.SortOrder));
			return items;
		}
	}

	public static List<StoreItem> LocalItems {
		get {
			var items = Util.FilterList (Items, i => i.Type == StoreItemType.Local);
			items.Sort (Util.GetComparisonAscending<StoreItem, int> (i => i.Quantity));
			return items;
		}
	}
	
	public static List<StoreItem> InAppItems {
		get {
			var items = Util.FilterList (Items, i => i.Type == StoreItemType.Consumable || i.Type == StoreItemType.OneTime || i.Type == StoreItemType.Offer);
			items.Sort (Util.GetComparisonAscending<StoreItem, int> (i => i.Quantity));
			return items;
		}
	}

	/// <summary>
	/// Gets the consumable in app items.
	/// </summary>
	/// <value>The consumable in app items.</value>
	public static List<StoreItem> ConsumableInAppItems {
		get {
			var items = Util.FilterList (Items, i => i.Type == StoreItemType.Consumable);
			items.Sort (Util.GetComparisonAscending<StoreItem, int> (i => i.Quantity));
			return items;
		}
	}
	

	static Store () {
		Load ();
		//string[] productIdentifiers = Items.Select(i => i.ProductId).ToArray();
		//InAppPurchaseManager.Instance.RequestProductData();

		#if UNITY_IPHONE
				//fetch price of in-app items from iTunes store
				//StoreKitBinding.requestProductData(productIdentifiers);
		#endif
	}

	/// <summary>
	/// Purchase the specified item.
	/// </summary>
	/// <param name='item'>
	/// If set to <c>true</c> item.
	/// </param>
	public static void Purchase (StoreItem item, Action<PurchaseStatus, string> callback) {
		/*#if UNITY_ANDROID && !UNITY_EDITOR  //TODO: Add OneTme
		if (!Etcetera.CheckIfAccountPresent ()) {
			if(null != callback) {
				callback(PurchaseStatus.NoAccount, null);
			}
			return;
		}
		#endif*/

		June.Payments.PurchaseManager.Instance.PurchaseProduct (item.ProductId, item.Id, (purchaseStatus, error) => {
			if(purchaseStatus == PurchaseStatus.Success) {
				if(item.Type == StoreItemType.OneTime) {
					//TODO: Add OneTme
				}
				else if(item.Type == StoreItemType.Consumable) {
					//TODO: Add Consumable
				}
			}

			if(null != callback)
				callback(purchaseStatus, error);
		});
	}
	

	private static void Load () {
		if (null == _Items) {
			_Items = Util.DeSerializeFromResource<List<StoreItem>> ("Store");
		}
	}
	
	/// <summary>
	/// Gets the store item by identifier.
	/// </summary>
	/// <returns>
	/// The store item by identifier.
	/// </returns>
	/// <param name='identifier'>
	/// Identifier.
	/// </param>
	public static StoreItem GetStoreItemByIdentifier (string identifier) {
		return Items.FirstOrDefault (i => i.ProductId == identifier);
	}

	/// <summary>
	/// Gets the store item by identifier.
	/// </summary>
	/// <returns>The store item by identifier.</returns>
	/// <param name="id">Identifier.</param>
	public static StoreItem GetStoreItemById (string id) {
		return Util.FirstOrDefault(Items, i => i.Id == id);
	}

	/// <summary>
	/// Gets the remove ads item.
	/// </summary>
	/// <returns>The remove ads item.</returns>
	public static StoreItem GetRemoveAdsItem () {
		return Items.FirstOrDefault (i => i.Id == RemoveAds);
	}

	/// <summary>
	/// Gets the coin doubler item.
	/// </summary>
	/// <returns>The coin doubler item.</returns>
	public static StoreItem GetCoinDoublerItem() {
		return Items.FirstOrDefault (i => i.Id == CoinDoubler);
	}

		#if UNITY_IPHONE && INAPP_ENABLED
		/// <summary>
		/// Updates the store kit items.
		/// </summary>
		/// <param name='products'>
		/// Products.
		/// </param>
		public static void UpdateStoreItems(List<StoreKitProduct> products) {
			_Products = products;

			foreach(var p in products) {
				var item = GetStoreItemByIdentifier(p.productIdentifier);
				if(null != item) {
					item.StoreKitProduct = p;
		
					//Remove store item from list if item is not supposed to be purchased in that store!
					if(false == item.CanPurchase) {
						Items.Remove(item);
					}
				}
			}
		}
		#elif UNITY_ANDROID && !UNITY_AMAZON && INAPP_ENABLED
		public static void UpdateStoreItems(List<GoogleSkuInfo> products) {
			_Products = products;
			foreach(var p in products) {
				var item = GetStoreItemByIdentifier(p.productId);
				if(null != item) {
					item.GoogleSkuInfo = p;

					//Remove store item from list if item is not supposed to be purchased in that store!
					if(false == item.CanPurchase) {
						Items.Remove(item);
					}
				}
			}
		}
		#elif UNITY_AMAZON && INAPP_ENABLED
		public static void UpdateStoreItems(List<AmazonItem> products) {
			foreach(var p in products) {
				var item = GetStoreItemByIdentifier(p.sku);
				if(null != item) {
					item.AmazonItem = p;
				}
			}
		}
		#endif

	public static int GetPriceForCardPack (string id, string currencyType) {
		if ("coin" == currencyType.ToLower ()) {
			switch (id) {
			case "IGD-001":
				return 150;
			case "IGD-002":
				return 250;
			case "IGD-003":
				return 350;
			}
		}
		else {
			switch (id) {
			case "IGD-001":
				return 1;
			case "IGD-002":
				return 2;
			case "IGD-003":
				return 3;
			}
		}

		return 0;
	}

	/// <summary>
	/// Gets the in app for coins.
	/// </summary>
	/// <returns>The in app for coins.</returns>
	/// <param name="requiredCoins">Required coins.</param>
	public static StoreItem GetInAppForCoins(int requiredCoins) {
		foreach (var item in ConsumableInAppItems) {
			if(item.Quantity >= requiredCoins) {
				return item;
			}
		}

		return null;
	}

}

public enum PurchaseStatus {
	Success,
	Cancelled,
	Failure,
	NoAccount
}