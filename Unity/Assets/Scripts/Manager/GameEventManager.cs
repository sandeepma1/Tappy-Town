using UnityEngine;
using System.Collections;

public static class GameEventManager
{


	#if UNITY_IOS

	//public static string FacebookAppId 	= "??";
	public static string FacebookAppId 		= "955508744536243";

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
	public static string BundleVersion 		= "0.1";
	public static string FlurryApiKey 		= "J5CS7CZ548VFSK4F2K4R";
	public const string TappyTownApp_URL 	= "https://play.google.com/store/apps/details?id=com.pooch.crossytown";

	#endif


	//Global Variables
	static public string gameVersion = "0.8_alfa";
	
	//static public int maxLevels = 5;
	//static public int currentPlayingLevel = 1;
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
