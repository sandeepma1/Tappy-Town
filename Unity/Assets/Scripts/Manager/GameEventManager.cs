using UnityEngine;
using System.Collections;

public static class GameEventManager
{
	// tweakable values edit here
	static public int coinAskList1 = 100;
	static public int coinAskList2 = 300;
	static public int coinAskList3 = 500;
	static public int coinAskList4 = 700;
	static public int coinAskList5 = 1000;
	static public int coinAskList6 = 5;

	static public int gameBrightness = 203;


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
