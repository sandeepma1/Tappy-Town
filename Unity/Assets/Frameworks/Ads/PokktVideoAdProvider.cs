//using UnityEngine;
//using System.Collections;
//using June.VideoAds;
//using Pokkt;
//
//using Logging = June.Util;
//
//namespace June.VideoAds {
//
//	/// <summary>
//	/// Pokkt video ad provider.
//	/// </summary>
//	public class PokktVideoAdProvider : IVideoAdProvider {
//
//		public const string APPLICATION_ID = GameConfig.PokktApplicationId;
//		public const string SECURITY_KEY = GameConfig.PokktSecurityKey;
//
//		private PokktConfig _Config;
//
//		/// <summary>
//		/// Initializes a new instance of the <see cref="PokktVideoAdProvider"/> class.
//		/// </summary>
//		public PokktVideoAdProvider() {
//#if UNITY_IPHONE
//			if(Settings.iOSVersion != 0f && Settings.iOSVersion < 7.0f)
//				return;
//#endif
//			Initialize(null);
//			AddEvents();
//		}
//
//		/// <summary>
//		/// Releases unmanaged resources and performs other cleanup operations before the <see cref="PokktVideoAdProvider"/> is
//		/// reclaimed by garbage collection.
//		/// </summary>
//		~PokktVideoAdProvider() {
//			RemoveEvents();
//		}
//
//		#region implemented abstract members of IVideoAdProvider
//		/// <summary>
//		/// Initialize the specified config.
//		/// </summary>
//		/// <param name="config">Config.</param>
//		public override void Initialize (System.Collections.Generic.IDictionary<string, object> config) {
//			#if UNITY_IPHONE
//			if(Settings.iOSVersion != 0f && Settings.iOSVersion < 7.0f)
//				return;
//			#endif
//			Logging.Log("[PokktVideoAdProvider] Initialize ");
//			_Config = new PokktConfig ();
//
//			// set pokkt params
//			_Config.SecurityKey = SECURITY_KEY;
//			_Config.ApplicationId = APPLICATION_ID;
//			_Config.IntegrationType = PokktIntegrationType.INTEGRATION_TYPE_VIDEO;
//
//			//TODO: Set Analytics Type to GoogleAnalytics
//			//_Config.SelectedAnalyticsType = Pokkt
//			//_Config.GoogleAnalyticsID = GameConfig.GoogleAnalyticsTrackingCode;
//			//_Config.SelectedAnalyticsType = "GOOGLE_ANALYTICS";
//
//			_Config.AutoCacheVideo = true;
//			_Config.SkipEnabled = true;
//
//			PokktManager.SetDebug(false);
//
//			_Config.ThirdPartyUserId = PlayerProfile.FullName;
//			PokktManager.InitPokkt();
//
//			this._IsInitialized = true;
//		}
//
//		/// <summary>
//		/// Adds the events.
//		/// </summary>
//		private void AddEvents() {
//			PokktManager.Dispathcer.VideoCompletedEvent += HandleVideoCompletedEvent;
//			PokktManager.Dispathcer.VideoClosedEvent += HandleVideoClosedEvent;
//			PokktManager.Dispathcer.VideoSkippedEvent += HandleVideoSkippedEvent;
//		}
//
//		/// <summary>
//		/// Removes the events.
//		/// </summary>
//		private void RemoveEvents() {
//			PokktManager.Dispathcer.VideoCompletedEvent -= HandleVideoCompletedEvent;
//			PokktManager.Dispathcer.VideoClosedEvent -= HandleVideoClosedEvent;
//			PokktManager.Dispathcer.VideoSkippedEvent -= HandleVideoSkippedEvent;
//		}
//
//		#region Event Handlers
//		/// <summary>
//		/// Handles the video completed event.
//		/// </summary>
//		/// <param name="obj">Object.</param>
//		void HandleVideoCompletedEvent (string obj) {
//			Logging.Log("[PokktVideoAdProvider] HandleVideoCompletedEvent " + obj);
//			FireOnAdEnd(true);
//		}
//
//		/// <summary>
//		/// Handles the video closed event.
//		/// </summary>
//		/// <param name="obj">Object.</param>
//		void HandleVideoClosedEvent (string obj) {
//			Logging.Log("[PokktVideoAdProvider] HandleVideoClosedEvent " + obj);
//			FireOnAdEnd(false);
//		}
//
//		/// <summary>
//		/// Handles the video skipped event.
//		/// </summary>
//		/// <param name="obj">Object.</param>
//		void HandleVideoSkippedEvent (string obj) {
//			Logging.Log("[PokktVideoAdProvider] HandleVideoSkippedEvent " + obj);
//			FireOnAdEnd(false);
//		}
//		#endregion
//
//		/// <summary>
//		/// Shows the ad.
//		/// </summary>
//		/// <param name="config">Config.</param>
//		/// <param name="callback">Callback.</param>
//		public override void ShowAd (System.Collections.Generic.IDictionary<string, object> config, System.Action<bool> callback) {
//			Logging.Log("[PokktVideoAdProvider] ShowAd ");
//			if(IsSupported == false) {
//				if(null != callback)
//					callback(false);
//				return;
//			}
//
//			_OnAdEndCallback = callback;
//			PokktManager.GetVideoNonIncent(_Config);
//		}
//
//		/// <summary>
//		/// Gets the name of the provider.
//		/// </summary>
//		/// <value>The name of the provider.</value>
//		public override string ProviderName {
//			get {
//				return Providers.Pokkt;
//			}
//		}
//
//		/// <summary>
//		/// Gets a value indicating whether this instance is supported.
//		/// </summary>
//		/// <value>true</value>
//		/// <c>false</c>
//		public override bool IsSupported {
//			get {
//				#if UNITY_ANDROID
//				return true;
//				#else
//				if(Settings.iOSVersion != 0f && Settings.iOSVersion >= 7.0f)
//					return true;
//				else
//					return false;
//				#endif
//			}
//		}
//
//		private bool _IsInitialized = false;
//		/// <summary>
//		/// Gets a value indicating whether this instance is initialized.
//		/// </summary>
//		/// <value>true</value>
//		/// <c>false</c>
//		public override bool IsInitialized {
//			get {
//				return _IsInitialized;
//			}
//		}
//
//		/// <summary>
//		/// Gets a value indicating whether this instance is ready.
//		/// </summary>
//		/// <value>true</value>
//		/// <c>false</c>
//		public override bool IsReady {
//			get {
//				if(IsSupported == false)
//					return false;
//				return PokktManager.IsVideoAvailable();
//			}
//		}
//
//		/// <summary>
//		/// Refresh this instance.
//		/// </summary>
//		public override void Refresh () {
//			Logging.Log("[PokktVideoAdProvider] Refresh ");
//			if(IsSupported == false)
//				return;
//			PokktManager.CacheVideoCampaign();
//		}
//		#endregion
//	}
//
//}