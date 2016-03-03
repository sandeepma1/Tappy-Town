using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Prime31;

using Logger = UnityEngine.Debug;
using June;

/// <summary>
/// Generic Wrapper with convinience methods for Facebook in Unity.
/// https://developers.facebook.com/docs/unity/reference/
/// </summary>
public class FacebookSDK {
	
	#region FACEBOOK SETTINGS
	
	public static readonly string FACEBOOK_APP_ID = GameEventManager.FacebookAppId;
	//cd441edfaf2659575e3fe488e7a9e05b
	/*
	 * The permissions this game should ask from the user
	 * This is a comma separated field, some of the permissions that can be requested are
	 * 
	 * "" 			 	- empty string means basic permission
	 * "email" 		 	- gives us users email adderss
	 * "user_birthday" 	- gives us users bithday
	 * 
	 * More permission here - https://developers.facebook.com/docs/facebook-login/permissions/
	 * 
	 * For example "email,user_bithday" will request for basic profile, email and users bithday.
	 */
	public static readonly string[] FACEBOOK_PERMISSIONS = { "user_friends" };
	
	#endregion
	private static FacebookSDK _Instance = null;
	/// <summary>
	/// Gets the instance.
	/// </summary>
	/// <value>The instance.</value>
	public static FacebookSDK Instance {
		get {
			if(null == _Instance) {
				_Instance = new FacebookSDK();
			}
			return _Instance;
		}
	}
	
	#region PROPERTIES
	
	/// <summary>
	/// Gets the facebook failure count.
	/// </summary>
	/// <value>
	/// The facebook failure count.
	/// </value>
	public static int FacebookFailureCount {
		get {
			return LocalStore.Instance.GetIntOrDefault(LocalStorageKeys.FACEBOOK_FAILED_COUNT);
		}
	}
	
	/// <summary>
	/// Check the user's authorization status. 
	/// false if the user has not logged into Facebook, or hasn't authorized your app; true otherwise. 
	/// Most often, this will be in the logic that determines whether to show a login control.
	/// </summary>
	public bool IsLoggedIn {
		get {
			#if UNITY_EDITOR
				return false;
			#elif UNITY_IPHONE || UNITY_STANDALONE_OSX
				return FacebookBinding.isSessionValid();
			#elif UNITY_ANDROID
				return FacebookAndroid.isSessionValid();
			#endif
		}
	}
	
	/// <summary>
	/// The user's Facebook user ID, when a user is logged in and has authorized your app, or an empty string if not.
	/// </summary>
	public string UserId {
		get {
			#if UNITY_EDITOR
				return "100002583150520"; //Thomas John's Id
			#else
				string userId = (null != UserProfile && UserProfile.ContainsKey("id")) 
					? (string)UserProfile["id"] 
					: LocalStore.Instance.GetStringOrDefault(LocalStorageKeys.FACEBOOK_USER_ID);
				return userId ?? string.Empty;
			#endif
		}
	}
	
	/// <summary>
	/// The access token granted to your app when the user most recently authorized it; otherwise, an empty string. 
	/// This value is used implicitly for any FB-namespace method that requires an access token.
	/// </summary>
	public string AccessToken {
		get {
			#if UNITY_EDITOR
			return "CAACEdEose0cBAAw1bVRT7uP7SoaaU91lclaJemDVyiExtlekFYzZAFwkNV0hJsnJwKtSGIZApLlrwt4Ft6nRjupevshM7WCS2NHhzbYSulMUyZAiEVmNUtWZBEoNoL6vZAVmiNtkSRNBlkf4KodLZCrikik8YBW0Qt3fbOExfOnEf689XQ8LtoqJZAqDgT4oP1al1xcxbMpRwZDZD";
			#else
			string accessToken = Facebook.instance.accessToken ?? LocalStore.Instance.GetStringOrDefault(LocalStorageKeys.FACEBOOK_ACCESS_TOKEN);
			return accessToken ?? string.Empty;
			#endif
		}
	}
	
	private IDictionary<string, object> _UserProfile;
	/// <summary>
	/// Gets the user profile.
	/// </summary>
	/// <value>
	/// The user profile.
	/// </value>
	public IDictionary<string, object> UserProfile {
		get {
			return _UserProfile;
		}
	}

	/// <summary>
	/// Gets the name.
	/// </summary>
	/// <value>The name.</value>
	public string Name {
		get {
			return (null != UserProfile && UserProfile.ContainsKey(Schema.FacebookProfile.Name)) 
				? (string)UserProfile[Schema.FacebookProfile.Name]
				: string.Empty;
		}
	}
	
	/// <summary>
	/// Gets the first name of the logged in user.
	/// </summary>
	/// <value>
	/// The first name.
	/// </value>
	public string FirstName {
		get {
			return (null != UserProfile && UserProfile.ContainsKey(Schema.FacebookProfile.FirstName)) 
				? (string)UserProfile[Schema.FacebookProfile.FirstName]
				: string.Empty;
		}
	}
	
	/// <summary>
	/// Gets the app identifier.
	/// </summary>
	public string AppId {
		get {
			return FACEBOOK_APP_ID;
		}
	}

	public int FBConnectShownLevel {
		get {
			return LocalStore.Instance.GetIntOrDefault(LocalStorageKeys.FB_CONNECT_POPUP_SHOWN_LEVEL);
		}
		set {
			LocalStore.Instance.SetInt(LocalStorageKeys.FB_CONNECT_POPUP_SHOWN_LEVEL, value);
		}
	}

	public int FBConnectShownCount {
		get {
			return LocalStore.Instance.GetIntOrDefault(LocalStorageKeys.FB_CONNECT_POPUP_SHOWN_COUNT);
		}
		set {
			LocalStore.Instance.SetInt(LocalStorageKeys.FB_CONNECT_POPUP_SHOWN_COUNT, value);
		}
	}


	#endregion
	
	/// <summary>
	/// Initializes a new instance of the <see cref="FacebookSDK"/> class.
	/// </summary>
	public FacebookSDK() {
			bool sessionExists = LocalStore.Instance.HasKey(LocalStorageKeys.FACEBOOK_USER_ID);
		FacebookManager.sessionOpenedEvent += AuthSuccess;
		FacebookManager.loginFailedEvent += AuthFailure;
		FacebookManager.shareDialogFailedEvent += DialogFailedEventHandler;
		FacebookManager.shareDialogSucceededEvent += DialogCompletedWithUrlEvent;
		
		#if UNITY_IPHONE || UNITY_STANDALONE_OSX
			FacebookBinding.init();
			FacebookBinding.setSessionLoginBehavior(FacebookSessionLoginBehavior.Native);
			if(sessionExists) {
				CheckAndUpdateAuthToken(FacebookBinding.getAccessToken());
			}
		#elif UNITY_ANDROID
			FacebookAndroid.init(true);
			FacebookAndroid.setSessionLoginBehavior(FacebookSessionLoginBehavior.SSO_WITH_FALLBACK);
			if(sessionExists) {
				CheckAndUpdateAuthToken(FacebookAndroid.getAccessToken());
			}
		#endif
	}
	
	/// <summary>
	/// Raises the init complete event.
	/// </summary>
//	protected void OnInitComplete() {
//		string userId = FB.UserId ?? string.Empty;
//		string token = FB.AccessToken ?? string.Empty;
//		Logger.Log("[FacebookSDK] On Init Complete, UserId:" + userId + " Token:" + token);
//		_IsInitialized = true;
//		CheckAndUpdateAuthToken();
//	}
	
	/// <summary>
	/// Raises the hide unity event.
	/// </summary>
	/// <param name='isGameShown'>
	/// Is game shown.
	/// </param>
	protected void OnHideUnity(bool isGameShown) {
		Logger.Log("[FacebookSDK] On Hide Unity, Is Game Shown : " + isGameShown);
	}
	
	/// <summary>
	/// Prompts the user to authorize your application using the Login Dialog appropriate to the platform. 
	/// If the user is already logged in and has authorized your application, 
	/// checks whether all permissions in the scope parameter have been granted, and if not, 
	/// prompts the user for any that are newly requested. 
	/// Usually, you'll call it once to ask the user for authentication, then again to request additional permissions as required.
	/// </summary>
	public void Login() {
		Login (FACEBOOK_PERMISSIONS, null, null);
	}
	
	/// <summary>
	/// Login the specified authCallback.
	/// </summary>
	/// <param name='authCallback'>
	/// Auth callback.
	/// </param>
	public void Login(Action<bool> authCallback) {
		Login (FACEBOOK_PERMISSIONS, authCallback, null);
	}
	
	/// <summary>
	/// Login the specified authCallback and profileReceived.
	/// </summary>
	/// <param name='authCallback'>
	/// Auth callback.
	/// </param>
	/// <param name='profileReceived'>
	/// Profile received.
	/// </param>
	public void Login(Action<bool> authCallback, Action<IDictionary<string, object>> profileReceived) {
		Login(FACEBOOK_PERMISSIONS, authCallback, profileReceived);
	}

	
	/// <summary>
	/// Prompts the user to authorize your application using the Login Dialog appropriate to the platform. 
	/// If the user is already logged in and has authorized your application, 
	/// checks whether all permissions in the scope parameter have been granted, and if not, 
	/// prompts the user for any that are newly requested. 
	/// Usually, you'll call it once to ask the user for authentication, then again to request additional permissions as required.
	/// https://developers.facebook.com/docs/unity/reference/FB.Login/
	/// </summary>
	/// <param name='scope'>
	/// A list of Facebook permissions requested from the user
	/// </param>
	public void Login(string[] scope, Action<bool> authCallback, Action<IDictionary<string, object>> profileReceived) {
		Etcetera.m_IsShowingNativePopup = true;
		_UserAuthCallback = authCallback;
		_UserProfileReceived = profileReceived;
		#if UNITY_IPHONE || UNITY_STANDALONE_OSX
			Logger.Log("[FacebookSDK] - Calling FacebookBinding Login method");
			FacebookBinding.loginWithReadPermissions(scope);
		#elif UNITY_ANDROID
			FacebookAndroid.loginWithReadPermissions(scope);
		#endif
	}
	
	/// <summary>
	/// Log the user out of both your site and Facebook. 
	/// Will also invalidate any access token that you have for the user that was issued before the logout.
	/// https://developers.facebook.com/docs/unity/reference/FB.Logout/
	/// </summary>
	public void Logout() {
		LocalStore.Instance.DeleteKey(LocalStorageKeys.FACEBOOK_USER_ID);
		LocalStore.Instance.DeleteKey(LocalStorageKeys.FACEBOOK_ACCESS_TOKEN);
		_UserProfile = null;

		#if UNITY_IPHONE || UNITY_STANDALONE_OSX
			FacebookBinding.logout();
		#elif UNITY_ANDROID
			FacebookAndroid.logout();
		#endif

			June.MessageBroker.Publish(June.Messages.AppFacebookLoggedOut);

	}
	
	private Action<bool> _UserAuthCallback;
	private Action<IDictionary<string, object>> _UserProfileReceived;
	
	/// <summary>
	/// Authentication success callback.
	/// </summary>
	protected void AuthSuccess() {
		Etcetera.m_IsShowingNativePopup = false;
        Logger.Log("[FacebookSDK] User Logged In, Token- " + Facebook.instance.accessToken);
		LocalStore.Instance.SetString(LocalStorageKeys.FACEBOOK_ACCESS_TOKEN, Facebook.instance.accessToken);
		Logger.Log("[FacebookSDK] Fetching Profile");
		LoadUserProfile(
			profile => {
				Logger.Log("[FacebookSDK] Profile Fetched");
				if(null != _UserAuthCallback) {
					_UserAuthCallback(true);
				}

				June.MessageBroker.Publish(June.Messages.AppFacebookConnected);
			});
	}
	
	/// <summary>
	/// Authentication failure callback.
	/// </summary>
	protected void AuthFailure(P31Error error) {
		Etcetera.m_IsShowingNativePopup = false;
        Logger.Log("[FacebookSDK] Login failed - " + error.ToString());
		LocalStore.Instance.Increment(LocalStorageKeys.FACEBOOK_FAILED_COUNT);
		if(null != _UserAuthCallback) {
			_UserAuthCallback(false);
		}
	}
	
	/// <summary>
	/// Loads the user profile.
	/// </summary>
	public void LoadUserProfile() {
		LoadUserProfile(null);
	}
	
	/// <summary>
	/// Loads the user profile picture.
	/// </summary>
	/// <returns>
	/// The user profile picture.
	/// </returns>
	public void LoadUserProfile(Action<IDictionary<string, object>> callback) {
		if(FacebookCombo.isSessionValid() && null == _UserProfile) {
			Facebook.instance.graphRequest("me",
				HTTPVerb.GET,
				(error, response) => {
					bool status = false;
					IDictionary<string, object> responseObj = (IDictionary<string, object>)response;
					Logger.Log("[FacebookSDK] LoadUserProfile");
					if(null != responseObj && !responseObj.ContainsKey("error")) {
						status = true;
						_UserProfile = responseObj;
						Logger.Log("[FacebookSDK] LoadUserProfile Success - " + this.UserId);
						LocalStore.Instance.SetString(LocalStorageKeys.FACEBOOK_USER_ID, this.UserId);
					}
				
					if(false == status) {
						_UserProfile = null;
						Logger.Log("[FacebookSDK] LoadUserProfile Error - " + response);
					}
				
					if(null != callback) {
						callback(_UserProfile);
					}
				});
		}
	}

	protected void CheckAndUpdateAuthToken(string authToken) {
		if(!string.IsNullOrEmpty(authToken)
			&& LocalStore.Instance.HasKey(LocalStorageKeys.FACEBOOK_ACCESS_TOKEN) 
			&& LocalStore.Instance.GetString(LocalStorageKeys.FACEBOOK_ACCESS_TOKEN) != authToken) {
			Logger.Log(string.Format("[FacebookSDK] CheckAndUpdateAuthToken\n Existing: {0}\n New: {1}", 
				LocalStore.Instance.GetString(LocalStorageKeys.FACEBOOK_ACCESS_TOKEN), 
				authToken));
			LocalStore.Instance.SetString(LocalStorageKeys.FACEBOOK_ACCESS_TOKEN, authToken);
		}
	}
	
	/// <summary>
	/// Gets the friends.
	/// </summary>
	/// <param name='callback'>
	/// Callback.
	/// </param>
	public void GetFriends(Action<List<FacebookFriend>> callback) {
//		Logger.Log("[FacebookSDK] GetFriends");
//		Facebook.instance.graphRequest("me/friends?fields=id,name,installed", HTTPVerb.GET,
//			(str, result) => {
//				Logger.Log("[FacebookSDK] GetFriends Result Str: " + str + " Result: " + ((Dictionary<string, object>)result).toJson());
//				List<FacebookFriend> friends = null;
//				var resultJson = result as IDictionary<string, object>;
//				if(null != resultJson && resultJson.ContainsKey(Schema.FacebookProfile.FriendsList)) {
//					var friendsJson = (List<object>)resultJson[Schema.FacebookProfile.FriendsList];
//					if(null != friendsJson) {
//						friends = FacebookFriend.GetFriendsFromJson(friendsJson);
//					}
//				}
//				if(null != callback) {
//					callback(friends);
//				}
//			});
	}
	
	/// <summary>
	/// Gets the friends to invite.
	/// </summary>
	/// <returns>
	/// The friends to invite.
	/// </returns>
	public void GetFriendsToInvite(Action<List<FacebookFriend>> callback) {
//		Logger.Log("[FacebookSDK] GetFriendsToInvite");
//		GetFriends(
//			allFriends => {
//				List<FacebookFriend> friendsToInvite = null;
//				if(null != allFriends) {
//					Logger.Log("[FacebookSDK] GetFriendsToInvite, total friends - " + allFriends.Count);
//					friendsToInvite = Util.FilterList(allFriends, f => !f.IsApplicationInstalled);
//				}
//				if(null != callback) {
//					callback(friendsToInvite);
//				}
//			});
	}
	
	private Action<bool> _ShowRequestDialogCallback = null;
	/// <summary>
	/// Shows the request dialog.
	/// </summary>
	/// <param name='title'>
	/// Title.
	/// </param>
	/// <param name='message'>
	/// Message.
	/// </param>
	/// <param name='callback'>
	/// Callback.
	/// </param>
	public void ShowInviteDialog(Action<List<string>> callback) {
//		var parameters = new Dictionary<string, string>() {
//			{ "title", "NinjumpRace" },
//			{ "message", "Download this awesome game!" }
//		};
//		_ShowRequestDialogCallback = callback;
//		Logger.Log("[FacebookSDK] ShowInviteDialog");
//#if UNITY_IPHONE || UNITY_STANDALONE_OSX
//		FacebookBinding.showDialog("apprequests", parameters);
//#elif UNITY_ANDROID
//		FacebookAndroid.showDialog("apprequests", parameters);
//#endif
	}
	
	/// <summary>
	/// Dialogs the failed event handler.
	/// </summary>
	/// <param name='obj'>
	/// Object.
	/// </param>
	protected void DialogFailedEventHandler(P31Error error) {
		Logger.Log("[FacebookSDK] DialogFailedEventHandler - " + error.message);
		if(null != _ShowRequestDialogCallback) {
			_ShowRequestDialogCallback(false);
		}
	}
	
	/// <summary>
	/// Dialogs the completed with URL event.
	/// </summary>
	/// <param name='obj'>
	/// Object.
	/// </param>
	protected void DialogCompletedWithUrlEvent(string response) {
		Logger.Log("[FacebookSDK] DialogCompletedWithUrlEvent - " + response);
		if(null != _ShowRequestDialogCallback) {
#if UNITY_ANDROID
//			if(string.IsNullOrEmpty(response) || false == response.Contains("post_id"))
//				_ShowRequestDialogCallback(false);
//			else
			_ShowRequestDialogCallback(true);
#else
			_ShowRequestDialogCallback(true);
#endif
		}
	}

	/// <summary>
	/// Shows the share dialog.
	/// </summary>
	/// <param name='name'>
	/// Name.
	/// </param>
	/// <param name='link'>
	/// Link.
	/// </param>
	/// <param name='pictureURL'>
	/// Picture UR.
	/// </param>
	/// <param name='caption'>
	/// Caption.
	/// </param>
	/// <param name='description'>
	/// Description.
	/// </param>
	public void ShowShareDialog(string name, string link, string pictureURL, string description, Action<bool> callback) {
		_ShowRequestDialogCallback = callback;
		var properties = new Dictionary<string, object>() {
			{ "contentTitle", name },
			{ "contentURL", link },
			{ "contentDescription", description }
		};
		if(!string.IsNullOrEmpty(pictureURL)) {
			properties.Add("imageURL", pictureURL);
		}
		FacebookCombo.showFacebookShareDialog(properties);
	}

	/// <summary>
	/// Shows the share dialog.
	/// </summary>
	/// <param name="callback">Callback.</param>
	public void ShowShareDialog(Action<bool> callback) {
		ShowShareDialog(
			name: "Chhota Bheem Race",
			link: Etcetera.ShareURL, 
			pictureURL: Etcetera.AppImageURL, 
			description: "Race against all your favourite characters from the world of Bheem.",
			callback: callback);
	}
}

public class FacebookFriend {
	public string UserId;
	public string Name;
	public double BestTime;
	public bool IsApplicationInstalled;
	public int ZoneNumber;
	public int LevelNumber;
	public bool[] ChallengesCompleted;

	public string CharacterName;
	public string RacesPlayed;
	public string RacesWon;
	public string WinPercentage;
	public string TotalHits;
	public string TotalDeaths;
	public string Trophies;
	
	public bool IsCurrentPlayer {
		get {
			return UserId == FacebookSDK.Instance.UserId;
		}
	}
	
	public string FirstName {
		get {
			string[] parts = Name.Split(' ');
			return (null != parts && parts.Length > 0) ? parts[0] : Name;
		}
	}
	
	public string LastName {
		get {
			string[] parts = Name.Split(' ');
			return (null != parts && parts.Length > 0) ? parts[parts.Length-1] : Name;
		}
	}
	
	public string ImageURL {
		get {
			return string.Format("https://graph.facebook.com/{0}/picture", UserId);
		}
	}
	
	public FacebookFriend() {
		this.UserId = string.Empty;
		this.Name = string.Empty;
		this.BestTime = 0;
		this.IsApplicationInstalled = false;
		this.ZoneNumber = 0;
		this.LevelNumber = 0;
		this.ChallengesCompleted = null;
		this.CharacterName = string.Empty;
		this.RacesPlayed = "NA";
		this.RacesWon = "NA";
		this.WinPercentage = "NA";
		this.TotalHits = "NA";
		this.TotalDeaths = "NA";
		this.Trophies = "NA";
	}
	
	public FacebookFriend(IDictionary<string, object> friend) : this() {
		//Initialize object with json
		if(null != friend) {
			
			if(friend.ContainsKey("fbid")) {
				this.UserId = (string)friend["fbid"];
			}
			
			if(friend.ContainsKey("id")) {
				this.UserId = (string)friend["id"];
			}
			
			if(friend.ContainsKey("f")) {
				this.UserId = (string)friend["f"];
			}

			if(friend.ContainsKey("fb")) {
				this.UserId = (string)friend["fb"];
			}
			
			if(friend.ContainsKey("name")) {
				this.Name = (string)friend["name"];
			}

			if(friend.ContainsKey("nm")) {
				this.Name = (string)friend["nm"];
			}
			
			if(friend.ContainsKey("n")) {
				this.Name = (string)friend["n"];
			}
			
			if(friend.ContainsKey("installed")) {
				this.IsApplicationInstalled = (bool)friend["installed"];
			}
			
			if(friend.ContainsKey("bt")) {
				this.BestTime = Convert.ToSingle(friend["bt"]);
			}
			
			if(friend.ContainsKey("lv")) {
				this.LevelNumber = Convert.ToInt32(friend["lv"]);
			}
			
			if(friend.ContainsKey("zn")) {
				this.ZoneNumber = Convert.ToInt32(friend["zn"]);
			}
			
			if(friend.ContainsKey("current_zone")) {
				this.ZoneNumber = Convert.ToInt32(friend["current_zone"]);
			}

			if(friend.ContainsKey("ch")) {
				this.CharacterName = (string)friend["ch"];
			}

			string temp = string.Empty;
			if(friend.ContainsKey("rp")) {
				temp = friend["rp"].ToString();
				this.RacesPlayed = ("-1" == temp ? "NA" : temp);
			}

			if(friend.ContainsKey("rw")) {
				temp = friend["rw"].ToString();
				this.RacesWon = ("-1" == temp ? "NA" : temp);
			}

			if(friend.ContainsKey("wp")) {
				temp = friend["wp"].ToString();
				this.WinPercentage = ("-1" == temp ? "NA" : temp);
			}

			if(friend.ContainsKey("ht")) {
				temp = friend["ht"].ToString();
				this.TotalHits = ("-1" == temp ? "NA" : temp);
			}

			if(friend.ContainsKey("dt")) {
				temp = friend["dt"].ToString();
				this.TotalDeaths = ("-1" == temp ? "NA" : temp);
			}

			if(friend.ContainsKey("tr")) {
				temp = friend["tr"].ToString();
				this.Trophies = ("-1" == friend["tr"].ToString() ? "NA" : friend["tr"].ToString());
			}
			
			if(friend.ContainsKey("cc")) {
				this.ChallengesCompleted = new bool[3] { false, false, false };
				List<object> challenges = (List<object>)friend["cc"];
				if(null != challenges && challenges.Count > 0) {
					for(int i=0; i<3; i++) {
						if(challenges.Count > i) {
							this.ChallengesCompleted[i] = Convert.ToInt32(challenges[i]) == 1;
						}
					}
				}
			}
		}
	}
	
	public static List<FacebookFriend> GetFriendsFromJson(List<object> friendsJson) {
		List<FacebookFriend> friends = new List<FacebookFriend>();
		foreach(object ff in friendsJson) {
			friends.Add(new FacebookFriend((IDictionary<string, object>)ff));
		}
		return friends;
	}
	
	public static List<FacebookFriend> GetFriendsFromJson(string json) {
		if(!string.IsNullOrEmpty(json)) {
			object friendsJson = SimpleJson.SimpleJson.DeserializeObject(json);
			return GetFriendsFromJson((SimpleJson.JsonArray)friendsJson);			
		}
		return null;
	}
	
	private void PopulateZoneAndLevelFromKey(string key) {
		string[] parts = key.Split('_');
		this.ZoneNumber = Convert.ToInt32(parts[0].Replace("Zone", string.Empty));
		this.LevelNumber = Convert.ToInt32(parts[1]);
	}
}

public enum FacebookAuthStatus {
	Success,
	Failed,
	AlreadyExists
}