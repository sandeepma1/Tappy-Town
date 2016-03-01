//#if UNITY_ANDROID
//using UnityEngine;
//using System.Collections;
//
//using Logger = UnityEngine.Debug;
//
///// <summary>
///// VMAX provider listener.
///// </summary>
//public class VMAXProviderListener : MonoBehaviour {
//
//	private const string NAME = "VMAXProviderListener";
//
//	private static VMAXProviderListener _Instance;
//
//	/// <summary>
//	/// Gets the name of the callback.
//	/// </summary>
//	/// <value>The name of the callback.</value>
//	public static string CallbackName {
//		get {
//			return null != _Instance ? NAME : _Instance.gameObject.name;
//		}
//	}
//
//	void Awake() {
//		Logger.Log("[VMAXProviderListener] AWAKE");
//		this.gameObject.name = NAME;
//		DontDestroyOnLoad(this.gameObject);
//		_Instance = this;
//	}
//
//	void Start() {
//		Logger.Log("[VMAXProviderListener] START");
//	}
//	
//	/* This Callback method is invoked when Ad is clicked or interaction happen with Ad */
//	public void didInteractWithAd(string unknown) {
//		Logger.Log("[VMAXProviderListener] didInteractWithAd unknown:" + unknown);
//	}
//	
//	/* This Callback method is invoked when fetched Ad is successfully rendered */
//	public void adViewDidLoadAd(string requestCode) {
//		Logger.Log("[VMAXProviderListener] adViewDidLoadAd requestCode:" + requestCode);
//	}
//	
//	/* This Callback method is invoked when Ad is going to be shown */
//	public void willPresentOverlay(string unknown) {
//		Logger.Log("[VMAXProviderListener] willPresentOverlay unknown:" + unknown);
//	}
//	
//	/* This Callback method is invoked when Ad is dismissed */
//	public void willDismissOverlay(string requestCode) {
//		Logger.Log("[VMAXProviderListener] willDismissOverlay requestCode:" + requestCode);
//		//Set IsReady to false
//		if(null != VMAXPlugin.Instance) {
//			VMAXPlugin.Instance.IsReady = false;
//		}
//		//Cache ads
//		if(June.Ads.AdManager.Interstitial.Contians(June.Ads.Providers.VMAX)) {
//			June.Ads.AdManager.Interstitial[June.Ads.Providers.VMAX].Refresh();
//		}
//	}
//	
//	/* This Callback method is invoked when Ad is failed to load because of any reason if any. */
//	public void didFailedToLoadAd(string unknown) {
//		Logger.Log("[VMAXProviderListener] didFailedToLoadAd unknown:" + unknown);
//	}
//	
//	/* This Callback method is invoked When Ad is going to leave current App and open any outside app */
//	public void willLeaveApp(string unknown) {
//		Logger.Log("[VMAXProviderListener] willLeaveApp unknown:" + unknown);
//	}
//	
//	/* This Callback method is invoked when Ad is clicked or interaction happen with Ad */
//	public void onConnectionFailure(string unknown) {
//		Logger.Log("[VMAXProviderListener] onConnectionFailure unknown:" + unknown);
//	}
//	
//	/* This Callback method is invoked when Ad is cached successfully */
//	public void adViewDidCacheAd(string requestCode) {
//		Logger.Log("[VMAXProviderListener] adViewDidCacheAd requestCode:" + requestCode);
//		if(null != VMAXPlugin.Instance) {
//			VMAXPlugin.Instance.IsReady = true;
//		}
//	}
//	
//	/* This Callback method is invoked when Ad is failed to Cache because of any reason if any. */
//	public void didFailedToCacheAd(string requestCode) {
//		Logger.Log("[VMAXProviderListener] didFailedToCacheAd requestCode:" + requestCode);
//		if(null != VMAXPlugin.Instance) {
//			VMAXPlugin.Instance.IsReady = false;
//		}
//	}
//	
//}
//#endif