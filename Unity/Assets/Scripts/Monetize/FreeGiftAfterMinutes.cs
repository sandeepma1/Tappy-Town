using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FreeGiftAfterMinutes : MonoBehaviour
{
	DateTime currentDate, addedDate;
	TimeSpan zeroTime = new TimeSpan (0, 0, 1, 0, 0);
	TimeSpan difference;
	string myString;

	public GameObject mainCanvas, menuCanvas, mainCamera, menuCamera;

	public Text giftTimerText, giftText, giftTimerText1, giftText1;
	public GameObject giftButton, giftButton1;
	public GameObject redeemMenu;
	public Text coinText;
	public GameObject bin, cart, backButton, redeemButton, particle;
	public Text gotCoinText;
	public Animator anim;

	void Awake ()
	{
		//****************************  Run Once ************************************************
		if (!June.LocalStore.Instance.GetBool ("runOnce")) {
			June.LocalStore.Instance.SetBool ("runOnce", true);
			currentDate = System.DateTime.Now;
			addedDate = currentDate.AddHours (0.025f); // Edit this for first-time free gift time
			June.LocalStore.Instance.SetString ("addedDate", addedDate.ToBinary ().ToString ());
		}//**************************************************************************************
	}

	void Start ()
	{
		coinText.text = June.LocalStore.Instance.GetInt ("coins").ToString ();
		CalculateTimeDifference ();
		InvokeRepeating ("CalculateTimeDifference", 0f, 59f);
	}

	void CalculateTimeDifference ()
	{
		currentDate = System.DateTime.Now;
		DateTime oldDate1 = DateTime.FromBinary (Convert.ToInt64 (June.LocalStore.Instance.GetString ("addedDate")));
		difference = oldDate1.Subtract (currentDate);

		if (difference.Subtract (zeroTime).TotalSeconds >= 0) {
			myString = String.Format ("{0:D1}h {1:D1}m", difference.Hours, difference.Minutes, difference.Seconds);
			giftText.text = "Free gift in";
			giftText1.text = "Free gift in";
			giftTimerText.text = myString;
			giftTimerText1.text = myString;
		} else {
			giftText.text = ("Free Gift Ready");
			giftText1.text = ("Free Gift Ready");
			giftTimerText.text = "";
			giftTimerText1.text = "";
			giftButton1.GetComponent<Image> ().color = Color.red;
			giftButton.GetComponent<Image> ().color = Color.red;
		}
		if (difference.TotalSeconds <= 59) {
			//print ("less thenxero");
			June.LocalStore.Instance.SetBool ("isReady", true);
		}
	}

	public void GiftButtonClicked ()
	{
		CalculateTimeDifference ();
		print (June.LocalStore.Instance.GetBool ("isReady"));
		if (June.LocalStore.Instance.GetBool ("isReady")) {
			OpenRedeemMenu ();
		}
	}

	public void RedeemedButtonClicked ()
	{
		June.LocalStore.Instance.SetBool ("isReady", false);
		giftButton.GetComponent<Image> ().color = Color.white;
		giftButton1.GetComponent<Image> ().color = Color.white;
		AddDate ();
		StartCoroutine ("PlayerGetCoinsAnimation");
	}

	IEnumerator PlayerGetCoinsAnimation ()
	{
		particle.SetActive (false);
		backButton.SetActive (false);
		redeemButton.SetActive (false);
		anim.Play ("binRumbleAnimation");
		yield return new WaitForSeconds (2f);
		//********************************************************* Random Coin Logic
		int coinsInGift = UnityEngine.Random.Range (2, 5) * 20;    
		//**************************************************************************************************
		June.LocalStore.Instance.SetInt ("coins", coinsInGift + June.LocalStore.Instance.GetInt ("coins"));
		gotCoinText.text = "+" + coinsInGift;
		coinText.text = June.LocalStore.Instance.GetInt ("coins").ToString ();
		yield return new WaitForSeconds (3);
		SceneManager.LoadSceneAsync ("level");
		//CloseRedeemMenu ();
	}

	public void CloseRedeemMenu ()
	{

		if (GameEventManager.isNightMode) {
			IGMLogic.m_instance.shadowLight.gameObject.SetActive (false);
			//IGMLogic.m_instance.light2.gameObject.SetActive (false);
		}	
		redeemMenu.SetActive (false);
		IGMLogic.m_instance.mainCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
		IGMLogic.m_instance.isTextMeshesVisible (true);
	}

	public void OpenRedeemMenu ()
	{
		IGMLogic.m_instance.mainCanvas.renderMode = RenderMode.ScreenSpaceCamera;
		IGMLogic.m_instance.isTextMeshesVisible (false);
		if (GameEventManager.isNightMode) {
			IGMLogic.m_instance.shadowLight.gameObject.SetActive (true);
			//IGMLogic.m_instance.light2.gameObject.SetActive (true);
		}
		particle.SetActive (true);
		redeemMenu.SetActive (true);
	}

	void AddDate ()
	{
		addedDate = currentDate.AddHours (1f);
		June.LocalStore.Instance.SetString ("addedDate", addedDate.ToBinary ().ToString ());
	}
}