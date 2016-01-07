using UnityEngine;
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
	public GameObject playerMesh, playerSupport, board, skateboardGO, balloonGO, blobShadowGO;
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
	CharacterController c;
	int diedCounter = 0;
	int coinsToAsk = 0;
	int lastBest = 0;
	bool isEnableFlappy = false;
	public static ManJump m_instance = null;
	public Text debugText;
	bool isDeath = false;

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
		//Vector3 v3Pos = new Vector3 (0.0f, 1.0f, 0.25f);
		//transform.position = Camera.main.ViewportToWorldPoint (v3Pos);// gui.camera.ViewportToWorldPoint(v3Pos);
	}

	void Update ()
	{
		if (GameEventManager.GetState () == GameEventManager.E_STATES.e_game) {			
			if (controller.isGrounded || inAirJumpBox || isEnableFlappy) {
				if (Input.GetMouseButton (0) && !EventSystem.current.IsPointerOverGameObject () || Input.GetKey (KeyCode.Space)) {
					if (!isEnableFlappy) {
						jumpCount++;
					}
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
					if (!isEnableFlappy) {
						jumpCount++;
					}
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
			if (transform.localPosition.y < -5) {
				print ("dued");
				playerPartiallyDied ();
			}

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
			/*playerMesh.SetActive (false);
			skateboardGO.SetActive (false);*/
			DisplayPlayerObject (false);
			yield return new WaitForSeconds (0.1f);
			/*playerMesh.SetActive (true);
			skateboardGO.SetActive (true);*/
			DisplayPlayerObject (true);
			yield return new WaitForSeconds (0.1f);
		}
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.tag == "death" && !isDeath) {
			playerPartiallyDied ();
		}
		if (other.gameObject.tag == "pickable_coin") {
			IGMLogic.m_instance.anim.CrossFadeInFixedTime ("coinScale", 0.2f);
			coinParticle.Play ();
		
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
			PlayerPrefs.SetInt ("Mission_BalloonCount", PlayerPrefs.GetInt ("Mission_BalloonCount") + 1);
			print ("BalloonUsed");
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

	void DisplayPlayerObject (bool isActive)
	{
		playerMesh.SetActive (isActive);
		skateboardGO.SetActive (isActive);
		blobShadowGO.SetActive (isActive);
		if (isEnableFlappy) {
			balloonGO.SetActive (isActive);
		}
	}

	void playerPartiallyDied ()
	{
		isDeath = true;
		lastBest = PlayerPrefs.GetInt ("lastBestScore");
		PlayerPrefs.SetInt ("PlayerDeath", PlayerPrefs.GetInt ("PlayerDeath") + 1);
		dieSound.Play ();
		playerDieParticle.Play ();
		DisplayPlayerObject (false);
		lastBest = (lastBest / 10) * 3; //***************Pay coins to continue appears only if score is > 60% of best score... change 6
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
		}
	}

	void playerDied ()
	{
		PlayerPrefs.SetInt ("PlayerTotalJumps", PlayerPrefs.GetInt ("PlayerTotalJumps") + jumpCount);
		PlayerPrefs.SetInt ("Mission_JumpCount", PlayerPrefs.GetInt ("Mission_JumpCount") + jumpCount);
		SetCharacterControllerCollisionStatus (false);
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
		isDeath = false;
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
		SetCharacterControllerCollisionStatus (false);
		GameEventManager.SetState (GameEventManager.E_STATES.e_game);
		IGMLogic.m_instance.ClosePayToContinueMenu ();
		yield return new WaitForSeconds (2);
		playerSupport.SetActive (false);
		SetCharacterControllerCollisionStatus (true);
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