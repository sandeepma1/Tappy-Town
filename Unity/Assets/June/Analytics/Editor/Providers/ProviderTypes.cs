using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace June.Analytics.Providers {

	/// <summary>
	/// Provider types.
	/// </summary>
	public partial class ProviderTypes  {

		/// <summary>
		/// The amplitude analytics provider.
		/// </summary>
		public static string Amplitude = "Amplitude";

		/// <summary>
		/// The google analytics provider.
		/// </summary>
		public const string GoogleAnalytics = "GoogleAnalytics";

		/// <summary>
		/// The flurry provider.
		/// </summary>
		public const string Flurry = "Flurry";

		/// <summary>
		/// The localytics provider.
		/// </summary>
		public const string Localytics = "Localytics";

		/// <summary>
		/// The kochava provider.
		/// </summary>
		public const string Kochava = "Kochava";

	}

	/// <summary>
	/// Provider type enum.
	/// </summary>
	public enum ProviderTypeEnum {
		GoogleAnalytics,
		Flurry,
		Localytics,
		Amplitude
	}
}