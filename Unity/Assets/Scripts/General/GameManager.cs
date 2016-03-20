using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// Game manager.
/// Aniket Kayande
/// 12.09.12
/// </summary>
using System.Collections;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using June;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{
	//public GameState m_GameState;
	public bool m_bMusicOn;
	public static DateTime LAST_ON_FOCUS_TIMESTAMP = DateTime.Now;
	public June.Payments.PurchaseManager InAppPurchase = null;

	void Awake ()
	{
		DontDestroyOnLoad (gameObject);
		Etcetera.EnableImmersiveMode ();
		this.InAppPurchase = June.Payments.PurchaseManager.Instance;
	}

	void Start ()
	{
				

	}

	void StartEngine (GameObject go)
	{
		
	}

	public void PauseGame ()
	{
	}

	public void ResumeGame ()
	{
	}

	public void StartMenuMusic ()
	{
				
	}

	public void StopMenuMusic ()
	{

	}

	void OnApplicationPause (bool pause)
	{
		if (pause) {
			Debug.Log ("Application paused");
		} else {
			Debug.Log ("Application unpaused");
			Etcetera.EnableImmersiveMode ();
		}		
	}

	// Application specific calls
	void OnApplicationFocus (bool focus)
	{

	}

	//..
	public static string SCENE_MAIN_MENU = "SplashScreen";
	//..
	public string currentScene = "";

	public void loadSceneWithName (string sceneName)
	{
		currentScene = sceneName;
	}

	void backKeyManager ()
	{
		if (currentScene.Equals (SCENE_MAIN_MENU)) {
			
		}
	}
		
	
}
