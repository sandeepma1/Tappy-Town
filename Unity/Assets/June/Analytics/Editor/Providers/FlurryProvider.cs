/*
 * Flurry provider v0.1
 * --------------------
 * 
 * This class has been autogenarated, DO NOT MODIFY.
 * 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Prime31;
using System.Text;

namespace June.Analytics.Providers {
	
	/// <summary>
	/// Flurry provider.
	/// </summary>
	public partial class FlurryProvider : IAnalyticsProvider {
		#region implemented abstract members of IAnalyticsProvider
		
		/// <summary>
		/// Gets the name of the provider.
		/// </summary>
		/// <value>The name of the provider.</value>
		public override string ProviderName {
			get {
				return ProviderTypes.Flurry;
			}
		}
		
		/// <summary>
		/// Logs the event.
		/// </summary>
		/// <param name="eventName">Event name.</param>
		/// <param name="parameters">Parameters.</param>
		public override void LogEvent (string eventName, IDictionary<string, string> parameters) {
			//Debug.Log("[Flurry] LogEvent: " + eventName);
			#if UNITY_IOS
			FlurryAnalytics.logEventWithParameters(eventName, FlurryParamDictFromDict(parameters), false);
			#else
			PreLogEvent(eventName, parameters);
			FlurryAnalytics.logEvent(eventName, FlurryParamDictFromDict(parameters));
			PostLogEvent(eventName, parameters);
			#endif
		}
		
		/// <summary>
		/// Logs a custom event, need to be implemented by each provider.
		/// </summary>
		/// <param name="methodName">Method name.</param>
		/// <param name="parameters">Parameters.</param>
		public override void LogCustom (string methodName, IDictionary<string, object> parameters) {
			switch(methodName) {
			case CustomMethods.StartTimedEvent:
				if(null != parameters && parameters.ContainsKey("eventName")) {
					#if UNITY_IOS
					FlurryAnalytics.logEventWithParameters(
						(string)parameters["eventName"], 
						FlurryParamDictFromDict(ConvertToStringDictionary(parameters)),
						true);
					#else 
					FlurryAnalytics.logEvent(
						(string)parameters["eventName"], 
						FlurryParamDictFromDict(ConvertToStringDictionary(parameters)),
						true);
					#endif
				}					
				break;
			case CustomMethods.EndTimedEvent:
				if(null != parameters && parameters.ContainsKey("eventName")) {
					#if UNITY_IOS
					FlurryAnalytics.endTimedEvent(
						(string)parameters["eventName"], 
						FlurryParamDictFromDict(ConvertToStringDictionary(parameters)));
					#else
					FlurryAnalytics.endTimedEvent(
						(string)parameters["eventName"], 
						FlurryParamDictFromDict(ConvertToStringDictionary(parameters)));
					#endif
				}					
				break;
			default:
				break;
			}
		}
		
		#endregion
		
		/// <summary>
		/// Flurry parameter dictionary from dictionary.
		/// </summary>
		/// <returns>The parameter dict from dict.</returns>
		/// <param name="dict">Dict.</param>
		static Dictionary<string, string> FlurryParamDictFromDict(IDictionary<string, string> dict) {
			//Debug.Log("[Flurry] Param Dict->Dict");
			return new Dictionary<string, string>() {
				{ "PP", FlurryParamStrFromDict(dict) }
			};
		}
		
		/// <summary>
		/// Flurry parameter string from dictionary.
		/// </summary>
		/// <returns>The parameter from dict.</returns>
		/// <param name="d">D.</param>
		static string FlurryParamStrFromDict (IDictionary<string, string> d) {
			//Debug.Log("[Flurry] Param Dict->Str");
			// Build up each line one-by-one and then trim the end
			StringBuilder builder = new StringBuilder ();
			if(null != d) {
				foreach (KeyValuePair<string, string> pair in d) {
					builder.Append (pair.Key).Append (":").Append (pair.Value).Append (';');
				}
			}
			string parameter = builder.ToString ();
			// Remove the final delimiter
			parameter = parameter.TrimEnd (';');
			//Debug.Log("[Flurry] Param Str:: " + parameter);
			return parameter;
		}
		
		///
		public class CustomMethods {
			public const string StartTimedEvent = "ste";
			public const string EndTimedEvent = "ete";
		}
	}
}