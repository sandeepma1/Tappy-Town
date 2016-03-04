//using System;
//using UnityEngine;
//using System.Collections;
//using GoogleMobileAds.Api;
//
//using Logger = June.Util;
//
//public class DFPInterstitialAdProvider  {
//	
//	private const string AD_UNIT_ID = GameConfig.DFP_AD_UNIT_ID;
//
//	/// <summary>
//	/// Occurs when on ad closed.
//	/// </summary>
//	public static event Action OnAdClosed;
//
//	/// <summary>
//	/// Occurs when an ad failed to load.
//	/// </summary>
//	public static event Action OnAdFailedToLoad;
//
//	/// <summary>
//	/// The interstitial.
//	/// </summary>
//	private static InterstitialAd Interstitial;
//
//	/// <summary>
//	/// Initialize this instance.
//	/// </summary>
//	public static void Initialize() {
//		CreateAndLoadInterstitial();
//	}
//
//	/// <summary>
//	/// Gets a value indicating is loaded.
//	/// </summary>
//	/// <value><c>true</c> if is loaded; otherwise, <c>false</c>.</value>
//	public static bool IsLoaded {
//		get {
//			return null != Interstitial ? Interstitial.IsLoaded() : false;
//		}
//	}
//
//	/// <summary>
//	/// Shows the interstitial ad.
//	/// </summary>
//	public static void ShowInterstitialAd() {
//		Logger.Log("[DFPInterstitialAdProvider] ShowInterstitialAd IsLoaded: " + IsLoaded);
//		if(null != Interstitial && Interstitial.IsLoaded()) {
//			Interstitial.Show();
//		}
//		else {
//			if(null != Interstitial) {
//				Interstitial.Destroy();
//			}
//			Interstitial = null;
//			CreateAndLoadInterstitial();			
//		}
//		
//	}
//
//	/// <summary>
//	/// Creates the and load interstitial.
//	/// </summary>
//	public static void CreateAndLoadInterstitial() {	
//		Logger.Log("[DFPInterstitialAdProvider] Creating And loading an interstitial ad");
//
//		// Initialize an InterstitialAd.
//		Interstitial = new InterstitialAd(AD_UNIT_ID);
//
//		//Register for ad events.
//		//Interstitial.AdLoaded += delegate(object sender, EventArgs args) {};
//		//Interstitial.AdOpened += delegate(object sender, EventArgs args) {};
//		//Interstitial.AdClosing += delegate(object sender, EventArgs args) {};
//		//Interstitial.AdLeftApplication += delegate(object sender, EventArgs args) {};
//		
//		Interstitial.AdFailedToLoad += HandleAdFailedToLoad;		
//		Interstitial.AdClosed += HandleAdClosed;
//		
//		
//		// Load the InterstitialAd with an AdRequest.
//		Interstitial.LoadAd(CreateAdRequest());
//	}
//	
//	// Returns an ad request with custom ad targeting.
//	private static AdRequest CreateAdRequest() {
//		return new AdRequest.Builder()
//			.AddTestDevice(AdRequest.TestDeviceSimulator)
//				.AddTestDevice("bd7da63bfadad6c879f61bf36041f039e163a5b0")
//				.AddTestDevice("7eb5a19bd87ccf4ea88e70a52da5e56e432bd755")
//				.AddTestDevice("3f52b4965b3b5bd9db16df4aac11341a74327e99")
//				.AddTestDevice("60005104f782e1db2b74be316d38922787786619")
//
//				//.AddTestDevice("C7A96C4329FD0393BB76E3F128FA2655")
//				//.AddTestDevice("cbb885a2393d797b94f2bb10ad7de7f5")
//				//.AddTestDevice("4f266adf23a175729ba0ef2ce7f2b1ae")
//				//.AddTestDevice("DCB1959B945D8DA57E37347994D7CDD5")
//				//.AddTestDevice("34C6B3A631EEC61071F8F8E082DBC77C")
//				//.AddTestDevice("B808D62546A8EEC7C79ED586800CE94F")
//				//.AddTestDevice("1BC5C3BA327774C59F0F6FF6E52E9867")
//
//				//.AddKeyword("game")
//				//.SetGender(Gender.Male)
//				//.SetBirthday(new DateTime(1985, 1, 1))
//				//.TagForChildDirectedTreatment(false)
//				//.AddExtra("color_bg", "9B30FF")
//				.Build();
//	}
//
//	#region Event Handlers
//
//	/// <summary>
//	/// Handles the ad closed event.
//	/// </summary>
//	/// <param name="sender">Sender.</param>
//	/// <param name="args">Arguments.</param>
//	private static void HandleAdClosed(object sender, EventArgs args) {
//		Logger.Log("[DFPInterstitialAdProvider] HandleAdClosed");
//		if(null != Interstitial) {
//			Interstitial.Destroy();
//		}
//		Interstitial = null;
//		CreateAndLoadInterstitial();
//		if(null != OnAdClosed) {
//			OnAdClosed();
//		}
//	}
//
//	/// <summary>
//	/// Handles the ad failed to load event.
//	/// </summary>
//	/// <param name="sender">Sender.</param>
//	/// <param name="args">Arguments.</param>
//	private static void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args) {
//		Logger.LogError("[DFPInterstitialAdProvider] HandleAdFailedToLoad AdUnitId: " + AD_UNIT_ID +" - Failed to load: " + args.Message);
//		if(null != OnAdFailedToLoad) {
//			OnAdFailedToLoad();
//		}
//	}
//
//	#endregion
//	
//}
