/*
 * Google Analytics Provider v0.1
 * ------------------------------
 * 
 * This class has been autogenarated, DO NOT MODIFY.
 * 
 */ 

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace June.Analytics.Providers {

	/// <summary>
	/// Google analytics provider.
	/// </summary>
	public partial class GoogleAnalyticsProvider : IAnalyticsProvider {
		#region Implemented abstract members of IAnalyticsProvider

		/// <summary>
		/// Gets the name of the provider.
		/// </summary>
		/// <value>The name of the provider.</value>
		public override string ProviderName {
			get {
				return "GoogleAnalytics";
			}
		}

		/// <summary>
		/// Logs the event.
		/// </summary>
		/// <param name="eventName">Event name.</param>
		/// <param name="parameters">Parameters.</param>
		public override void LogEvent (string eventName, IDictionary<string, string> parameters) {

		}

		#endregion
	}
}