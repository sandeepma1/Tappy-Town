using UnityEngine;
using System.Collections;

public static class GameEventManager
{
	#if UNITY_IOS
	public static string FacebookAppId 		= "955508744536243"; //TODO
	public static string AmplitudeAPIKey 	= "??";
	public static string BundleVersion 		= "0.1";
	public static string FlurryApiKey 		= "WCTPRDVKT2Q4HGGQWHXT";
	public static string TappyTownApp_URL 	= "https://itunes.apple.com/app/id1065763162?mt=8";
	public static string fyberAppId 				= "41195";
	public static string fyberSecurityToken 		= "758aa4b685a9441b0041b48df61d60ad";
	public static string fyberCustomCurrencyName 	= "hint";
	public class InAppProductIds
	{
		public static string Package_1 =  "com.junesoftware.tappytown.package1";
	}



#else
	public static string InAppKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEArRLBB2bYtgQIdST8i/w8G/EP0kNMW5GUaMBAY7ea/ZM76zuIclySSF0b4hpET8gpUo5B2i6gR0vy0IoH1WhY9ErX2cGno5KwOs+KeeEpCpnXVEEJMsGd127hdoeN5QvLc8R6EjyHRkVSHo0ZonO8NNIIZhqotNE5269qrmFV/13lAhYGXKiaDd0MI+yV9qYr739XeiF7rN66q59LPUGswBudqJG0enlZBFTNR0NBskUfuRTcUnD45sAmaYH7meeCYgDKsHz8MdTuaz4FoymkVeRPZ2qpzAImQPgA7o6za+sUaYz5L45pdE8OysxW06DkSJj3/3d7C0shEalMPQZNbQIDAQAB";
	public static string FacebookAppId = "955508744536243";
	//TODO
	public static string BundleVersion = "0.1";
	public static string FlurryApiKey = "J5CS7CZ548VFSK4F2K4R";
	public const string TappyTownApp_URL = "https://play.google.com/store/apps/details?id=com.pooch.crossytown";
	public const string BundleIdentifier = "com.junesoftware.tappytown";
	public static string fyberAppId = "41515";
	public static string fyberSecurityToken = "eff828e8b5279f9d7dd86ac791c45e13";
	public static string fyberCustomCurrencyName = "hint";

	public class InAppProductIds
	{
		public static string Package_1 = "com.junesoftware.tappytown.package1";
	}
	#endif

	// tweakable values edit here
	static public int coinAskList1 = 100;
	static public int coinAskList2 = 300;
	static public int coinAskList3 = 500;
	static public int coinAskList4 = 700;
	static public int coinAskList5 = 1000;
	static public int coinAskList6 = 5;

	static public int gameBrightness = 203;

	static public int gatchaSpinValue = 100;

	//Global Variables
	static public string gameVersion = "0.9_beta";

	static public int currentLevelAttempts = 0;
	static public int GPSLoginCounter = 0;

	static public bool cubeTouchableAfterRotate = false, checkIfRigBodiesIsSleeping;
	static public bool hidePauseMenu, pause;
	static public bool PMlevel1 = true, showAreUSureWindow, isLevelLoaded, islevelCompleted, isNightMode;



	public delegate void GameEvent ();

	public enum E_STATES
	{
		e_mainMenu,
		e_game,
		e_pause,
		e_levelMenu,
		e_nextLevel,
		e_previousLevel,
		e_resetLevel,
		e_levelFinish}

	;

	public enum E_CLICKSTATE
	{
		e_delayFinish,
		e_delayStart}

	;
	
	//--------------
	
	static E_STATES m_gameState = E_STATES.e_mainMenu;
	
	static E_CLICKSTATE m_click_state = E_CLICKSTATE.e_delayStart;
	
	//----------------
	
	public static void SetClickState (E_CLICKSTATE clickstate)
	{
		m_click_state = clickstate;
	}

	public static E_CLICKSTATE GetClickState ()
	{
		return m_click_state;
	}
	
	//---------------
	
	public static void SetState (E_STATES state)
	{
		m_gameState = state;
	}

	public static E_STATES GetState ()
	{
		return m_gameState;
	}
	
	//-------------------

}
