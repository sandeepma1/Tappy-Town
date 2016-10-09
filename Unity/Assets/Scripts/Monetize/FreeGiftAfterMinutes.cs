using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

//using UnityEngine.SceneManagement;

public class FreeGiftAfterMinutes : MonoBehaviour
{
	DateTime currentDate, addedDate;
	TimeSpan zeroTime = new TimeSpan (0, 0, 1, 0, 0);
	TimeSpan difference;
	string myString;
	public Text giftTimerText, giftText, giftStatusText;
	public GameObject giftButton, promoStripGiftButton;
	public GameObject redeemMenu;
	public GameObject backButton, redeemButton, particle;
	public Text gotCoinText;
	public Animator anim;
	public static FreeGiftAfterMinutes m_instance = null;

	void Awake ()
	{
		m_instance = this;
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
			giftText.text = "";
			giftTimerText.text = myString;
		} else {
			giftText.text = ("Gift Ready");
			giftTimerText.text = "";
			giftButton.GetComponent<Image> ().color = Color.red;
		}
		if (difference.TotalSeconds <= 59) {
			June.LocalStore.Instance.SetBool ("isReady", true);
		}
	}

	public string GiftTimeRemaining ()
	{
		string remainingTime = "";
		currentDate = System.DateTime.Now;
		DateTime oldDate1 = DateTime.FromBinary (Convert.ToInt64 (June.LocalStore.Instance.GetString ("addedDate")));
		difference = oldDate1.Subtract (currentDate);

		if (difference.Subtract (zeroTime).TotalSeconds >= 0) {
			myString = String.Format ("{0:D1}h {1:D1}m", difference.Hours, difference.Minutes, difference.Seconds);
			remainingTime = myString;
		} else {
			remainingTime = "";
		}
		return remainingTime;
	}

	public void GiftButtonClicked ()
	{
		CalculateTimeDifference ();
		if (June.LocalStore.Instance.GetBool ("isReady")) {
			OpenRedeemMenu ();
		}
	}

	public void RedeemedButtonClicked ()
	{
		June.LocalStore.Instance.SetBool ("isReady", false);
		giftButton.GetComponent<Image> ().color = Color.white;
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
		CoinCalculation.m_instance.UpdateCurrencyOnUI ();
		yield return new WaitForSeconds (2);
		CloseRedeemMenu ();
		//promoStripGiftButton.SetActive (false);
		GiftButtonClicked ();
		giftStatusText.text = "Next Gift in";
		//GameManagers.m_instance.Restartlevel ();
	}

	public void CloseRedeemMenu ()
	{
		if (GameEventManager.isNightMode) {
			IGMLogic.m_instance.shadowLight.gameObject.SetActive (false);
		}	
		redeemMenu.SetActive (false);
		IGMLogic.m_instance.mainCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
		IGMLogic.m_instance.scrollingBG.SetActive (false);
		IGMLogic.m_instance.isTextMeshesVisible (true);
		IGMLogic.m_instance.playerShadow.SetActive (true);
	}

	public void OpenRedeemMenu ()
	{
		IGMLogic.m_instance.mainCanvas.renderMode = RenderMode.ScreenSpaceCamera;
		IGMLogic.m_instance.scrollingBG.SetActive (true);
		IGMLogic.m_instance.isTextMeshesVisible (false);
		if (GameEventManager.isNightMode) {
			IGMLogic.m_instance.shadowLight.gameObject.SetActive (true);
		}
		particle.SetActive (true);
		redeemMenu.SetActive (true);
		IGMLogic.m_instance.playerShadow.SetActive (false);
	}

	void AddDate ()
	{
		addedDate = currentDate.AddHours (GameEventManager.freeGiftTimeDelay);
		June.LocalStore.Instance.SetString ("addedDate", addedDate.ToBinary ().ToString ());
	}
}