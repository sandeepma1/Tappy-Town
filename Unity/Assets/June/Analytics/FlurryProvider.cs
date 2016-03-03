using UnityEngine;
using System.Collections;

namespace June.Analytics.Providers
{
	/// <summary>
	/// FlurryProvider.
	/// </summary>
	public partial class FlurryProvider : IAnalyticsProvider
	{

		/// <summary>
		/// Initializes a new instance of the <see cref="June.Analytics.Providers.FlurryProvider"/> class.
		/// </summary>
		public FlurryProvider () : base ()
		{
			#if UNITY_IOS
			Prime31.FlurryAnalytics.setAppVersion (GameEventManager.BundleVersion);
			#endif
			Prime31.FlurryAnalytics.startSession (GameEventManager.FlurryApiKey, true);

			this.IsInitialized = true;
		}
	}
}