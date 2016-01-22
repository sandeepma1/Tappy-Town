﻿using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


	/// <summary>
	/// Base config.
	/// </summary>
	public abstract class BaseConfig<T, U> : BaseModel where U : BaseModel where T : BaseConfig<T, U>, new() {

		private static readonly string ServerTimestamp = "sts";
				
		private static object _padLock_ = new object();
		
		#region Singleton
		private static T _Instance;
		/// <summary>
		/// Gets the instance.
		/// </summary>
		/// <value>The instance.</value>
		public static T Instance {
			get {
				if(null == _Instance) {
					lock(_padLock_) {
						if(null == _Instance) {
							_Instance = new T();
							_Instance.Load();
						}
					}
				}
				return _Instance;
			}
		}
		
		#endregion
		
		protected List<U> _Items = null;
		/// <summary>
		/// Gets or sets the items.
		/// </summary>
		/// <value>The items.</value>
		public virtual List<U> Items {
			get {
				if(null == _Items) {
					lock(_padLock_) {
						if(null == _Items) {
							LoadItems();
						}
					}
				}
				return _Items;
			}
		}
		
		/// <summary>
		/// Gets the name of the resource.
		/// </summary>
		/// <value>The name of the resource.</value>
		public abstract string ResourceName { 
			get;
		}
		
		/// <summary>
		/// Gets the root key.
		/// </summary>
		/// <value>The root key.</value>
		public abstract string RootKey {
			get;
		}
		
		/// <summary>
		/// Gets the deserialize func.
		/// </summary>
		/// <value>The deserialize func.</value>
		public virtual Func<string, IDictionary<string, object>> DeserializeFunc {
			get {
				return Util.DeSerializeJsonDocFromResource;
			}
		}
		
		/// <summary>
		/// Gets the item converter.
		/// </summary>
		/// <returns>The item converter.</returns>
		/// <typeparam name="U">The 1st type parameter.</typeparam>
		public abstract Func<IDictionary<string, object>, U> ItemConverter {
			get;
		}
		
		/// <summary>
		/// Load this instance.
		/// </summary>
		protected virtual void Load() {

			UpdateDoc(DeserializeFunc(ResourceName));
			LoadItems();
		}
		
		/// <summary>
		/// Loads the items.
		/// </summary>
		protected virtual void LoadItems() {
			var rawItems = null != RootKey ? Get<SimpleJson.JsonArray>(RootKey) : null;
			if(null != rawItems) {
				_Items = new List<U>();
				foreach(var ri in rawItems) {
					if(ri is IDictionary<string, object>) {
						_Items.Add(ItemConverter((IDictionary<string, object>)ri));
					}
				}
			}
		}
		
		public void Reload()
		{
			Load();
		}
		
		public long LastModifiedTimestamp
		{
			get {
				long lmts = GetLong(ServerTimestamp);
				return lmts;
			}
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="BaseConfig`2"/> class.
		/// </summary>
		public BaseConfig() : this(null) { }
		
		/// <summary>
		/// Initializes a new instance of the <see cref="BaseConfig`2"/> class.
		/// </summary>
		/// <param name="doc">Document.</param>
		public BaseConfig(IDictionary<string, object> doc) : base(doc) { }
		
		
	}
