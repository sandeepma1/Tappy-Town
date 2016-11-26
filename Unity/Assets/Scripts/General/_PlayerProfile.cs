using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using June;

public class PlayerProfile
{

	/// <summary>
	/// Initializes the <see cref="PlayerProfile"/> class.
	/// </summary>
	static PlayerProfile ()
	{
		//SecuredPlayerPrefs.SetSecretKey(SECRET_KEY);
	}

	private static object _IsDirtyLock = new object ();
	private static bool _IsDirty = false;

	/// <summary>
	/// Tells if the player profile data has been modified
	/// </summary>
	public static bool IsDirty {
		get {
			return _IsDirty;
		}
	}

	private static DateTime _LastChangeTimestamp = DateTime.UtcNow;

	/// <summary>
	/// The last change timestamp.
	/// </summary>
	public static DateTime LastChangeTimestamp {
		get {
			return _LastChangeTimestamp;
		}
	}


	public static void MarkAsDirty ()
	{
		lock (_IsDirtyLock) {
			_IsDirty = true;
			_LastChangeTimestamp = DateTime.UtcNow;
		}
	}

	public static void MarkAsClean (DateTime timestamp)
	{
		lock (_IsDirtyLock) {
			if (timestamp == _LastChangeTimestamp) {
				_IsDirty = false;
			}
		}
	}


	/*	public static bool ShowRateUs {
		get { 
			return LocalStore.Instance.GetBoolOrDefault (LocalStorageKeys.SHOW_RATE_US_POPUP);
		}
		set {
			LocalStore.Instance.SetBool (LocalStorageKeys.SHOW_RATE_US_POPUP, value);
		}
	}*/

	/*	public static bool HasRatedTheApp {
		get {
			return LocalStore.Instance.GetBoolOrDefault (LocalStorageKeys.HAS_RATED_THE_APP);
		}
		set {
			LocalStore.Instance.SetBool (LocalStorageKeys.HAS_RATED_THE_APP, value);
		}
	}*/

	/// <summary>
	/// Gets or sets a value indicating is mismatch available.
	/// </summary>
	/// <value><c>true</c> if is mismatch available; otherwise, <c>false</c>.</value>
	/*	public static bool IsFacebookMismatchAvailable {
		get {
			return LocalStore.Instance.GetBoolOrDefault (LocalStorageKeys.IS_FB_MISMATCH);
		}
		set {
			LocalStore.Instance.SetBool (LocalStorageKeys.IS_FB_MISMATCH, value);
		}
	}*/

	/// <summary>
	/// Gets or sets the name of the mismatch.
	/// </summary>
	/// <value>The name of the mismatch.</value>
	/*	public static string MismatchName {
		get {
			return LocalStore.Instance.GetStringOrDefault (LocalStorageKeys.MISMATCH_NAME);
		}
		set {
			LocalStore.Instance.SetString (LocalStorageKeys.MISMATCH_NAME, value);
		}
	}
	*/
	/// <summary>
	/// Gets or sets the last app version.
	/// </summary>
	/// <value>The last app version.</value>
	/*public static float LastAppVersion {
		get {
			return LocalStore.Instance.GetFloatOrDefault (LocalStorageKeys.LAST_APP_VERSION);
		}
		set {
			LocalStore.Instance.SetFloat (LocalStorageKeys.LAST_APP_VERSION, value);
		}
	}

	/// <summary>
	/// Gets or sets the identifier.
	/// </summary>
	/// <value>The identifier.</value>
	public static string PlayerId {
		get {
			return LocalStore.Instance.GetStringOrDefault (LocalStorageKeys.PLAYER_ID);
		}
		set {
			LocalStore.Instance.SetString (LocalStorageKeys.PLAYER_ID, value);
		}
	}


	public static string FullName {
		get {
			var fullName = LocalStore.Instance.GetStringOrDefault (LocalStorageKeys.FULL_NAME);
			return string.IsNullOrEmpty (fullName) ? PlayerName : fullName;
		}
		set {
			LocalStore.Instance.SetString (LocalStorageKeys.FULL_NAME, value);
		}
	}


	/// <summary>
	/// Gets or sets the name.
	/// </summary>
	/// <value>
	/// The name.
	/// </value>
	public static string PlayerName {
		get {
			return LocalStore.Instance.GetStringOrDefault (LocalStorageKeys.PLAYER_NAME);
		}
		set {
			LocalStore.Instance.SetString (LocalStorageKeys.PLAYER_NAME, value);
		}
	}*/

	private static string _Name;

	/// <summary>
	/// Gets the username.
	/// </summary>
	/// <value>The username.</value>
	/*	public static string Username {
		get {
			return PlayerName;
			//			if(string.IsNullOrEmpty(_Name)) {
			//				_Name = GameHubManager.UsernameInKeyChain;
			//			}
			//
			//			if(string.IsNullOrEmpty(_Name)) {
			//				return "Ninja";
			//			}
			//
			//
			//			return !string.IsNullOrEmpty(_Name) ? _Name.ToLower() : _Name;
		}
	}*/


	/// <summary>
	/// Sets the username.
	/// </summary>
	/// <returns><c>true</c>, if username was set, <c>false</c> otherwise.</returns>
	/// <param name="username">Username.</param>
	public static bool SetUserDetails (string username)
	{
		if (string.IsNullOrEmpty (username)) {
			return false;
		}

		//		GameHubManager.SetUsernameInKeyChain(username);
		//
		//#if UNITY_IPHONE
		//		if(GameHubManager.GameCenter.IsLoggedIn) {
		//			GameHubManager.GameCenter.UpdateState(GameHubState.Success);
		//			NinjumpAPI.UpdateGameCenterId(GameHubManager.GameCenter.Id, GameHubManager.GameCenter.Alias, (bool status) => {
		//				if(status) {
		//					GameHubManager.GameCenter.UpdateState(GameHubState.Success);
		//				}
		//			});
		//		}
		//#elif UNITY_ANDROID && !UNITY_AMAZON
		//		if(GameHubManager.GooglePlay.IsLoggedIn) {
		//			GameHubManager.GooglePlay.UpdateState(GameHubState.Success);
		//			NinjumpAPI.UpdateGooglePlayId(GameHubManager.GooglePlay.Id, GameHubManager.GooglePlay.Alias, (bool status) => {
		//				if(status) {
		//					GameHubManager.GooglePlay.UpdateState(GameHubState.Success);
		//				}
		//			});
		//		}
		//		#elif UNITY_ANDROID && UNITY_AMAZON
		//		if(GameHubManager.GameCircle.IsLoggedIn) {
		//			GameHubManager.GameCircle.UpdateState(GameHubState.Success);
		//			NinjumpAPI.UpdateGameCircleUserId(GameHubManager.GameCircle.Id, GameHubManager.GameCircle.Alias, (bool status) => {
		//				if(status) {
		//					GameHubManager.GameCircle.UpdateState(GameHubState.Success);
		//				}
		//			});
		//		}
		//#endif
		//		
		//		if(GameHubManager.Facebook.IsLoggedIn) {
		//			NinjumpAPI.UpdateFacebook((bool status, string error) => {
		//				if(status) {
		//					GameHubManager.Facebook.UpdateState(GameHubState.Success);
		//				}
		//			});
		//		}

//		PushNotificationManager.Register();
		return true;
	}


	private static int _IsAppRated;

	/// <summary>
	/// Gets or sets the rated app.
	/// </summary>
	/// <value>
	/// The rated app.
	/// </value>
	/*	public static bool IsAppRated {
		get {
			return GenericGet (LocalStorageKeys.IS_RATED_APP_KEY, ref _IsAppRated) == 1;
		}
		set {
			GenericSet (LocalStorageKeys.IS_RATED_APP_KEY, value ? 1 : 0, ref _IsAppRated, LocalStore.Instance.SetInt);
		}
	}*/


	#region InApp Currency


	/// <summary>
	/// Gets a value indicating if player is registered.
	/// </summary>
	/// <value><c>true</c> if is registered; otherwise, <c>false</c>.</value>
	public static bool IsRegistered {
		get {
			//return LocalStore.Instance.GetBool (LocalStorageKeys.IS_REGISTERED);
			return true;

			//			return GameHubManager.IsUsernamePresentInKeyChain;
		}
	}

	#endregion

	/// <summary>
	/// Gets or sets the coins.
	/// </summary>
	/// <value>
	/// The coins.
	/// </value>
	/*	public static int Coins {
		get {
			//			return null != NinjumpAPI.Player && null != NinjumpAPI.Player.PlayerStat
			//				? NinjumpAPI.Player.PlayerStat.Coins
			//					: LocalStore.Instance.GetIntOrDefault(LocalStorageKeys.COINS_KEY);

			return  LocalStore.Instance.GetIntOrDefault (LocalStorageKeys.COINS_KEY);
		}
		private set {
			LocalStore.Instance.SetInt (LocalStorageKeys.COINS_KEY, value);
		}
	}*/



	/*
	public static bool AddFeatureToInventory (string id)
	{
		return AddToUnlockedItems (id);
	}

*/

	#region Public Methods

	/// <summary>
	/// Determines if is purchased the specified id.
	/// </summary>
	/// <returns><c>true</c> if is purchased the specified id; otherwise, <c>false</c>.</returns>
	/// <param name="id">Identifier.</param>
	/*	public static bool IsPurchased (string id)
	{
		var array = LocalStore.Instance.GetStringArray (LocalStorageKeys.UNLOCKED_ITEMS);
		if (null != array) {
			var unlockedContent = array.ToList ();

			if (null != unlockedContent && unlockedContent.Contains (id)) {
				return true;
			}
		}

		return false;
	}*/


	/*public static void Initialize ()
	{	
		Util.Log ("*** WARNING *** [PlayerProfile] Initialize Values to Defaults");
		IsAppRated = false;

		_Name = null;

		LocalStore.Instance.SetString (LocalStorageKeys.PLAYER_NAME, "Bheem");
		LocalStore.Instance.SetString (LocalStorageKeys.FULL_NAME, "Bheem");
		LocalStore.Instance.SetBool (LocalStorageKeys.IS_DETAILED_STATS_ENABLED, false);
		LocalStore.Instance.SetInt (LocalStorageKeys.AD_FREQUENCY, 1);
		LocalStore.Instance.SetInt (LocalStorageKeys.INSTALLED_DATE, Util.CurrentUTCTimestamp);
		LocalStore.Instance.SetString (LocalStorageKeys.LADOOS_KEY, "20");
		LocalStore.Instance.SetString (LocalStorageKeys.CURRENT_OUTFIT, "IGC-001");
		LocalStore.Instance.SetInt (LocalStorageKeys.COINS_KEY, 1000);
		LocalStore.Instance.SetBool (LocalStorageKeys.IS_ENGLISH, true);
		LocalStore.Instance.SetInt ("version_key", 1);
		LocalStore.Instance.SetInt (LocalStorageKeys.OFFER_SEQUENCE, 0);

		AddToUnlockedItems ("IGC-001");

		//	Settings.get().isSoundOn = true;
//		Settings.IsSoundFxOn = true;



		Save ();

	}*/

	/*private static bool AddToUnlockedItems (string id)
	{
		var array = LocalStore.Instance.GetStringArray (LocalStorageKeys.UNLOCKED_ITEMS);

		List<string> unlockedContent = null;
		if (null == array) {
			unlockedContent = new List<string> ();
		} else {
			unlockedContent = array.ToList ();
		}

		if (!unlockedContent.Contains (id)) {
			unlockedContent.Add (id);
			LocalStore.Instance.SetStringArray (LocalStorageKeys.UNLOCKED_ITEMS, unlockedContent.ToArray ());
		}

		return true;
	}

	/// <summary>
	/// Saves the data.
	/// </summary>
	public static void Save ()
	{
		Save (null);
	}

	/// <summary>
	/// Saves the data.
	/// </summary>
	public static void Save (Action<bool> callback)
	{
		LocalStore.Instance.Save ();
	}

	/// <summary>
	/// Clears all.
	/// </summary>
	public static void LogoutAndClearAll ()
	{
		bool hasShownNewCharBadge = LocalStore.Instance.GetBoolOrDefault (LocalStorageKeys.DONT_SHOW_STORE_BADGE);
		//		NinjumpAPI.Logout ();
		//		GameHubManager.ClearKeyChain ();

		LocalStore.Instance.DeleteAll ();

		// Set inapp message shown to true again
		LocalStore.Instance.SetBool (LocalStorageKeys.HAS_SHOWN_INAPP_MESSAGE, true);
		LocalStore.Instance.SetBool (LocalStorageKeys.DONT_SHOW_STORE_BADGE, hasShownNewCharBadge);
	}

	/// <summary>
	/// Applications the on suspending.
	/// </summary>
	public static void ApplicationOnSuspending ()
	{
		// Calculate energy
	}

	public static void UpdateCoins (int coinsToModify)
	{
		if (0 > coinsToModify) {
			CoinsSpent += coinsToModify * -1;
//			GameManager.instance.PlayerGameStats.UpdateCoinsSpentInMissionBy( coinsToModify * -1);
		}

		Coins += coinsToModify;
		Dictionary<string, object> parameters = new Dictionary<string, object> () {
			{ "amount", Coins },
			{ "oldamount",Coins - coinsToModify }
		};

		//MessageBroker.Publish (Messages.AppCoinsHudRefresh, parameters);


	}
*/


	/// <summary>
	/// Refresh the Player Profile, all values will be fetched from LocalStorage again.
	/// </summary>
	public static void Refresh ()
	{
		_IsAppRated = 0;
	}


	/// <summary>
	/// Update player details
	/// </summary>
	/// <returns><c>true</c>, if gift was accepted, <c>false</c> otherwise.</returns>
	/// <param name="ge">Ge.</param>
	public static bool UpdatePlayerDetails (GameElement ge, int value)
	{
		bool status = false;
		if (null != ge) {
			switch (ge.Type) {
				case GameElementType.Outfit:
					status = true;

					break;
				case GameElementType.Coin:
					//UpdateCoins (value);

					status = true;

					break;
				case GameElementType.GameFeature:
					status = true;

					break;
				case GameElementType.Ladoo:

					status = true;

					break;


				default:
					break;
			}
		}
		return status;
	}



	#endregion

	#region Helpers

	/// <summary>
	/// Generics the get.
	/// </summary>
	/// <param name="key">The key.</param>
	/// <param name="currentValue">The current value.</param>
	/// <param name="getMethod">The get method.</param>
	/// <returns></returns>
	/*private static int GenericGet (string key, ref int currentValue)
	{
		if (0 >= currentValue) {
			currentValue = LocalStore.Instance.GetInt (key);
		}
		return currentValue;
	}

	/// <summary>
	/// Generics the get.
	/// </summary>
	/// <param name="key">The key.</param>
	/// <param name="currentValue">The current value.</param>
	/// <param name="getMethod">The get method.</param>
	/// <returns></returns>
	private static float GenericGet (string key, ref float currentValue)
	{
		if (0 == currentValue) {
			currentValue = LocalStore.Instance.GetFloat (key);
		}
		return currentValue;
	}
*/
	/// <summary>
	/// Generics the get.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="key">The key.</param>
	/// <param name="currentValue">The current value.</param>
	/// <param name="getMethod">The get method.</param>
	/// <returns></returns>
	private static T GenericGet<T> (string key, ref T currentValue, Func<string, T> getMethod) where T : class
	{
		if (null == currentValue) {
			currentValue = getMethod (key);
		}
		return currentValue;
	}

	/// <summary>
	/// Generics the set.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="key">The key.</param>
	/// <param name="newValue">The new value.</param>
	/// <param name="currentValue">The current value.</param>
	/// <param name="setMethod">The set method.</param>
	private static void GenericSet<T> (string key, T newValue, ref T currentValue, Action<string, T> setMethod)
	{
		currentValue = newValue;
		setMethod (key, newValue);
		MarkAsDirty ();
	}

	#endregion

	#region Player stats and events related


	/// <summary>
	/// Gets the installed date.
	/// </summary>
	/// <value>
	/// The installed date.
	/// </value>
	/*	public static DateTime InstalledDate {
		get {
			return Util.FromUnixTimestamp (
				LocalStore.Instance.GetIntOrDefault (LocalStorageKeys.INSTALLED_DATE));
		}
	}

	/// <summary>
	/// Gets the days since install.
	/// </summary>
	/// <value>
	/// The days since install.
	/// </value>
	public static int DaysSinceInstall {
		get {
			return Convert.ToInt32 ((DateTime.UtcNow - InstalledDate).TotalDays);
		}
	}

	/// <summary>
	/// Gets the coins spent.
	/// </summary>
	/// <value>The coins spent.</value>
	public static int CoinsSpent {
		get {
			return LocalStore.Instance.GetIntOrDefault (LocalStorageKeys.COINS_SPENT_KEY);
		}
		private set {
			LocalStore.Instance.SetInt (LocalStorageKeys.COINS_SPENT_KEY, value);
		}
	}


	/// <summary>
	/// Gets or sets the in app purchase count.
	/// </summary>
	/// <value>The in app purchase count.</value>
	public static int InAppPurchaseCount {
		get {
			return LocalStore.Instance.GetIntOrDefault (LocalStorageKeys.INAPP_PURCHASE_COUNT_KEY);
		}
		set {
			LocalStore.Instance.SetInt (LocalStorageKeys.INAPP_PURCHASE_COUNT_KEY, value);
		}
	}


	public static int VideoCoinCount {
		get {
			return LocalStore.Instance.GetIntOrDefault (LocalStorageKeys.VIDEO_COIN_COUNT);
		}
		set {
			LocalStore.Instance.SetInt (LocalStorageKeys.VIDEO_COIN_COUNT, value);
		}
	}

	public static int VideoAdsShownCount {
		get {
			return LocalStore.Instance.GetIntOrDefault (LocalStorageKeys.VIDEO_SHOWN_COUNT);
		}
		set {
			LocalStore.Instance.SetInt (LocalStorageKeys.VIDEO_SHOWN_COUNT, value);
		}
	}
*/

	#endregion
}

