using UnityEngine;
using System.Collections;
using June;

public class GameInitializer : MonoBehaviour
{
		[SerializeField]
		private GameObject
				m_GameManager;
		
//		[SerializeField]	
//		private GameObject
//				m_NetManager;
		[SerializeField]
		public GameObject
				m_AssetManager;
//		[SerializeField]
//		public GameObject
//				m_BFTrinityBootstrap;
//		[SerializeField]
//		public GameObject
//				m_KochavaObject;
//		[SerializeField]
//		public GameObject m_Fyber;
		
//	public GameObject m_GoogleAnalytics;

	//-- public GameObject m_VMAXListener;

		void Awake ()
		{
			Dispatcher.Initialize();
			/*if(LocalStorage.Instance.HasKey(LocalStorageKeys.INSTALLED_DATE) == false)
				LocalStorage.Instance.SetInt(LocalStorageKeys.INSTALLED_DATE,Util.CurrentUTCTimestamp);*/
				
				if (GameObject.Find ("GameManager") == null && m_GameManager != null) {
						GameObject go = (GameObject)Instantiate (m_GameManager, Vector3.zero, Quaternion.identity);
						go.name = "GameManager";
				}
				
				if (GameObject.Find ("AssetManager") == null && m_AssetManager != null) {
						GameObject go = (GameObject)Instantiate (m_AssetManager, Vector3.zero, Quaternion.identity);
						go.name = "AssetManager";
				}

				/*
				  
				 if (GameObject.Find ("GoogleAnalytics") == null && m_GoogleAnalytics != null) {
					GameObject go = (GameObject)Instantiate (m_GoogleAnalytics, Vector3.zero, Quaternion.identity);
					var gav3 = go.GetComponent<GoogleAnalyticsV3>();
					gav3.bundleVersion = GameConfig.BundleVersion;
					gav3.bundleIdentifier = GameConfig.BundleIdentifier;
					go.name = "GoogleAnalytics";
				}
				#if UNITY_ANDROID
				if(GameObject.Find("VMAXProviderListener") == null && null != m_VMAXListener) {
					GameObject go = (GameObject)Instantiate(m_VMAXListener, Vector3.zero, Quaternion.identity);
					go.name = "VMAXProviderListener";
					June.Ads.AdManager.Interstitial.Init();
				}
				#endif
				#if VIDEO_ADS
				if(GameObject.Find("AdmofiAndroidManager") == null) {
					GameObject go = new GameObject("AdmofiAndroidManager");
					go.AddComponent("AdmofiAndroidManager");
				}
				#endif


				if(GameObject.Find("Fyber") == null && m_Fyber != null) {
					GameObject go = (GameObject)Instantiate (m_Fyber, Vector3.zero, Quaternion.identity);
					go.name = "Fyber";
				}*/
			
		}

	void Start () {

		Util.Log("[GameInit] JuneVideAdManaher.IsInit - " + June.VideoAds.VideoAdManager.IsInitialized);
		if (false == June.VideoAds.VideoAdManager.IsInitialized) {
			June.VideoAds.VideoAdManager.Initialize ();
		}
		if (false == June.VideoAds.VideoAdManager.IsReady) {
			June.VideoAds.VideoAdManager.Refresh ();
		}

		//#if !UNITY_EDITOR
		/*	DFPInterstitialAdProvider.Initialize();*/
		//#endif

		June.Analytics.AnalyticsManager.Init ();

	}
}
