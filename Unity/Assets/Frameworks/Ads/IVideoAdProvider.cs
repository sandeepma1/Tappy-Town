namespace June.VideoAds {

	using UnityEngine;
	using System;
	using System.Collections;
	using System.Collections.Generic;

	/// <summary>
	/// Abstract interface for video ad providers.
	/// </summary>
	public abstract class IVideoAdProvider {

		public event Action OnAdStart;
		/// <summary>
		/// Fires the on ad start event.
		/// </summary>
		protected void FireOnAdStart() {
			Util.Log("[IVideoAdProvider] " + ProviderName + " FireOnAdStart");
			if(null != OnAdStart) {
				OnAdStart();
			}
			VideoAdManager.FireOnVideoAdStarted();
		}

		/// <summary>
		/// Occurs when on ad end.
		/// Contains one parameter which is a boolean,
		/// this specifies if the ad was watched successfully.
		/// </summary>
		public event Action<bool> OnAdEnd;
		/// <summary>
		/// Fires the on ad ended event.
		/// </summary>
		protected void FireOnAdEnd(bool hasCompleted) {
			Util.Log("[IVideoAdProvider] " + ProviderName + " FireOnAdEnd hasCompleted:" + hasCompleted);
			if(hasCompleted) {
				VideoAdManager.VideoAdsWatched += 1;
			}
			if(null != _OnAdEndCallback) {
				_OnAdEndCallback(hasCompleted);
				_OnAdEndCallback = null;
			}
			if(null != OnAdEnd) {
				OnAdEnd(hasCompleted);
			}
			VideoAdManager.FireOnVideoAdEnded(hasCompleted);
		}

		protected Action<bool> _OnAdEndCallback = null;

		/// <summary>
		/// Gets the name of the provider.
		/// </summary>
		/// <value>The name of the provider.</value>
		public abstract string ProviderName {
			get;
		}

		/// <summary>
		/// Gets a value indicating whether this instance is supported.
		/// </summary>
		/// <value><c>true</c> if this instance is supported; otherwise, <c>false</c>.</value>
		public abstract bool IsSupported {
			get;
		}

		/// <summary>
		/// Gets a value indicating whether this instance is initialized.
		/// </summary>
		/// <value><c>true</c> if this instance is initialized; otherwise, <c>false</c>.</value>
		public abstract bool IsInitialized {
			get;
		}

		/// <summary>
		/// Gets a value indicating whether this instance is ready.
		/// </summary>
		/// <value><c>true</c> if this instance is ready; otherwise, <c>false</c>.</value>
		public abstract bool IsReady {
			get;
		}

		/// <summary>
		/// Initialize this instance.
		/// </summary>
		public virtual void Initialize() {
			Initialize(null);
		}

		/// <summary>
		/// Initialize the specified config.
		/// </summary>
		/// <param name="config">Config.</param>
		public abstract void Initialize(IDictionary<string, object> config);

		/// <summary>
		/// Initializes a new instance of the <see cref="June.VideoAds.IVideoAdProvider"/> class.
		/// </summary>
		public virtual void Refresh() {
		}

		/// <summary>
		/// Shows the ad.
		/// </summary>
		public virtual void ShowAd() {
			ShowAd(null);
		}

		/// <summary>
		/// Shows the ad.
		/// </summary>
		/// <param name="callback">Callback.</param>
		public virtual void ShowAd(Action<bool> callback) {
			ShowAd(null, callback);
		}

		/// <summary>
		/// Shows the ad.
		/// </summary>
		/// <param name="config">Config.</param>
		public abstract void ShowAd(IDictionary<string, object> config, Action<bool> callback);

		/// <summary>
		/// Gets the parameter value.
		/// </summary>
		/// <returns>The parameter value.</returns>
		/// <param name="parameters">Parameters.</param>
		/// <param name="key">Key.</param>
		/// <param name="defaultValue">Default value.</param>
		protected static object GetParameterValue(IDictionary<string, object> parameters, string key, object defaultValue) {
			return (null != parameters && parameters.ContainsKey(key)) ? parameters[key] : defaultValue;
		}
	}
}