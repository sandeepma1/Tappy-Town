using UnityEngine;
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
	public Camera mainCamera;
	//, menuCamera;
	public Image blankLogo;
	public GameObject gameName;
	public GameObject UI;
	public GameObject pauseButton, playButton;
	public GameObject startGameGO;
	public GameObject startGameButton;
	public GameObject settingsMenuGO, missionBanner;
	public GameObject charSelcMenu, charSelcLogic, payToContinueMenu, newHighScoreMenu, missionCompleteMenu, missionPrizeRedeemMenu, previewMesh, scrollView;
	public GameObject unlockNewCharacterButton, gatchaMenu, InGameStoreMenu, inAppStoreMenu;
	public Text levelText, countDownAfterResumeText, payCoinsToContinueTextInButton, newHighScoreText;
	//payCoinsToContinueText
	public GameObject coinMono, tokenMono;
	public TextMesh lastBestScore, highScore, highScoreText, mission1;
	public GameObject statsWindow, creditsWindow, resetGameWindow;
	public Toggle toggleMuteButton, toggleShadowsButton, toggleLevel;
	public Light shadowLight;
	//******************************
	public Text t_deaths, t_distance, t_jumps, t_coins, t_coinsSpent, t_secretCoins;
	//*****************************
	public GameObject helpMenu;
	public GameObject watchAdsGO, rateUsGO, freeGiftGO;
	Hashtable optional;
	public Animator anim;
	public bool isCharacterChanged = false;
	Vector3 cameraPos;

	public static IGMLogic m_instance = null;

	public FacebookConnectScript _FacebookConnectObject;

	// Use this for initialization

	void Awake ()
	{	
		m_instance = this;	
		coinMono.SetActive (false);
		tokenMono.SetActive (false);
		//SetMainCameraCanvas (true);
		payToContinueMenu.SetActive (false);
		cameraPos = new Vector3 (-10f, 16.5f, -32.5f);
		Camera.main.transform.localPosition = cameraPos;
		Camera.main.transform.localRotation = Quaternion.Euler (22.5f, 25, 0);
		mainCamera.gameObject.SetActive (true);
		//menuCamera.gameObject.SetActive (false);
		anim = mainCanvas.GetComponent<Animator> ();
		blankLogo.gameObject.SetActive (true);
		gameName.SetActive (true);
		GameEventManager.SetState (GameEventManager.E_STATES.e_pause);
		GameEventManager.currentLevelAttempts++;
		//attempts.text = GameEventManager.currentLevelAttempts.ToString ();
		lastBestScore.text = June.LocalStore.Instance.GetInt ("lastBestScore").ToString ();
		pauseButton.SetActive (false);
		toggleMuteButton.isOn = June.LocalStore.Instance.GetBool ("mute");
		toggleShadowsButton.isOn = June.LocalStore.Instance.GetBool ("shadow");
		toggleLevel.isOn = June.LocalStore.Instance.GetBool ("levelProgression");
//		toggleScreenRotationButton.isOn = June.LocalStore.Instance.GetBool ("screenRotation");
		//****************************  Run Once ************************************************
		if (June.LocalStore.Instance.GetInt ("runOnceTutorial1") <= 0) {
			June.LocalStore.Instance.SetInt ("runOnceTutorial1", 1);
			June.LocalStore.Instance.SetInt ("coins", 100);
			June.LocalStore.Instance.SetBool ("useLevelProgress", true);
			//tutorialMenu1.SetActive (true);
		}//**************************************************************************************
		//****************************  Run Once ************************************************
		if (June.LocalStore.Instance.GetInt ("runOnceTutorial2") <= 0) {
			June.LocalStore.Instance.SetInt ("runOnceTutorial2", 1);

			string[] ids = new string[25];
			for (int i = 0; i < 25; i++) {
				ids [i] = "";
			}
			ids [0] = "IGC-000";
			ids [1] = "IGC-ZZZ";
			//ids [1] = "IGC-010";
			June.LocalStore.Instance.SetStringArray ("unlockedCharacters", ids);
			//tutorialMenu2.SetActive (true);
		}//**************************************************************************************
	}

	void Start ()
	{
		June.LocalStore.Instance.SetInt ("levelAttempts", June.LocalStore.Instance.GetInt ("levelAttempts") + 1);
		UI.gameObject.SetActive (true);
		optional = new Hashtable ();
		optional.Add ("ease", LeanTweenType.easeInOutQuart);
		//SetMainCameraCanvas (true);
		if (June.LocalStore.Instance.GetBool ("useLevelProgress")) {
			levelText.text = "Level " + June.LocalStore.Instance.GetInt ("PlayerXP").ToString ();
		} else {
			levelText.text = "Level " + 50;
		}
		//mainCanvas.renderMode = RenderMode.ScreenSpaceCamera;

		if (_FacebookConnectObject)
			_FacebookConnectObject.OnConnectEnded += HandleFacebookConnection;

	}

	public void isTextMeshesVisible (bool isActive)
	{		
		mission1.gameObject.SetActive (isActive);
		lastBestScore.gameObject.SetActive (isActive);
		highScore.gameObject.SetActive (isActive);
		highScoreText.gameObject.SetActive (isActive);
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
		//SetMainCameraCanvas (true);
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

	public void SettingsButtonPressed ()
	{
		settingsMenuGO.SetActive (true);
	}

	public void CloseSettingsMenu ()
	{
		settingsMenuGO.SetActive (false);
		//SetMainCameraCanvas (true);
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

	public void OpenCharacterSelectionMenu ()
	{
		mainCanvas.renderMode = RenderMode.ScreenSpaceCamera;

		if (GameEventManager.isNightMode) {
			shadowLight.gameObject.SetActive (true);
		}
		GameEventManager.SetState (GameEventManager.E_STATES.e_pause);
		charSelcMenu.SetActive (true);
		isTextMeshesVisible (false);
	}

	public void CloseCharacterSelectionMenu ()
	{
		if (GameEventManager.isNightMode) {
			shadowLight.gameObject.SetActive (false);
		}

		if (isCharacterChanged) {
			SceneManager.LoadScene ("level");
		} else {
			charSelcMenu.SetActive (false);
			isTextMeshesVisible (true);
			mainCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
		}
	}

	public void FaceBookButtonOnTap ()
	{
	
	
	}

	void HandleFacebookConnection (bool isConnected)
	{
		if (isConnected) {
			SaveStringArray.AddCharIDtoUnlock ("IGC-002");
			Etcetera.ShowAlert ("Facebook", "Facebook login Done.", "OK");

		} else {
			Etcetera.ShowAlert ("Facebook", "You need to connect to facebook to get your free character.", "OK");
		}
	}

	public void SelectChar ()
	{

		//TODO-Pooch-Developer
		/*June.LocalStore.Instance.SetFloat ("lastScrollValue", CharacterSelection.m_instance.scrollValue.x);
		GameEventManager.SetState (GameEventManager.E_STATES.e_pause);
		charSelcLogic.GetComponent<CharacterSelection> ().SetCharName ();
		SceneManager.LoadSceneAsync ("level");*/
	}

	public void OpenGatchaMenu ()
	{
		//mainCanvas.renderMode = RenderMode.ScreenSpaceCamera;
		GameEventManager.SetState (GameEventManager.E_STATES.e_pause);
		//SetMainCameraCanvas (false);
		gatchaMenu.SetActive (true);
		isTextMeshesVisible (false);
	}

	//public void SetMainCameraCanvas (bool active)
	//{
	//mainCamera.gameObject.SetActive (active);
	//mainCanvas.gameObject.SetActive (active);
	//menuCamera.gameObject.SetActive (!active);
	//menuCanvas.gameObject.SetActive (!active);
	//}

	public void ShowSettingsMenu ()
	{
		//SetMainCameraCanvas (false);
		settingsMenuGO.SetActive (true);
	}

	public void ShowStatsMenu ()
	{		
		//SetMainCameraCanvas (false);
		PopulateStatsValues ();
		statsWindow.SetActive (true);
	}

	void PopulateStatsValues ()
	{
		t_deaths.text = June.LocalStore.Instance.GetInt ("PlayerDeath").ToString ();
		t_distance.text = June.LocalStore.Instance.GetInt ("PlayerDistanceCovered").ToString ();
		t_jumps.text = June.LocalStore.Instance.GetInt ("PlayerTotalJumps").ToString ();
		t_coins.text = June.LocalStore.Instance.GetInt ("PlayerCoinsCollected").ToString ();
		t_coinsSpent.text = June.LocalStore.Instance.GetInt ("PlayerCoinsSpent").ToString ();
		t_secretCoins.text = June.LocalStore.Instance.GetInt ("PlayerSecretCoins").ToString ();
	}

	public void ShowCreditsMenu ()
	{
		//SetMainCameraCanvas (false);
		creditsWindow.SetActive (true);
	}

	public void ShowResetGameMenu ()
	{
		//SetMainCameraCanvas (false);
		resetGameWindow.SetActive (true);
	}

	public void ShowPayCoinToContinueMenu (int coins)
	{
		pauseButton.SetActive (false);
		payToContinueMenu.SetActive (true);
		if (coins <= 5) {
			//payCoinsToContinueText.text = "Pay " + coins.ToString () + " Tokens to CONTINUE";
			coinMono.SetActive (false);
			tokenMono.SetActive (true);
		} else {
			//payCoinsToContinueText.text = "Pay " + coins.ToString () + " Coins to CONTINUE";
			coinMono.SetActive (true);
			tokenMono.SetActive (false);
		}
		payCoinsToContinueTextInButton.text = coins.ToString ();
	}

	public void ShowHelpMenu ()
	{
		helpMenu.SetActive (true);
		//SetMainCameraCanvas (false);
	}

	public void ClosePayToContinueMenu ()
	{
		payToContinueMenu.SetActive (false);
	}

	public void RateUs ()
	{
		June.MessageBroker.Publish (June.Messages.RateUsOk);
		Application.OpenURL (GameEventManager.TappyTownApp_URL);
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

	/*public void ShowLevelCompleteMenu (int jumpCount)
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
*/
	/*public void GameComplete ()
	{
		gameCompleteMenuGO.SetActive (true);
	}*/

	public void LevelCompleteManager ()
	{
		/*if (June.LocalStore.Instance.GetInt ("unlockedLevels") < GameEventManager.currentPlayingLevel) {
			June.LocalStore.Instance.SetInt ("unlockedLevels", GameEventManager.currentPlayingLevel);
		}*/
	}

	public void ShowPromoBanner ()
	{		
		anim.Play ("PromoStrap1");
	}

	public void ShowNewHighScoreMenu ()
	{
		newHighScoreText.text = June.LocalStore.Instance.GetInt ("lastBestScore").ToString ();
		newHighScoreMenu.SetActive (true);
	}

	public void CloseNewHighScoreMenu ()
	{
		newHighScoreMenu.SetActive (false);
	}

	public void ShowMissionCompleteMenu ()
	{
		//newHighScoreText.text = June.LocalStore.Instance.GetInt ("lastBestScore").ToString ();
		missionCompleteMenu.SetActive (true);
	}

	public void CloseMissionCompleteMenu ()
	{
		missionCompleteMenu.SetActive (false);
	}

	public void ShowMissionPrizeRedeemMenu ()
	{
		missionPrizeRedeemMenu.SetActive (false);
	}

	public void ShowInGameStoreMenu ()
	{
		mainCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
		InGameStoreMenu.SetActive (true);
		CharMeshesOnCanvas (false);
	}

	public void CloseInGameStoreMenu ()
	{
		mainCanvas.renderMode = RenderMode.ScreenSpaceCamera;
		InGameStoreMenu.SetActive (false);
		CharMeshesOnCanvas (true);
	}

	public void ShowinAppStoreMenu ()
	{				
		inAppStoreMenu.SetActive (true);
		CharMeshesOnCanvas (false);
	}

	public void CloseinAppStoreMenu ()
	{
		inAppStoreMenu.SetActive (false);
		CharMeshesOnCanvas (true);
	}

	void CharMeshesOnCanvas (bool isActive)
	{
		
		previewMesh.SetActive (isActive);
		scrollView.SetActive (isActive);
	}

	public void ToggleMute ()
	{
		June.LocalStore.Instance.SetBool ("mute", toggleMuteButton.isOn);
		if (June.LocalStore.Instance.GetBool ("mute")) {
			AudioListener.volume = 0;
		} else {
			AudioListener.volume = 1;
		}
	}

	public void ToggleShadow ()
	{
		June.LocalStore.Instance.SetBool ("shadow", toggleShadowsButton.isOn);
		if (June.LocalStore.Instance.GetBool ("shadow")) {
			shadowLight.shadows = LightShadows.None;
		} else {
			shadowLight.shadows = LightShadows.Hard;
		}
	}

	public void ToggleLevel ()
	{
		June.LocalStore.Instance.SetBool ("levelProgression", toggleLevel.isOn);
		if (June.LocalStore.Instance.GetBool ("levelProgression")) {
			June.LocalStore.Instance.SetBool ("useLevelProgress", false);//on
		} else {
			June.LocalStore.Instance.SetBool ("useLevelProgress", true);//off
		}
	}

	public IEnumerator Shake ()
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
		//PlayerPrefs.DeleteAll ();
		June.LocalStore.Instance.DeleteAll ();
		SceneManager.LoadSceneAsync ("1Loading");
	}
}
