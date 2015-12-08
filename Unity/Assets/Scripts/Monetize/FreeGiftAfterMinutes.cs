using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

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
	Animator anim;

	void Awake ()
	{
		anim = bin.GetComponent<Animator> ();
		//****************************  Run Once ************************************************
		if (!PlayerPrefsX.GetBool ("runOnce")) {
			PlayerPrefsX.SetBool ("runOnce", true);
			currentDate = System.DateTime.Now;
			addedDate = currentDate.AddHours (0.1f);
			PlayerPrefs.SetString ("addedDate", addedDate.ToBinary ().ToString ());
		}//**************************************************************************************
	}
	void Start ()
	{
		coinText.text = PlayerPrefs.GetInt ("Coins").ToString ();
		InvokeRepeating ("CalculateTimeDifference", 0f, 59f);
	}

	void CalculateTimeDifference ()
	{
		currentDate = System.DateTime.Now;
		DateTime oldDate1 = DateTime.FromBinary (Convert.ToInt64 (PlayerPrefs.GetString ("addedDate")));
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
		if (difference.TotalSeconds <= 0) {
			//print ("less thenxero");
			PlayerPrefsX.SetBool ("isReady", true);
		}
	}

	public void GiftButtonClicked ()
	{
		if (PlayerPrefsX.GetBool ("isReady")) {
			OpenRedeemMenu ();
		}
	}
	public void RedeemedButtonClicked ()
	{
		PlayerPrefsX.SetBool ("isReady", false);
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
		yield return new WaitForSeconds (1.25f);
		//********************************************************* Random Coin Logic
		int coinsInGift = UnityEngine.Random.Range (2, 5) * 20;    
		//**************************************************************************************************
		PlayerPrefs.SetInt ("Coins", coinsInGift + PlayerPrefs.GetInt ("Coins"));
		gotCoinText.text = "+" + coinsInGift;
		coinText.text = PlayerPrefs.GetInt ("Coins").ToString ();

		yield return new WaitForSeconds (3);
		Application.LoadLevel ("level");
		CloseRedeemMenu ();
	}
	public void CloseRedeemMenu ()
	{
		mainCamera.SetActive (true);
		mainCanvas.SetActive (true);
		menuCamera.SetActive (false);
		menuCanvas.SetActive (false);
		redeemMenu.SetActive (false);
	}
	public void OpenRedeemMenu ()
	{
		mainCamera.SetActive (false);
		mainCanvas.SetActive (false);
		menuCamera.SetActive (true);
		menuCanvas.SetActive (true);
		particle.SetActive (true);
		redeemMenu.SetActive (true);
	}

	void AddDate ()
	{
		addedDate = currentDate.AddHours (3f);
		PlayerPrefs.SetString ("addedDate", addedDate.ToBinary ().ToString ());
	}

}