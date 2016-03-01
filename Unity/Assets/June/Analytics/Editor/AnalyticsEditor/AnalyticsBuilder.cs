using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace June.Analytics.AnalyticsEditor {

	/// <summary>
	/// Analytics builder.
	/// </summary>
	public class AnalyticsBuilder {

		/// <summary>
		/// Gets the june path.
		/// </summary>
		/// <value>The june path.</value>
		public static string JunePath {
			get {
				return Path.Combine(Application.dataPath, "June");
			}
		}

		/// <summary>
		/// Gets the editor june path.
		/// </summary>
		/// <value>The editor june path.</value>
		public static string EditorJunePath {
			get {
				return Path.Combine(Application.dataPath, "Editor/June");
			}
		}

		/// <summary>
		/// Gets the analytics path.
		/// </summary>
		/// <value>The analytics path.</value>
		public static string AnalyticsPath {
			get {
				return Path.Combine(JunePath, "Analytics");
			}
		}

		/// <summary>
		/// Gets the editor analytics path.
		/// </summary>
		/// <value>The editor analytics path.</value>
		public static string EditorAnalyticsPath {
			get {
				return Path.Combine(AnalyticsPath, "Editor");
			}
		}
		
		/// <summary>
		/// Gets the auto gen path.
		/// </summary>
		/// <value>The auto gen path.</value>
		public static string AutoGenPath {
			get {
				return Path.Combine(AnalyticsPath, "autogen");
			}
		}

		/// <summary>
		/// Gets the editor analytics provider path.
		/// </summary>
		/// <value>The editor analytics provider path.</value>
		public static string EditorAnalyticsProviderPath {
			get {
				return Path.Combine(EditorAnalyticsPath, "Providers");
			}
		}

		/// <summary>
		/// Gets the plugin path.
		/// </summary>
		/// <value>The plugin path.</value>
		public static string PluginPath {
			get {
				return Path.Combine(EditorAnalyticsPath, "_Plugins");
			}
		}

		/// <summary>
		/// Build this instance.
		/// </summary>
		public static void Build() {
			CheckAndCreateDirectory();
			CopyProviders();
			PopulateAndCopyAnalyticsManager();
			UnityEditor.AssetDatabase.Refresh();
		}

		/// <summary>
		/// Checks the and create directory.
		/// </summary>
		private static void CheckAndCreateDirectory() {

			if(false == Directory.Exists(JunePath)) {
				Directory.CreateDirectory(JunePath);
			}

			if(false == Directory.Exists(AnalyticsPath)) {
				Directory.CreateDirectory(AnalyticsPath);
			}

			if(false == Directory.Exists(AutoGenPath)) {
				Directory.CreateDirectory(AutoGenPath);
			}
		}

		/// <summary>
		/// Copies the providers.
		/// </summary>
		private static void CopyProviders() {
			string iAnalyticsProvider = "IAnalyticsProvider.cs";
			string filePath = Path.Combine(EditorAnalyticsProviderPath, iAnalyticsProvider);
			if(File.Exists(filePath)) {
				string destFilePath = Path.Combine(AutoGenPath, iAnalyticsProvider);
				File.Copy(filePath, destFilePath, true);
			}

			string providerTypes = "ProviderTypes.cs";
			filePath = Path.Combine(EditorAnalyticsProviderPath, providerTypes);
			if(File.Exists(filePath)) {
				string destFilePath = Path.Combine(AutoGenPath, providerTypes);
				File.Copy(filePath, destFilePath, true);
			}

			const string initTemplateTag = "__PROVIDER_NAME__";
			string initTemplate = File.ReadAllText(Path.Combine(EditorAnalyticsProviderPath, "_InitTemplateProvider.cs"));

			foreach(var provider in AnalyticsConfig.Instance.Providers) {
				string fileName = provider + "Provider.cs";
				string sourcePath = Path.Combine(EditorAnalyticsProviderPath, fileName);
				if(File.Exists(sourcePath)) {
					string destPath = Path.Combine(AutoGenPath, fileName);
					File.Copy(sourcePath, destPath, true);
				}

				string userFile = Path.Combine(AnalyticsPath, fileName);
				if(false == File.Exists(userFile)) {
					File.WriteAllText(userFile, initTemplate.Replace(initTemplateTag, provider + "Provider"));
				}
			}
		}

		/// <summary>
		/// Populates the and copy analytics manager.
		/// </summary>
		private static void PopulateAndCopyAnalyticsManager() {
			string aMgrPath = Path.Combine(AutoGenPath, "AnalyticsManager.cs");
			string analyticsManager = PopulateAnalyticsManager();
			if(false == string.IsNullOrEmpty(analyticsManager)) {
				File.WriteAllText(aMgrPath, analyticsManager);
			}

			string userFile = Path.Combine(AnalyticsPath, "AnalyticsManager.cs");
			if(false == File.Exists(userFile)) {
				string sourceFile = Path.Combine(EditorAnalyticsPath, "AnalyticsManager_template.cs");
				File.Copy(sourceFile, userFile, true);
			}
		}

		/// <summary>
		/// Populates the analytics manager.
		/// </summary>
		private static string PopulateAnalyticsManager() {
			StringBuilder template = null;
			string templateMgrPath = Path.Combine(EditorAnalyticsPath, "AnalyticsManager.cs");
			if(File.Exists(templateMgrPath)) {
				template = new StringBuilder(File.ReadAllText(templateMgrPath));
				PopulateAnalyticsManagerProviders(template);
				PopulateAnalyticsManagerParameters(template);
				PopulateAnalyticsManagerEvents(template);
			}
			return null == template ? null : template.ToString();
		}

		/// <summary>
		/// Populates the analytics manager providers.
		/// </summary>
		/// <param name="template">Template.</param>
		private static void PopulateAnalyticsManagerProviders(StringBuilder template) {
			const string providerInstancesTag = "//<<PROVIDER_INSTANCES>>";
			const string providerInstanceFormat = "\t\t\t\t\t\t{{ ProviderTypes.{0}, new {0}Provider() }}";

			List<string> providerInstances = new List<string>();
			foreach(var prov in AnalyticsConfig.Instance.Providers) {
				providerInstances.Add(string.Format(providerInstanceFormat, prov));
			}

			template.Replace(providerInstancesTag, 
			                 string.Join("," + System.Environment.NewLine, providerInstances.ToArray()));
		}

		/// <summary>
		/// Populates the analytics manager parameters.
		/// </summary>
		/// <param name="template">Template.</param>
		private static void PopulateAnalyticsManagerParameters(StringBuilder template) {
			const string parameterTag = "//<<PARAMETERS>>";
			string parameterFormat = "\t\t\tpublic const string {0} = \"{0}\";";

			StringBuilder parameters = new StringBuilder("\t\tpublic class Parameters {");
			parameters.AppendLine();
			foreach(var para in AnalyticsConfig.Instance.AllParameters) {
				parameters.AppendFormat(parameterFormat, para.Code);
				parameters.AppendLine();
			}
			parameters.AppendLine("\t\t}");

			template.Replace(parameterTag, parameters.ToString());
		}

		/// <summary>
		/// Populates the analytics manager events.
		/// </summary>
		/// <param name="template">Template.</param>
		private static void PopulateAnalyticsManagerEvents(StringBuilder template) {
			const string eventsTag = "//<<EVENTS>>";
			const string eventFormat = "\t\t\tpublic static readonly string {0} = \"{0}\";";
			const string eventParamFormat = "\t\t\t{{ Events.{0}, new Event() {{ Name=Events.{0}, Parameters=new string[] {{ \"{1}\" }}, Providers=new string[] {{ \"{2}\" }}, SubscribedMessage=\"{3}\" }} }}";

			List<string> eventsList = new List<string>();

			StringBuilder events = new StringBuilder("\t\tpublic class Events {");
			events.AppendLine();
			foreach(var ev in AnalyticsConfig.Instance.AllEvents) {
				events.AppendFormat(eventFormat, ev.Name);
				events.AppendLine();
				eventsList.Add(
					string.Format(eventParamFormat, 
				              ev.Name, 
				              string.Join("\",\"", ev.Parameters.ToArray()),
				              string.Join("\",\"", ev.Providers.ToArray()),
				              ev.SubscribedMessage));
			}
			events.AppendLine("\t\t}");

			events.AppendLine();

			events.AppendLine("\t\tpublic static Dictionary<string, Event> EVENTS = new Dictionary<string, Event>() {");
			events.Append(string.Join("," + System.Environment.NewLine, eventsList.ToArray()));
			events.AppendLine();
			events.AppendLine("\t\t};");
			

			template.Replace(eventsTag, events.ToString());
		}

		/// <summary>
		/// Generates the parameter switch cases.
		/// </summary>
		/// <returns>The parameter switch cases.</returns>
		public static string GenerateParameterSwitchCases() {
			StringBuilder parameters = new StringBuilder("switch(parameterName) {");
			parameters.AppendLine();
			foreach(var para in AnalyticsConfig.Instance.AllParameters) {
				parameters.AppendFormat("\tcase Parameters.{0}:", para.Code);
				parameters.AppendLine();
				parameters.AppendLine("\t\tbreak;");
			}
			parameters.AppendLine("\tdefault:");
			parameters.AppendLine("\t\tbreak;");
			parameters.AppendLine("}");
			return parameters.ToString();
		}
	}
}