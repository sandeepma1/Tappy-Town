//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//using System;
//
//public class JuneAndroidNativeCallbackManager : MonoBehaviour {
//
//	static GameObject _receiver;
//	public static void Initialize () {
//	//	if (Application.platform == RuntimePlatform.Android) {
//			if (_receiver == null) {
//			_receiver = new GameObject ("JuneAndroidNativeCallbackManager");
//			_receiver.AddComponent<JuneAndroidNativeCallbackManager>();
//			DontDestroyOnLoad(_receiver);
//			}
//	//	}
//	}
//	Dictionary<string,Action> NativeCallbacks = new Dictionary<string,Action>(){
//		{"SeventyNineAdFinished",AdFinished},
//		{"SeventyNineAdClosed",AdClosed}
//	};
//
//	public static Action AdFinished, AdClosed;
//
//
//	public  void OnMessage(string type){
//		Debug.Log("[JuneAndroidNativeCallbackManager]ON MESSAGE");
//		if(type != null && NativeCallbacks.ContainsKey(type) && NativeCallbacks[type] != null)
//		NativeCallbacks[type]();
//	}
//}
