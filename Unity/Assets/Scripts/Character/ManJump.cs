using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//using UnityEngine.SceneManagement;

public class ManJump : MonoBehaviour
{
	public static ManJump m_instance = null;

	public GameObject playerMesh, playerSupport, board, skateboardGO, balloonGO, blobProjector, wantToContinueMenu;
	public ParticleSystem playerDieParticle, speedUpParticle, coinParticle, skateSparksTrail, landingParticles;
	public Text counDownText, continueText;
	public Text debugText;
	public GameObject igmLogic;
	public AudioSource dieSound, coinPickupSound, skateLoop;
	public GameObject tapnHoldText;
	public Animator coinGot;
	float jumpSpeedTemp = 0;
	float gravityTemp = 0;
	private Vector3 moveDirection = Vector3.zero;
	CharacterController controller;
	int jumpCount = 0;
	bool inAirJumpBox = false;
	GameObject character;
	Hashtable optional;
	Vector3 iniScale;
	int coinMultipler = 0;
	int[] coinAskList;
	CharacterController c;
	int coinsToAsk = 0;
	//int lastBest = 0;
	bool isEnableFlappy = false;
	bool isDeath = false;
	bool isInCar = false;
	bool isBlinking = false;
	float speed = 10, s = 3.4f, iniSpeed;
	bool isInvi = false;
	float jumpSpeed = 0.0F;
	float gravity = 0.0F;

	//public GameObject playerGO

	void Awake ()
	{		
		m_instance = this;
		IniPlayerPosValues ();	
		playerSupport.SetActive (false);
		SwitchBalloon ();
		c = GetComponent<CharacterController> ();
		string charName = CharacterManager.CurrentCharacterSelected.PrefabName;
		playerMesh = Instantiate (Resources.Load ("Characters/CharactersMesh/" + charName) as GameObject);
		playerMesh.GetComponent<MeshRenderer> ().sharedMaterial.SetColor ("_Color", new Color32 (200, 200, 200, 255));
		playerMesh.transform.parent = this.transform;
		playerMesh.transform.localPosition = new Vector3 (0, -0.5f, 0.05f);
		playerMesh.transform.localEulerAngles = new Vector3 (270, 315, 0);
	}

	void Start ()
	{
		//StopParticle (skateSparksTrail);
		controller = GetComponent<CharacterController> ();
		iniScale = playerMesh.transform.localScale;
		optional = new Hashtable ();
		optional.Add ("ease", LeanTweenType.easeOutBack);
		iniSpeed = GameEventManager.playerMoveInSeconds;
		coinAskList = new int[6];
		coinAskList [0] = GameEventManager.coinAskList1;
		coinAskList [1] = GameEventManager.coinAskList2;
		coinAskList [2] = GameEventManager.coinAskList3;
		coinAskList [3] = GameEventManager.coinAskList4;
		coinAskList [4] = GameEventManager.coinAskList5;
		coinAskList [5] = GameEventManager.coinAskList6;
	}

	public void IniPlayerPosValues ()
	{
		jumpSpeed = GameEventManager.playerJumpSpeed;
		gravity = GameEventManager.playerGravity;
		jumpSpeedTemp = jumpSpeed;
		gravityTemp = gravity;
	}

	void Update ()
	{
		//*************************************  Car mMechanics ******************************
		if (Input.GetMouseButtonUp (0) && isInCar) {
			s = iniSpeed;
			MovingPlatform.m_instance.SpeedStopper (iniSpeed);
			return;
		}
		if (Input.GetMouseButton (0) && isInCar) {
			if (s >= 0) {
				debugText.text = s.ToString ();
				s -= Time.deltaTime * speed;
				MovingPlatform.m_instance.SpeedStopper (s);
			}
			return;
		}
		/*if (Input.GetMouseButtonUp (1)) {
			//isInvi = !isInvi;
			//playerSupport.SetActive (isInvi);
			//PromoStripsManager.m_instance.ShowPromoStrips ();
		}*/
		//*************************************  Car mMechanics ******************************
		if (GameEventManager.GetState () == GameEventManager.E_STATES.e_game) {			
			if (controller.isGrounded || inAirJumpBox || isEnableFlappy) {
				if (!skateSparksTrail.isPlaying) {
					skateSparksTrail.Play ();
					skateLoop.Play ();
					landingParticles.Play ();
				}

				if (Input.GetMouseButton (0) && !EventSystem.current.IsPointerOverGameObject () || Input.GetKey (KeyCode.Space)) {					
					if (!isEnableFlappy) {
						jumpCount++;
					}
					jumpSpeed = isEnableFlappy ? jumpSpeedTemp / 3 : jumpSpeedTemp; //shortest if
					moveDirection.y = jumpSpeed;
					//print ("jumping");
					StopParticle (skateSparksTrail);
					skateLoop.Pause ();
				}
			}
			//rint ("ground");

			gravity = isEnableFlappy ? gravityTemp / 2f : gravityTemp; //shortest if

			moveDirection.y -= gravity * Time.deltaTime * 1f;
			controller.Move (moveDirection * Time.deltaTime);
			transform.localPosition = new Vector3 (1, transform.localPosition.y, 0);
			transform.localRotation = new Quaternion (0, 0, 0, 0);
			if (transform.localPosition.y < -5) {
				PlayerPartiallyDied ();
			}

		}
		//StopParticle (skateSparksTrail);

		if (Input.GetMouseButtonDown (2)) {	
			//PlayParticle (skateSparksTrail);
			//print (skateSparksTrail.isPlaying);
			//GameManagers.m_instance.Restartlevel ();
			//SceneManager.LoadSceneAsync ("level");
		}
	}

	void PlayParticle (ParticleSystem p)
	{
		if (p.isPlaying) {
			p.Stop ();
			p.Clear ();
			p.time = 0;
		}
		if (!p.isPlaying) {
			p.Play ();
		}
	}

	void StopParticle (ParticleSystem p)
	{
		if (p.isPlaying) {
			p.Stop ();
			p.Clear ();
			p.time = 0;
		}
	}

	IEnumerator BlinkCharacter (int times)
	{
		for (int i = 0; i < times; i++) {
			DisplayPlayerObject (false);
			yield return new WaitForSeconds (0.1f);
			DisplayPlayerObject (true);
			yield return new WaitForSeconds (0.1f);
		}
	}

	void OnTriggerEnter (Collider other)
	{
		switch (other.gameObject.tag) {
		case "death":			
			if (!isDeath && !isInvi) {
				PlayerPartiallyDied ();
			}
			break;
		case "pickable_coin":
			IGMLogic.m_instance.anim.CrossFadeInFixedTime ("coinScale", 0.2f);
			PlayParticle (coinParticle);
			coinPickupSound.Play ();
			//coinGot.Play ("coinGot");
			CoinCalculation.m_instance.AddCoins (1);
			ReActivateCoins (other.gameObject);
			break;
		case "pickable_token":
			//IGMLogic.m_instance.anim.CrossFadeInFixedTime ("coinScale", 0.2f);
			CoinCalculation.m_instance.AddToken (1);
			ReActivateCoins (other.gameObject);
			break;
		case "speedUp":
			SpeedUpPlayer ();
			break;
		case "balloonStart":			
			TutorialManager.m_instance.ShowBalloonTutorial ();
			isEnableFlappy = true;
			other.gameObject.SetActive (false);
			ReActivateCoins (other.gameObject);
			SwitchBalloon ();
			break;
		case "balloonEnd":
			isEnableFlappy = false;
			SwitchBalloon ();
			break;
		case "miniCarStart":
			isInCar = true;
			MiniCarSequenceStart (other.gameObject);
			break;
		case "miniCarEnd":			
			isInCar = false;
			MiniCarSequenceEnd ();
			break;
		default:
			break;
		}
	}

	void OnTriggerStay (Collider other)
	{
		if (other.gameObject.tag == "airJump") {
			inAirJumpBox = true;
		}
	}

	void OnTriggerExit (Collider other)
	{
		if (other.gameObject.tag == "airJump") {
			inAirJumpBox = false;
		}
	}

	void MiniCarSequenceStart (GameObject miniCar)
	{
		miniCar.GetComponent<BoxCollider> ().enabled = false;
		miniCar.transform.parent = transform;
		/*float view = 15;
		Camera.main.fieldOfView = view;*/
	}

	void MiniCarSequenceEnd ()
	{
		/*GameObject miniCarStartBlock = null;
		miniCarStartBlock = GameObject.Find ("d2mc0");
		print (miniCarStartBlock.name);*/
		//miniCar.transform.parent = transform;
		foreach (Transform miniCar in transform) {
			if (miniCar.CompareTag ("miniCarStart")) {
				miniCar.gameObject.SetActive (false);
				//miniCar.transform.parent = miniCarStartBlock.transform;
			}
		}
	}

	void SwitchBalloon ()
	{
		if (isEnableFlappy) {
			balloonGO.SetActive (true);
			skateboardGO.SetActive (false);
			//blobShadowGO.SetActive (false);
		} else {
			balloonGO.SetActive (false);
			skateboardGO.SetActive (true);
			//blobShadowGO.SetActive (true);
		}
	}

	public void SetCharacterControllerCollisionStatus (bool active)
	{
		c.detectCollisions = active;
		GetComponent<BoxCollider> ().enabled = active;
	}

	void DisplayPlayerObject (bool isActive)
	{
		playerMesh.SetActive (isActive);
		blobProjector.gameObject.SetActive (isActive);
		skateboardGO.SetActive (isActive);
		if (isEnableFlappy) {
			balloonGO.SetActive (isActive);
			skateboardGO.SetActive (false);
		}
	}

	void PlayerPartiallyDied ()
	{
		if (isBlinking) {
			return;
		}
		IGMLogic.m_instance.StartCoroutine ("Shake");
		isDeath = true;
		June.LocalStore.Instance.SetInt ("PlayerDeath", June.LocalStore.Instance.GetInt ("PlayerDeath") + 1);
		dieSound.Play ();
		PlayParticle (playerDieParticle);
		DisplayPlayerObject (false);
		if (transform.root.position.x >= 50) { // && isDiedOnce == false			
			GameEventManager.SetState (GameEventManager.E_STATES.e_pause);
			StopCoroutine ("PlayerDiedStartTimer");
			StartCoroutine ("PlayerDiedStartTimer");
		} else {
			PlayerDiedCalculateStats ();
		}
	}

	public void PlayerDiedCalculateStats ()
	{
		DisplayPlayerObject (false);
		IGMLogic.m_instance.pauseButton.SetActive (false);
		June.LocalStore.Instance.SetInt ("PlayerTotalJumps", June.LocalStore.Instance.GetInt ("PlayerTotalJumps") + jumpCount);
		June.LocalStore.Instance.SetInt ("Mission_JumpCount", June.LocalStore.Instance.GetInt ("Mission_JumpCount") + jumpCount);
		GameEventManager.SetState (GameEventManager.E_STATES.e_pause);

		MissionLogic.m_instance.CheckIfMissionCompleted ();

		if (MovingPlatform.m_instance.isHighScore ()) {
			IGMLogic.m_instance.ShowNewHighScoreMenu (); 
		} else {
			ShowPromoBanner ();
		}		
		//*********************************************************************************************  End session
	}

	public void ShowPromoBanner ()
	{
		MissionLogic.m_instance.CheckIfMissionCompleted ();
		IGMLogic.m_instance.ShowPlayButton ();
		PromoStripsManager.m_instance.ShowPromoStrips ();
	}

	IEnumerator PlayerDiedStartTimer ()
	{
		counDownText.gameObject.SetActive (true);
		continueText.gameObject.SetActive (true);

		if (coinMultipler < coinAskList.Length) {
			coinsToAsk = coinAskList [coinMultipler];
			IGMLogic.m_instance.ShowPayCoinToContinueMenu (coinsToAsk);
		} else {
			IGMLogic.m_instance.ShowPayCoinToContinueMenu (5);
		}

		counDownText.text = "5";
		yield return new WaitForSeconds (1);
		counDownText.text = "4";
		yield return new WaitForSeconds (1);
		counDownText.text = "3";
		yield return new WaitForSeconds (1);
		counDownText.text = "2";
		yield return new WaitForSeconds (1);
		counDownText.text = "1";
		yield return new WaitForSeconds (1);
		counDownText.text = "";
		counDownText.gameObject.SetActive (false);
		continueText.gameObject.SetActive (false);
		IGMLogic.m_instance.ClosePayToContinueMenu ();
		PlayerDiedCalculateStats ();
	}

	public void UserPayingForCoins ()
	{
		if (coinsToAsk <= 5) {
			if (coinsToAsk <= June.LocalStore.Instance.GetInt ("tokens")) {
				UserHadAndPaidCoins ();
			} else {
				AskForMoreCoins ();
			}
		} else {
			if (coinsToAsk <= June.LocalStore.Instance.GetInt ("coins")) {				
				UserHadAndPaidCoins ();
			} else {
				AskForMoreCoins ();
			}
		}
	}

	public void UserWatchingAdsForResume ()
	{
		if (June.VideoAds.VideoAdManager.IsReady) {
			//IGMLogic.m_instance.PauseGame ();
		}
		//print ("June.VideoAds.VideoAdManager.IsReady : " + June.VideoAds.VideoAdManager.IsReady);
		June.MessageBroker.Publish (June.Messages.ResumeWatchAdTap, null);
		//#if !UNITY_EDITOR
		//print ("[MissionToast] WinPrizeButtonOnTap Show Ad");
		bool showingAd = June.VideoAds.VideoAdManager.Show (status => {
			//print ("[MissionToast] VideoAdManager.Show Callback hasCompleted:" + status);
			//	AudioListener.pause = false;
			if (status) {
				//Etcetera.ShowAlert ("Coins", "You got " + GameConfig.CoinsForVideoAd + " coins!", "Awesome", (buttonText) => {} );		
				UserHadWatchedVideoAd ();
				//print (" status : " + status);
			} else {
				Etcetera.ShowAlert ("", "You need to watch the entire video to get your reward.", "OK");
				//print (" status : " + status);
				//IGMLogic.m_instance.ResumeGame ();
			}
		});
	}

	public void UserWatchingAdsForCoins ()
	{
		//IGMLogic.m_instance.PauseGame ();
		//print (" June.VideoAds.VideoAdManager.IsReady : " + June.VideoAds.VideoAdManager.IsReady);
		June.MessageBroker.Publish (June.Messages.ResumeWatchAdTap, null);
		//#if !UNITY_EDITOR
		//print ("[MissionToast] WinPrizeButtonOnTap Show Ad");
		bool showingAd = June.VideoAds.VideoAdManager.Show (status => {
			//print ("[MissionToast] VideoAdManager.Show Callback hasCompleted:" + status);
			//	AudioListener.pause = false;
			if (status) {
				//Etcetera.ShowAlert ("Coins", "You got " + GameConfig.CoinsForVideoAd + " coins!", "Awesome", (buttonText) => {} );		
				CoinCalculation.m_instance.AddCoins (30);
				//print (" status : " + status);
			} else {
				Etcetera.ShowAlert ("", "You need to watch the entire video to get your reward.", "OK");
				//print (" status : " + status);
				//IGMLogic.m_instance.ResumeGame ();
			}
		});
	}

	public void UserHadWatchedVideoAd ()
	{
		wantToContinueMenu.SetActive (false);
		//IGMLogic.m_instance.ResumeGame ();
		GameEventManager.SetState (GameEventManager.E_STATES.e_game);
		StopCoroutine ("PlayerDiedStartTimer");
		counDownText.gameObject.SetActive (false);
		continueText.gameObject.SetActive (false);
		StartCoroutine ("EnablePlayersColliderAfterWait");
		isDeath = false;
	}

	void UserHadAndPaidCoins ()
	{
		coinMultipler++;
		StopCoroutine ("PlayerDiedStartTimer");
		counDownText.gameObject.SetActive (false);
		continueText.gameObject.SetActive (false);
		StartCoroutine ("EnablePlayersColliderAfterWait");
		if (coinsToAsk <= 5) {
			June.LocalStore.Instance.SetInt ("tokens", June.LocalStore.Instance.GetInt ("tokens") - coinsToAsk);
		} else {
			June.LocalStore.Instance.SetInt ("coins", June.LocalStore.Instance.GetInt ("coins") - coinsToAsk);
		}
		CoinCalculation.m_instance.UpdateCurrencyOnUI ();
		isDeath = false;
		IGMLogic.m_instance.pauseButton.SetActive (true);
	}

	void AskForMoreCoins ()
	{
		IGMLogic.m_instance.ShowInGameStoreMenu ();
	}

	IEnumerator EnablePlayersColliderAfterWait ()
	{
		transform.localPosition = new Vector3 (1, 6f, 0);
		playerSupport.SetActive (true);
		StartCoroutine ("BlinkCharacter", 10);
		isBlinking = true;
		//SetCharacterControllerCollisionStatus (false);
		GameEventManager.SetState (GameEventManager.E_STATES.e_game);
		IGMLogic.m_instance.ClosePayToContinueMenu ();
		yield return new WaitForSeconds (2);
		playerSupport.SetActive (false);
		isBlinking = false;
		//SetCharacterControllerCollisionStatus (true);
		//diedCounter++;
	}

	/*void LevelFinished ()
	{
		IGMLogic.m_instance.ShowLevelCompleteMenu (jumpCount);
	}*/

	void SpeedUpPlayer ()
	{
		speedUpParticle.Play ();
		transform.parent.GetComponent<MovingPlatform> ().SpeedUp ();
	}

	void ReActivateCoins (GameObject coinGO)
	{
		coinGO.SetActive (false);
		StartCoroutine ("ActivateCoin", coinGO);
	}

	IEnumerator ActivateCoin (GameObject coinGo)
	{
		//yield return new WaitForSeconds (0.25f);
		//coinParticle.Stop ();
		yield return new WaitForSeconds (2.5f);
		if (GameEventManager.GetState () == GameEventManager.E_STATES.e_game) {			
			coinGo.SetActive (true);
		} else {
			StartCoroutine ("ActivateCoin", coinGo);
		}
	}

	IEnumerator showTurotialTapnHold ()
	{
		tapnHoldText.SetActive (true);
		yield return new WaitForSeconds (3);
		tapnHoldText.SetActive (false);
	}

	IEnumerator small ()
	{
		LeanTween.cancel (playerMesh);
		LeanTween.scale (playerMesh, new Vector3 (1.15f, 1.15f, 0.75f), 0.25f, optional);
		yield return new WaitForSeconds (0.25f);
		LeanTween.scale (playerMesh, iniScale, 0.15f, optional);
	}

	IEnumerator big ()
	{
		LeanTween.cancel (playerMesh);
		LeanTween.scale (playerMesh, new Vector3 (0.9f, 0.9f, 1.25f), 0.25f, optional);
		yield return new WaitForSeconds (0.25f);
		LeanTween.scale (playerMesh, iniScale, 0.15f, optional);
	}

	IEnumerator rotate ()
	{
		LeanTween.cancel (playerMesh);
		LeanTween.rotateX (playerMesh, 620, 0.5f, optional);
		yield return new WaitForSeconds (0.15f);
		LeanTween.rotateX (playerMesh, 270, 0f, optional);
	}

}