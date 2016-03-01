using UnityEngine;
using System.Collections;

namespace June.Analytics {
	using System.Collections;
	using System.Collections.Generic;
	using June.Analytics.Providers;

	public partial class AnalyticsManager {

		public class Parameters {
		}

		public static Dictionary<string, Event> EVENTS = new Dictionary<string, Event>() { };
	}
}