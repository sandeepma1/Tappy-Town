using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System;
using System.Collections;
using System.Collections.Generic;
using June.Analytics.Providers;

namespace June.Analytics.AnalyticsEditor {
	
	/// <summary>
	/// Analytics window.
	/// </summary>
	public class AnalyticsWindow : EditorWindow {
		
		public static bool IsDirty { get; private set; }
		
		public const int INDENT_OFFSET = 20;
		
		public Vector2 _ScrollPosition;
		
		private bool _ProviderFoldout;
		private ReorderableList _ProviderList;
		private IAnalyticsProvider _SelectedProvider;
		private string _ProviderType = null;
		
		private bool _ParameterFoldout;
		private ReorderableList _ParameterList;
		private string _NewParameterCode;
		private string _NewParameterName;
		private bool _NewParameterIsCustom;
		private int _NewParameterEventsCount;
		private bool _IsAddNewParameter = false;
		private bool _ShowParameterDetails = false;
		
		private bool _EventsFoldout;
		private ReorderableList _EventList;
		private AnalyticsEvent _SelectedEvent;
		private string _NewEventName = string.Empty;
		private List<string> _NewEventParameters;
		private List<String> _newEventProviders;
		private ReorderableList _EventParameterList;
		private ReorderableList _EventProviderList;
		private bool _IsAddNewEvent = false;
		private string _SelectedEventMessage = string.Empty;
		
		public void OnEnable() {
			Initialize();
			Dispatcher.Initialize();
		}
		
		/// <summary>
		/// Sets as dirty.
		/// </summary>
		private void SetAsDirty(bool isDirty = true) {
			AnalyticsWindow.IsDirty = isDirty;
		}
		
		private void Initialize() {
			InitializeProviderList();
			InitializeParameterList();
			InitializeEventList();
		}
		
		/// <summary>
		/// Initializes the provider list.
		/// </summary>
		private void InitializeProviderList() {
			_ProviderList = new ReorderableList(
				AnalyticsConfig.Instance.ProviderInstances , //elements
				typeof(June.Analytics.Providers.IAnalyticsProvider),//elementType
				false,								//draggable
				true,								//displayHeader
				true,								//displayAddButton
				true);								//displayRemoveButton
			
			_ProviderList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
				var analyticsProvider = (IAnalyticsProvider)_ProviderList.list[index];
				EditorGUI.LabelField(rect, analyticsProvider.ProviderName);
			};
			
			_ProviderList.drawHeaderCallback = (Rect rect) => {
				EditorGUI.LabelField(rect, "Analytics Providers");
			};
			
			_ProviderList.onAddCallback = (ReorderableList list) => {
				Debug.Log("[onAddCallback] " + _ProviderType);
				if(!string.IsNullOrEmpty(_ProviderType) 
				   && Providers.ProviderTypes.INSTANCES.ContainsKey(_ProviderType)
				   && false == AnalyticsConfig.Instance.ProviderInstances.Contains(Providers.ProviderTypes.INSTANCES[_ProviderType])) {
					list.list.Add(Providers.ProviderTypes.INSTANCES[_ProviderType]);
				}
			};
			
			_ProviderList.onAddDropdownCallback = (Rect rect, ReorderableList list) => {
				var menu = new GenericMenu();				;
				foreach(var item in Enum.GetNames(typeof(ProviderTypeEnum))) {
					menu.AddItem(new GUIContent(item), false, (object obj) => { 
						string str = obj.ToString();
						_ProviderType = obj.ToString(); 
						var provider = Providers.ProviderTypes.INSTANCES[str];
						if(false == AnalyticsConfig.Instance.ProviderInstances.Contains(provider)) {
							AnalyticsConfig.Instance.ProviderInstances.Add(provider);
							SetAsDirty();
						}
					}, item);
				}
				menu.ShowAsContext();
			};
			
			_ProviderList.onRemoveCallback = (ReorderableList list) => {
				var provider = (IAnalyticsProvider)list.list[list.index];
				if(EditorUtility.DisplayDialog(
					title: "Remove Provider",
					message: string.Format("Are you sure you want to remove {0} provider, it has {1} events associated with it.", provider.ProviderName, provider.EventsCount),
					ok: "Remove", 
					cancel: "Cancel")) {
					AnalyticsConfig.Instance.ProviderInstances.Remove(provider);
					_SelectedProvider = null;
					SetAsDirty();
				}
			};
			
			_ProviderList.onSelectCallback = (ReorderableList list) => {
				_SelectedProvider = (IAnalyticsProvider)list.list[list.index];
			};
		}
		
		private void InitializeParameterList() {
			_ParameterList = new ReorderableList(
				AnalyticsConfig.Instance.AllParameters,
				typeof(AnalyticsParameter),
				false,
				true,
				true,
				true);
			
			_ParameterList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
				var parameter = (AnalyticsParameter)_ParameterList.list[index];
				if(null != parameter) {
					EditorGUI.LabelField(rect, parameter.DisplayStr);
				}
			};
			
			_ParameterList.drawHeaderCallback = (Rect rect) => {
				EditorGUI.LabelField(rect, "Parameters");
			};
			
			_ParameterList.onAddCallback = (ReorderableList list) => {
				_ShowParameterDetails = true;
				_NewParameterCode = string.Empty;
				_NewParameterName = string.Empty;
				_NewParameterIsCustom = false;
				_NewParameterEventsCount = 0;
				_IsAddNewParameter = true;
			};
			
			_ParameterList.onRemoveCallback = (ReorderableList list) => {
				var selectedParam = (AnalyticsParameter)list.list[list.index];
				if(EditorUtility.DisplayDialog(
					title: "Remove Parameter",
					message: string.Format("Are you sure you want to remove {0} parameter, it has {1} events associated with it.", selectedParam.Code, selectedParam.EventsCount),
					ok: "Remove", 
					cancel: "Cancel")) {
					list.list.RemoveAt(list.index);
					AnalyticsConfig.Instance.RemoveParameter(selectedParam.Code);
					_NewParameterCode = null;
					_NewParameterEventsCount = 0;
					_ShowParameterDetails = false;
					SetAsDirty();
				}
			};
			
			_ParameterList.onSelectCallback = (ReorderableList list) => {
				var selectedParam = (AnalyticsParameter)list.list[list.index];
				_NewParameterCode = selectedParam.Code;
				_NewParameterName = selectedParam.Name;
				_NewParameterIsCustom = selectedParam.IsCustom;
				_NewParameterEventsCount = selectedParam.EventsCount;
				_ShowParameterDetails = true;
				_IsAddNewParameter = false;
			};
		}
		
		private void InitializeEventList() {
			_EventList = new ReorderableList(
				AnalyticsConfig.Instance.AllEvents,
				typeof(AnalyticsEvent),
				false,
				true,
				true,
				true);
			
			_EventList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
				var ev = (AnalyticsEvent)_EventList.list[index];
				EditorGUI.LabelField(rect, ev.Name);
			};
			
			_EventList.drawHeaderCallback = (Rect rect) => {
				EditorGUI.LabelField(rect, "Events");
			};
			
			_EventList.onAddCallback = (ReorderableList list) => {
				_IsAddNewEvent = true;
				_NewEventName = string.Empty;
			};
			
			_EventList.onRemoveCallback = (ReorderableList list) => {
				var selectedEvent = (AnalyticsEvent)list.list[list.index];
				AnalyticsConfig.Instance.RemoveEvent(selectedEvent.Name);
				SetAsDirty();
			};
			
			_EventList.onSelectCallback = (ReorderableList list) => {
				_SelectedEvent = (AnalyticsEvent)list.list[list.index];
				_SelectedEventMessage = _SelectedEvent.SubscribedMessage;
				_IsAddNewEvent = false;
				InitializeEventDetailsList();
			};
		}
		
		private void InitializeEventDetailsList() {
			_EventProviderList = new ReorderableList(
				_SelectedEvent.Providers,
				typeof(string),
				false,
				false,
				true,
				true);
			
			_EventProviderList.onAddDropdownCallback = (Rect rect, ReorderableList list) => {
				var menu = new GenericMenu();				;
				foreach(var item in AnalyticsConfig.Instance.Providers) {
					menu.AddItem(new GUIContent(item), false, (object obj) => { 
						string provider = obj.ToString();
						if(null != _SelectedEvent && false == _SelectedEvent.Providers.Contains(provider)) {
							_SelectedEvent.AddProvider(provider);
							SetAsDirty();
						}
					}, item);
				}
				menu.ShowAsContext();
			};
			
			_EventProviderList.onRemoveCallback = (ReorderableList list) => {
				var providerName = (string)list.list[list.index];
				_SelectedEvent.RemoveProvider(providerName);
				SetAsDirty();
			};
			
			_EventParameterList = new ReorderableList(
				_SelectedEvent.Parameters,
				typeof(string),
				false,
				false,
				true,
				true);
			
			_EventParameterList.onAddDropdownCallback = (Rect rect, ReorderableList list) => {
				var menu = new GenericMenu();				;
				foreach(var item in AnalyticsConfig.Instance.AllParameters) {
					menu.AddItem(new GUIContent(item.DisplayStr), false, (object obj) => { 
						AnalyticsParameter parameter = (AnalyticsParameter)obj;
						if(null != _SelectedEvent && false == _SelectedEvent.Parameters.Contains(parameter.Code)) {
							_SelectedEvent.AddParameter(parameter.Code);
							SetAsDirty();
						}
					}, item);
				}
				menu.ShowAsContext();
			};
			
			_EventParameterList.onRemoveCallback = (ReorderableList list) => {
				var parameterCode = (string)list.list[list.index];
				_SelectedEvent.RemoveParameter(parameterCode);
				SetAsDirty();
			};
		}
		
		/// <summary>
		/// Shows the analytics editor window.
		/// </summary>
		[MenuItem("Build/Analytics")]
		public static void ShowAnalyticsEditorWindow() {
			GetWindow<AnalyticsWindow>("AnalyticsWindow");
		}
		
		/// <summary>
		/// Raises the GU event.
		/// </summary>
		private void OnGUI() {
			GUILayout.BeginHorizontal (EditorStyles.toolbar);
			GUILayout.Label("Analytics Editor");
			if(GUILayout.Button("Build", EditorStyles.toolbarButton)) {
				AnalyticsBuilder.Build();
			}
			if(GUILayout.Button("Params", EditorStyles.toolbarButton)) {
				Debug.Log(AnalyticsBuilder.GenerateParameterSwitchCases());
			}
			if (GUILayout.Button ("Refresh", EditorStyles.toolbarButton)) {
				_ProviderList = null;
				_ParameterList = null;
				_ShowParameterDetails = false;
				_EventList = null;
				_SelectedEvent = null;
				_EventProviderList = null;
				_EventParameterList = null;
				AnalyticsConfig.Instance.Refresh();
				Initialize();
			}
			GUILayout.EndHorizontal();
			
			_ScrollPosition = EditorGUILayout.BeginScrollView(_ScrollPosition); {
				if(IsDirty) {
					EditorGUILayout.HelpBox("Modifications have not been saved.", MessageType.Warning);
					if(GUI.Button(EditorGUILayout.GetControlRect(true, 30), "Save Changes")) {
						//TODO: Persist changes
						AnalyticsConfig.Instance.Save();
						SetAsDirty(isDirty: false);
					}
					EditorGUILayout.Separator();
				}
				
				_ProviderFoldout = EditorGUILayout.Foldout(_ProviderFoldout, string.Format("Providers ({0})", AnalyticsConfig.Instance.ProviderInstances.Count.ToString()));
				if(_ProviderFoldout) {
					Rect rect = EditorGUILayout.GetControlRect(true, CalculateBetterListHeight(_ProviderList.count));
					rect.height = Mathf.Max(rect.height, 45);
					rect.x += INDENT_OFFSET;
					rect.width -= INDENT_OFFSET;
					_ProviderList.DoList(rect);
					
					if(null != _SelectedProvider) {
						EditorGUI.indentLevel += 1;
						EditorGUILayout.LabelField("Name", _SelectedProvider.ProviderName);
						EditorGUILayout.LabelField("Events Count", _SelectedProvider.EventsCount.ToString());
						if(GUILayout.Button("Install Plugin")) {
							_SelectedProvider.InstallPlugin();
						}
						EditorGUI.indentLevel -= 1;
					}
				}
				
				EditorGUILayout.Separator();
				
				_ParameterFoldout = EditorGUILayout.Foldout(_ParameterFoldout, string.Format("Parameters ({0})", AnalyticsConfig.Instance.Parameters.Count));
				if(_ParameterFoldout) {
					Rect rect = EditorGUILayout.GetControlRect(true, CalculateBetterListHeight(_ParameterList.count));
					rect.x += INDENT_OFFSET;
					rect.width -= INDENT_OFFSET;
					_ParameterList.DoList(rect);
					
					if(_IsAddNewParameter) {
						if(null == _NewParameterCode) {
							_NewParameterCode = string.Empty;
							_NewParameterName = string.Empty;
							_NewParameterIsCustom = false;
						}
					}
					if(_ShowParameterDetails) {
						EditorGUI.indentLevel += 1;
						EditorGUILayout.LabelField("Parameter Details", EditorStyles.boldLabel);
						if(_IsAddNewParameter) {
							_NewParameterCode = EditorGUILayout.TextField("Code", _NewParameterCode);
						}
						else {
							EditorGUILayout.LabelField("Code", _NewParameterCode);
						}
						_NewParameterName = EditorGUILayout.TextField("Name", _NewParameterName);
						_NewParameterIsCustom = EditorGUILayout.Toggle("IsCustom", _NewParameterIsCustom);
						EditorGUILayout.LabelField("Events Count", _NewParameterEventsCount.ToString());
						
						var buttonRect = EditorGUILayout.GetControlRect();
						buttonRect.x += INDENT_OFFSET;
						buttonRect.width -= INDENT_OFFSET;
						buttonRect.width /= 3;
						
						if(GUI.Button(buttonRect, "Cancel")) {
							_IsAddNewParameter = false;
							_ShowParameterDetails = false;
							_NewParameterCode = null;
						}
						
						buttonRect.x += buttonRect.width;

						if(GUI.Button(buttonRect, "Add 2 All")) {
							AnalyticsConfig.Instance.AddParameterToAllEvents(_NewParameterCode);
							SetAsDirty();
						}

						buttonRect.x += buttonRect.width;
						
						if(GUI.Button(buttonRect, _IsAddNewParameter ? "Add" : "Update")) {
							AnalyticsConfig.Instance.AddParameter(_NewParameterCode, _NewParameterName, _NewParameterIsCustom);
							_IsAddNewParameter = false;
							_ShowParameterDetails = false;
							SetAsDirty();
						}
						
						EditorGUI.indentLevel -= 1;
					}
				}
				
				EditorGUILayout.Separator();
				
				_EventsFoldout = EditorGUILayout.Foldout(_EventsFoldout, string.Format("Events ({0})", AnalyticsConfig.Instance.AllEvents.Count));
				if(_EventsFoldout) {
					Rect rect = EditorGUILayout.GetControlRect(true, CalculateBetterListHeight(_EventList.count));
					rect.x += INDENT_OFFSET;
					rect.width -= INDENT_OFFSET;
					_EventList.DoList(rect);
					
					if(null != _SelectedEvent || _IsAddNewEvent) {
						EditorGUI.indentLevel += 1;
						EditorGUILayout.LabelField("Event Details", EditorStyles.whiteLargeLabel);
						
						if(_IsAddNewEvent) {
							_NewEventName = EditorGUILayout.TextField("Event Name", _NewEventName);
							
							var buttonRect = EditorGUILayout.GetControlRect();
							buttonRect.x += INDENT_OFFSET;
							buttonRect.width -= INDENT_OFFSET;
							buttonRect.width /= 2;
							
							if(GUI.Button(buttonRect, "Cancel")) {
								_IsAddNewEvent = false;
								_SelectedEvent = null;
							}
							
							buttonRect.x += buttonRect.width;
							
							if(GUI.Button(buttonRect, "Add")) {
								AnalyticsConfig.Instance.AddEvent(_NewEventName);
								_IsAddNewEvent = false;
								_SelectedEvent = null;
								SetAsDirty();
							}
						}
						else {
							EditorGUILayout.LabelField("Name", _SelectedEvent.Name);
							
							EditorGUILayout.LabelField("Providers", EditorStyles.boldLabel);
							Rect editRect = EditorGUILayout.GetControlRect(true, CalculateBetterListHeight(_SelectedEvent.Providers.Count));
							editRect.x += INDENT_OFFSET;
							editRect.width -= INDENT_OFFSET;
							_EventProviderList.DoList(editRect);

							EditorGUILayout.BeginHorizontal(); {
								EditorGUILayout.LabelField("Parameters", EditorStyles.boldLabel);
								if(GUILayout.Button("Add all parameters", EditorStyles.toolbarButton)) {
									foreach(var p in AnalyticsConfig.Instance.AllParameters) {
										if(null != _SelectedEvent && null != p && !string.IsNullOrEmpty(p.Code)) {
											_SelectedEvent.AddParameter(p.Code);
										}
									}
									SetAsDirty(true);
								}
							} EditorGUILayout.EndHorizontal();
							editRect = EditorGUILayout.GetControlRect(true, CalculateBetterListHeight(_SelectedEvent.Parameters.Count));
							editRect.x += INDENT_OFFSET;
							editRect.width -= INDENT_OFFSET;
							_EventParameterList.DoList(editRect);
							EditorGUILayout.Separator();
							_SelectedEvent.SubscribedMessage = EditorGUILayout.TextField("Subscriber Messager", _SelectedEvent.SubscribedMessage);
							if(_SelectedEventMessage != _SelectedEvent.SubscribedMessage) {
								SetAsDirty(true);
								_SelectedEventMessage = _SelectedEvent.SubscribedMessage;
							}
							EditorGUILayout.Separator();
							EditorGUILayout.BeginHorizontal();
							EditorGUILayout.LabelField("References", EditorStyles.boldLabel);
							if(GUILayout.Button("Refresh", EditorStyles.toolbarButton)) {
								_SelectedEvent.UpdateReferences();
							}
							EditorGUILayout.EndHorizontal();
							var references = _SelectedEvent.References;
							if(null != references) {
								foreach(var refer in references) {
									GUIContent content = new GUIContent(string.Format("{0}:{1}", refer.Key.Name, refer.Value),
									                                    string.Format("{0}:{1}", refer.Key.FullName, refer.Value));
									EditorGUILayout.LabelField(content);
									//EditorGUIUtility.PingObject(Resources.Load(refer.Key.FullName));
								}
							}
						}
						EditorGUI.indentLevel -= 1;
					}
				}
			} EditorGUILayout.EndScrollView();
		}

		private static float[] _MinListHeight = { 60f, 60f, 80f };
		private static float[] _ListHeightFactor = { 25f, 25f, 25f, 24f, 24f, 23f };
		public static float CalculateBetterListHeight(int itemCount) {
			return Mathf.Max(
				Mathf.Max(itemCount, 1) * _ListHeightFactor[Mathf.Min(5, itemCount)] + 15f,
				_MinListHeight[Mathf.Min(2, itemCount)]);
		}
	}
}