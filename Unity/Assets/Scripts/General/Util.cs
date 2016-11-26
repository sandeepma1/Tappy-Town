using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using System.Text;
using System.Security.Cryptography;
using System.Collections;
using System.Threading;

namespace June {
	public static partial class Util 
	{
		//private readonly static Func<string, string> JSON_ENCODER_DECODER = ROT47Str;
		private static int _RequestNumber = 0;
		
		public static int GetRequestNumber() {
			return Interlocked.Increment(ref _RequestNumber);
		}

		//private static bool _IsInternetReachable = true;
		/// <summary>
		/// Gets a value indicating is internet reachable.
		/// </summary>
		/// <value><c>true</c> if is internet reachable; otherwise, <c>false</c>.</value>
		/*public static bool IsInternetReachable {
			get {
				return _IsInternetReachable;
			}
			set {
				_IsInternetReachable = value;
			}
		}

		/// <summary>
		/// Shuffle the specified list.
		/// </summary>
		/// <param name='list'>
		/// List.
		/// </param>
		/// <typeparam name='T'>
		/// The 1st type parameter.
		/// </typeparam>
		public static List<T> Shuffle<T> (List<T> list)
		{
			System.Random rng = new System.Random (DateTime.Now.Millisecond);
			List<T> shuffleList = new List<T> (list);
			int n = list.Count;
			while (n > 1) {
				n--;
				int k = rng.Next (n + 1);
				T value = shuffleList [k];
				shuffleList [k] = shuffleList [n];
				shuffleList [n] = value;  
			}
			return shuffleList;
		}
		*/
		/// <summary>
		/// Gets the current UTC timestamp.
		/// </summary>
		/// <value>
		/// The current UTC timestamp.
		/// </value>
		/*public static int CurrentUTCTimestamp {
			get {
				return (int)ToUnixTimestamp (DateTime.UtcNow);
			}
		}*/
		
		/*
		/// <summary>
		/// Convert from the unix timestamp.
		/// </summary>
		/// <returns>
		/// The date time object.
		/// </returns>
		/// <param name='timestamp'>
		/// Timestamp.
		/// </param>
		public static DateTime FromUnixTimestamp (long timestamp)
		{
			DateTime unixRef = new DateTime (1970, 1, 1, 0, 0, 0);
			return unixRef.AddSeconds (timestamp);
		}*/
		/// <summary>
		/// Gets the current UTC time.
		/// </summary>
		/// <value>The current UTC time.</value>
		public static int CurrentUTCTime{
			get{
				
				return (int)ToUnixTimestamp(System.DateTime.UtcNow);
			}
		}
		
	/*	/// <summary>
		/// Convert to unix timestamp.
		/// </summary>
		/// <returns>
		/// The unix timestamp.
		/// </returns>
		/// <param name='dt'>
		/// Date time object
		/// </param>
		public static long ToUnixTimestamp (System.DateTime dt)
		{
			DateTime unixRef = new DateTime (1970, 1, 1, 0, 0, 0);
			return (dt.Ticks - unixRef.Ticks) / 10000000;
		}
		*/
		/*
		/// <summary>
		/// Gets the time zone.
		/// </summary>
		/// <returns>The time zone.</returns>
		public static double GetTimeZone() {
			var date = DateTime.Now;
			return Math.Round((date - date.ToUniversalTime()).TotalHours, 1, MidpointRounding.AwayFromZero);
		}*/
		
		public static string DeviceId {
			get {
				return SystemInfo.deviceUniqueIdentifier;
			}
		}
		
		public static string DeviceModel {
			get {
				return SystemInfo.deviceModel;
			}
		}
		
		/*
		/// <summary>
		/// Contains the specified data and value.
		/// </summary>
		/// <param name='data'>
		/// If set to <c>true</c> data.
		/// </param>
		/// <param name='value'>
		/// If set to <c>true</c> value.
		/// </param>
		public static bool Contains (string[] data, string value)
		{
			if (null != data) {
				foreach (string s in data) {
					if (s == value)
						return true;
				}
			}
			return false;
		}


		
		/// <summary>
		/// Contains the specified list and value.
		/// </summary>
		/// <param name="list">List.</param>
		/// <param name="value">Value.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static bool Contains<T> (IEnumerable<T> list, T value) {
			foreach (T item in list) {
				if (item.Equals (value)) {
					return true;
				}
			}
			return false;
		}


		/// <summary>
		/// Count the specified items in the list using the comparator.
		/// </summary>
		/// <param name='list'>
		/// List.
		/// </param>
		/// <param name='action'>
		/// Action.
		/// </param>
		/// <typeparam name='T'>
		/// The 1st type parameter.
		/// </typeparam>
		public static int Count<T> (IEnumerable<T> list, Func<T, bool> comparator)
		{
			return FilterList<T> (list, comparator).Count;
		}*/


		/// <summary>
		/// Shuffles the specified list.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list">The list.</param>
		public static void Shuffle<T>(IList<T> list) {
			System.Random rng = new System.Random(DateTime.Now.Second);
			for (int n = list.Count - 1; n > 0; n--) {
				int k = rng.Next(n + 1);
				T value = list[k];
				list[k] = list[n];
				list[n] = value;
			}
		}
		
		/*
		/// <summary>
		/// SHA1 encode.
		/// </summary>
		/// <param name="plainText">The plain text.</param>
		/// <returns></returns>
		/// <remarks></remarks>
		public static string SHA1Encode (string plainText)
		{
			var inputBytes = ASCIIEncoding.ASCII.GetBytes (plainText);        
			var hashBytes = new SHA1Managed ().ComputeHash (inputBytes);
			StringBuilder hashValue = new StringBuilder ();
			Array.ForEach<byte> (hashBytes, b => hashValue.Append (b.ToString ("x2")));
			return hashValue.ToString ();
		}
		
		*/
		/*
		/// <summary>
		/// Executes the post command.
		/// </summary>
		/// <returns>
		/// The post command.
		/// </returns>
		/// <param name='url'>
		/// URL.
		/// </param>
		/// <param name='data'>
		/// Data.
		/// </param>
		/// <param name='callback'>
		/// The callback to be called after processing the request.
		/// </param>
		public static IEnumerator ExecutePostCommand (string url, string data, Action<WWW> callback)
		{
			var form = new WWWForm ();
			form.headers ["Content-Type"] = "application/json";
			int requestNo = GetRequestNumber();
			Log (string.Format("[HTTP POST] Request {0} {1} {2}", requestNo, url, data));
			
			using (WWW www = new WWW(url, Encoding.UTF8.GetBytes(data), form.headers))
			{
				yield return www;
				if (www.isDone && null != callback)
				{
					callback (www);
				}		
			}
		}
		*/
		/*
		/// <summary>
		/// Executes the get command.
		/// </summary>
		/// <returns>
		/// The get command.
		/// </returns>
		/// <param name='url'>
		/// URL.
		/// </param>
		/// <param name='callback'>
		/// Callback.
		/// </param>
		public static IEnumerator ExecuteGetCommand (string url, Action<WWW> callback)
		{
			int requestNo = GetRequestNumber();
//			Util.Log (string.Format("[HTTP GET] Request {0} {1}", requestNo, url));
			
			if (!Util.IsInternetReachable) {
				if (null != callback) {
					callback (null);
				}
				yield break;
			}

			using (WWW www = new WWW(url)) {
				
				yield return www;
				//Util.Log (string.Format("[HTTP GET] Response {0} {1}", requestNo, (!string.IsNullOrEmpty (www.error)) ? (" Error - " + www.error) : www.text));
				if (www.isDone && null != callback) {
					callback (www);
				}
			}
		}*/

		/// <summary>
		/// ROT47 the string
		/// </summary>
		/// <returns>The Rot47 string.</returns>
		/// <param name="value">Value.</param>
		public static string ROT47Str(string value) {
			if (!string.IsNullOrEmpty(value)) {
				char[] array = value.ToCharArray();
				for (int i = 0; i < array.Length; i++) {
					int number = (int)array[i];
					if (number >= 33 && number <= 126) {
						array[i] = (char)(33 + ((number + 14) % 94));
					}
					else {
						array[i] = (char)number;
					}
				}
				return new string(array);
			}
			return value;
		}
		/*
		/// <summary>
		/// Reads the text from resource.
		/// </summary>
		/// <returns>
		/// The text from resource.
		/// </returns>
		/// <param name='resourceName'>
		/// Resource name.
		/// </param>
		public static string ReadTextFromResource (string resourceName)
		{
			TextAsset data = (TextAsset)Resources.Load (resourceName, typeof(TextAsset));
			return null != data ? data.text : null;
		}
		
		/// <summary>
		/// Filters the list.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list">The list.</param>
		/// <param name="comparator">The comparator.</param>
		/// <returns></returns>
		public static List<T> FilterList<T> (IEnumerable<T> list, Func<T, bool> comparator)
		{
			List<T> filteredList = new List<T> ();
			foreach (T item in list) {
				if (comparator (item)) {
					filteredList.Add (item);
				}
			}
			return filteredList;
		}
		*/

		/// <summary>
		/// Filters the list.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list">The list.</param>
		/// <param name="comparator">The comparator.</param>
		/// <returns></returns>
		public static List<T> FilterAllList<T> (IEnumerable<T> list)
		{
			List<T> filteredList = new List<T> ();
			foreach (T item in list) {
					filteredList.Add (item);
			}
			return filteredList;
		}

		/*
		/// <summary>
		/// Firsts the or default.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list">The list.</param>
		/// <param name="comparator">The comparator.</param>
		/// <returns></returns>
		public static T FirstOrDefault<T> (IEnumerable<T> list, Func<T, bool> comparator)
		{
			foreach (T item in list) {
				if (comparator (item)) {
					return item;
				}
			}
			return default(T);
		}

		/// <summary>
		/// Find if any element in the list matches the predicate specified
		/// </summary>
		/// <param name="items">Items.</param>
		/// <param name="comparator">Comparator.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static bool Any<T> (IEnumerable<T> items, Func<T, bool> comparator)
		{
			if (null == items)
				return false;
			
			foreach (T item in items) {
				if (comparator (item)) {
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Select the specified list and selector.
		/// </summary>
		/// <param name="list">List.</param>
		/// <param name="selector">Selector.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		/// <typeparam name="V">The 2nd type parameter.</typeparam>
		public static List<V> Select<T, V>(IEnumerable<T> list, Func<T, V> selector) {
			List<V> values = new List<V> ();
			
			foreach (T item in list) {
				values.Add(selector(item));
			}
			
			return values;
		}

		/// <summary>
		/// Gets the comparison ascending.
		/// </summary>
		/// <returns>The comparison ascending.</returns>
		/// <param name="fieldSelector">Field selector.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		/// <typeparam name="F">The 2nd type parameter.</typeparam>
		public static Comparison<T> GetComparisonAscending<T, F>(Func<T, F> fieldSelector) where F : IComparable {
			return (o1, o2) => {
				return fieldSelector(o1).CompareTo(fieldSelector(o2));
			};
		}

		public static string ToTitleCase (string word)
		{
			if (word == null)
				return null;
			
			if (word.Length > 1)
				return char.ToUpper (word [0]) + word.Substring (1);
			
			return word.ToUpper ();
		}*/

		/// <summary>
		/// Updates the vector3.
		/// </summary>
		/// <returns>The vector3.</returns>
		/// <param name="v">V.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <param name="z">The z coordinate.</param>
		public static Vector3 UpdateVector3 (Vector3 v, float x, float y, float z)
		{
			v.x = x;
			v.y = y;
			v.z = z;
			return v;
		}
		/*
		/// <summary>
		/// Adds the element to array.
		/// </summary>
		/// <param name='array'>
		/// Array.
		/// </param>
		/// <param name='newItem'>
		/// New item.
		/// </param>
		/// <typeparam name='T'>
		/// The 1st type parameter.
		/// </typeparam>
		public static void AddElementToArray<T> (ref T[] array, T newItem)
		{
			Array.Resize (ref array, array.Length + 1);
			array [array.Length - 1] = newItem;
			//Log("AddElementToArray" + newItem);
		}*/



		/// <summary>
		/// Log the specified message.
		/// </summary>
		/// <param name="message">Message.</param>
		public static void Log (object message)
		{			
			Debug.Log(message);
		}
		
		public static void LogError(object message, UnityEngine.Object context = null)
		{
			//Debug.LogError(message,context);
		}

		public static void LogDictionary (object obj)
		{
			if(obj.GetType() == new Dictionary<string, object>().GetType())
			{
				IDictionary <string,object> dict = obj as IDictionary<string,object>;
				StringBuilder str = new StringBuilder("{\n");
				foreach(KeyValuePair<string,object> pair in dict)
				{
					str.AppendFormat("\n{0} : {1}",pair.Key,pair.Value);
				}
				str.Append("\n}");
				Log (str);
			}
			else
				Log(obj);
		}

		public static void LogList (object obj)
		{
			
			if(obj.GetType() == new List<string>().GetType())
			{
				List<string> list = obj as List<string>;
				StringBuilder str = new StringBuilder("{\n");
				foreach(string value in list)
				{
					str.AppendFormat("\n{0}",value);
				}
				str.Append("\n}");
				Log (str);
			}
			else
				Log(obj);
			
		}
		/*
		/// <summary>
		/// Medians the specified list.
		/// </summary>
		/// <param name="list">The list.</param>
		/// <returns></returns>
		public static int Median(int[] list) {
			int result = 0;
			Array.Sort(list);
			var listSize = list.Length;
			
			int midIndex = listSize / 2;
			if (0 == listSize % 2) {    // Even number of elements            
				result = ((list[midIndex - 1] + list[midIndex]) / 2);
			}
			else {                      // Odd number of elements
				result = list[midIndex];
			}
			
			return result;
		}
		*/
		#region JSON File de/serialize
		/*
		/// <summary>
		/// Serializes the json document to file.
		/// </summary>
		/// <returns><c>true</c>, if json document to file was serialized, <c>false</c> otherwise.</returns>
		/// <param name="filePath">File path.</param>
		/// <param name="jsonDoc">Json document.</param>
		public static bool SerializeJsonDocToFile(string filePath, IDictionary<string, object> jsonDoc) {
			return SerializeJsonDocToFile(filePath, jsonDoc, JSON_ENCODER_DECODER);
		}

		/// <summary>
		/// Serializes the json document to file.
		/// </summary>
		/// <returns><c>true</c>, if json document to file was serialized, <c>false</c> otherwise.</returns>
		/// <param name="filePath">File path.</param>
		/// <param name="encodeMethod">Encode method.</param>
		public static bool SerializeJsonDocToFile(string filePath, IDictionary<string, object> jsonDoc, Func<string, string> encodeMethod) {
			bool result = false;
			try {
				if(null != jsonDoc) {
					string resultStr = SimpleJson.SimpleJson.SerializeObject(jsonDoc);
					if(!string.IsNullOrEmpty(resultStr)) {
						if(null != encodeMethod) {
							resultStr = encodeMethod(resultStr);
						}
						
						if(!string.IsNullOrEmpty(resultStr)) {
							if(!File.Exists(filePath))
							{
								StreamWriter sw = File.CreateText(filePath);
								sw.Close();
							}
							File.WriteAllText(filePath, resultStr);
							result = true;
						}
					}
				}
			}
			catch(Exception ex) {
				Debug.Log("[Util] Error in persisting file: " + ex.Message);
				result = false;
			}
			return result;
		}

		/// <summary>
		/// DeSerialize json document from file.
		/// </summary>
		/// <returns>The serialize json document from file.</returns>
		/// <param name="filePath">File path.</param>
		public static IDictionary<string, object> DeSerializeJsonDocFromFile(string filePath) {
			return DeSerializeJsonDocFromFile(filePath, JSON_ENCODER_DECODER);
		}

		/// <summary>
		/// DeSerialize json document from file.
		/// </summary>
		/// <returns>The serialize json document from file.</returns>
		/// <param name="filePath">File path.</param>
		public static IDictionary<string, object> DeSerializeJsonDocFromFile(string filePath, Func<string, string> decodeMethod) {
			IDictionary<string, object> result = null;
			if (File.Exists (filePath)) {
				string fileData = File.ReadAllText(filePath);
				if(null != decodeMethod && !string.IsNullOrEmpty(fileData)) {
					fileData = decodeMethod(fileData);
				}
				if(!string.IsNullOrEmpty(fileData)) {
					result = (IDictionary<string, object>)SimpleJson.SimpleJson.DeserializeObject(fileData);
				}
			}
			return result;
		}

		/// <summary>
		/// Deserialize json document from resource.
		/// </summary>
		/// <returns>The serialize json document from resource.</returns>
		/// <param name="resourceName">Resource name.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static IDictionary<string, object> DeSerializeJsonDocFromResource(string resourceName) {
			IDictionary<string, object> result = null;
			string text = Util.ReadTextFromResource (resourceName);
			if(null != text) {
				object obj;
				if(SimpleJson.SimpleJson.TryDeserializeObject(text, out obj)) {
					result = (IDictionary<string, object>)obj;
				}
			}
			return result;
		}

		/// <summary>
		/// Deserialize json document from cache or resource.
		/// </summary>
		/// <returns>The deserialized json document from cache or resource.</returns>
		/// <param name="resourceName">Resource name.</param>
		public static IDictionary<string, object> DeSerializeJsonDocFromCacheOrResource(string resourceName) {
			IDictionary<string, object> result = null;
			string filePath = Path.Combine(Application.persistentDataPath, resourceName);
			if(File.Exists(filePath)) {
				Log("[DE-SERIALIZE] OPENING FILE :" + filePath);
				result = DeSerializeJsonDocFromFile(filePath, null);
			}
			if(null == result) {
				Log("[DE-SERIALIZE] OPENING RESOURCE :" + resourceName);
				result = DeSerializeJsonDocFromResource(resourceName);
			}
			return result;
		}

		/// <summary>
		/// DeSerializes the JSON string.
		/// </summary>
		/// <returns>
		/// Dictionary object.
		/// </returns>
		/// <param name='jsonStr'>
		/// Json string.
		/// </param>
		public static IDictionary<string, object> DeSerializeJSON (string jsonStr) {
			return (IDictionary<string, object>)SimpleJson.SimpleJson.DeserializeObject (jsonStr);
		}
		*/
		
		#endregion
		public static void SetLayer(int newLayer, Transform trans)
		{
			trans.gameObject.layer = newLayer;
			foreach (Transform child in trans)
			{
				child.gameObject.layer = newLayer;
				if (child.childCount > 0)
				{
					SetLayer(newLayer, child.transform);
				}
			}
		}
	}
	}