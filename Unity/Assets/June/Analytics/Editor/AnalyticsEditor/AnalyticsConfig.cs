using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;
using June.Core;


namespace June.Analytics.AnalyticsEditor {

	/// <summary>
	/// Analytics config.
	/// </summary>
	public class AnalyticsConfig : BaseConfig<AnalyticsConfig, AnalyticsConfig> {

		internal class Schema {
			public const string Providers = "providers";
			public const string Parameters = "parameters";
			public const string Events = "events";
		}

		#region implemented abstract members of BaseConfig

		public override List<AnalyticsConfig> Items {
			get {
				//IGNORING THIS LIST, use `Instance.Events` instead.
				return null;
			}
		}

		/// <summary>
		/// Gets the name of the resource.
		/// </summary>
		/// <value>The name of the resource.</value>
		public override string ResourceName {
			get {
				return "Events";
			}
		}

		/// <summary>
		/// Gets the root key.
		/// </summary>
		/// <value>The root key.</value>
		public override string RootKey {
			get {
				return "events";
			}
		}

		/// <summary>
		/// Gets the deserialize func.
		/// </summary>
		/// <value>The deserialize func.</value>
		public override System.Converter<string, IDictionary<string, object>> DeserializeFunc {
			get {
				return ReadEventsConfig;
			}
		}

		/// <summary>
		/// Gets the item converter.
		/// </summary>
		/// <returns>The item converter.</returns>
		/// <typeparam name="U">The 1st type parameter.</typeparam>
		/// <value>The item converter.</value>
		public override System.Converter<IDictionary<string, object>, AnalyticsConfig> ItemConverter {
			get {
				return doc => this;
			}
		}

		/// <summary>
		/// Loads the items.
		/// </summary>
		protected override void LoadItems () { /* DO NOTHING */ }

		/// <summary>
		/// Reads the events config.
		/// </summary>
		/// <returns>The events config.</returns>
		/// <param name="fileName">File name.</param>
		public static IDictionary<string, object> ReadEventsConfig(string fileName) {
			string analyticsPath = Path.Combine(Application.dataPath, "June/Analytics/Editor");
			string eventsConfigPath = Path.Combine(analyticsPath, fileName + ".json");
			string fileContents = File.ReadAllText(eventsConfigPath);
			return (IDictionary<string, object>)SimpleJson.SimpleJson.DeserializeObject(fileContents);
		}

		#endregion

		#region Providers
		private List<string> _Providers = null;
		/// <summary>
		/// Gets the providers.
		/// </summary>
		/// <value>The providers.</value>
		public List<string> Providers {
			get {
				if(null == _Providers) {
					_Providers = GetStringList(Schema.Providers);
				}
				return _Providers;
			}
		}

		private List<June.Analytics.Providers.IAnalyticsProvider> _ProviderInstances;
		/// <summary>
		/// Gets the provider instances.
		/// </summary>
		/// <value>The provider instances.</value>
		public List<June.Analytics.Providers.IAnalyticsProvider> ProviderInstances {
			get {
				if(null == _ProviderInstances) {
					_ProviderInstances = new List<June.Analytics.Providers.IAnalyticsProvider>();
					foreach(var prod in Providers) {
						if(!string.IsNullOrEmpty(prod) && June.Analytics.Providers.ProviderTypes.INSTANCES.ContainsKey(prod)) {
							_ProviderInstances.Add(June.Analytics.Providers.ProviderTypes.INSTANCES[prod]);
						}
					}
				}
				return _ProviderInstances;
			}
		}
		#endregion

		#region Parameters

		private IDictionary<string, object> _ParameterDoc {
			get {
				return Get<IDictionary<string, object>>(Schema.Parameters);
			}
		}

		private Dictionary<string, AnalyticsParameter> _Parameters = null;
		/// <summary>
		/// Gets the parameters.
		/// </summary>
		/// <value>The parameters.</value>
		public Dictionary<string, AnalyticsParameter> Parameters {
			get {
				if(null == _Parameters) {
					_Parameters = AnalyticsParameter.ConvertToDictionary(_ParameterDoc);
				}
				return _Parameters;
			}
		}

		private List<AnalyticsParameter> _AllParameters = null;
		/// <summary>
		/// Gets all parameters.
		/// </summary>
		/// <value>All parameters.</value>
		public List<AnalyticsParameter> AllParameters {
			get {
				if(null == _AllParameters) {
					_AllParameters = AnalyticsParameter.ConvertToList(_ParameterDoc);
				}
				return _AllParameters;
			}
		}

		/// <summary>
		/// Adds/Updates the parameter.
		/// </summary>
		/// <returns><c>true</c>, if parameter was added, <c>false</c> otherwise.</returns>
		/// <param name="code">Code.</param>
		/// <param name="name">Name.</param>
		/// <param name="isCustom">If set to <c>true</c> is custom.</param>
		public bool AddParameter(string code, string name, bool isCustom) {
			bool status = false;
			var doc = _ParameterDoc;
			if(null != doc && false == doc.ContainsKey(code.ToUpper())) {
				var pDoc = new Dictionary<string, object>() {
					{ AnalyticsParameter.Schema.Name, name ?? string.Empty },
					{ AnalyticsParameter.Schema.IsCustom, isCustom }
				};
				doc.Add(code.ToUpper(), pDoc);
				if(null != _AllParameters) {
					_AllParameters.Add(new AnalyticsParameter(code.ToUpper(), pDoc));
				}
				status = true;
			}
			else if(null != doc && true == doc.ContainsKey(code.ToUpper())) {
				var pDoc = (IDictionary<string, object>)doc[code.ToUpper()];
				if(null != pDoc && pDoc.ContainsKey(AnalyticsParameter.Schema.Name)) {
					pDoc[AnalyticsParameter.Schema.Name] = name;
				}
				if(null != pDoc && pDoc.ContainsKey(AnalyticsParameter.Schema.IsCustom)) {
					pDoc[AnalyticsParameter.Schema.IsCustom] = isCustom;
				}
			}
			return status;
		}

		/// <summary>
		/// Removes the parameter.
		/// </summary>
		/// <param name="parameter">Parameter.</param>
		public bool RemoveParameter(AnalyticsParameter parameter) {
			if(null != parameter && !string.IsNullOrEmpty(parameter.Code)) {
				return RemoveParameter(parameter.Code);
			}
			return false;
		}

		/// <summary>
		/// Removes the parameter.
		/// </summary>
		/// <param name="parameterCode">Parameter code.</param>
		public bool RemoveParameter(string parameterCode) {
			bool status = false;
			var doc = _ParameterDoc;
			if(null != doc && !string.IsNullOrEmpty(parameterCode) && doc.ContainsKey(parameterCode.ToUpper())) {
				doc.Remove(parameterCode.ToUpper());
				if(null != _AllParameters) {
					int index = _AllParameters.FindIndex(p => 0 == string.Compare(p.Code, parameterCode, true));
					if(-1 != index) {
						_AllParameters.RemoveAt(index);
					}

					//Remove parameter from events
					foreach(var ev in AllEvents) {
						ev.RemoveParameter(parameterCode);
					}
				}
				status = true;
			}
			return status;
		}

		/// <summary>
		/// Adds the parameter to all events.
		/// </summary>
		/// <param name="parameterCode">Parameter code.</param>
		public void AddParameterToAllEvents(string parameterCode) {
			var doc = _ParameterDoc;
			if(null != doc && !string.IsNullOrEmpty(parameterCode) && doc.ContainsKey(parameterCode.ToUpper())) {
				foreach(var ev in AllEvents) {
					ev.AddParameter(parameterCode);
				}
			}
		}

		#endregion

		#region Events

		/// <summary>
		/// Gets the event document.
		/// </summary>
		/// <value>The _ event document.</value>
		private IDictionary<string, object> _EventsDoc {
			get {
				return Get<IDictionary<string, object>>(Schema.Events);
			}
		}


		public Dictionary<string, AnalyticsEvent> _Events = null;
		/// <summary>
		/// Sets the events.
		/// </summary>
		/// <value>The events.</value>
		public Dictionary<string, AnalyticsEvent> Events {
			get {
				if(null == _Events) {
					_Events = AnalyticsEvent.ConvertToDictionary(_EventsDoc);
				}
				return _Events;
			}
		}

		private List<AnalyticsEvent> _AllEvents = null;
		/// <summary>
		/// Gets all events.
		/// </summary>
		/// <value>All events.</value>
		public List<AnalyticsEvent> AllEvents {
			get {
				if(null == _AllEvents) {
					_AllEvents = AnalyticsEvent.ConvertToList(_EventsDoc);
				}
				return _AllEvents;
			}
		}

		/// <summary>
		/// Adds the event.
		/// </summary>
		/// <returns><c>true</c>, if event was added, <c>false</c> otherwise.</returns>
		/// <param name="eventName">Event name.</param>
		public bool AddEvent(string eventName) {
			bool status = false;
			var doc = _EventsDoc;
			if(null != doc && false == doc.ContainsKey(eventName)) {
				var eDoc = new Dictionary<string, object>() {
					{ AnalyticsEvent.Schema.Parameters, new SimpleJson.JsonArray() },
					{ AnalyticsEvent.Schema.Providers, new SimpleJson.JsonArray() }
				};
				foreach(var provider in Providers) {
					((List<object>)eDoc[AnalyticsEvent.Schema.Providers]).Add(provider);
				}
				var aEvent = new AnalyticsEvent(eventName, eDoc);
				doc.Add(eventName, eDoc);
				if(null != _AllEvents && -1 == _AllEvents.FindIndex(e => e.Name == eventName)) {
					_AllEvents.Add(aEvent);
				}
				if(null != _Events && false == _Events.ContainsKey(eventName)) {
					_Events.Add(eventName, aEvent);
				}
				status = true;
			}
			return status;
		}

		/// <summary>
		/// Removes the event.
		/// </summary>
		/// <returns><c>true</c>, if event was removed, <c>false</c> otherwise.</returns>
		/// <param name="eventname">Eventname.</param>
		public bool RemoveEvent(string eventName) {
			bool status = false;
			var doc = _EventsDoc;
			if(null != doc && true == doc.ContainsKey(eventName)) {
				doc.Remove(eventName);
				if(null != _AllEvents && -1 != _AllEvents.FindIndex(e => e.Name == eventName)) {
					_AllEvents.RemoveAll(ae => ae.Name == eventName);
				}
				if(null != _Events && true == _Events.ContainsKey(eventName)) {
					_Events.Remove(eventName);
				}
				status = true;
			}
			return status;
		}

		#endregion

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="June.Analytics.AnalyticsEditor.AnalyticsConfig"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="June.Analytics.AnalyticsEditor.AnalyticsConfig"/>.</returns>
		public override string ToString () {
			return string.Format ("[AnalyticsConfig: Providers={0}, Parameters={1}, Events:{2}]", 
			                      Providers, 
			                      Parameters, 
			                      (null == Events) ? "<NULL>" : Events.Count.ToString());
		}

		/// <summary>
		/// Clears the cache.
		/// </summary>
		public void Refresh() {
			_Parameters = null;
			_AllParameters = null;
			_Events = null;
			_AllEvents = null;
			_Providers = null;
			_ProviderInstances = null;
			Load ();
		}
		

		#if UNITY_EDITOR
		/// <summary>
		/// Persists the data back to the file.
		/// </summary>
		public void Save () {
			string analyticsPath = Path.Combine(Application.dataPath, "June/Analytics/Editor");
			string eventsConfigPath = Path.Combine(analyticsPath, ResourceName + ".json");

			//Updating parameters
			SimpleJson.JsonArray jArray = null;

			jArray = Get<SimpleJson.JsonArray>(Schema.Providers);
			if(null != jArray) {
				jArray.Clear();
				_ProviderInstances.ForEach(s => jArray.Add(s.ProviderName));
				_Providers = null;
			}

			//Write data
			File.WriteAllText(eventsConfigPath, SimpleJson.SimpleJson.SerializeObject(_Record));
		}
		#endif
	}

	/// <summary>
	/// Analytics event.
	/// </summary>
	public class AnalyticsEvent : BaseModel {

		internal class Schema {
			public const string Providers = "providers";
			public const string Parameters = "params";
			public const string SubscribedMessage = "msg";
		}

		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name { get; private set; }

		private List<string> _Providers;
		/// <summary>
		/// Gets the providers.
		/// </summary>
		/// <value>The providers.</value>
		public List<string> Providers {
			get {
				if(null == _Providers) {
					_Providers = GetStringList(Schema.Providers);
				}
				return _Providers;
			}
		}
		
		private List<string> _Parameters = null;
		/// <summary>
		/// Gets the parameters.
		/// </summary>
		/// <value>The parameters.</value>
		public List<string> Parameters {
			get {
				if(null == _Parameters) {
					_Parameters = GetStringList(Schema.Parameters);
				}
				return _Parameters;
			}
		}

		private List<KeyValuePair<FileInfo, int>> _References;
		/// <summary>
		/// Gets the references.
		/// </summary>
		/// <value>The references.</value>
		public List<KeyValuePair<FileInfo, int>> References {
			get {
				return _References;
			}
			set {
				_References = value;
			}
		}

		/// <summary>
		/// Gets or sets the subscribed message.
		/// </summary>
		/// <value>The subscribed message.</value>
		public string SubscribedMessage {
			get {
				return GetString(Schema.SubscribedMessage);
			}
			set {
				Set (Schema.SubscribedMessage, value);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="June.Analytics.AnalyticsEditor.AnalyticsEvent"/> class.
		/// </summary>
		/// <param name="doc">Document.</param>
		public AnalyticsEvent(string name, IDictionary<string, object> doc) : base(doc) { 
			this.Name = name;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="June.Analytics.AnalyticsEditor.AnalyticsEvent"/> class.
		/// </summary>
		public AnalyticsEvent() : this(string.Empty, null) { }

		/// <summary>
		/// Adds the parameter.
		/// </summary>
		/// <param name="parameterCode">Parameter code.</param>
		public void AddParameter(string parameterCode) {
			var parameters = Get<SimpleJson.JsonArray>(Schema.Parameters);
			if(null != parameters && false == parameters.Contains(parameterCode)) {
				parameters.Add(parameterCode);
				if(null != _Parameters) {
					_Parameters.Add(parameterCode);
				}
			}
		}

		/// <summary>
		/// Removes the parameter.
		/// </summary>
		/// <param name="parameterCode">Parameter code.</param>
		public void RemoveParameter(string parameterCode) {
			var parameters = Get<SimpleJson.JsonArray>(Schema.Parameters);
			if(null != parameters && true == parameters.Contains(parameterCode)) {
				parameters.Remove(parameterCode);
				if(null != _Parameters) {
					_Parameters.Remove(parameterCode);
				}
			}
		}

		/// <summary>
		/// Adds the provider.
		/// </summary>
		/// <param name="providerName">Provider name.</param>
		public void AddProvider(string providerName) {
			var providers = Get<SimpleJson.JsonArray>(Schema.Providers);
			if(null != providers && false == providers.Contains(providerName)) {
				providers.Add(providerName);
				if(null != _Providers) {
					_Providers.Add(providerName);
				}
			}
		}

		/// <summary>
		/// Removes the provider.
		/// </summary>
		/// <param name="providerName">Provider name.</param>
		public void RemoveProvider(string providerName) {
			var providers = Get<SimpleJson.JsonArray>(Schema.Providers);
			if(null != providers && true == providers.Contains(providerName)) {
				providers.Remove(providerName);
				if(null != _Providers) {
					_Providers.Remove(providerName);
				}
			}
		}

		/// <summary>
		/// Updates the references.
		/// </summary>
		public void UpdateReferences() {
			_References = JuneEditorUtils.FindReferences("Events." + Name);
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="June.Analytics.AnalyticsEditor.AnalyticsEvent"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="June.Analytics.AnalyticsEditor.AnalyticsEvent"/>.</returns>
		public override string ToString () {
			return string.Format ("[AnalyticsEvent: Name={0}, Providers={1}, Parameters={2} SubsMsg:{3}]", Name, Providers, Parameters, SubscribedMessage);
		}

		/// <summary>
		/// Converts to objects.
		/// </summary>
		/// <returns>The to objects.</returns>
		/// <param name="doc">Document.</param>
		public static Dictionary<string, AnalyticsEvent> ConvertToDictionary(IDictionary<string, object> doc) {
			return ConvertToObjects<Dictionary<string, AnalyticsEvent>>(doc, (dict, aev) => dict.Add(aev.Name, aev));
		}

		/// <summary>
		/// Converts to list.
		/// </summary>
		/// <returns>The to list.</returns>
		/// <param name="doc">Document.</param>
		public static List<AnalyticsEvent> ConvertToList(IDictionary<string, object> doc) {
			return ConvertToObjects<List<AnalyticsEvent>>(doc, (list, aev) => list.Add(aev));
		}

		/// <summary>
		/// Converts to objects.
		/// </summary>
		/// <returns>The to objects.</returns>
		/// <param name="doc">Document.</param>
		/// <param name="addMethod">Add method.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T ConvertToObjects<T>(IDictionary<string, object> doc, Action<T, AnalyticsEvent> addMethod) where T : class, new() {
			T events = default(T);
			if(null != doc) {
				events = new T();
				foreach(var kv in doc) {
					if(null != kv.Value && kv.Value is IDictionary<string, object>) {
						var aev = new AnalyticsEvent(kv.Key, (IDictionary<string, object>)kv.Value);
						addMethod(events, aev);
					}
				}
			}
			return events;
		}
	}

	/// <summary>
	/// Analytics parameter.
	/// </summary>
	public class AnalyticsParameter : BaseModel {

		internal class Schema {
			public const string Name = "name";
			public const string IsCustom = "ic";
		}

		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name {
			get {
				return GetString(Schema.Name);
			}
			set {
				Set (Schema.Name, value);
			}
		}

		/// <summary>
		/// Gets the code.
		/// </summary>
		/// <value>The code.</value>
		public string Code {
			get; set;
		}

		/// <summary>
		/// Gets a value indicating whether this instance is custom.
		/// </summary>
		/// <value><c>true</c> if this instance is custom; otherwise, <c>false</c>.</value>
		public bool IsCustom {
			get {
				return GetBool(Schema.IsCustom);
			}
			set {
				Set (Schema.IsCustom, value);
			}
		}

		/// <summary>
		/// Gets the display string.
		/// </summary>
		/// <value>The display string.</value>
		public string DisplayStr {
			get {
				return string.Format("{0}{1}: {2} [{3}]", this.IsCustom ? "*" : string.Empty, this.Code, this.Name, this.EventsCount);
			}
		}

		/// <summary>
		/// Gets the events count.
		/// </summary>
		/// <value>The events count.</value>
		public int EventsCount {
			get {
				return AnalyticsConfig.Instance.AllEvents.Count(ev => ev.Parameters.Contains(this.Code));
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="June.Analytics.AnalyticsEditor.AnalyticsParameter"/> class.
		/// </summary>
		/// <param name="code">Code.</param>
		/// <param name="doc">Document.</param>
		public AnalyticsParameter(string code, IDictionary<string, object> doc) : base(doc) {
			this.Code = code.ToUpper();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="June.Analytics.AnalyticsEditor.AnalyticsParameter"/> class.
		/// </summary>
		/// <param name="code">Code.</param>
		/// <param name="name">Name.</param>
		/// <param name="isCustom">If set to <c>true</c> is custom.</param>
		public AnalyticsParameter(string code, string name, bool isCustom) : 
			this(code.ToUpper(), 
			    new Dictionary<string, object>() {
					{ Schema.Name, name },
					{ Schema.IsCustom, isCustom }
				}) { }

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="June.Analytics.AnalyticsEditor.AnalyticsParameter"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="June.Analytics.AnalyticsEditor.AnalyticsParameter"/>.</returns>
		public override string ToString () {
			return string.Format ("[AnalyticsParameter: Name={0}, Code={1}, IsCustom={2}]", Name, Code, IsCustom);
		}

		/// <summary>
		/// Converts to dictionary.
		/// </summary>
		/// <returns>The to dictionary.</returns>
		/// <param name="doc">Document.</param>
		public static Dictionary<string, AnalyticsParameter> ConvertToDictionary(IDictionary<string, object> doc) {
			return ConvertToObjects<Dictionary<string, AnalyticsParameter>>(doc, (dict, p) => dict.Add(p.Code, p));
		}

		/// <summary>
		/// Converts to list.
		/// </summary>
		/// <returns>The to list.</returns>
		/// <param name="doc">Document.</param>
		public static List<AnalyticsParameter> ConvertToList(IDictionary<string, object> doc) {
			return ConvertToObjects<List<AnalyticsParameter>>(doc, (list, p) => list.Add(p));
		}

		/// <summary>
		/// Converts to objects.
		/// </summary>
		/// <returns>The to objects.</returns>
		/// <param name="doc">Document.</param>
		/// <param name="addMethod">Add method.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T ConvertToObjects<T>(IDictionary<string, object> doc, Action<T, AnalyticsParameter> addMethod) where T : class, new() {
			T parameters = default(T);
			if(null != doc) {
				parameters = new T();
				foreach(var kv in doc) {
					if(null != kv.Value && kv.Value is IDictionary<string, object>) {
						var p = new AnalyticsParameter(kv.Key, (IDictionary<string, object>)kv.Value);
						addMethod(parameters, p);
					}
				}
			}
			return parameters;
		}
	}
}