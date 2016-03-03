using UnityEngine;
using System.Collections.Generic;
using System.Xml.Serialization;
using Prime31;
using June;

/// <summary>
/// Store item.
/// </summary>
public class StoreItem {

	public const string EXPECTED_CURRENCY_CODE = "INR";

	/// <summary>
	/// The identifier.
	/// </summary>
	public string Id;
	/// <summary>
	/// The name.
	/// </summary>
	public string Name;
	/// <summary>
	/// The product identifier.
	/// </summary>
	private string _ProductId;

	public string ProductId {
		get {
			return string.IsNullOrEmpty(_ProductId) ? null : _ProductId;//.Replace("junesoftware", Settings.InAppIdentifier).Replace("junesoftware", Settings.InAppIdentifier);
		}
		set {
			_ProductId = value;
		}
	}

	/// <summary>
	/// Gets a value indicating whether this instance can purchase.
	/// </summary>
	/// <value><c>true</c> if this instance can purchase; otherwise, <c>false</c>.</value>
	public bool CanPurchase {
		get {
			// HACK: If cash purchase and currency code is not INR (indian rupees) ignore purchases with default price lesser than Rs 60
//			#if UNITY_ANDROID && !UNITY_EDITOR
//			return Type != StoreItemType.Local 
//				&& null != this.GoogleSkuInfo
//				&& 0 != string.Compare(CurrencyCode, EXPECTED_CURRENCY_CODE, false) 
//				&& _DefaultPrice < 60d
//					? false
//					: true;
//			#elif UNITY_IPHONE && !UNITY_EDITOR
//			return Type != StoreItemType.Local 
//				&& null != _StoreKitProduct
//				&& 0 != string.Compare(CurrencyCode, EXPECTED_CURRENCY_CODE, false) 
//				&& _DefaultPrice < 60d
//					? false
//					: true;
//			#else
			return true;
//			#endif
		}
	}

	/// <summary>
	/// The _ default price.
	/// </summary>
	[XmlElement("DefaultPrice")]
	public double _DefaultPrice;
	private decimal _AppStorePrice = -1m;

	/// <summary>
	/// Gets the current price.
	/// </summary>
	/// <value>The current price.</value>
	[XmlIgnore]
	public double CurrentPrice {
		get {
			#if (UNITY_IPHONE || UNITY_STANDALONE_OSX) && INAPP_ENABLED
			return (null != StoreKitProduct) ? double.Parse(StoreKitProduct.price) : _DefaultPrice;
			#elif UNITY_ANDROID && !UNITY_AMAZON && INAPP_ENABLED
			//fetch current price from store -or- return default price
			return (null != GoogleSkuInfo) ? (double)((double)GoogleSkuInfo.priceAmountMicros / 1000000d) : _DefaultPrice;
			//return (null != GoogleSkuInfo && !string.IsNullOrEmpty(this.GoogleSkuInfo.price)) ? GetPrice (this.GoogleSkuInfo.price) : _DefaultPrice;
			#elif UNITY_AMAZON && INAPP_ENABLED
			return (null != this.AmazonItem && !string.IsNullOrEmpty(this.AmazonItem.price)) ? GetPrice (this.AmazonItem.price) : _DefaultPrice;
			#else
			return _DefaultPrice;
			#endif
		}
	}

	[XmlIgnore]
	public string CurrentPriceStr { 
		get {
			#if (UNITY_IPHONE || UNITY_STANDALONE_OSX) && INAPP_ENABLED
			return (null != StoreKitProduct) ? string.Format("{0}{1:0.##}", CurrencySymbol, StoreKitProduct.price) : string.Format("{0}{1:0.##}", _DefaultCurrencySymbol, _DefaultPrice);
			#elif UNITY_ANDROID && !UNITY_AMAZON && INAPP_ENABLED
			//fetch current price from store -or- return default price
			return (null != GoogleSkuInfo && !string.IsNullOrEmpty(this.GoogleSkuInfo.price)) ? this.GoogleSkuInfo.price : string.Format("{0}{1:0.##}", _DefaultCurrencySymbol, _DefaultPrice);
			#elif UNITY_AMAZON && INAPP_ENABLED
			return (null != this.AmazonItem && !string.IsNullOrEmpty(this.AmazonItem.price)) ? this.AmazonItem.price : string.Format("{0}{1:0.##}", _DefaultCurrencySymbol, _DefaultPrice); ;
			#else
			return string.Format("{0}{1:0.##}", _DefaultCurrencySymbol, _DefaultPrice);
			#endif
		}
	}
	
	public string CurrencyCode {
		get {
			string currencyCode = string.Empty;
			#if (UNITY_IPHONE || UNITY_STANDALONE_OSX) && INAPP_ENABLED
			currencyCode = (null != this.StoreKitProduct) ? this.StoreKitProduct.currencyCode : "USD";;
			#elif UNITY_ANDROID && !UNITY_AMAZON && INAPP_ENABLED
			currencyCode = (null != this.GoogleSkuInfo && !string.IsNullOrEmpty(this.GoogleSkuInfo.priceCurrencyCode)) ? this.GoogleSkuInfo.priceCurrencyCode : "USD";
			#elif UNITY_AMAZON && INAPP_ENABLED
			currencyCode = "USD";
			#else
			currencyCode = "INR";
			#endif
			return currencyCode;
		}
	}

	private const string _DefaultCurrencySymbol = "Rs";
	
	public string CurrencySymbol {
		get {
			// Check for inidan rupee symbol
			string symbol = string.Empty;
			#if UNITY_EDITOR
			symbol = _DefaultCurrencySymbol;
			#elif (UNITY_IPHONE || UNITY_STANDALONE_OSX) && INAPP_ENABLED
			symbol = (null != this.StoreKitProduct) ? this.StoreKitProduct.currencySymbol : _DefaultCurrencySymbol;
			symbol = (0 == string.Compare(CurrencyCode, "INR", true))?"Rs":symbol;
			#elif UNITY_ANDROID && !UNITY_AMAZON && INAPP_ENABLED
			symbol = (null != this.GoogleSkuInfo) ? GetSymbol(this.GoogleSkuInfo.price) : _DefaultCurrencySymbol;
			symbol = (0 == string.Compare(CurrencyCode, "INR", true))?"Rs":symbol;
			#elif UNITY_AMAZON && INAPP_ENABLED
			symbol = _DefaultCurrencySymbol;
			#endif
			return symbol;
		}
	}

	public string Description;
	public int Quantity;
	public string Image;
	public string Type;
	public bool IsStoreVisible;
	public int SortOrder;
	
	#if (UNITY_IPHONE || UNITY_STANDALONE_OSX) && INAPP_ENABLED
		private StoreKitProduct _StoreKitProduct;
		[XmlIgnore]
		public StoreKitProduct StoreKitProduct {
			get {
				if(null == _StoreKitProduct) {
					if(null != Store._Products) {
						_StoreKitProduct = Util.FirstOrDefault(Store._Products, p => p.productIdentifier == this.ProductId);
					}
				}

				return _StoreKitProduct;
			}
			set {
				_StoreKitProduct = value;
			}
		}
	#endif
	
	#if UNITY_ANDROID && INAPP_ENABLED
		#if UNITY_AMAZON
			// Amazon Kindle Store
			[XmlIgnore]
			public AmazonItem AmazonItem;
			[XmlIgnore]
			public string PublicInAppKeyAndroid;
		#else
			//Google Android
			[XmlIgnore]
			public GoogleSkuInfo GoogleSkuInfo;
		#endif
	#endif

	/// <summary>
	/// Gets the price.
	/// </summary>
	/// <returns>The price.</returns>
	/// <param name="priceStr">Price string.</param>
	private double GetPrice (string priceStr) {
		try {
			string[] parts = priceStr.Split(' ');
			return null != parts && parts.Length > 1 ? System.Convert.ToDouble (parts [parts.Length - 1]) : _DefaultPrice;
		}
		catch {
			return _DefaultPrice;
		}
	}

	/// <summary>
	/// Gets the symbol.
	/// </summary>
	/// <returns>The symbol.</returns>
	/// <param name="str">String.</param>
	private string GetSymbol (string str) {
		try {
			string symbol = string.Empty;
			for (int i=0; i<str.Length; i++) {
				if (char.IsNumber (str [i])) {
					break;
				}
				symbol += str [i];
			}
			return symbol.Trim();
		}
		catch {
			return _DefaultCurrencySymbol;
		}
	}

	public override string ToString () {
		string nativeStr = string.Empty;
#if UNITY_ANDROID && !UNITY_AMAZON && INAPP_ENABLED
		nativeStr = GoogleSkuInfo.ToString();
#elif UNITY_ANDROID && UNITY_AMAZON && INAPP_ENABLED
		nativeStr = AmazonItem.ToString();
#elif UNITY_IPHONE && INAPP_ENABLED
		nativeStr = null != StoreKitProduct ? StoreKitProduct.ToString() : "StoreItem: " + ProductId;
#endif
		return string.Format ("[StoreItem: CurrentPrice={0}, CurrencyCode={1}, CurrencySymbol={2}, CurrentPriceStr={3} NativeObject:{4}]", CurrentPrice, CurrencyCode, CurrencySymbol, CurrentPriceStr, nativeStr);
	}
}

public class StoreItemType {
	public const string Consumable = "Consumable";
	public const string OneTime = "OneTime";
	public const string Local = "Local";
	public const string Offer = "Offer";
}