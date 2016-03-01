namespace June.VideoAds {

	using UnityEngine;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;

	/// <summary>
	/// Video ad manager.
	/// </summary>
	public class VideoAdManager {

		/// <summary>
		/// Ad Providers initialization methods
		/// </summary>
		private static readonly Dictionary<string, Func<IVideoAdProvider>> AD_PROVIDERS = new Dictionary<string, Func<IVideoAdProvider>>() {
			#if !UNITY_IPHONE 
			{ June.VideoAds.Providers.SeventyNine, 	() => new SeventyNineVideoAdProvider() },
			#endif
			//{ June.VideoAds.Providers.Pokkt, 		() => new PokktVideoAdProvider() },
			{ June.VideoAds.Providers.Fyber, 		() => new FyberVideoAdProvider() }
		};

		/// <summary>
		/// The Default Providers to be intialized
		/// The priority is given by the array index, lower indexes have higher priority.
		/// </summary>
		private static readonly string[] DEFAULT_PROVIDERS = { 
			#if !UNITY_IPHONE 
				June.VideoAds.Providers.SeventyNine, 
			#endif
			//June.VideoAds.Providers.Pokkt,
			June.VideoAds.Providers.Fyber
		};

		/// <summary>
		/// Gets or sets the video ads watched.
		/// </summary>
		/// <value>The video ads watched.</value>
		public static int VideoAdsWatched {
			get;
			set;
		}

		/// <summary>
		/// Gets the last watched provider.
		/// </summary>
		/// <value>The last watched provider.</value>
		public static string LastWatchedProvider {
			get;
			private set;
		}

		#region Events
		public static event Action OnVideoAdStarted;
		/// <summary>
		/// Fires the any video ad started.
		/// </summary>
		internal static void FireOnVideoAdStarted() {
			if(null != OnVideoAdStarted) {
				OnVideoAdStarted();
			}
		}

		public static event Action<bool> OnVideoAdEnded;
		/// <summary>
		/// Fires the any video ad ended.
		/// </summary>
		/// <param name="hasCompleted">If set to <c>true</c> has completed.</param>
		internal static void FireOnVideoAdEnded(bool hasCompleted) {
			if(null != OnVideoAdEnded) {
				OnVideoAdEnded(hasCompleted);
			}
		}
		#endregion

		private static IVideoAdProvider[] _Providers;
		/// <summary>
		/// Gets the providers.
		/// </summary>
		/// <value>The providers.</value>
		public static IVideoAdProvider[] Providers {
			get {
				if(null == _Providers) {
					Initialize();
				}
				return _Providers;
			}
		}

		/// <summary>
		/// Initialize this instance.
		/// </summary>
		public static void Initialize() {
			VideoAdManager.VideoAdsWatched = 0;
			if(null == _Providers) {
				_Providers = new IVideoAdProvider[DEFAULT_PROVIDERS.Length];
				for(int i=0; i<DEFAULT_PROVIDERS.Length; i++) {
					_Providers[i] = AD_PROVIDERS[DEFAULT_PROVIDERS[i]]();
				}
			}
			else if(false == IsInitialized) {
				if(null != Providers && Providers.Length > 0) {
					foreach(var p in Providers) {
						if(false == p.IsInitialized) {
							p.Initialize();
						}
					}
				}
			}
		}

		/// <summary>
		/// Gets the ready providers.
		/// </summary>
		/// <value>The ready providers.</value>
		public static IVideoAdProvider[] ReadyProviders {
			get {
				if(null != Providers) {
					return Providers.Where(p => p.IsReady).ToArray();
				}
				return null;
			}
		}

		/// <summary>
		/// Gets a value indicating is ready.
		/// </summary>
		/// <value><c>true</c> if is ready; otherwise, <c>false</c>.</value>
		public static bool IsReady {
			get {
				if(null != Providers) {
					return Util.Any(Providers, p => p.IsReady);
				}
				return false;
			}
		}

		/// <summary>
		/// Gets a value indicating is initialized.
		/// </summary>
		/// <value><c>true</c> if is initialized; otherwise, <c>false</c>.</value>
		public static bool IsInitialized {
			get {
				if(null != Providers) {
					return Util.Any(Providers, p => p.IsInitialized);
				}
				return false;
			}
		}

		private static int _LastShownProvider = -1;
		/// <summary>
		/// Show this instance.
		/// </summary>
		public static bool Show(Action<bool> callback) {
			if(null != Providers)
				Debug.Log ("[VideoAdManager : Show] Providers.Length : " + Providers.Length);
			else
				Debug.Log ("[VideoAdManager : Show] Providers : null" );
			
			if(null != Providers && Providers.Length > 0) {
				foreach(var p in Providers) {
					if(false == p.IsInitialized) {
						p.Initialize();
					}
				}
			}

			if(false == IsReady && null != Providers && Providers.Length > 0) {
				Refresh();
			}

			if (null != ReadyProviders)
				Debug.Log ("[VideoAdManager : Show] ReadyProviders.Length : " + ReadyProviders.Length);
			else
				Debug.Log ("[VideoAdManager : Show] ReadyProviders : null");		


			if(null != ReadyProviders && ReadyProviders.Length > 0) {
				Debug.Log ("[VideoAdManager : Show] ShowAd 1" );
				LastWatchedProvider = ReadyProviders[0].ProviderName;
				Debug.Log ("[VideoAdManager : Show] ShowAd 2" );

				ReadyProviders[0].ShowAd(callback);
				Debug.Log ("[VideoAdManager : Show] ShowAd 3" );

				return true;
			}

			Debug.Log ("[VideoAdManager : Show] END" );
			return false;
		}

		/// <summary>
		/// Refresh this instance.
		/// </summary>
		public static void Refresh() {
			#if UNITY_ANDROID
			RefreshAndroid();
			#else
			if(null != Providers && Providers.Length > 0) {
				foreach(var p in Providers) {
					if(p.IsSupported && p.IsInitialized && false == p.IsReady) {
						p.Refresh();
					}
				}
			}
			#endif
		}

		/// <summary>
		/// Refresh's the android providers.
		/// </summary>
		public static void RefreshAndroid() {
			#if UNITY_ANDROID
			if(null != Providers && Providers.Length > 0) {
				var seventyNine = Util.FirstOrDefault(Providers, p => p.ProviderName == June.VideoAds.Providers.SeventyNine);
				var pokkt = Util.FirstOrDefault(Providers, p => p.ProviderName == June.VideoAds.Providers.Pokkt);
				var fyber = Util.FirstOrDefault(Providers, p => p.ProviderName == June.VideoAds.Providers.Fyber);

				// If seventy Nine is not ready then refresh Pokkt.
				if(null != seventyNine &&
			   	true == seventyNine.IsSupported &&
			   	true == seventyNine.IsInitialized && 
			   	false == seventyNine.IsReady) {
					seventyNine.Refresh();

					if(null != pokkt && 
					true == pokkt.IsSupported &&
					true == pokkt.IsInitialized &&
					false == pokkt.IsReady) {
						pokkt.Refresh();
					}
				}

				if(null != fyber && 
				true == fyber.IsSupported &&
				true == fyber.IsInitialized &&
			   	false == fyber.IsReady) {
					fyber.Refresh();
				}
			}
			#endif
		}
	}

	/// <summary>
	/// Video Ad Providers.
	/// </summary>
	public class Providers {
		public const string Fyber = "Fyber";
		public const string UnityAds = "UnityAds";
		public const string Vungle = "Vungle";
		public const string AdColony = "AdColony";
		public const string AdMob = "AdMob";
		public const string SeventyNine = "SeventyNine";
		public const string Pokkt = "Pokkt";
	}
}