using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace June.Analytics.Providers {

	/// <summary>
	/// IAnalytics provider.
	/// </summary>
	public abstract partial class IAnalyticsProvider {

		/// <summary>
		/// Gets the name of the provider.
		/// </summary>
		/// <value>The name of the provider.</value>
		public abstract string ProviderName {
			get;
		}

		/// <summary>
		/// Gets or sets a value indicating whether this instance is initialized.
		/// </summary>
		/// <value><c>true</c> if this instance is initialized; otherwise, <c>false</c>.</value>
		public virtual bool IsInitialized {
			get; protected set;
		}

		/// <summary>
		/// Initializes the subscribers.
		/// </summary>
		public virtual void InitializeSubscribers() { }

		/// <summary>
		/// This method is called before event is logged.
		/// </summary>
		/// <param name="eventName">Event name.</param>
		/// <param name="parameters">Parameters.</param>
		public virtual void PreLogEvent(string eventName, IDictionary<string, string> parameters) { }
		
		/// <summary>
		/// This method is called after event is logged.
		/// </summary>
		/// <param name="eventName">Event name.</param>
		/// <param name="parameters">Parameters.</param>
		public virtual void PostLogEvent(string eventName, IDictionary<string, string> parameters) { }

		/// <summary>
		/// Logs the event.
		/// </summary>
		/// <param name="eventName">Event name.</param>
		/// <param name="parameters">Parameters.</param>
		public abstract void LogEvent(string eventName, IDictionary<string, string> parameters);

		/// <summary>
		/// Logs a custom event, need to be implemented by each provider.
		/// </summary>
		/// <param name="methodName">Method name.</param>
		/// <param name="parameters">Parameters.</param>
		public virtual void LogCustom(string methodName, IDictionary<string, object> parameters) { }

		/// <summary>
		/// Converts to string dictionary.
		/// </summary>
		/// <returns>The to string dictionary.</returns>
		/// <param name="dictionary">Dictionary.</param>
		public static Dictionary<string, string> ConvertToStringDictionary(IDictionary<string, object> dictionary) {
			Dictionary<string, string> strDictionary = null;
			if(null != dictionary) {
				strDictionary = new Dictionary<string, string>();
				foreach(var kv in dictionary) {
					strDictionary.Add(kv.Key, (null != kv.Value ? kv.Value.ToString() : string.Empty));
				}
			}
			return strDictionary;
		}

		/// <summary>
		/// Converts to object dictionary.
		/// </summary>
		/// <returns>The to object dictionary.</returns>
		/// <param name="dictionary">Dictionary.</param>
		public static Dictionary<string, object> ConvertToObjectDictionary(IDictionary<string, string> dictionary) {
			Dictionary<string, object> objDictionary = null;
			if(null != dictionary) {
				objDictionary = new Dictionary<string, object>();
				foreach(var kv in dictionary) {
					objDictionary.Add(kv.Key, kv.Value);
				}
			}
			return objDictionary;
		}

		public override string ToString () {
			return string.Format ("[IAnalyticsProvider: ProviderName={0}, IsInitialized={1}]", ProviderName, IsInitialized);
		}
	}
}