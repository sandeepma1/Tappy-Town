﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IGMLogic : MonoBehaviour
{
	public Canvas mainCanvas, menuCanvas;
	public float duration = 0;
	public float magnitude = 0;
	public bool isGamePause = false;
	public GameObject pauseMenuGO;
	public Camera mainCamera, menuCamera;
	public Image blankLogo;
	public GameObject gameName;
	public Text debugText;
	public GameObject UI;
	public GameObject pauseButton, playButton;
	public GameObject startGameGO;
	public GameObject startGameButton;
	public GameObject settingsMenuGO, missionBanner;
	public GameObject levelCompleteMenuGO, gameCompleteMenuGO, charSelcMenu, charSelcLogic, payToContinueMenu;
	public GameObject unlockNewCharacterButton, unlockNewCharacterMenu;
	public Text jumpText, attemptsText, timeText, levelText, jumpText1, attemptsText1, timeText1, countDownAfterResumeText, payCoinsToContinueText, payCoinsToContinueTextInButton;
	public GameObject coinMono, tokenMono;
	public TextMesh lastBestScore;
	private Vector3 velocity = Vector3.zero;
	public GameObject statsWindow, creditsWindow, resetGameWindow;
	public Toggle toggleMuteButton, toggleShadowsButton, toggleLevel;
	public Light shadowLight;
	//******************************
	public Text t_deaths, t_distance, t_jumps, t_coins, t_coinsSpent, t_secretCoins;
	//*****************************
	public GameObject helpMenu;
	public GameObject watchAdsGO, rateUsGO, freeGiftGO;
	public GameObject movingPlatform;
	float timer = 0;
	Hashtable optional;
	public Animator anim;
	Vector3 cameraPos;

	public static IGMLogic m_instance = null;

	// Use this for initialization

	void Awake ()
	{	
		m_instance = this;	
		coinMono.SetActive (false);
		tokenMono.SetActive (false);
		SetMainCameraCanvas (true);
		payToContinueMenu.SetActive (false);
		cameraPos = new Vector3 (-10f, 16.5f, -32.5f);
		Camera.main.transform.localPosition = cameraPos;
		Camera.main.transform.localRotation = Quaternion.Euler (22.5f, 25, 0);
		mainCamera.gameObject.SetActive (true);
		menuCamera.gameObject.SetActive (false);
		anim = mainCanvas.GetComponent<Animator> ();
		blankLogo.gameObject.SetActive (true);
		gameName.SetActive (true);
		GameEventManager.SetState (GameEventManager.E_STATES.e_pause);
		GameEventManager.currentLevelAttempts++;
		//attempts.text = GameEventManager.currentLevelAttempts.ToString ();
		lastBestScore.text = PlayerPrefs.GetInt ("lastBestScore").ToString ();
		pauseButton.SetActive (false);
		toggleMuteButton.isOn = PlayerPrefsX.GetBool ("mute");
		toggleShadowsButton.isOn = PlayerPrefsX.GetBool ("shadow");
		toggleLevel.isOn = PlayerPrefsX.GetBool ("levelProgression");
//		toggleScreenRotationButton.isOn = PlayerPrefsX.GetBool ("screenRotation");
		//****************************  Run Once ************************************************
		if (PlayerPrefs.GetInt ("runOnceTutorial1") <= 0) {
			PlayerPrefs.SetInt ("runOnceTutorial1", 1);
			PlayerPrefs.SetInt ("Coins", 100);
			//tutorialMenu1.SetActive (true);
		}//**************************************************************************************
		//****************************  Run Once ************************************************
		if (PlayerPrefs.GetInt ("runOnceTutorial2") <= 0) {
			PlayerPrefs.SetInt ("runOnceTutorial2", 1);
			//tutorialMenu2.SetActive (true);
		}//**************************************************************************************
	}

	void Start ()
	{
		PlayerPrefs.SetInt ("levelAttempts", PlayerPrefs.GetInt ("levelAttempts") + 1);
		/*if (PlayerPrefs.GetInt ("levelAttempts") >= 10) {
			Social.ReportProgress ("CgkIqM2wutYIEAIQAw", 10, (bool success) => {
			});			 
		}*/
		//coinsText.text = "0";
		StartCoroutine ("BlankScreen");
		UI.gameObject.SetActive (true);
		optional = new Hashtable ();
		optional.Add ("ease", LeanTweenType.easeInOutQuart);
		SetMainCameraCanvas (true);
		if (PlayerPrefsX.GetBool ("useLevelProgress")) {
			levelText.text = "Level " + PlayerPrefs.GetInt ("PlayerXP").ToString ();
		} else {
			levelText.text = "Level " + 50;
		}
	}

	public void PauseGame ()
	{
		GameEventManager.SetState (GameEventManager.E_STATES.e_pause);
		isGamePause = !isGamePause;
		pauseButton.SetActive (false);
		ShowIGM ();
	}

	public void ResumeGame ()
	{
		playButton.SetActive (false);
		StartCoroutine ("StartCountDownAfterResume");
	}

	void OnApplicationPause (bool pauseStatus)
	{
		if (GameEventManager.GetState () == GameEventManager.E_STATES.e_game && pauseStatus) {	
			PauseGame ();
		}
	}

	IEnumerator StartCountDownAfterResume ()
	{
		countDownAfterResumeText.text = "3";
		yield return new WaitForSeconds (1);
		countDownAfterResumeText.text = "2";
		yield return new WaitForSeconds (1);
		countDownAfterResumeText.text = "1";
		yield return new WaitForSeconds (1);
		countDownAfterResumeText.text = "";
		isGamePause = !isGamePause;
		pauseButton.SetActive (true);
		HideIGM ();
		playButton.SetActive (true);
		GameEventManager.SetState (GameEventManager.E_STATES.e_game);
	}

	public void ResetLevel ()
	{
		watchAdsGO.SetActive (false);
		rateUsGO.SetActive (false);
		freeGiftGO.SetActive (false);

		//playerDiedButtons.SetActive (false);
		blankLogo.gameObject.SetActive (true);
		gameName.SetActive (true);
		SetMainCameraCanvas (true);
		anim.Play ("LevelEndAnimation");
		StartCoroutine ("LevelRestartWithWait");
	}

	IEnumerator LevelRestartWithWait ()
	{
		yield return new WaitForSeconds (1.2f);
		SceneManager.LoadSceneAsync ("level");
	}

	public void ShowIGM ()
	{
		pauseMenuGO.SetActive (true);
	}

	public void HideIGM ()
	{
		pauseMenuGO.SetActive (false);
	}

	public void GoToHome ()
	{
		watchAdsGO.SetActive (false);
		rateUsGO.SetActive (false);
		freeGiftGO.SetActive (false);
		anim.Play ("LevelEndAnimation");
		StartCoroutine ("GotoMainMenuWait");
	}

	IEnumerator GotoMainMenuWait ()
	{
		yield return new WaitForSeconds (1f);
	}

	public void SettingsButtonPressed ()
	{
		//GameEventManager.SetState (GameEventManager.E_STATES.e_pause);
		settingsMenuGO.SetActive (true);
	}

	public void CloseSettingsMenu ()
	{
		settingsMenuGO.SetActive (false);
		SetMainCameraCanvas (true);
		//GameEventManager.SetState (GameEventManager.E_STATES.e_game);
	}

	public void CloseStatssmenu ()
	{
		statsWindow.SetActive (false);
	}

	public void CloseCreditsMenu ()
	{
		creditsWindow.SetActive (false);
	}

	public void CloseResetGameMenu ()
	{
		resetGameWindow.SetActive (false);
	}

	public void CloseHelpMenu ()
	{
		helpMenu.SetActive (false);
	}

	public void CloseCharacterSelectionMenu1 ()
	{
		SetMainCameraCanvas (true);
		charSelcMenu.SetActive (false);
		if (GameEventManager.isNightMode) {
			shadowLight.gameObject.SetActive (false);
			//light2.gameObject.SetActive (false);
		}
	}

	public void CloseUnlockNewCharacterMenu ()
	{
		SetMainCameraCanvas (true);
		unlockNewCharacterMenu.SetActive (false);
	}

	public void SelectChar ()
	{
		print (CharacterSelection.m_instance.scrollValue.x);
		PlayerPrefs.SetFloat ("lastScrollValue", CharacterSelection.m_instance.scrollValue.x);

		print (PlayerPrefs.GetFloat ("lastScrollValue"));
		GameEventManager.SetState (GameEventManager.E_STATES.e_pause);
		charSelcLogic.GetComponent<CharacterSelection> ().SetCharName ();
		SceneManager.LoadSceneAsync ("level");
	}

	public void OpenCharacterSelectionMenu ()
	{
		if (GameEventManager.isNightMode) {
			shadowLight.gameObject.SetActive (true);
			//light2.gameObject.SetActive (true);
		}
		GameEventManager.SetState (GameEventManager.E_STATES.e_pause);
		SetMainCameraCanvas (false);
		charSelcMenu.SetActive (true);
		charSelcLogic.GetComponent<CharacterSelection> ().ScanAllChars ();
	}

	public void OpenUnlockNewCharacterMenu ()
	{		
		GameEventManager.SetState (GameEventManager.E_STATES.e_pause);
		SetMainCameraCanvas (false);
		unlockNewCharacterMenu.SetActive (true);
	}

	public void SetMainCameraCanvas (bool active)
	{
		mainCamera.gameObject.SetActive (active);
		mainCanvas.gameObject.SetActive (active);
		menuCamera.gameObject.SetActive (!active);
		menuCanvas.gameObject.SetActive (!active);
	}

	public void ShowSettingsMenu ()
	{
		SetMainCameraCanvas (false);
		settingsMenuGO.SetActive (true);
	}

	public void ShowStatsMenu ()
	{		
		SetMainCameraCanvas (false);
		PopulateStatsValues ();
		statsWindow.SetActive (true);
	}

	void PopulateStatsValues ()
	{
		t_deaths.text = PlayerPrefs.GetInt ("PlayerDeath").ToString ();
		t_distance.text = PlayerPrefs.GetInt ("PlayerDistanceCovered").ToString ();
		t_jumps.text = PlayerPrefs.GetInt ("PlayerTotalJumps").ToString ();
		t_coins.text = PlayerPrefs.GetInt ("PlayerCoinsCollected").ToString ();
		t_coinsSpent.text = PlayerPrefs.GetInt ("PlayerCoinsSpent").ToString ();
		t_secretCoins.text = PlayerPrefs.GetInt ("PlayerSecretCoins").ToString ();
	}

	public void ShowCreditsMenu ()
	{
		SetMainCameraCanvas (false);
		creditsWindow.SetActive (true);
	}

	public void ShowResetGameMenu ()
	{
		SetMainCameraCanvas (false);
		resetGameWindow.SetActive (true);
	}

	public void ShowPayCoinToContinueMenu (int coins)
	{
		pauseButton.SetActive (false);
		payToContinueMenu.SetActive (true);
		if (coins <= 5) {
			payCoinsToContinueText.text = "Pay " + coins.ToString () + " Tokens to CONTINUE";
			coinMono.SetActive (false);
			tokenMono.SetActive (true);
		} else {
			payCoinsToContinueText.text = "Pay " + coins.ToString () + " Coins to CONTINUE";
			coinMono.SetActive (true);
			tokenMono.SetActive (false);
		}
		payCoinsToContinueTextInButton.text = coins.ToString ();
	}

	/*public void ShowPayTokenToContinueMenu (int token)
	{
		pauseButton.SetActive (false);
		payToContinueMenu.SetActive (true);
		payTokenToContinueText.text = "Pay " + token.ToString () + " Token to CONTINUE";
		payTokenToContinueTextInButton.text = token.ToString ();
	}*/

	public void ShowHelpMenu ()
	{
		helpMenu.SetActive (true);
		SetMainCameraCanvas (false);
	}

	public void ClosePayToContinueMenu ()
	{
		payToContinueMenu.SetActive (false);
	}

	public void RateUs ()
	{
		Application.OpenURL ("https://play.google.com/store/apps/details?id=com.pooch.crossytown");
	}

	public void OpenSandeepTwitterPage ()
	{
		Application.OpenURL ("https://twitter.com/sandeepmamdikar");
	}

	public void StartGame ()
	{
		pauseButton.SetActive (true);
		startGameGO.SetActive (false);
		freeGiftGO.SetActive (false);
		GameEventManager.SetState (GameEventManager.E_STATES.e_game);
		pauseButton.SetActive (true);
		LeanTween.moveX (missionBanner, -25, 0.5f, optional);

	}

	/*IEnumerator MoveMissionBanner ()
	{
		float moveIn = 0.5f;

		yield return new WaitForSeconds (moveIn);
		missionBanner.SetActive (false);
	}
*/
	public void ShowLevelCompleteMenu (int jumpCount)
	{
		pauseButton.SetActive (false);
		GameEventManager.SetState (GameEventManager.E_STATES.e_pause);
		levelCompleteMenuGO.SetActive (true);
		jumpText.text = jumpCount.ToString ();
		timeText.text = timer.ToString ("F2");
		attemptsText.text = GameEventManager.currentLevelAttempts.ToString ();
		LevelCompleteManager ();
		print ("game complete");
		gameCompleteMenuGO.SetActive (true);
		jumpText1.text = jumpCount.ToString ();
		timeText1.text = timer.ToString ("F2");
		attemptsText1.text = GameEventManager.currentLevelAttempts.ToString ();
	}

	public void GameComplete ()
	{
		gameCompleteMenuGO.SetActive (true);
	}

	public void LevelCompleteManager ()
	{
		/*if (PlayerPrefs.GetInt ("unlockedLevels") < GameEventManager.currentPlayingLevel) {
			PlayerPrefs.SetInt ("unlockedLevels", GameEventManager.currentPlayingLevel);
		}*/
	}

	public void KillPlayer ()
	{
		movingPlatform.GetComponent<MovingPlatform> ().SaveLastBestRunScore ();
		pauseButton.SetActive (false);
		GameEventManager.SetState (GameEventManager.E_STATES.e_pause);
		StartCoroutine ("Shake");
		anim.Play ("PromoStrap1");
		StartCoroutine ("PlayerDiedMenuWait");
	}

	IEnumerator PlayerDiedMenuWait ()
	{
		yield return new WaitForSeconds (1F);
		//anim.Play ("playerDiedButtonComeUp");
	}


	public void ToggleMute ()
	{
		PlayerPrefsX.SetBool ("mute", toggleMuteButton.isOn);
		if (PlayerPrefsX.GetBool ("mute")) {
			AudioListener.volume = 0;
		} else {
			AudioListener.volume = 1;
		}
	}

	public void ToggleShadow ()
	{
		PlayerPrefsX.SetBool ("shadow", toggleShadowsButton.isOn);
		if (PlayerPrefsX.GetBool ("shadow")) {
			shadowLight.shadows = LightShadows.None;
		} else {
			shadowLight.shadows = LightShadows.Hard;
		}
	}

	public void ToggleLevel ()
	{
		PlayerPrefsX.SetBool ("levelProgression", toggleLevel.isOn);
		if (PlayerPrefsX.GetBool ("levelProgression")) {
			PlayerPrefsX.SetBool ("useLevelProgress", false);//on
		} else {
			PlayerPrefsX.SetBool ("useLevelProgress", true);//off
		}
	}

	IEnumerator BlankScreen ()
	{
		//yield return new WaitForSeconds (0.45f);
		//blankLogo.gameObject.SetActive (false);		
		yield return new WaitForSeconds (0.25f);
		//gameName.gameObject.SetActive (false);
	}

	IEnumerator Shake ()
	{
		float elapsed = 0.0f;
		Vector3 originalCamPos = Camera.main.transform.localPosition;
		while (elapsed < duration) {
			elapsed += Time.deltaTime;
			float percentComplete = elapsed / duration;
			float damper = 1.0f - Mathf.Clamp (1f * percentComplete - 3.0f, 0.0f, 1.0f);
			// map value to [-1, 1]
			float ranX = 0.1f, ranY = 0.4f;
			float x = originalCamPos.x + (Random.Range (ranX, ranY));
			float y = originalCamPos.y + (Random.Range (ranX, ranY));
			float z = originalCamPos.z + (Random.Range (ranX, ranY));
			x *= magnitude * damper;
			y *= magnitude * damper;
			z *= magnitude * damper;
			Camera.main.transform.localPosition = new Vector3 (x, originalCamPos.y, z);
			yield return null;
		}
		Camera.main.transform.localPosition = originalCamPos;
	}

	public void ResetGameData ()
	{
		PlayerPrefs.DeleteAll ();
		SceneManager.LoadSceneAsync ("1Loading");
	}
}
