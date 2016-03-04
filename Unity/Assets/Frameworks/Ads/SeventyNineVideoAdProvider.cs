//using UnityEngine;
//using System.Collections;
//using June.VideoAds;
//using System;
//
//using Logger = June.Util;
//
//public class SeventyNineVideoAdProvider : IVideoAdProvider {
//	
//	#if UNITY_ANDROID
//		public const string publisherId = GameConfig.SeventyNinepublisherId;
//		public const string inlineVideoAdzoneId = GameConfig.SeventyNineinlineVideoAdzoneId;
//	#endif
//
//	const string CLASS_NAME = "com.junesoftware.seventynineunitylibrary.SeventyNineHelper";
//	public void init() {
//		JuneAndroidNativeCallbackManager.Initialize();
//		#if UNITY_ANDROID
//			try {
//				using(AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
//					if(null != unityPlayer) {
//						using(AndroidJavaObject currActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) {
//							if(null != currActivity) {
//								using (AndroidJavaClass cls = new AndroidJavaClass (CLASS_NAME)) {
//									object[] arr = new object[]{
//										currActivity,
//										publisherId
//									};
//									
//									cls.CallStatic ("init", arr);
//
//								}
//							}
//						}
//					}
//				}
//			}
//			catch(System.Exception ex) {
//				Logger.Log("[SeventyNineVideoAdProvider] Init Called: " + ex.ToString());
//			}
//		#elif UNITY_IOS
//
//		#endif
//	}
//	
//	#region implemented abstract members of IVideoAdProvider
//	
//	/// <summary>
//	/// Initialize the specified config.
//	/// </summary>
//	/// <param name="config">Config.</param>
//	public override void Initialize (System.Collections.Generic.IDictionary<string, object> config) {
//		#if UNITY_ANDROID
//		if(false == IsSeventyNineSupported()) {
//			return;
//		}
//		#endif
//		if(!this._IsInitialized) {
//			AddEvents();
//			Logger.Log("[SeventyNine] Instance.Start");
//			init();
//			RequestOffers();
//			this._IsInitialized = true;
//		}
//		else {
//			Logger.Log("[SeventyNine] INSTANCE IS NULL");
//		}
//	}
//	
//	/// <summary>
//	/// Refresh this instance.
//	/// </summary>
//	public override void Refresh () {
//		RequestOffers();
//	}
//	
//	/// <summary>
//	/// Shows the ad.
//	/// </summary>
//	/// <param name="config">Config.</param>
//	/// <param name="callback">Callback.</param>
//	public override void ShowAd (System.Collections.Generic.IDictionary<string, object> config, Action<bool> callback) {
//		if(IsSupported == false) {
//			if(null != callback)
//				callback(false);
//			return;
//		}
//		if(false == _IsInitialized) {
//			Initialize();
//		}
//		else if(false == IsReady) {
//			Logger.Log("[SeventyNine] IS READY FALSE");
//			RequestOffers();
//		}
//		else {
//			_OnAdEndCallback = callback;
//			Logger.Log("[SeventyNine] SHOW Ad");
//			FireOnAdStart();
//			#if UNITY_ANDROID
//			if (Application.platform == RuntimePlatform.Android) {
//				using (AndroidJavaClass cls = new AndroidJavaClass (CLASS_NAME)) {
//					 cls.CallStatic("showAd");
//				}
//			}
//			#elif UNITY_IOS
//
//			#endif
//		}
//	}
//	
//	/// <summary>
//	/// Gets the name of the provider.
//	/// </summary>
//	/// <value>The name of the provider.</value>
//	public override string ProviderName {
//		get {
//			return Providers.SeventyNine;
//		}
//	}
//	
//	/// <summary>
//	/// Gets a value indicating whether this instance is supported.
//	/// </summary>
//	/// <value>true</value>
//	/// <c>false</c>
//	public override bool IsSupported {
//		get {
//			#if UNITY_EDITOR || UNITY_ANDROID
//			return IsSeventyNineSupported();
//			#else
//			return false;
//			#endif
//		}
//	}
//	
//	private bool _IsInitialized;
//	/// <summary>
//	/// Gets a value indicating whether this instance is initialized.
//	/// </summary>
//	/// <value>true</value>
//	/// <c>false</c>
//	public override bool IsInitialized {
//		get { return _IsInitialized; }
//	}
//	
//	private bool _IsReady = false;
//	/// <summary>
//	/// Gets a value indicating whether this instance is ready.
//	/// </summary>
//	/// <value>true</value>
//	/// <c>false</c>
//	public override bool IsReady {
//		get { 
//			#if UNITY_ANDROID
//
//			if(false == IsSeventyNineSupported())
//				return false;
//
//			if (Application.platform == RuntimePlatform.Android) {
//				using (AndroidJavaClass cls = new AndroidJavaClass (CLASS_NAME)) {
//					object[] arr = new object[]{
//						inlineVideoAdzoneId,
//						"",//MEDIA TYPE
//						"mid"//AD TYPE
//					};
//					return cls.CallStatic<bool> ("isAdReady", arr);
//				}
//			}
//			#elif UNITY_IOS
//
//			#endif
//			return false;
//		}
//	}
//	
//	#endregion
//	
//	public SeventyNineVideoAdProvider() {
//		Logger.Log("[SeventyNineVideoAdProvider] Ctor");
//	#if UNITY_ANDROID
//		if(IsSeventyNineSupported())
//	#endif
//			Initialize();
//	}
//	
//	~SeventyNineVideoAdProvider() {
//		RemoveEvents();
//	}
//
//	
//	private void RequestOffers() {
//		Logger.Log("[SeventyNineVideoAdProvider] RequestOffers ISAdReady" +IsReady);
//	}
//	
//	private void AddEvents() {
//		JuneAndroidNativeCallbackManager.AdClosed += OnAdClosed;
//		JuneAndroidNativeCallbackManager.AdFinished += OnAdFinished;
//	}
//	
//	private void RemoveEvents() {
//		JuneAndroidNativeCallbackManager.AdClosed -= OnAdClosed;
//		JuneAndroidNativeCallbackManager.AdFinished -= OnAdFinished;
//	}
//	
//	#region Events
//
//	void OnAdFinished () {
//		Logger.Log("[SeventyNineVideoAdProvider] OnAdFinished");
//		FireOnAdEnd(true);
//		RequestOffers();
//	}
//
//	void OnAdClosed () {
//		Logger.Log("[SeventyNineVideoAdProvider] OnAdClosed");
//		FireOnAdEnd(false);
//		RequestOffers();
//	}
//	
//
//	#endregion
//
//	/// <summary>
//	/// Determines if seventy nine is supported.
//	/// </summary>
//	/// <returns><c>true</c> if seventy nine is supported; otherwise, <c>false</c>.</returns>
//	private static bool IsSeventyNineSupported() {
//		return Settings.AndroidOSVersion > 10;
//	}
//}
