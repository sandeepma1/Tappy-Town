using System;
using System.Collections;
using System.Collections.Generic;
using June;
using June.Core;



public class GameElementManager : BaseConfig<GameElementManager,GameElement> {

	public const string FILE_NAME = "jsons/gameelement";
    public const string ROOT_FIELD_NAME = "gameelement";


		/// <summary>
		/// Gets the name of the resource.
		/// </summary>
		/// <value>The name of the resource.</value>
		public override string ResourceName {
			get {
						return FILE_NAME;
			}
		}
		
		/// <summary>
		/// Gets the root key.
		/// </summary>
		/// <value>The root key.</value>
		public override string RootKey {
			get {
				return ROOT_FIELD_NAME;
			}
		}
		
		/// <summary>
		/// Gets the item converter.
		/// </summary>
		/// <returns>The item converter.</returns>
		/// <typeparam name="U">The 1st type parameter.</typeparam>
		/// <value>The item converter.</value>
		public override System.Converter<System.Collections.Generic.IDictionary<string, object>, GameElement> ItemConverter {
			get {
						return (doc) => new GameElement(doc);
			}
		}


	public BaseCollection<GameElement> _Elements;
	/// <summary>
	/// Gets the elements.
	/// </summary>
	/// <value>The elements.</value>
	public BaseCollection<GameElement> Elements {
		get {
			if(null == _Elements) {
				_Elements = new BaseCollection<GameElement>(
					(SimpleJson.JsonArray)_Record[ROOT_FIELD_NAME], 
					doc => new GameElement(doc));
			}
			return _Elements;
		}
	}

	/// <summary>
	/// Gets the game elements.
	/// </summary>
	/// <value>The game elements.</value>
	public static List<GameElement> GameElements {
		get {
			return Instance.Items;
		}
	}

#if UNITY_EDITOR
	private static  string[] _GameElementStrings;
	/// <summary>
	/// Sets the game element strings.
	/// </summary>
	/// <value>The game element strings.</value>
	public static string[] GameElementStrings {
		get {
			if(null == _GameElementStrings && null != GameElements) {
				_GameElementStrings = new string[GameElements.Count];
				int index=0;
				foreach(var ge in GameElements) {
					_GameElementStrings[index++] = ge.ToString();
				}
			}
			return _GameElementStrings;
		}
	}
#endif


	/// <summary>
	/// Gets the game element by identifier.
	/// </summary>
	/// <returns>The game element by identifier.</returns>
	/// <param name="id">Identifier.</param>
	public static GameElement GetGameElementById(string id) {

		return Util.FirstOrDefault(GameElements, g => g.Id == id);
	}

	public static List<GameElement> GetGameElementsByIds(IEnumerable<string> ids) {
		List<GameElement> elements = new List<GameElement>();
		foreach(var id in ids) {
			elements.Add(GetGameElementById(id));
		}
		return elements;
	}

	/// <summary>
	/// Gets the game elements by identifiers.
	/// </summary>
	/// <returns>The game elements by identifiers.</returns>
	/// <param name="ids">Identifiers.</param>
	public static List<GameElement> GetGameElementsByIds(List<string> ids) {
		return GetGameElementsByIds(ids);
	}

	/// <summary>
	/// Gets the type of the game elements by currency.
	/// </summary>
	/// <returns>The game elements by currency type.</returns>
	/// <param name="currencyType">Currency type.</param>
	public static List<GameElement> GetGameElementsByCurrencyType(string currencyType) {
		return Util.FilterList(GameElements, ge => ge.Currency.CanBePurchasedUsingCurrency(currencyType));
	}

		/// <summary>
		/// Gets the type of the game elements by currency.
		/// </summary>
		/// <returns>The game elements by currency type.</returns>
		/// <param name="currencyType">Currency type.</param>
		public static List<GameElement> GetGameElementWithCollectibleCurrency(string collectibleId) {
				return Util.FilterList(GameElements, ge => ge.Currency.CanBePurchasedUsingCurrency("collectible") && ge.Currency.GetCurrencyByCurrencyType("collectible").CollectibleTypeId == collectibleId);
		}

	/// <summary>
	/// Accepts the gift.
	/// </summary>
	/// <returns><c>true</c>, if gift was accepted, <c>false</c> otherwise.</returns>
	/// <param name="id">Identifier.</param>
	/// <param name="value">Value.</param>
	public static bool AcceptGift(string id, int value) {
		bool status = false;
		var ge = GetGameElementById(id);
		if(null != ge) {
			status = AcceptGift(ge, value);
		}
		return status;
	}

	/// <summary>
	/// Games the element type count.
	/// </summary>
	/// <returns>The element type count.</returns>
	/// <param name="type">Type.</param>
	public static int GameElementTypeCount(string type) {
		return null != GameElements
			? Util.Count<GameElement>(GameElements, ge => ge.Type == type)
			: 0;
	}
	

	/// <summary>
	/// Accepts the gift.
	/// </summary>
	/// <returns><c>true</c>, if gift was accepted, <c>false</c> otherwise.</returns>
	/// <param name="ge">Ge.</param>
	public static bool AcceptGift(GameElement ge, int value) {
		bool status = false;
		// TODO Update this once API is added

//		if(null != ge) {
//			switch(ge.Type) {
//			case GameElementType.Outfit:
//				if(null != ChhotaBheemRushApi.Player) {
//					ChhotaBheemRushApi.Player.AddUnlockedContent(ge.Id);
//					status = true;
//				}
//				break;
//			case GameElementType.Coin:
//				if(null != ChhotaBheemRushApi.Player && null != ChhotaBheemRushApi.Player.Wallet) {
//					if(ge.IsCustomCurrency) {
//						ChhotaBheemRushApi.Player.Wallet.Coins += value;
//					}
//					else {
//						ChhotaBheemRushApi.Player.Wallet.Coins += ge.Value;
//					}
//					status = true;
//				}
//				break;
//			case GameElementType.GameFeature:
//				if(null != ChhotaBheemRushApi.Player) {
//					ChhotaBheemRushApi.Player.AddGameFeatureUnlocked(ge.Id);
//					status = true;
//				}
//				break;
//			case GameElementType.Ladoo:
//				if(null != ChhotaBheemRushApi.Player && null != ChhotaBheemRushApi.Player.Wallet) {
//					if(ge.IsCustomCurrency) {
//						ChhotaBheemRushApi.Player.Wallet.Ladoos += value;
//					}
//					else {
//						ChhotaBheemRushApi.Player.Wallet.Ladoos += ge.Value;
//					}
//					status = true;
//				}
//				break;
//
//			default:
//				break;
//			}
//		}
		return status;
	}

	/// <summary>
	/// Adds the content of the to unlocked.
	/// </summary>
	/// <returns><c>true</c>, if to unlocked content was added, <c>false</c> otherwise.</returns>
	/// <param name="id">Identifier.</param>
	public static bool AddToUnlockedContent(string id) {
		return AddToUnlockedContent(GetGameElementById(id));
	}

	/// <summary>
	/// Adds the content to unlocked.
	/// </summary>
	/// <returns><c>true</c>, if to unlocked content was added, <c>false</c> otherwise.</returns>
	/// <param name="ge">Ge.</param>
	public static bool AddToUnlockedContent(GameElement ge) {
		bool status = false;
//		if(null != ge) {
//			switch(ge.Type) {
//			case GameElementType.GameFeature:
//				if(null != ChhotaBheemRushApi.Player) {
//					ChhotaBheemRushApi.Player.AddGameFeatureUnlocked(ge.Id);
//					status = true;
//				}
//				break;
//
//			default:
//				if(null != ChhotaBheemRushApi.Player) {
//					ChhotaBheemRushApi.Player.AddUnlockedContent(ge.Id);
//					status = true;
//				}
//				break;
//			}
//		}
		return status;
	}

	/// <summary>
	/// Gets the type of the count of.
	/// </summary>
	/// <returns>The count of type.</returns>
	/// <param name="gameElementIds">Game element identifiers.</param>
	/// <param name="type">Type.</param>
	public static int GetCountOfType(List<string> gameElementIds, string type) {
		return Util.Count(GetGameElementsByIds(gameElementIds), ge => ge.Type == type);
	}

	#if UNITY_EDITOR
	public static int IndexOf(string gameElementId) {
		for(int index=0; index<GameElements.Count; index++) {
			if(0 == string.Compare(gameElementId, GameElements[index].Id, true)) {
				return index;
			}
		}
		return 0;
	}
	#endif
}

/// <summary>
/// Game element.
/// </summary>
public partial class GameElement : BaseModel {

	public string Id {
		get {
			return GetString("_id");
		}
	}

	public string Type {
		get {
			return GetString("tp");
		}
	}

	public string Description {
		get {
			return GetString("ds");
		}
	}

	public int Value {
		get {
			return GetInt("va");
		}
	}
		

	public bool IsConsumable{
		get{
			
			return GetBool("ic");
		}
	}


	
	public GameElement Parent {
		get {
			if(GameElementType.Color == this.Type) {
				// GameElement colours are of the following format `IGL-005-001`
				// We need to tazke `005` and combine it with `IGR` to find the corresponding character.
				string parentId = string.Format("IGR-{0}", this.Id.Substring(4, 3));
				return GameElementManager.GetGameElementById(parentId);
			}
			return null;
		}
	}

	private GameElementCurrencyCollection _Currency;
	/// <summary>
	/// Gets the currency.
	/// </summary>
	/// <value>The currency.</value>
	public GameElementCurrencyCollection Currency {
		get {
			if(null == _Currency) {
				_Currency = new GameElementCurrencyCollection((SimpleJson.JsonArray)_Record["co"], doc => new GameElementCurrency(doc));
			}
			return _Currency;
		}
	}

	/// <summary>
	/// Gets a value indicating whether this instance is custom currency.
	/// </summary>
	/// <value><c>true</c> if this instance is custom currency; otherwise, <c>false</c>.</value>
	public bool IsCustomCurrency {
		get {
			foreach(var c in Currency) {
				if(c.Type == GameElementCurrencyType.CUSTOM)
					return true;
			}
			return false;
		}
	}



	/// <summary>
	/// Gets or sets the before purchase CTA count.
	/// </summary>
	/// <value>The before purchase CTA count.</value>
	public int BeforePurchaseCTACount {
		get {
			return LocalStore.Instance.GetIntOrDefault(string.Format(LocalStorageKeys.CTA_BEFORE_PURCHASE_COUNT_FORMAT, this.Id));
		}
		set {
			LocalStore.Instance.SetInt(string.Format(LocalStorageKeys.CTA_BEFORE_PURCHASE_COUNT_FORMAT, this.Id), value);
		}
	}
	
	/// <summary>
	/// Gets or sets a value indicating this instance's can purchase CTA count.
	/// </summary>
	/// <value><c>true</c> if this instance can purchase CTA count; otherwise, <c>false</c>.</value>
	public int CanPurchaseCTACount {
		get {
			return LocalStore.Instance.GetIntOrDefault(string.Format(LocalStorageKeys.CTA_CAN_PURCHASE_COUNT_FORMAT, this.Id));
		}
		set {
			LocalStore.Instance.SetInt(string.Format(LocalStorageKeys.CTA_CAN_PURCHASE_COUNT_FORMAT, this.Id), value);
		}
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="GameElement"/> class.
	/// </summary>
	/// <param name="doc">Document.</param>
	public GameElement(IDictionary<string, object> doc) : base(doc) { }

	public static GameElement GetElementBy(string id, string type, int value, string currency, float currencyValue) {
		var dict = new Dictionary<string, object> () {
			{ "_id", id },
			{ "tp", type },
			{ "ds", string.Empty },
			{ "va", value },
			{ "co", new SimpleJson.JsonArray() {
					new Dictionary<string, object> () {
						{ "c", currency },
						{ "v", currencyValue }
					}
				}
			}
		};

		return new GameElement (dict);
	}

	/// <summary>
	/// Returns a <see cref="System.String"/> that represents the current <see cref="GameElement"/>.
	/// </summary>
	/// <returns>A <see cref="System.String"/> that represents the current <see cref="GameElement"/>.</returns>
	public override string ToString () {
		return string.Format ("[GameElement: Id={0}, Type={1}, Description={2}, Value={3}]", Id, Type, Description, Value);
	}

}

/// <summary>
/// Game element currency collection.
/// </summary>
public class GameElementCurrencyCollection : BaseCollection<GameElementCurrency> {

	/// <summary>
	/// Initializes a new instance of the <see cref="GameElementCurrencyCollection"/> class.
	/// </summary>
	/// <param name="array">Array.</param>
	/// <param name="ctor">Ctor.</param>
	public GameElementCurrencyCollection(SimpleJson.JsonArray array, Func<IDictionary<string, object>, GameElementCurrency> ctor) : base(array, ctor) { }

	/// <summary>
	/// Gets the type of the currency by currency.
	/// </summary>
	/// <returns>The currency by currency type.</returns>
	/// <param name="currencyType">Currency type.</param>
	public GameElementCurrency GetCurrencyByCurrencyType(string currencyType) {
		foreach (GameElementCurrency item in _Items) {
			if(item.Type == currencyType) {
				return item;
			}
		}

		return null;
	}

	/// <summary>
	/// Determines whether this instance can be purchased using currency the specified currencyType.
	/// </summary>
	/// <returns><c>true</c> if this instance can be purchased using currency the specified currencyType; otherwise, <c>false</c>.</returns>
	/// <param name="currencyType">Currency type.</param>
	public bool CanBePurchasedUsingCurrency(string currencyType) {
		return null != GetCurrencyByCurrencyType(currencyType);
	}

	/// <summary>
	/// Gets a value indicating whether this instance is free.
	/// </summary>
	/// <value><c>true</c> if this instance is free; otherwise, <c>false</c>.</value>
	public bool IsFree {
		get {
			if(null != _Items) {
				foreach(var currency in _Items) {
					if(currency.Value > 0f) {
						return false;
					}
				}
			}
			return true;
		}
	}
}

/// <summary>
/// Game element currency.
/// </summary>
public class GameElementCurrency : BaseModel {
	public string Type {
		get {
			return GetString("c");
		}
	}

	public float Value {
		get {
			return GetFloat("v");
		}
	}

	public string CollectibleTypeId {
				get {
						return GetString("i");
				}
		}

	public GameElementCurrency(IDictionary<string, object> doc) : base(doc) { }
}

/// <summary>
/// Game element type.
/// </summary>
public class GameElementType {

	#if UNITY_EDITOR
	public static readonly string[] TYPES = {
		Ladoo,
		Coin,
		Outfit,
		Color,
		Taunt,
		GameFeature,
		CoinMultiplier,
		DailyUseItem,
		Offer
	};
	#endif

	public const string Ladoo = "ladoos";
	public const string Coin = "coins";
	public const string Outfit = "character";
	public const string Color = "color";
	public const string DailyUseItem = "item";

	public const string Collectible = "collectible";

	public const string Hat = "hat";
	public const string Bat = "bat";
	public const string Ball = "ball";
	public const string Note = "note";


	public const string Taunt = "taunt";
	public const string GameFeature = "gamefeature";
	public const string CoinMultiplier = "coinmultiplier";

	public const string Offer = "offer";

}

public class GameElementCurrencyType {
	public const string USD = "usd";
	public const string Ladoo = "ladoos";
	public const string Collectible = "collectible";
	public const string DailyItem = "item";

	public const string Coin = "coins";
	public const string CUSTOM = "custom";
}