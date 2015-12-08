using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ManJump : MonoBehaviour
{
	public float jumpSpeed = 21.0F;
	public float gravity = 96.0F;
	private Vector3 moveDirection = Vector3.zero;
	CharacterController controller;
	public GameObject playerMesh, skateBoardGO, board;
	public ParticleSystem playerDieParticle, speedUpParticle;
	public GameObject skateboardGO, playerShadowQuad;
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
	float g = 0;
	CharacterController c;
	int diedCounter = 0;
	int coinsToAsk = 0;
	int lastBest = 0;
	//public GameObject playerGO
	void Awake ()
	{
		c = GetComponent<CharacterController> ();
		board.GetComponent<TextureScroll> ().xScrollSpeed = life;
		dieSound = GetComponent<AudioSource> ();

		string charName =  PlayerPrefs.GetString ("currentCharacterSelected", "chr_raver3");
		character = Instantiate (Resources.Load ("Characters/" + charName) as GameObject);
		character.transform.parent = this.transform;
		character.transform.localPosition = new Vector3 (0, -0.5f, 0);
		character.transform.localEulerAngles = new Vector3 (270, 315, 0);
		lastBest = PlayerPrefs.GetInt ("lastBestScore");
	}
	void Start ()
	{
		foreach (Transform trans in this.transform) {
			if (trans.name.StartsWith ("chr_")) {
				playerMesh = trans.gameObject;
			}
		}
		controller = GetComponent<CharacterController> ();
		iniScale = playerMesh.transform.localScale;
		optional = new Hashtable ();
		optional.Add ("ease", LeanTweenType.easeOutBack);
	}
	void Update ()
	{
		if (GameEventManager.GetState () == GameEventManager.E_STATES.e_game) {
			//g = gravity;
			if (controller.isGrounded || inAirJumpBox) {
				if (Input.GetMouseButton (0) && !EventSystem.current.IsPointerOverGameObject () || Input.GetKey (KeyCode.Space)) {
					jumpCount++;
					if (jumpCount >= 20) {
						Social.ReportProgress ("CgkIqM2wutYIEAIQBA", 20, (bool success) => {
						});			 
					}
					moveDirection.y = jumpSpeed;
					//g = gravity;
				}
				if (tempJump == true) {
					StartCoroutine ("BlinkCharacter");
					tempJump = false;
					life--;
					board.GetComponent<TextureScroll> ().xScrollSpeed = life;
					jumpCount++;
					moveDirection.y = jumpSpeed;
					//g = gravity;
				}
				//http://docs.unity3d.com/ScriptReference/CharacterController.Move.html
				if (controller.velocity.normalized == Vector3.down) {
					//StartCoroutine ("small");
					//g = gravity / 2;
				}
			}
			moveDirection.y -= gravity * Time.deltaTime * 1.1f;
			controller.Move (moveDirection * Time.deltaTime);
			transform.localPosition = new Vector3 (1, transform.localPosition.y, 0);
			transform.localRotation = new Quaternion (0, 0, 0, 0);
			//print ("repo");
		}
		if (Input.GetMouseButtonDown (2)) {
			Application.LoadLevel (Application.loadedLevel);
		}
	}

	IEnumerator BlinkCharacter (int times)
	{
		for (int i = 0; i < times; i++) {
			playerMesh.SetActive (false);
			skateBoardGO.SetActive (false);
			yield return new WaitForSeconds (0.1f);
			playerMesh.SetActive (true);
			skateBoardGO.SetActive (true);
			yield return new WaitForSeconds (0.1f);
		}
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.tag == "death") {
			playerPartiallyDied ();
		}
		if (other.gameObject.tag == "pickable_coin") {
			igmLogic.GetComponent<CoinCalculation> ().AddCoins (1);
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
	}

	void ReActivateCoins (GameObject coinGO)
	{
		coinGO.SetActive (false);
		StartCoroutine ("ActivateCoin", coinGO);
	}

	IEnumerator ActivateCoin (GameObject coinGo)
	{
		yield return new WaitForSeconds (1);
		coinGo.SetActive (true);
	}

	IEnumerator showTurotialTapnHold ()
	{
		tapnHoldText.SetActive (true);
		yield return new WaitForSeconds (3);
		tapnHoldText.SetActive (false);
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

	void playerDied ()
	{
		/*if (life > 0) {
			tempJump = true;
		} else {*/
		SetCharacterControllerCollisionStatus (false);
		dieSound.Play ();
		playerDieParticle.Play ();
		playerMesh.SetActive (false);
		skateboardGO.SetActive (false);
		playerShadowQuad.SetActive (false);
		igmLogic.GetComponent<IGMLogic> ().KillPlayer ();
		transform.parent.GetComponent<MovingPlatform> ().lastBestFun (false);
		//}
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
		print (PlayerPrefs.GetInt ("Coins"));
		PlayerPrefs.SetInt ("Coins", PlayerPrefs.GetInt ("Coins") - coinsToAsk);
		igmLogic.GetComponent<CoinCalculation> ().UpdateCoinsOnUI ();
		print (PlayerPrefs.GetInt ("Coins"));
	}

	IEnumerator EnablePlayersColliderAfterWait ()
	{
		StartCoroutine ("BlinkCharacter", 10);
		SetCharacterControllerCollisionStatus (false);
		GameEventManager.SetState (GameEventManager.E_STATES.e_game);
		igmLogic.GetComponent<IGMLogic> ().ClosePayToContinueMenu ();
		yield return new WaitForSeconds (2);
		SetCharacterControllerCollisionStatus (true);
		diedCounter++;
	}

	public void SetCharacterControllerCollisionStatus (bool active)
	{
		c.detectCollisions = active;
		GetComponent<BoxCollider> ().enabled = active;
	}

	void playerPartiallyDied ()
	{
		dieSound.Play ();
		playerDieParticle.Play ();
		playerMesh.SetActive (false);
		skateboardGO.SetActive (false);
		playerShadowQuad.SetActive (false);

		lastBest = lastBest / 10;
		lastBest = lastBest * 6;//***************Pay coins to continue appears only if score is > 60% of best score... change 6
		if (lastBest < 100) {
			lastBest = 100;
		}
		if (transform.root.position.x >= lastBest && diedCounter < 2) {
			print ("Died Times: " + diedCounter);
			GameEventManager.SetState (GameEventManager.E_STATES.e_pause);
			StopCoroutine ("PlayerDiedStartTimer");
			StartCoroutine ("PlayerDiedStartTimer");
		} else {
			playerDied ();
		}

	}

	IEnumerator PlayerDiedStartTimer ()
	{
		counDownText.gameObject.SetActive (true);
		continueText.gameObject.SetActive (true);
		coinMultipler++;
		coinsToAsk = coinMultipler * 20;
		igmLogic.GetComponent<IGMLogic> ().ShowPayToContinueMenu (coinsToAsk);
		counDownText.text = "7";
		yield return new WaitForSeconds (1);
		counDownText.text = "6";
		yield return new WaitForSeconds (1);
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
		igmLogic.GetComponent<IGMLogic> ().ClosePayToContinueMenu ();
		playerDied ();
	}

	void LevelFinished ()
	{
		igmLogic.GetComponent<IGMLogic> ().ShowLevelCompleteMenu (jumpCount);
		transform.parent.GetComponent<MovingPlatform> ().lastBestFun (true);
		//igmLogic.GetComponent<IGMLogic> ().PauseGame ();
	}
	void SpeedUpPlayer ()
	{
		speedUpParticle.Play ();
		transform.parent.GetComponent<MovingPlatform> ().SpeedUp ();
	}
}