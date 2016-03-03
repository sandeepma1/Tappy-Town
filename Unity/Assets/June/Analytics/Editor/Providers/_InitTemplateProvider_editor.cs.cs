using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace June.Analytics.Providers {
	/// <summary>
	/// __PROVIDER_NAME__.
	/// </summary>
	public partial class __PROVIDER_NAME__ : IAnalyticsProvider {
		
		#region implemented abstract members of IAnalyticsProvider
		public override void LogEvent (string eventName, IDictionary<string, string> parameters) {
			throw new System.NotImplementedException ();
		}
		public override string ProviderName {
			get {
				throw new System.NotImplementedException ();
			}
		}
		#endregion
		
	}
}