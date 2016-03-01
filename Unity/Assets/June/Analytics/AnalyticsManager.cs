using UnityEngine;
using System.Collections;

namespace June.Analytics {

	/// <summary>
	/// Analytics manager.
	/// </summary>
	public partial class AnalyticsManager  {

		/// <summary>
		/// Gets a value indicating is initialized.
		/// </summary>
		/// <value><c>true</c> if is initialized; otherwise, <c>false</c>.</value>
		public static bool IsInitialized {
			get; private set;
		}

		/// <summary>
		/// Initializes the <see cref="June.Analytics.AnalyticsManager"/> class.
		/// </summary>
		static AnalyticsManager() {
			Init();
		}

		/// <summary>
		/// Init this instance.
		/// </summary>
		public static void Init() {
			if(false == IsInitialized) {
				if(Providers.Count > 0) {
					Debug.Log("[AnalyticsManager] Providers Initialized : " + Providers.Count);
					IsInitialized = true;
				}
			}
		}

		/// <summary>
		/// Gets the parameter value.
		/// </summary>
		/// <returns>The parameter value.</returns>
		/// <param name="parameterName">Parameter name.</param>
		protected static string GetParameterValue(string parameterName) {
			string value = "NA";
			switch(parameterName) {
				//TODO
				default:
					break;
			}
			return value;
		}
	}
}