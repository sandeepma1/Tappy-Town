using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace June.Analytics.Providers {
	public partial class ProviderTypes {

		public static Dictionary<string, IAnalyticsProvider> _INSTANCES;
		/// <summary>
		/// The INSTANCEs.
		/// </summary>
		/// <value>The INSTANCE.</value>
		public static Dictionary<string, IAnalyticsProvider> INSTANCES {
			get {
				if(null == _INSTANCES) {
					_INSTANCES = new Dictionary<string, IAnalyticsProvider>() {
						{ GoogleAnalytics, new GoogleAnalyticsProvider() },
						{ Flurry, new FlurryProvider() },
						{ Amplitude, new AmplitudeProvider() },
						{ Localytics, new LocalyticsProvider() }
					};
				}
				return _INSTANCES;
			}
		}

		private static Dictionary<string, string> _PLUGINS;
		public static Dictionary<string, string> PLUGINS {
			get {
				if(null == _PLUGINS) {
#if UNITY_IPHONE
					_PLUGINS = new Dictionary<string, string>() {
						{ Flurry, System.IO.Path.Combine(June.Analytics.AnalyticsEditor.AnalyticsBuilder.PluginPath, "iOS/Flurry_2015-02-02.unitypackage") },
						{ Localytics, System.IO.Path.Combine(June.Analytics.AnalyticsEditor.AnalyticsBuilder.PluginPath, "iOS/Localytics_2015-01-22.unitypackage") }
					};
#elif UNITY_ANDROID
					_PLUGINS = new Dictionary<string, string>() {
						{ Flurry, System.IO.Path.Combine(June.Analytics.AnalyticsEditor.AnalyticsBuilder.PluginPath, "Android/Flurry_2015-02-07.unitypackage") },
						{ Localytics, System.IO.Path.Combine(June.Analytics.AnalyticsEditor.AnalyticsBuilder.PluginPath, "Android/Localytics_2015-02-07.unitypackage") }
					};
#else
					_PLUGINS = new Dictionary<string, string>() {
						{ Flurry, System.IO.Path.Combine(June.Analytics.AnalyticsEditor.AnalyticsBuilder.PluginPath, "Android/Flurry_2015-02-07.unitypackage") },
						{ Localytics, System.IO.Path.Combine(June.Analytics.AnalyticsEditor.AnalyticsBuilder.PluginPath, "Android/Localytics_2015-02-07.unitypackage") }
					};
#endif
				}
				return _PLUGINS;
			}
		}
	}
}