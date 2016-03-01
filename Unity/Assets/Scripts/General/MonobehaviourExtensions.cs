using System;
using UnityEngine;
using System.Collections;

public static class MonobehaviourExtensions {

	#region scene callback handlers
	
	public static Action<T> VerifyCallback<T>(this MonoBehaviour obj, Action<T> callback) {
		return (T item) => {
			if(obj != null) {
				callback(item);
			}
		};
	}
	
	public static Action<T, U> VerifyCallback<T, U>(this MonoBehaviour obj, Action<T, U> callback) {
		return (T item, U item1) => {
			if(obj != null) {
				callback(item, item1);
			}
		};
	}
	
	public static Action<T, U, V> VerifyCallback<T, U, V>(this MonoBehaviour obj, Action<T, U, V> callback) {
		return (T item, U item1, V item2) => {
			if(obj != null) {
				callback(item, item1, item2);
			}
		};
	}
	
	#endregion
}
