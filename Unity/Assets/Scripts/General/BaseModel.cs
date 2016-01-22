using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

	public class BaseModel
	{
		
		protected IDictionary<string, object> _Record;
		
		/// <summary>
		/// Gets or sets the <see cref="BaseModel"/> with the specified key.
		/// </summary>
		/// <param name="key">Key.</param>
		public object this[string key] {
			get {
				return _Record[key];
			}
			set {
				_Record[key] = value;
			}
		}
		
		/// <summary>
		/// Gets the identifier.
		/// </summary>
		/// <value>
		/// The identifier.
		/// </value>
		public virtual string Id {
			get {
				return GetString("id");
			}
		}
		/// <summary>
		/// Gets the created time stamp.
		/// </summary>
		/// <value>
		/// The created time stamp.
		/// </value>
		public virtual DateTime CreatedOn {
			get {
				return Util.FromUnixTimestamp(CreatedOnTimeStamp);
			}
		}
		
		/// <summary>
		/// Gets the created on time stamp.
		/// </summary>
		/// <value>The created on time stamp.</value>
		public virtual long CreatedOnTimeStamp {
			get {
				return GetLong("cts");
			}
		}

		/// <summary>
		/// Gets the last modified time stamp.
		/// </summary>
		/// <value>
		/// The last modified time stamp.
		/// </value>
		public virtual DateTime LastModified {
			get {
				return Util.FromUnixTimestamp(LastModifiedTimeStamp);
			}
			protected set {
				LastModifiedTimeStamp = Util.ToUnixTimestamp(value);
			}
		}
		
		/// <summary>
		/// Gets or sets the last modified time stamp.
		/// </summary>
		/// <value>
		/// The last modified time stamp.
		/// </value>
		public virtual long LastModifiedTimeStamp {
			get {
				return GetLong("lts");
			}
			protected set {
				Set("lts", value);
			}
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="BaseModel"/> class.
		/// </summary>
		/// <param name="doc">The document.</param>
		public BaseModel(IDictionary<string, object> doc) {
			UpdateDoc(doc);
		}
		
		/// <summary>
		/// Updates the document.
		/// </summary>
		/// <param name="updatedDoc">Updated document.</param>
		public virtual void UpdateDoc(IDictionary<string, object> updatedDoc) {
			this._Record = updatedDoc;
		}
		
		/// <summary>
		/// Gets the int.
		/// </summary>
		/// <returns>The int.</returns>
		/// <param name="key">Key.</param>
		public int GetInt(string key) {
			int result = 0;
			if(_Record.ContainsKey(key)) {
				result = int.Parse(_Record[key].ToString());
			}
			return result;
		}
		
		/// <summary>
		/// Gets the long.
		/// </summary>
		/// <returns>The long.</returns>
		/// <param name="key">Key.</param>
		public long GetLong(string key) {
			long result = 0;
			if(_Record.ContainsKey(key)) {
				result = long.Parse(_Record[key].ToString());
			}
			return result;
		}
		
		/// <summary>
		/// Gets the float.
		/// </summary>
		/// <returns>The float.</returns>
		/// <param name="key">Key.</param>
		public float GetFloat(string key) {
			float result = 0.0f;
			if(_Record.ContainsKey(key)) {
				result = float.Parse(_Record[key].ToString());
			}
			return result;
		}
		
		/// <summary>
		/// Gets the bool.
		/// </summary>
		/// <returns><c>true</c>, if bool was gotten, <c>false</c> otherwise.</returns>
		/// <param name="key">Key.</param>
		public bool GetBool(string key) {
			bool result = false;
			if(_Record.ContainsKey(key)) {
				result = (bool)_Record[key];
			}
			return result;
		}
		
		/// <summary>
		/// Gets the string.
		/// </summary>
		/// <returns>The string.</returns>
		/// <param name="key">Key.</param>
		public string GetString(string key) {
			return Get<string>(key);
		}
		
		/// <summary>
		/// Gets as string.
		/// </summary>
		/// <returns>The as string.</returns>
		/// <param name="key">Key.</param>
		public string GetAsString(string key) {
			string value = string.Empty;
			if(_Record.ContainsKey(key)) {
				value = _Record[key].ToString();
			}
			return value;
		}
		
		/// <summary>
		/// Gets the string list.
		/// </summary>
		/// <returns>The string list.</returns>
		/// <param name="key">Key.</param>
		public List<string> GetStringList(string key) {
			List<object> objects = Get<List<object>> (key);
			List<string> strings = new List<string>();
			
			if(null != objects) {
				foreach(object o in objects) {
					if(null != o && o is String) {
						strings.Add((string)o);
					}
				}
			}
			
			return strings;
		}
		
		/// <summary>
		/// Get the specified key.
		/// </summary>
		/// <param name="key">Key.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public T Get<T>(string key) {
			T result = default(T);
			if(_Record.ContainsKey(key) && _Record[key] is T) {
				result = (T)_Record[key];
			}
			return result;
		}
		
		/// <summary>
		/// Gets the enum.
		/// </summary>
		/// <returns>The enum.</returns>
		/// <param name="key">Key.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public T GetEnum<T>(string key) {
			return (T)Enum.Parse(typeof(T), GetString(key));
		}
		
		/// <summary>
		/// Set the specified key and value.
		/// </summary>
		/// <param name="key">Key.</param>
		/// <param name="value">Value.</param>
		public void Set(string key, object value) {
			if(null != _Record) {
				if(_Record.ContainsKey(key)) {
					_Record[key] = value;
				}
				else {
					_Record.Add(key, value);
				}
			}
		}

		public virtual void Save(string key)
		{
			string resultStr = SimpleJson.SimpleJson.SerializeObject(_Record);
			PlayerPrefs.SetString(key,resultStr);
		}
		
		public virtual string ToJson()
		{
			return SimpleJson.SimpleJson.SerializeObject(_Record);
		}
	}
