using UnityEngine;
using System.Collections;

public static class GameEventManager
{

	// tweakable values edit here
	//**************** Player
	static public Vector3 playerMoveDirection = new Vector3 (1, 0, 0);
	static public float playerMoveInSeconds = 10f;
	static public float playerJumpSpeed = 24;
	static public float playerGravity = 120f;
	//10 24 120
	//**************** End Player

	static public int coinAskList0 = 0;
	static public int coinAskList1 = 50;
	static public int coinAskList2 = 100;
	static public int coinAskList3 = 200;
	static public int coinAskList4 = 300;
	static public int coinAskList5 = 500;
	static public int coinAskList6 = 700;
	static public int coinAskList7 = 1000;

	static public int gameBrightness = 203;

	static public int gatchaSpinValue = 100;
	static public int missionCompleteTokenAmount = 5;
	static public int patternLength = 30;

	//1f = 1hr
	static public float freeGiftTimeDelay = 3f;


	//Global Variables
	static public string gameVersion = "0.1.9";
	static public int currentLevelAttempts = 0;
	static public int GPSLoginCounter = 0;
	static public bool isNightMode = false;
	static public bool showMissionBanner = true;

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
