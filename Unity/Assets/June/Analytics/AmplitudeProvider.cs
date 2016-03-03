//using UnityEngine;
//using System.Collections;
//
//namespace June.Analytics.Providers {
//	/// <summary>
//	/// AmplitudeProvider.
//	/// </summary>
//	public partial class AmplitudeProvider : IAnalyticsProvider {
//
//		/// <summary>
//		/// Initializes a new instance of the <see cref="June.Analytics.Providers.AmplitudeProvider"/> class.
//		/// </summary>
//		public AmplitudeProvider() : base() {
//
//			//TODO: Write initialization code.
//			#if UNITY_IOS
//				Amplitude.Instance.init(GameConfig.AmplitudeAPIKey);
//			#endif
//			this.IsInitialized = true;
//		}
//	}
//}