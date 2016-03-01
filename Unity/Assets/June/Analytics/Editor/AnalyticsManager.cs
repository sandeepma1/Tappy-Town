/*
 * Analytics Manager v0.1
 * ----------------------
 * 
 * This class has been autogenarated, DO NOT MODIFY.
 * 
 */ 

namespace June.Analytics {
	using System.Linq;
	using System.Collections;
	using System.Collections.Generic;
	using June.Analytics.Providers;


	/// <summary>
	/// Analytics manager.
	/// </summary>
	public partial class AnalyticsManager {

		private static Dictionary<string, IAnalyticsProvider> _Providers = null;
		/// <summary>
		/// Gets the providers.
		/// </summary>
		/// <value>The providers.</value>
		public static Dictionary<string, IAnalyticsProvider> Providers {
			get {
				if(null == _Providers) {
					_Providers = new Dictionary<string, IAnalyticsProvider>() {
//<<PROVIDER_INSTANCES>>
					};
					InitializeSubscribedEvents();
				}
				return _Providers;
			}
		}

		/// <summary>
		/// Initializes the subscribed events.
		/// </summary>
		private static void InitializeSubscribedEvents() {
			foreach(var evnt in EVENTS) {
				if(false == string.IsNullOrEmpty(evnt.Value.SubscribedMessage)) {
					string eventName = evnt.Key;
					MessageBroker.Instance.Subscribe(evnt.Value.SubscribedMessage, (msg, param) => {
						Dictionary<string, string> evParameters = null;
						if(null != param && param.Count > 0) {

							foreach(var keyValue in param) {
								if(!string.IsNullOrEmpty(keyValue.Key) && keyValue.Value != null) {
									if(null == evParameters) {
										evParameters = new Dictionary<string, string>();
									}
									evParameters.Add(keyValue.Key, keyValue.Value.ToString());
								}
							}
						}
						if(null != evParameters && evParameters.Count > 0) {
							LogEvent(eventName, evParameters);
						}
						else {
							LogEvent(eventName);
						}
					});
				}
			}
		}

		/// <summary>
		/// Analytics Event
		/// </summary>
		public class Event {
			public string Name;
			public string[] Parameters;
			public string[] Providers;
			public string SubscribedMessage;
		}

//<<PARAMETERS>>

//<<EVENTS>>

		/// <summary>
		/// Logs the event.
		/// </summary>
		/// <param name="eventName">Event name.</param>
		public static void LogEvent(string eventName) {
			LogEvent(eventName, new Dictionary<string, string>());
		}

		/// <summary>
		/// Logs the event.
		/// </summary>
		/// <param name="eventName">Event name.</param>
		/// <param name="parameters">Parameters.</param>
		public static void LogEvent(string eventName, IDictionary<string, string> parameters) {
			LogEvent(EVENTS[eventName], parameters);
		}

		/// <summary>
		/// Logs the event.
		/// </summary>
		/// <param name="evnt">Evnt.</param>
		/// <param name="parameters">Parameters.</param>
		public static void LogEvent(Event evnt, IDictionary<string, string> parameters) {
			if(null != evnt) {

				//Populate missing parameter values.
				PopulateParameterValues(evnt, parameters);

				// Call log event for each of the providers.
				foreach(var prov in evnt.Providers) {
					if(Providers.ContainsKey(prov)) {
						Providers[prov].LogEvent(evnt.Name, parameters);
					}
				}
			}
		}

		/// <summary>
		/// Populates the parameter values.
		/// </summary>
		/// <param name="evnt">Evnt.</param>
		/// <param name="parameters">Parameters.</param>
		protected static void PopulateParameterValues(Event evnt, IDictionary<string, string> parameters) {
			if(null != evnt && null != parameters) {
				foreach(var p in evnt.Parameters) {
					if(false == parameters.ContainsKey(p)) {
						parameters.Add(p, GetParameterValue(p));
					}
				}
			}
		}
	}
}