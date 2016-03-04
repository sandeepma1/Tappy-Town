using System;
using UnityEngine;
using System.Collections;

namespace June.VideoAds
{
	/// <summary>
	/// Fyber video ad provider.
	/// </summary>
	public class FyberVideoAdProvider : IVideoAdProvider
	{

		string appId = GameEventManager.fyberAppId;
		string securityToken = GameEventManager.fyberSecurityToken;
		string customCurrencyName = GameEventManager.fyberCustomCurrencyName;


		public string userId = null;

		public const string placementId = "1";
		public string currencyId = "";

		public static SponsorPay.SponsorPayPlugin Instance {
			get {
				return SponsorPayPluginMonoBehaviour.PluginInstance;
			}
		}

		#region implemented abstract members of IVideoAdProvider

		/// <summary>
		/// Initialize the specified config.
		/// </summary>
		/// <param name="config">Config.</param>
		public override void Initialize (System.Collections.Generic.IDictionary<string, object> config)
		{
			if (null != Instance) {
				AddEvents ();
				Util.Log ("[Fyber] Instance.Start");
				Instance.Start (appId, userId, securityToken);
				RequestOffers ();
				this._IsInitialized = true;
			} else {
				Util.Log ("[FYBER] INSTANCE IS NULL");
			}
		}

		/// <summary>
		/// Refresh this instance.
		/// </summary>
		public override void Refresh ()
		{
			RequestOffers ();
		}

		/// <summary>
		/// Shows the ad.
		/// </summary>
		/// <param name="config">Config.</param>
		/// <param name="callback">Callback.</param>
		public override void ShowAd (System.Collections.Generic.IDictionary<string, object> config, Action<bool> callback)
		{
			if (false == _IsInitialized) {
				Initialize ();
			} else if (false == IsReady) {
				Util.Log ("[Fyber] Request");
				RequestOffers ();
			} else {
				_OnAdEndCallback = callback;
				_IsReady = false;
				Util.Log ("[Fyber] StartBrandEngage");
				FireOnAdStart ();
				Instance.StartBrandEngage ();
			}
		}

		/// <summary>
		/// Gets the name of the provider.
		/// </summary>
		/// <value>The name of the provider.</value>
		public override string ProviderName {
			get {
				return Providers.Fyber;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance is supported.
		/// </summary>
		/// <value>true</value>
		/// <c>false</c>
		public override bool IsSupported {
			get {
				#if UNITY_EDITOR || UNITY_ANDROID || UNITY_IPHONE
				return true;
				#else
				return false;
				#endif
			}
		}

		private bool _IsInitialized;

		/// <summary>
		/// Gets a value indicating whether this instance is initialized.
		/// </summary>
		/// <value>true</value>
		/// <c>false</c>
		public override bool IsInitialized {
			get { return _IsInitialized; }
		}

		private bool _IsReady = false;

		/// <summary>
		/// Gets a value indicating whether this instance is ready.
		/// </summary>
		/// <value>true</value>
		/// <c>false</c>
		public override bool IsReady {
			get { return _IsReady; }
		}

		#endregion

		public FyberVideoAdProvider ()
		{
			Util.Log ("[Fyber] Ctor");
			Initialize ();
		}

		~FyberVideoAdProvider ()
		{
			RemoveEvents ();
		}

		private void RequestOffers ()
		{
			Util.Log ("[Fyber] RequestBrandEngageOffers");
			Instance.RequestBrandEngageOffers (null, customCurrencyName, false, currencyId, placementId);
		}

		private void AddEvents ()
		{

			Instance.EnableLogging (false);
			//Instance.SetLogLevel(SponsorPay.SPLogLevel.Info);

			// Register delegates to be notified of the result of the "get new coins" request
			//Instance.OnSuccessfulCurrencyRequestReceived += new SponsorPay.SuccessfulCurrencyResponseReceivedHandler(OnSuccessfulCurrencyRequestReceived);
			//Instance.OnDeltaOfCoinsRequestFailed += new SponsorPay.ErrorHandler(OnSPDeltaOfCoinsRequestFailed);

			// Register delegates to be notified of the result of a BrandEngage request
			Instance.OnBrandEngageRequestResponseReceived += new SponsorPay.BrandEngageRequestResponseReceivedHandler (OnSPBrandEngageResponseReceived);
			Instance.OnBrandEngageRequestErrorReceived += new SponsorPay.BrandEngageRequestErrorReceivedHandler (OnSPBrandEngageErrorReceived);
			
			// Register delegates to be notified when a native exception occurs on the plugin
			Instance.OnNativeExceptionReceived += new SponsorPay.NativeExceptionHandler (OnNativeExceptionReceivedFromSDK);
			//Instance.OnOfferWallResultReceived += new SponsorPay.OfferWallResultHandler(OnOFWResultReceived);
			Instance.OnBrandEngageResultReceived += new SponsorPay.BrandEngageResultHandler (OnMBEResultReceived);
			
			//Interstitial delegates
			Instance.OnInterstitialRequestResponseReceived += new SponsorPay.InterstitialRequestResponseReceivedHandler (OnSPInterstitialResponseReceived);
			Instance.OnInterstitialRequestErrorReceived += new SponsorPay.InterstitialRequestErrorReceivedHandler (OnSPInterstitialErrorReceived);
			
			Instance.OnInterstitialStatusCloseReceived += new SponsorPay.InterstitialStatusCloseHandler (OnSPInterstitialStatusCloseReceived);
			Instance.OnInterstitialStatusErrorReceived += new SponsorPay.InterstitialStatusErrorHandler (OnSPInterstitialStatusErrorReceived);

		}

		private void RemoveEvents ()
		{

			// Register delegates to be notified of the result of a BrandEngage request
			Instance.OnBrandEngageRequestResponseReceived -= OnSPBrandEngageResponseReceived; //new SponsorPay.BrandEngageRequestResponseReceivedHandler(OnSPBrandEngageResponseReceived);
			Instance.OnBrandEngageRequestErrorReceived -= OnSPBrandEngageErrorReceived; //new SponsorPay.BrandEngageRequestErrorReceivedHandler(OnSPBrandEngageErrorReceived);
			
			// Register delegates to be notified when a native exception occurs on the plugin
			Instance.OnNativeExceptionReceived -= OnNativeExceptionReceivedFromSDK; //new SponsorPay.NativeExceptionHandler(OnNativeExceptionReceivedFromSDK);
			//Instance.OnOfferWallResultReceived += new SponsorPay.OfferWallResultHandler(OnOFWResultReceived);
			Instance.OnBrandEngageResultReceived -= OnMBEResultReceived; //new SponsorPay.BrandEngageResultHandler(OnMBEResultReceived);
			
			//Interstitial delegates
			Instance.OnInterstitialRequestResponseReceived -= OnSPInterstitialResponseReceived; //new SponsorPay.InterstitialRequestResponseReceivedHandler(OnSPInterstitialResponseReceived);
			Instance.OnInterstitialRequestErrorReceived -= OnSPInterstitialErrorReceived; //new SponsorPay.InterstitialRequestErrorReceivedHandler(OnSPInterstitialErrorReceived);
			
			Instance.OnInterstitialStatusCloseReceived -= OnSPInterstitialStatusCloseReceived; //new SponsorPay.InterstitialStatusCloseHandler(OnSPInterstitialStatusCloseReceived);
			Instance.OnInterstitialStatusErrorReceived -= OnSPInterstitialStatusErrorReceived; //new SponsorPay.InterstitialStatusErrorHandler(OnSPInterstitialStatusErrorReceived);

		}

		#region Events

		// Registered to be called upon reception of the answer for a successful offer request
		public void OnSPBrandEngageResponseReceived (bool offersAvailable)
		{
			Util.Log ("[Fyber] OnSPBrandEngageResponseReceived " + offersAvailable);
			this._IsReady = offersAvailable;
		}

		// Registered to be called if an error is triggered by the offer request
		public void OnSPBrandEngageErrorReceived (string message)
		{
			Util.Log ("[Fyber] OnSPBrandEngageErrorReceived " + message);
		}

		// OnNativeExceptionReceivedFromSDK:
		public void OnNativeExceptionReceivedFromSDK (string message)
		{
			Util.Log ("[Fyber] OnNativeExceptionReceivedFromSDK " + message);
		}

		// OnMBEResultReceived:
		public void OnMBEResultReceived (string message)
		{
			// Message value can be the following:
			// ~ CLOSE_FINISHED - User has successfully completed the engagement
			// ~ CLOSE_ABORTED - User has cancelled the engagement before finishing it
			// ~ ERROR - An unknown error has occurred
			Util.Log ("[Fyber] OnMBEResultReceived " + message + " Watched:" + VideoAdManager.VideoAdsWatched);
			bool status = false;
			if (!string.IsNullOrEmpty (message) && 0 == string.Compare (message, "CLOSE_FINISHED", true)) {
				status = true;
			}

			FireOnAdEnd (status);
			RequestOffers ();
		}
		
		
		// OnSPInterstitialStatusCloseReceived:
		public void OnSPInterstitialStatusCloseReceived (string closeReason)
		{
//			interstitialAdStatus = "No offers";
//			dialogTitle = "Interstitial return status";
//			dialogMessage = closeReason;
//			showDialog = true;
			Util.Log ("[Fyber] OnSPInterstitialStatusCloseReceived " + closeReason);
			RequestOffers ();
		}
		
		
		// OnSPInterstitialStatusErrorReceived:
		public void OnSPInterstitialStatusErrorReceived (string message)
		{
//			interstitialAdStatus = "No offers";
//			dialogTitle = "Interstitial return error message";
//			dialogMessage = message;
//			showDialog = true;
			Util.Log ("[Fyber] OnSPInterstitialStatusErrorReceived " + message);
			RequestOffers ();
		}

		// Registered to be called upon reception of the answer for a successful offer request
		public void OnSPInterstitialResponseReceived (bool adsAvailable)
		{
			Util.Log ("[Fyber] OnSPInterstitialResponseReceived " + adsAvailable);
			//this._IsReady = adsAvailable;
		}
		
		// Registered to be called if an error is triggered by the offer request
		public void OnSPInterstitialErrorReceived (string message)
		{
			Util.Log ("[Fyber] OnSPInterstitialErrorReceived " + message);
			//RequestOffers();
		}

		#endregion
	}
}