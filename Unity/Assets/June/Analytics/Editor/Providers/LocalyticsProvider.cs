
namespace June.Analytics.Providers {

	using System.Collections;
	using System.Collections.Generic;

	public partial class LocalyticsProvider : IAnalyticsProvider {

		#region implemented abstract members of IAnalyticsProvider

		public override void LogEvent (string eventName, IDictionary<string, string> parameters) {
		}

		public override string ProviderName {
			get {
				return ProviderTypes.Localytics;
			}
		}

		#endregion
	}
}