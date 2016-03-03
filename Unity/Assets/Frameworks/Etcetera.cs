using System;
using UnityEngine;
using System.Collections;
using Prime31;
using System.Collections.Generic;
using June.Api;
using Logger = June.Util;
using June;
public class Etcetera {
		public static bool m_IsShowingNativePopup = false;
		private static Action<string> _AlertButtonClickedEventCallback = null;

		#if UNITY_ANDROID
		public const int THEME_DEVICE_DEFAULT_DARK = 4;
		public const int THEME_DEVICE_DEFAULT_LIGHT  = 5;
		public const int THEME_HOLO_DARK = 2;
		public const int THEME_HOLO_LIGHT = 3;
		public const int THEME_TRADITIONAL = 1;
		#endif

		#region Constructor / Initialize
		/// <summary>
		/// Initializes the <see cref="Etcetera"/> class.
		/// </summary>
		static Etcetera() {
				#if UNITY_ANDROID
				EtceteraAndroidManager.alertButtonClickedEvent += HandleEtceteraAlertButtonClickedEvent;
				#elif UNITY_IPHONE
				EtceteraManager.alertButtonClickedEvent += HandleEtceteraAlertButtonClickedEvent;
				#endif
		}
		#endregion

	#if UNITY_IOS
	public static IEnumerator takeScreenShot( string filename){
		return	EtceteraBinding.takeScreenShot(filename,imagePath =>
			{
				EtceteraBinding.saveImageToPhotoAlbum(imagePath);
				Debug.Log( "Screenshot taken and saved to: " + imagePath );
			});
	}
	#endif

		public static string AppUrl {
				get{
						#if UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_EDITOR
						string osInfo = SystemInfo.operatingSystem;
						if (osInfo.Contains ("6.0") || osInfo.Contains ("6.1") || osInfo.Contains ("6.2") || osInfo.Contains ("6.3")) {
								return "http://bitly.com/ChhotaBheemRaceIOS";
								//return "itms-apps://ax.itunes.apple.com/WebObjects/MZStore.woa/wa/viewContentsUserReviews?type=Purple+Software&id=1035636003";
						} else {
								//return "http://itunes.apple.com/app/id1035636003?mt=8";
								return "http://bitly.com/ChhotaBheemRaceIOS";
						}
						#elif UNITY_ANDROID
						return "https://play.google.com/store/apps/details?id=com.junesoftware.nazara.cbrace&hl=en";
						#endif
				}
		}

		public static string ShareURL {
				get{
						#if UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_EDITOR
						return "https://itunes.apple.com/app/apple-store/id1035636003?pt=84780&ct=facebook-share&mt=8";
						#elif UNITY_ANDROID
						return "https://play.google.com/store/apps/details?id=com.junesoftware.nazara.cbrace&referrer=utm_source%3Dfacebook%26utm_medium%3Dshare";
						#endif
				}
		}

		public static string AppImageURL {
				get {
						int imageIndex = UnityEngine.Random.Range(1, 3);
						switch(imageIndex) {
						case 1:
								return "https://chhotabheem.s3-ap-southeast-1.amazonaws.com/images/ChhotaBheemRace-FacebookLinkPost-02-1200x628-1.jpg";
						case 2:
								return "https://chhotabheem.s3-ap-southeast-1.amazonaws.com/images/ChhotaBheemRace-FacebookLinkPost-02-1200x628-2.jpg";
						default:
								return "https://chhotabheem.s3-ap-southeast-1.amazonaws.com/images/ChhotaBheemRace-FacebookLinkPost-02-1200x628-1.jpg";
						}
				}
		}

		#region Event Handlers
		private static void HandleEtceteraAlertButtonClickedEvent (string obj) {
				Etcetera.EnableImmersiveMode();
				Util.Log("[Etcetera] Alert Button Clicked - " + obj);
				m_IsShowingNativePopup = false;
				if(null != _AlertButtonClickedEventCallback) {
						_AlertButtonClickedEventCallback(obj);
				}
		}
		#endregion

		/// <summary>
		/// Shows the alert.
		/// </summary>
		/// <param name="title">Title.</param>
		/// <param name="message">Message.</param>
		/// <param name="positiveButton">Positive button.</param>
		public static void ShowAlert(string title, string message, string positiveButton) {
				ShowAlert(title, message, positiveButton, null);
		}

		/// <summary>
		/// Shows the alert.
		/// </summary>
		/// <param name='title'>
		/// Title.
		/// </param>
		/// <param name='message'>
		/// Message.
		/// </param>
		/// <param name='positiveButton'>
		/// Positive button.
		/// </param>
		public static void ShowAlert(string title, string message, string positiveButton, Action<string> callback) {
				_AlertButtonClickedEventCallback = callback;
				#if UNITY_EDITOR
				if(UnityEditor.EditorUtility.DisplayDialog (title, message, positiveButton)) {
						if(null != callback)
								callback(positiveButton);
				}
				#elif UNITY_ANDROID
				EtceteraAndroid.setAlertDialogTheme(THEME_DEVICE_DEFAULT_LIGHT);
				EtceteraAndroid.showAlert(title, message, positiveButton);
				#elif UNITY_IPHONE
				m_IsShowingNativePopup = true;
				EtceteraBinding.showAlertWithTitleMessageAndButton(title, message, positiveButton);
				#endif
				}

				/// <summary>
				/// Shows the alert.
				/// </summary>
				/// <param name='title'>
				/// Title.
				/// </param>
				/// <param name='message'>
				/// Message.
				/// </param>
				/// <param name='positiveButton'>
				/// Positive button.
				/// </param>
				/// <param name='negativeButton'>
				/// Negative button.
				/// </param>
				/// <param name='callback'>
				/// Callback, Argument consists of the button that was clicked on.
				/// </param>
				public static void ShowAlert(string title, string message, string positiveButton, string negativeButton, Action<string> callback) {
				_AlertButtonClickedEventCallback = callback;
				#if UNITY_EDITOR
				if(UnityEditor.EditorUtility.DisplayDialog (title, message, positiveButton, negativeButton)) {
				if(null != callback)
				callback(positiveButton);
				}
				else {
				if(null != callback)
				callback(negativeButton);
				}
				#elif UNITY_ANDROID
				EtceteraAndroid.setAlertDialogTheme(THEME_DEVICE_DEFAULT_LIGHT);
				EtceteraAndroid.showAlert(title, message, positiveButton, negativeButton);
				#elif UNITY_IPHONE
				m_IsShowingNativePopup = true;
				EtceteraBinding.showAlertWithTitleMessageAndButtons(title, message, new string[] { negativeButton, positiveButton });
				#endif
				}

				/// <summary>
				/// Determines if is SMS available.
				/// </summary>
				/// <returns><c>true</c> if is SMS available; otherwise, <c>false</c>.</returns>
				public static bool IsSMSAvailable {
				get {
				#if UNITY_ANDROID
				return EtceteraAndroid.isSMSComposerAvailable ();
				#elif UNITY_IPHONE
				return EtceteraBinding.isSMSAvailable ();
				#else
				return false;
				#endif
				}
				}

				/// <summary>
				/// Shows the SMS composer.
				/// </summary>
				/// <param name="body">Body.</param>
				public static void ShowSMSComposer(string body) {
				#if UNITY_ANDROID
				EtceteraAndroid.showSMSComposer(body);
				#elif UNITY_IPHONE
				EtceteraBinding.showSMSComposer(body);
				#endif
				}

				/// <summary>
				/// Determines if is email available.
				/// </summary>
				/// <returns><c>true</c> if is email available; otherwise, <c>false</c>.</returns>
				public static bool IsEmailAvailable {
				get {
				#if UNITY_ANDROID
				// TODO: check if email is available.
				return true;
				#elif UNITY_IPHONE
				return EtceteraBinding.isEmailAvailable ();
				#else
				return false;
				#endif
				}
				}

				/// <summary>
				/// Shows the email compose window.
				/// </summary>
				/// <param name='toAddress'>
				/// To address.
				/// </param>
				/// <param name='subject'>
				/// Subject.
				/// </param>
				/// <param name='text'>
				/// Text.
				/// </param>
				/// <param name='isHTML'>
				/// Is HTM.
				/// </param>
				public static void ShowEmailCompose(string toAddress, string subject, string text, bool isHTML) {
				#if UNITY_ANDROID
				EtceteraAndroid.showEmailComposer(toAddress, subject, text, isHTML);
				#elif UNITY_IPHONE
				EtceteraBinding.showMailComposer(toAddress, subject, text, isHTML);
				#endif
		}

		public static void playMovie( string pathOrUrl, uint bgColor, bool showControls, bool supportLandscape,bool supportPortrait, bool closeOnTouch ){
				#if UNITY_IPHONE
				EtceteraTwoBinding.playMovie(pathOrUrl,showControls,supportLandscape,supportPortrait);
				#elif UNITY_ANDROID
				EtceteraAndroid.playMovie(  pathOrUrl,  bgColor,showControls,EtceteraAndroid.ScalingMode.Fill,  closeOnTouch );
				#endif 

		}
		public static void ShowRateUs() {
				Application.OpenURL(AppUrl);
		}

		/// <summary>
		/// Shows the progress dialog.
		/// Must call `HideProgressDialog` to hide it
		/// </summary>
		/// <param name='title'>
		/// Title.
		/// </param>
		/// <param name='message'>
		/// Message.
		/// </param>
		public static void ShowProgressDialog(string title, string message) {
				#if UNITY_ANDROID
				EtceteraAndroid.showProgressDialog(title, message);
				#elif UNITY_IPHONE
				m_IsShowingNativePopup = true;
				EtceteraBinding.showBezelActivityViewWithLabel(title);
				#endif
				_ProgressDialogVisible = true;
		}

		/// <summary>
		/// Hides the progress dialog.
		/// </summary>
		public static void HideProgressDialog() {
				_ProgressDialogVisible = false;
				#if UNITY_ANDROID
				EtceteraAndroid.hideProgressDialog();
				EnableImmersiveMode();
				#elif UNITY_IPHONE
				m_IsShowingNativePopup = false;
				EtceteraBinding.hideActivityView();
				#endif
		}

		private static bool _ProgressDialogVisible = false;
		/// <summary>
		/// Shows the progress dialog.
		/// </summary>
		/// <param name='title'>
		/// Title.
		/// </param>
		/// <param name='message'>
		/// Message.
		/// </param>
		/// <param name='timeoutInSeconds'>
		/// Timeout in seconds.
		/// </param>
		/// <param name='timeoutCallback'>
		/// Timeout callback.
		/// </param>
		public static IEnumerator ShowProgressDialog(string title, string message, int timeoutInSeconds, Action timeoutCallback) {
				ShowProgressDialog(title, message);
				yield return new WaitForSeconds(timeoutInSeconds);
				if(true == _ProgressDialogVisible) {
						HideProgressDialog();
						if(null != timeoutCallback) {
								timeoutCallback();
						}
				}
		}
	/*
		/// <summary>
		/// Shows a compose email window with support request and device information.
		/// </summary>
		public static void SendSupportRequest() {
				string emailAddress = "support@nazara.com";
				string message = string.Format(@"




The following is some diagnostic information for us to better assist you.
User Name: {0}
UTC TIME : {1}
Game : Chhota Bheem
Version : {2}
ISFB : {3}
Device Model : {4}
Did : {5}
Graphics Memory Size : {6}
Operating System : {7}
System Memory Size : {8}",
						PlayerProfile.Username,
						System.DateTime.UtcNow.ToString(),
					June.Api.ChhotaBheemApi.APP_VERSION.ToString (),
						false,
						SystemInfo.deviceModel,
						SystemInfo.deviceUniqueIdentifier,
						SystemInfo.graphicsMemorySize,
						SystemInfo.operatingSystem,
						SystemInfo.systemMemorySize);

				var title = "Bheem Race Support";		
				#if UNITY_IPHONE
				title = "[iOS] Bheem Race Support";
				#elif UNITY_ANDROID
				title = "[Android] Bheem Race Support";
				#endif

				Etcetera.ShowEmailCompose(emailAddress, title, message, false);

	
		}
	*/
		public static void ShowWebView(string url){
				#if UNITY_IPHONE
				EtceteraBinding.showWebPage(url, true);
				#elif UNITY_ANDROID
				EtceteraAndroid.showWebView(url);
				#endif
		}

		public static void setBadgeCount(int count){
				#if UNITY_IPHONE
				EtceteraBinding.setBadgeCount(count);
				#endif
		}

		/// <summary>
		/// Gets the info plist value.
		/// </summary>
		/// <returns>The info plist value.</returns>
		/// <param name="key">Key.</param>
		public static string getInfoPlistValue(string key) {
				#if UNITY_IPHONE
				return EtceteraTwoBinding.getInfoPlistValue(key);
				#else
				// TODO for otehr platforms
				return string.Empty;
				#endif
		}

		/// <summary>
		/// Enables the immersive mode.
		/// </summary>
		public static void EnableImmersiveMode() {
				#if UNITY_ANDROID
				if(Application.platform == RuntimePlatform.Android) {
				EtceteraAndroid.enableImmersiveMode(true);
				}
				#endif
		}

		/// <summary>
		/// Gets the android account email.
		/// </summary>
		/// <value>The android account email.</value>
		public static string AccountEmail {
				get {
						return LocalStore.Instance.GetStringOrDefault(LocalStorageKeys.EMAIL) ?? string.Empty;
				}
				private set {
						if(!string.IsNullOrEmpty(value)) {
							LocalStore.Instance.SetString(LocalStorageKeys.EMAIL, value);
						}
				}
		}

		/// <summary>
		/// Checks if account present.
		/// </summary>
		/// <returns><c>true</c>, if if account present was checked, <c>false</c> otherwise.</returns>
		public static bool CheckIfAccountPresent() {
				bool status = false;
				#if UNITY_ANDROID
				try {
				if(Application.platform == RuntimePlatform.Android) {
				using(AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
				if(null != unityPlayer) {
				using(AndroidJavaObject currActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) {
				if(null != currActivity) {
				using(AndroidJavaClass accountMgr = new AndroidJavaClass("android.accounts.AccountManager")) {
				if(null != accountMgr) {
				using(AndroidJavaObject accMgrObj = accountMgr.CallStatic<AndroidJavaObject>("get", currActivity)) {
				if(null != accMgrObj) {
				AndroidJavaObject[] accounts = accMgrObj.Call<AndroidJavaObject[]>("getAccounts");
				if(null != accounts && accounts.Length > 0) {
				foreach(var acc in accounts) {
				if(null != acc) {
				string name = acc.Get <string>("name");
				string type = acc.Get <string>("type");

				if(!string.IsNullOrEmpty(name) 
				&& !string.IsNullOrEmpty(type)
				&& 0 == string.Compare(type, "com.google", true)) {
				AccountEmail = name;
				status = true;
				break;
				}

				Debug.Log("[Etcetra] CheckIfAccountPresent: " + (name ?? "<NULL>") + " | " + (type ?? "<NULL>"));
				}
				}
				//Dispose objects
				for(int i=0; i<accounts.Length; i++) {
				accounts[i].Dispose();
				}
				}
				else {
				Debug.Log("[Etcetra] CheckIfAccountPresent, NO ACCOUNTS FOUND.");
				}
				}
				}
				}
				}
				}
				}
				}
				}
				}
				}
				catch(Exception ex) {
				Debug.Log("[Etcetera] CheckIfAccountPresent ERROR: " + ex.Message);
				}
				#else
				status = true;
				#endif
				return status;
		}

		private static string _BundleVersion = string.Empty;
		/// <summary>
		/// Gets the bundle version.
		/// <description>
		/// ~~~ JAVA equivalent code ~~~
		/// String versionName = getPackageManager().getPackageInfo(getPackageName(), 0).versionName;
		/// </description>
		/// </summary>
		/// <returns>The bundle version.</returns>
		public static string GetBundleVersion() {
				#if UNITY_ANDROID
				if(Application.platform == RuntimePlatform.Android && string.IsNullOrEmpty(_BundleVersion)) {
				try {
				_BundleVersion = ExecuteNative<string>(
				initialParam: new AndroidJavaClass("com.unity3d.player.UnityPlayer"),
				actions: new Func<AndroidJavaObject, AndroidJavaObject>[] {
				player => player.GetStatic<AndroidJavaObject>("currentActivity"),
				activity => activity.Call<AndroidJavaObject>("getPackageManager"),
						pMgr => pMgr.Call<AndroidJavaObject>("getPackageInfo", GameEventManager.BundleIdentifier, 0) 
				},
				resultSelector: pInfo => pInfo.Get<string>("versionName"));
				}
				catch(Exception ex) {
				Debug.Log("[Etcetera] GetBundleVersion ERROR: " + ex.Message);
				}
				}
				#endif
				return _BundleVersion;
		}

		private static int _BundleVersionCode = 0;
		/// <summary>
		/// Gets the bundle version code.
		/// </summary>
		/// <returns>The bundle version code.</returns>
		public static int GetBundleVersionCode() {
				#if UNITY_ANDROID
				if(Application.platform == RuntimePlatform.Android) {
				try {
				_BundleVersionCode = ExecuteNative<int>(
				initialParam: new AndroidJavaClass("com.unity3d.player.UnityPlayer"),
				actions: new Func<AndroidJavaObject, AndroidJavaObject>[] {
				player => player.GetStatic<AndroidJavaObject>("currentActivity"),
				activity => activity.Call<AndroidJavaObject>("getPackageManager"),
						pMgr => pMgr.Call<AndroidJavaObject>("getPackageInfo", GameEventManager.BundleIdentifier, 0) 
				},
				resultSelector: pInfo => pInfo.Get<int>("versionCode"));
				}
				catch(Exception ex) {
				Debug.Log("[Etcetera] GetBundlerVersionCode ERROR: " + ex.Message);
				}
				}
				#endif
				return _BundleVersionCode;
		}

		public static int ANDROID_DEVICE_SDK_CODE = -1;
		/// <summary>
		/// Gets the android device SDK code.
		/// </summary>
		/// <returns>The android device SDK code.</returns>
		public static int GetAndroidDeviceSDKCode() {
				#if UNITY_ANDROID
				if(-1 == ANDROID_DEVICE_SDK_CODE) {
				ANDROID_DEVICE_SDK_CODE = new AndroidJavaClass("android.os.Build.VERSION").Get<int>("SDK_INT");
				}
				return ANDROID_DEVICE_SDK_CODE;
				#else
				return -1;
				#endif
		}

		/// <summary>
		/// Determines if is facebook app installed.
		/// </summary>
		/// <returns><c>true</c> if is facebook app installed; otherwise, <c>false</c>.</returns>
		public static bool IsFacebookAppInstalled() {
				return IsAppInstalled("com.facebook.katana");
		}

		/// <summary>
		/// Determines if is app installed the specified bundleID.
		/// </summary>
		/// <returns><c>true</c> if is app installed the specified bundleID; otherwise, <c>false</c>.</returns>
		/// <param name="bundleID">Bundle I.</param>
		public static bool IsAppInstalled(string bundleID) {
				#if UNITY_ANDROID
				return null != GetAppLaunchIntent(bundleID);
				#else
				return false;
				#endif
		}

		/// <summary>
		/// Determines if is app installed the specified bundleID.
		/// </summary>
		/// <returns><c>true</c> if is app installed the specified bundleID; otherwise, <c>false</c>.</returns>
		/// <param name="bundleID">Bundle I.</param>
		#if UNITY_ANDROID
		public static AndroidJavaObject GetAppLaunchIntent(string bundleID) {
		AndroidJavaObject launchIntent = null;
		try {
		launchIntent = ExecuteNative<AndroidJavaObject>(
		initialParam: new AndroidJavaClass("com.unity3d.player.UnityPlayer"),
		actions: new Func<AndroidJavaObject, AndroidJavaObject>[] {
		player => player.GetStatic<AndroidJavaObject>("currentActivity"),
		activity => activity.Call<AndroidJavaObject>("getPackageManager"),
		pm => pm.Call<AndroidJavaObject>("getLaunchIntentForPackage", bundleID)
		},
		resultSelector: jo => jo);
		}
		catch(Exception ex) {
		Debug.Log(ex);
		launchIntent = null;
		}
		return launchIntent;
		}
		#endif
		/// <summary>
		/// Gets the launch notification data.
		/// </summary>
		/// <returns>The launch notification data.</returns>
		public static string GetLaunchNotificationData() {
				return GetIntentExtraString("Notification_Data");
		}

		/// <summary>
		/// Gets the intent extra string.
		/// </summary>
		/// <returns>The intent extra string.</returns>
		/// <param name="key">Key.</param>
		public static string GetIntentExtraString(string key) {
				string extraValue = string.Empty;
				#if UNITY_ANDROID
				if(!string.IsNullOrEmpty(key) && Application.platform == RuntimePlatform.Android) {
				try {
				extraValue = ExecuteNative<string>(
				initialParam: new AndroidJavaClass("com.unity3d.player.UnityPlayer"),
				actions: new Func<AndroidJavaObject, AndroidJavaObject>[] {
				player => player.GetStatic<AndroidJavaObject>("currentActivity"),
				activity => activity.Call<AndroidJavaObject>("getIntent"),
				intent => intent.Call<AndroidJavaObject>("getExtras")
				},
				resultSelector: extras => extras.Call<string>("getString", key));
				}
				catch(Exception ex) {
				Debug.Log("[Etcetera] GetBundlerVersionCode ERROR: " + ex.Message);
				}
				}
				#endif
				return extraValue;
		}

		/// <summary>
		/// Gets the default language.
		/// </summary>
		/// <returns></returns>
		public static string GetDefaultLanguage() {
				string lang = string.Empty;
				#if UNITY_ANDROID
				if (Application.platform == RuntimePlatform.Android) {
				try {
				lang = ExecuteNative<string>(
				initialParam: null,
				actions: new Func<AndroidJavaObject, AndroidJavaObject>[] { 
				empty => new AndroidJavaClass("java.util.Locale")
				},
				resultSelector: locale => locale.Call<string>("getLanguage"));
				}
				catch (Exception ex) {
				Debug.Log("[Etcetera] GetBundlerVersionCode ERROR: " + ex.Message);
				}
				}
				#endif
				return lang;
		}

		#if UNITY_ANDROID

		private const int PERMISSION_GRANTED = 0;
		private const int PERMISSION_DENIED = -1;

		///<summary>
		/// Permission Class for Android
		/// <see cref="https://developer.android.com/reference/android/Manifest.permission.html"/>
		/// </summary>
		public class Permissions {

		public static readonly string[] ALL_PERMISSIONS = {
		//--Dangerous Permission--> PHONE
		READ_PHONE_STATE,

		//--Dangerous Permission--> CONTACTS
		//GET_ACCOUNTS,

		//--Dangerous Permission--> STORAGE
		WRITE_EXTERNAL_STORAGE,

		//--Dangerous Permission--> LOCATION
		//ACCESS_COARSE_LOCATION,
		//ACCESS_FINE_LOCATION,

		//--NEEDS TO BE MANUALLY GRANTED FROM SETTINGS--
		//WRITE_SETTINGS,

		//--Normal Permissions-- Automatically Granted
		//RECEIVE_BOOT_COMPLETED,
		//ACCESS_WIFI_STATE,
		//ACCESS_NETWORK_STATE,
		//INTERNET,
		//WAKE_LOCK,
		//VIBRATE,
		//ACCESS_LOCATION_EXTRA_COMMANDS

		//--Billing-- Custom permission, Granted Automatically
		//BILLING

		//--Depricated-- Not required for API 23
		//GET_TASKS,
		//USE_CREDENTIALS,
		};

		// --- DEPRICATED in API 21 ---
		public const string GET_TASKS = "android.permission.GET_TASKS";
		// --- DEPRICATED in API 23, This is only needed on API level 22 and below.
		public const string USE_CREDENTIALS = "android.permission.USE_CREDENTIALS";
		// $$$ Separate Permission $$$
		public const string BILLING = "com.android.vending.BILLING";

		//DANGEROUS PERMISSIONS
		//---------------------
		//https://developer.android.com/guide/topics/security/permissions.html#normal-dangerous

		// *** DANGEROUS, Group: PHONE
		public const string READ_PHONE_STATE = "android.permission.READ_PHONE_STATE" ;

		// *** DANGEROUS, Group: STORAGE
		public const string WRITE_EXTERNAL_STORAGE = "android.permission.WRITE_EXTERNAL_STORAGE";

		// *** DANGEROUS, Group: LOCATION
		public const string ACCESS_COARSE_LOCATION = "android.permission.ACCESS_COARSE_LOCATION";
		public const string ACCESS_FINE_LOCATION = "android.permission.ACCESS_FINE_LOCATION";

		// *** DANGEROUS Group: CONTACTS
		public const string GET_ACCOUNTS = "android.permission.GET_ACCOUNTS";

		// *** DANGEROUS, HAS TO BE MANUALLY GRANTED FROM SETTINGS !!!
		public const string WRITE_SETTINGS = "android.permission.WRITE_SETTINGS";

		//NORMAL Permission, automatically granted
		//----------------------------------------
		//https://developer.android.com/guide/topics/security/normal-permissions.html

		public const string RECEIVE_BOOT_COMPLETED = "android.permission.RECEIVE_BOOT_COMPLETED";
		public const string ACCESS_WIFI_STATE = "android.permission.ACCESS_WIFI_STATE";
		public const string ACCESS_NETWORK_STATE = "android.permission.ACCESS_NETWORK_STATE";
		public const string INTERNET = "android.permission.INTERNET";
		public const string WAKE_LOCK = "android.permission.WAKE_LOCK";
		public const string VIBRATE = "android.permission.VIBRATE";
		public const string ACCESS_LOCATION_EXTRA_COMMANDS = "android.permission.ACCESS_LOCATION_EXTRA_COMMANDS";


		public static IDictionary<string, bool> Status = new Dictionary<string, bool>();

		/// <summary>
		/// Gets the ALLOWED PERMISSIONS.
		/// </summary>
		/// <value>The ALLOWED PERMISSIONS.</value>
		public static List<string> ALLOWED_PERMISSIONS {
		get {
		List<string> permissions = new List<string>();
		foreach(var kv in Status) {
		if(true == kv.Value) {
		permissions.Add(kv.Key);
		}
		}
		return permissions;
		}
		}

		/// <summary>
		/// Gets the DENIED PERMISSIONS.
		/// </summary>
		/// <value>The DENIED PERMISSIONS.</value>
		public static List<string> DENIED_PERMISSIONS {
		get {
		List<string> permissions = new List<string>();
		foreach(var kv in Status) {
		if(false == kv.Value) {
		permissions.Add(kv.Key);
		}
		}
		return permissions;
		}
		}


		/// <summary>
		/// Determines if is granted the specified permission.
		/// </summary>
		/// <returns><c>true</c> if is granted the specified permission; otherwise, <c>false</c>.</returns>
		/// <param name="permission">Permission.</param>
		public static bool IsGranted(string permission) {
		return Status.ContainsKey(permission) ? Status[permission] : false;
		}

		/// <summary>
		/// Sets the status.
		/// </summary>
		/// <param name="permission">Permission.</param>
		/// <param name="status">If set to <c>true</c> status.</param>
		public static void SetStatus(string permission, bool status) {
		if(false == Permissions.Status.ContainsKey(permission)) {
		Permissions.Status.Add(permission, status);
		}
		Permissions.Status[permission] = status;
		}
		}

		/// <summary>
		/// Checks if permission granted.
		/// </summary>
		/// <returns><c>true</c>, if if permission granted was checked, <c>false</c> otherwise.</returns>
		/// <param name="permission">Permission.</param>
		public static bool CheckIfPermissionGranted(string permission) {
		int result = PERMISSION_DENIED;
		result = ExecuteNative<int>(
		initialParam: new AndroidJavaClass("com.unity3d.player.UnityPlayer"),
		actions: new Func<AndroidJavaObject, AndroidJavaObject>[] {
		player => player.GetStatic<AndroidJavaObject>("currentActivity")
		},
		resultSelector: activity => 
		new AndroidJavaClass("android.support.v4.content.ContextCompat").CallStatic<int>("checkSelfPermission", activity, permission));

		bool status = result == PERMISSION_GRANTED; 

		// Update local state of permissions
		Permissions.SetStatus(permission, status);

		return status;
		}

		/// <summary>
		/// Requests the permission.
		/// </summary>
		/// <param name="permissions">Permissions.</param>
		public static void RequestPermission(string[] permissions) {
		ExecuteNative<object>(
		initialParam: 
		new AndroidJavaClass("com.unity3d.player.UnityPlayer"),
		actions: new Func<AndroidJavaObject, AndroidJavaObject>[] {
		player => player.GetStatic<AndroidJavaObject>("currentActivity")
		},
		resultSelector: activity => {
		new AndroidJavaClass("android.support.v4.app.ActivityCompat")
		.CallStatic("requestPermissions", activity, permissions, 123);
		return null;
		});
		}
		#endif

		/// <summary>
		/// Executes the native.
		/// </summary>
		/// <returns>The native.</returns>
		/// <param name="initialParam">Initial parameter.</param>
		/// <param name="actions">Actions.</param>
		/// <param name="resultSelector">Result selector.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		#if UNITY_ANDROID
		public static T ExecuteNative<T>(AndroidJavaObject initialParam, Func<AndroidJavaObject, AndroidJavaObject>[] actions, Func<AndroidJavaObject, T> resultSelector) {
		T result = default(T);

		try {
		if(null != actions && actions.Length > 0) {
		AndroidJavaObject[] obj = new AndroidJavaObject[actions.Length + 1];
		obj[0] = initialParam;
		for(int i=0; i<actions.Length; i++) {
		if(i > 0 && null == obj[i]) {
		break;
		}
		obj[i+1] = actions[i](obj[i]);
		}

		if(null != obj[obj.Length - 1]) {
		result = resultSelector(obj[obj.Length - 1]);
		}

		for(int i=0; i<obj.Length; i++) {
		if(null != obj[i]) {
		obj[i].Dispose();
		}
		}
		}
		}
		catch(Exception ex) {
		Debug.Log("[Etcetra] ExecuteNative ERROR: " + ex.Message);
		}

		return result;
		}
		#endif
}
