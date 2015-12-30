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
	public GameObject playerMesh, board, skateboardGO, balloonGO, blobShadowGO;
	public ParticleSystem playerDieParticle, speedUpParticle;
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
	CharacterController c;
	int diedCounter = 0;
	int coinsToAsk = 0;
	int lastBest = 0;
	bool isEnableFlappy = false;
	public static ManJump m_instance = null;
	public Text debugText;

	//public GameObject playerGO
	void Awake ()
	{
		m_instance = this;
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
	}

	void Update ()
	{
		if (GameEventManager.GetState () == GameEventManager.E_STATES.e_game) {			
			if (controller.isGrounded || inAirJumpBox || isEnableFlappy) {
				if (Input.GetMouseButton (0) && !EventSystem.current.IsPointerOverGameObject () || Input.GetKey (KeyCode.Space)) {
					jumpCount++;
					/*if (jumpCount >= 20) {
						Social.ReportProgress ("CgkIqM2wutYIEAIQBA", 20, (bool success) => {
						});			 
					}*/
					jumpSpeed = isEnableFlappy ? jumpSpeedTemp / 3 : jumpSpeedTemp; //shortest if
					moveDirection.y = jumpSpeed;
				}
				if (tempJump == true) {
					StartCoroutine ("BlinkCharacter");
					tempJump = false;
					life--;
					board.GetComponent<TextureScroll> ().xScrollSpeed = life;
					jumpCount++;
					moveDirection.y = jumpSpeed;
				}
				//http://docs.unity3d.com/ScriptReference/CharacterController.Move.html
				if (controller.velocity.normalized == Vector3.down) {
					//StartCoroutine ("small");
					//g = gravity / 2;
				}
			}			
			gravity = isEnableFlappy ? gravityTemp / 2f : gravityTemp; //shortest if
			moveDirection.y -= gravity * Time.deltaTime * 1.1f;
			controller.Move (moveDirection * Time.deltaTime);
			transform.localPosition = new Vector3 (1, transform.localPosition.y, 0);
			transform.localRotation = new Quaternion (0, 0, 0, 0);
			//print ("repo");
		}
		if (Input.GetMouseButtonDown (2)) {
			//Application.LoadLevel (Application.loadedLevel);
			SceneManager.LoadSceneAsync ("level");
		}
	}

	IEnumerator BlinkCharacter (int times)
	{
		for (int i = 0; i < times; i++) {
			playerMesh.SetActive (false);
			skateboardGO.SetActive (false);
			yield return new WaitForSeconds (0.1f);
			playerMesh.SetActive (true);
			skateboardGO.SetActive (true);
			yield return new WaitForSeconds (0.1f);
		}
	}

	void OnTriggerEnter (Collider other)
	{
		debugText.text = other.name;
		if (other.gameObject.tag == "death") {
			playerPartiallyDied ();
		}
		if (other.gameObject.tag == "pickable_coin") {
			CoinCalculation.m_instance.AddCoins (1);
			ReActivateCoins (other.gameObject);
		}
		if (other.gameObject.tag == "levelEnd") {
			LevelFinished ();
		}
		if (other.gameObject.tag == "speedUp") {
			SpeedUpPlayer ();
		}
		if (other.gameObject.tag == "tutorialTapnHold") {
			StartCoroutine ("showTurotialTapnHold");
		}
		if (other.gameObject.tag == "balloonStart") {			
			isEnableFlappy = true;
			other.gameObject.SetActive (false);
			ReActivateCoins (other.gameObject);
			SwitchBalloon ();
		}
		if (other.gameObject.tag == "balloonEnd") {
			isEnableFlappy = false;
			SwitchBalloon ();
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
			print (inAirJumpBox);
		}
	}

	void SwitchBalloon ()
	{
		if (isEnableFlappy) {
			balloonGO.SetActive (true);
			skateboardGO.SetActive (false);
			blobShadowGO.SetActive (false);
		} else {
			balloonGO.SetActive (false);
			skateboardGO.SetActive (true);
			blobShadowGO.SetActive (true);
		}
	}



	public void SetCharacterControllerCollisionStatus (bool active)
	{
		c.detectCollisions = active;
		GetComponent<BoxCollider> ().enabled = active;
	}

	void playerPartiallyDied ()
	{
		lastBest = PlayerPrefs.GetInt ("lastBestScore");
		PlayerPrefs.SetInt ("PlayerDeath", PlayerPrefs.GetInt ("PlayerDeath") + 1);
		dieSound.Play ();
		playerDieParticle.Play ();
		playerMesh.SetActive (false);
		skateboardGO.SetActive (false);
		blobShadowGO.SetActive (false);
		lastBest = (lastBest / 10) * 3; //***************Pay coins to continue appears only if score is > 60% of best score... change 6
		if (lastBest < 100) {
			lastBest = 100;
		}
		if (transform.root.position.x >= lastBest && diedCounter <= 2) {
			diedCounter++;
			GameEventManager.SetState (GameEventManager.E_STATES.e_pause);
			StopCoroutine ("PlayerDiedStartTimer");
			StartCoroutine ("PlayerDiedStartTimer");
		} else {
			playerDied ();
		}
	}

	void playerDied ()
	{
		PlayerPrefs.SetInt ("PlayerTotalJumps", PlayerPrefs.GetInt ("PlayerTotalJumps") + jumpCount);

		SetCharacterControllerCollisionStatus (false);
		dieSound.Play ();
		playerDieParticle.Play ();
		playerMesh.SetActive (false);
		skateboardGO.SetActive (false);
		blobShadowGO.SetActive (false);
		IGMLogic.m_instance.KillPlayer ();
	}

	IEnumerator PlayerDiedStartTimer ()
	{
		counDownText.gameObject.SetActive (true);
		continueText.gameObject.SetActive (true);
		coinMultipler++;
		coinsToAsk = coinMultipler * 20;
		IGMLogic.m_instance.ShowPayToContinueMenu (coinsToAsk);
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
		if (coinsToAsk <= PlayerPrefs.GetInt ("Coins")) {
			UserHadAndPaidCoins ();
		} else {
			AskForMoreCoins ();
		}
	}

	void AskForMoreCoins ()
	{

	}

	void UserHadAndPaidCoins ()
	{
		StopCoroutine ("PlayerDiedStartTimer");
		counDownText.gameObject.SetActive (false);
		continueText.gameObject.SetActive (false);
		StartCoroutine ("EnablePlayersColliderAfterWait");
		PlayerPrefs.SetInt ("Coins", PlayerPrefs.GetInt ("Coins") - coinsToAsk);
		CoinCalculation.m_instance.UpdateCoinsOnUI ();
	}

	IEnumerator EnablePlayersColliderAfterWait ()
	{
		StartCoroutine ("BlinkCharacter", 10);
		SetCharacterControllerCollisionStatus (false);
		GameEventManager.SetState (GameEventManager.E_STATES.e_game);
		IGMLogic.m_instance.ClosePayToContinueMenu ();
		yield return new WaitForSeconds (2);
		SetCharacterControllerCollisionStatus (true);
		diedCounter++;
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