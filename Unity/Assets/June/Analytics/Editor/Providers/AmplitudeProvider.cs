using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace June.Analytics.Providers {

	/// <summary>
	/// Amplitude provider.
	/// </summary>
	public partial class AmplitudeProvider : IAnalyticsProvider {
		#region implemented abstract members of IAnalyticsProvider

		/// <summary>
		/// Logs the event.
		/// </summary>
		/// <param name="eventName">Event name.</param>
		/// <param name="parameters">Parameters.</param>
		public override void LogEvent (string eventName, IDictionary<string, string> parameters) {
			IDictionary<string, object> dict = null;
			if(null != parameters && parameters.Count > 0) {
				dict = ConvertToObjectDictionary(parameters);
			}
			Amplitude.Instance.setUserProperties (ConvertToObjectDictionary(parameters));
			Amplitude.Instance.logEvent (eventName);
		}


		public class CustomMethods {
			public const string StartSession = "startsession";
			public const string EndSession = "endsession";
		}

		/// <summary>
		/// Logs a custom event, need to be implemented by each provider.
		/// </summary>
		/// <param name="methodName">Method name.</param>
		/// <param name="parameters">Parameters.</param>
		public override void LogCustom (string methodName, IDictionary<string, object> parameters) {
			switch(methodName.ToLower()) {
			case CustomMethods.StartSession:
				if(IsInitialized && null != Amplitude.Instance) {
					Amplitude.Instance.startSession();
				}
				break;
			case CustomMethods.EndSession:
				if(IsInitialized && null != Amplitude.Instance) {
					Amplitude.Instance.endSession();
				}
				break;
			default:
				break;
			}
		}

		/// <summary>
		/// Gets the name of the provider.
		/// </summary>
		/// <value>The name of the provider.</value>
		public override string ProviderName {
			get {
				return ProviderTypes.Amplitude;
			}
		}

		#endregion
	}
}