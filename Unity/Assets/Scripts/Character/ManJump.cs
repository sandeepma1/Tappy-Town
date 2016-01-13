﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ManJump : MonoBehaviour
{
	public float jumpSpeed = 21.0F;
	public float gravity = 96.0F;
	float jumpSpeedTemp = 0;
	float gravityTemp = 0;
	private Vector3 moveDirection = Vector3.zero;
	CharacterController controller;
	public GameObject playerMesh, playerSupport, board, skateboardGO, balloonGO, blobProjector;
	public ParticleSystem playerDieParticle, speedUpParticle, coinParticle;
	public Text counDownText, continueText;
	Hashtable optional;
	public GameObject igmLogic;
	public AudioSource dieSound;
	int jumpCount = 0;
	bool inAirJumpBox = false;
	GameObject character;
	bool tempJump = false;
	int life = 0;
	public GameObject tapnHoldText;
	Vector3 iniScale;
	int coinMultipler = 0;
	int[] coinAskList;
	CharacterController c;
	int diedCounter = 0;
	int coinsToAsk = 0;
	int lastBest = 0;
	bool isEnableFlappy = false;
	public static ManJump m_instance = null;
	public Text debugText;
	bool isDeath = false;
	bool isInCar = false;
	bool isBlinking = false;

	float speed = 10, s = 3.4f, iniSpeed;


	//public GameObject playerGO
	void Awake ()
	{
		
		m_instance = this;
		playerSupport.SetActive (false);
		SwitchBalloon ();
		jumpSpeedTemp = jumpSpeed;
		gravityTemp = gravity;
		c = GetComponent<CharacterController> ();
		board.GetComponent<TextureScroll> ().xScrollSpeed = life;
		dieSound = GetComponent<AudioSource> ();
		string charName = PlayerPrefs.GetString ("currentCharacterSelected", "chr_mailman");
		playerMesh = Instantiate (Resources.Load ("Characters/" + charName) as GameObject);
		playerMesh.transform.parent = this.transform;
		playerMesh.transform.localPosition = new Vector3 (0, -0.5f, 0.05f);
		playerMesh.transform.localEulerAngles = new Vector3 (270, 315, 0);
	}

	void Start ()
	{
		controller = GetComponent<CharacterController> ();
		iniScale = playerMesh.transform.localScale;
		optional = new Hashtable ();
		optional.Add ("ease", LeanTweenType.easeOutBack);
		iniSpeed = MovingPlatform.m_instance.moveInSeconds;
		coinAskList = new int[6];
		coinAskList [0] = 100;
		coinAskList [1] = 300;
		coinAskList [2] = 500;
		coinAskList [3] = 700;
		coinAskList [4] = 1000;
		coinAskList [5] = 5;

		//Vector3 v3Pos = new Vector3 (0.0f, 1.0f, 0.25f);
		//transform.position = Camera.main.ViewportToWorldPoint (v3Pos);// gui.camera.ViewportToWorldPoint(v3Pos);
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
		//*************************************  Car mMechanics ******************************

		if (GameEventManager.GetState () == GameEventManager.E_STATES.e_game) {			
			if (controller.isGrounded || inAirJumpBox || isEnableFlappy) {				
				if (Input.GetMouseButton (0) && !EventSystem.current.IsPointerOverGameObject () || Input.GetKey (KeyCode.Space)) {					
					if (!isEnableFlappy) {
						jumpCount++;
					}
					jumpSpeed = isEnableFlappy ? jumpSpeedTemp / 3 : jumpSpeedTemp; //shortest if
					moveDirection.y = jumpSpeed;
				}

				/*if (tempJump == true) {// double balloon or pro skate
					StartCoroutine ("BlinkCharacter");
					tempJump = false;
					life--;
					board.GetComponent<TextureScroll> ().xScrollSpeed = life;
					if (!isEnableFlappy) {
						jumpCount++;
					}
					moveDirection.y = jumpSpeed;
				}
				//http://docs.unity3d.com/ScriptReference/CharacterController.Move.html
				if (controller.velocity.normalized == Vector3.down) {
					//StartCoroutine ("small");
					//g = gravity / 2;
				}*/
			}			
			gravity = isEnableFlappy ? gravityTemp / 2f : gravityTemp; //shortest if

			moveDirection.y -= gravity * Time.deltaTime * 1f;
			controller.Move (moveDirection * Time.deltaTime);
			transform.localPosition = new Vector3 (1, transform.localPosition.y, 0);
			transform.localRotation = new Quaternion (0, 0, 0, 0);
			if (transform.localPosition.y < -5) {
				playerPartiallyDied ();
			}
		}
		if (Input.GetMouseButtonDown (2)) {
			SceneManager.LoadSceneAsync ("level");
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
			if (!isDeath) {
				playerPartiallyDied ();
			}
			break;
		case "pickable_coin":
			IGMLogic.m_instance.anim.CrossFadeInFixedTime ("coinScale", 0.2f);
			coinParticle.Play ();		
			CoinCalculation.m_instance.AddCoins (1);
			ReActivateCoins (other.gameObject);
			break;
		case "pickable_token":
			//IGMLogic.m_instance.anim.CrossFadeInFixedTime ("coinScale", 0.2f);
			coinParticle.Play ();		
			CoinCalculation.m_instance.AddToken (1);
			ReActivateCoins (other.gameObject);
			break;
		case "speedUp":
			SpeedUpPlayer ();
			break;
		case "balloonStart":
			PlayerPrefs.SetInt ("Mission_BalloonCount", PlayerPrefs.GetInt ("Mission_BalloonCount") + 1);
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
		/*if (other.gameObject.tag == "death" && !isDeath) {
			playerPartiallyDied ();
		}
		if (other.gameObject.tag == "pickable_coin") {
			IGMLogic.m_instance.anim.CrossFadeInFixedTime ("coinScale", 0.2f);
			coinParticle.Play ();		
			CoinCalculation.m_instance.AddCoins (1);
			ReActivateCoins (other.gameObject);
		}
		if (other.gameObject.tag == "speedUp") {
			SpeedUpPlayer ();
		}
		if (other.gameObject.tag == "balloonStart") {		
			PlayerPrefs.SetInt ("Mission_BalloonCount", PlayerPrefs.GetInt ("Mission_BalloonCount") + 1);
			isEnableFlappy = true;
			other.gameObject.SetActive (false);
			ReActivateCoins (other.gameObject);
			SwitchBalloon ();
		}
		if (other.gameObject.tag == "balloonEnd") {
			isEnableFlappy = false;
			SwitchBalloon ();
		}
		if (other.gameObject.tag == "miniCar") {
			isInCar = true;
			MiniCarSequenceStart (other.gameObject);
		}*/
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

	void playerPartiallyDied ()
	{
		if (isBlinking) {
			return;
		}
		isDeath = true;
		lastBest = PlayerPrefs.GetInt ("lastBestScore");
		PlayerPrefs.SetInt ("PlayerDeath", PlayerPrefs.GetInt ("PlayerDeath") + 1);
		dieSound.Play ();
		playerDieParticle.Play ();
		DisplayPlayerObject (false);
		// ******************************************* For Monday build remove this
		if (transform.root.position.x >= 50) {
			GameEventManager.SetState (GameEventManager.E_STATES.e_pause);
			StopCoroutine ("PlayerDiedStartTimer");
			StartCoroutine ("PlayerDiedStartTimer");
		} else {
			playerDied ();
		}		
		// ******************************************* For Monday build remove this

		/*lastBest = (lastBest / 10) * 3; //***************Pay coins to continue appears only if score is > 60% of best score... change 6
		if (lastBest < 100) {
			lastBest = 100;
		}
		if (transform.root.position.x >= lastBest && diedCounter < 1) { //number of time to ask for coins/videoads if dies			
			diedCounter++;
			GameEventManager.SetState (GameEventManager.E_STATES.e_pause);
			StopCoroutine ("PlayerDiedStartTimer");
			StartCoroutine ("PlayerDiedStartTimer");
		} else {
			playerDied ();
		}*/
	}

	void playerDied ()
	{
		PlayerPrefs.SetInt ("PlayerTotalJumps", PlayerPrefs.GetInt ("PlayerTotalJumps") + jumpCount);
		PlayerPrefs.SetInt ("Mission_JumpCount", PlayerPrefs.GetInt ("Mission_JumpCount") + jumpCount);
		//SetCharacterControllerCollisionStatus (false);
		dieSound.Play ();
		playerDieParticle.Play ();
		DisplayPlayerObject (false);
		IGMLogic.m_instance.KillPlayer ();
		if (MissionLogic.m_instance.CheckIfMissionCompleted ()) {
			MissionLogic.m_instance.MissionCompleted ();
			print ("MissionCompleted");
		}
	}

	IEnumerator PlayerDiedStartTimer ()
	{
		counDownText.gameObject.SetActive (true);
		continueText.gameObject.SetActive (true);

		if (coinMultipler < coinAskList.Length) {
			coinsToAsk = coinAskList [coinMultipler];
			coinMultipler++;
			IGMLogic.m_instance.ShowPayCoinToContinueMenu (coinsToAsk);
		} else {
			IGMLogic.m_instance.ShowPayCoinToContinueMenu (5);
		}

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
		playerDied ();
	}

	public void UserPayingForCoins ()
	{
		if (coinsToAsk <= 5) {
			if (coinsToAsk <= PlayerPrefs.GetInt ("Token")) {
				UserHadAndPaidCoins ();
			} else {
				AskForMoreCoins ();
			}
		} else {
			if (coinsToAsk <= PlayerPrefs.GetInt ("Coins")) {
				UserHadAndPaidCoins ();
			} else {
				AskForMoreCoins ();
			}
		}
	}

	void AskForMoreCoins ()
	{
		// In App Purchase window
	}

	void UserHadAndPaidCoins ()
	{
		StopCoroutine ("PlayerDiedStartTimer");
		counDownText.gameObject.SetActive (false);
		continueText.gameObject.SetActive (false);
		StartCoroutine ("EnablePlayersColliderAfterWait");
		if (coinsToAsk <= 5) {
			PlayerPrefs.SetInt ("Token", PlayerPrefs.GetInt ("Token") - coinsToAsk);
		} else {
			PlayerPrefs.SetInt ("Coins", PlayerPrefs.GetInt ("Coins") - coinsToAsk);
		}
		CoinCalculation.m_instance.UpdateCurrencyOnUI ();
		isDeath = false;
		IGMLogic.m_instance.pauseButton.SetActive (true);
	}

	public void UserHadWatchedVideoAd ()
	{
		StopCoroutine ("PlayerDiedStartTimer");
		counDownText.gameObject.SetActive (false);
		continueText.gameObject.SetActive (false);
		StartCoroutine ("EnablePlayersColliderAfterWait");
		isDeath = false;
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

	void LevelFinished ()
	{
		IGMLogic.m_instance.ShowLevelCompleteMenu (jumpCount);
	}

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
		yield return new WaitForSeconds (2.5f);
		coinGo.SetActive (true);
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