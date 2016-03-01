using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using June.Analytics.AnalyticsEditor;
using System.Linq;
using System.IO;
using UnityEditor;

namespace June.Analytics.Providers {
	
	/// <summary>
	/// IAnalytics provider.
	/// </summary>
	public abstract partial class IAnalyticsProvider {

		/// <summary>
		/// Gets the events count.
		/// </summary>
		/// <value>The events count.</value>
		public int EventsCount {
			get {
				return AnalyticsConfig.Instance.AllEvents.Count(ev => ev.Providers.Contains(this.ProviderName));
			}
		}

		/// <summary>
		/// Installs the plugin.
		/// </summary>
		public virtual void InstallPlugin() {
			if(null != ProviderTypes.PLUGINS && ProviderTypes.PLUGINS.ContainsKey(this.ProviderName)) {
				string unityPkgLocation = ProviderTypes.PLUGINS[this.ProviderName];
				AssetDatabase.ImportPackage(packagePath: unityPkgLocation, interactive: true);
			}
			else {
				EditorUtility.DisplayDialog(this.ProviderName + " Plugin", "Plugin not found!", "Ok");
			}
		}
	}
}