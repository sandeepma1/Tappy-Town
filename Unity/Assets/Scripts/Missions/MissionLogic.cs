using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MissionLogic : MonoBehaviour
{
	public static MissionLogic m_instance = null;
	public TextMesh currentMissionText;
	//public int currentMissionSelected = 0;
	//lesser the value faster the blink
	float blinkRate = 0.5f;
	//bool isCurrentMissionCompleted = false;
	string[] mission = new string[4];
	// Use this for initialization
	void Awake ()
	{
		currentMissionText.text = "text";
		m_instance = this;
		//****************************  Run Once ************************************************
		if (PlayerPrefs.GetInt ("SetMissionOnce") <= 0) {
			PlayerPrefs.SetInt ("SetMissionOnce", 1);
			PlayerPrefs.SetInt ("CurrentMissionID", 0);
			SetCurrentMission ();
		}//**************************************************************************************
	}

	void Start ()
	{
		mission [0] = "Travel";
		mission [1] = "Use Balloon";
		mission [2] = "Jump";
		mission [3] = "Collect Coins";
		DisplayCurrentMission ();
		StartCoroutine ("FadeToTransprent");
	}

	IEnumerator FadeToTransprent ()
	{
		float alpha = currentMissionText.color.a;
		for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / blinkRate) {
			Color newColor = new Color (1, 1, 1, Mathf.Lerp (alpha, 0, t));
			currentMissionText.color = newColor;
			yield return null;
		}
		StartCoroutine ("FadeToOpaque");
	}

	IEnumerator FadeToOpaque ()
	{
		float alpha = currentMissionText.color.a;
		for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / blinkRate) {
			Color newColor = new Color (1, 1, 1, Mathf.Lerp (alpha, 1, t));
			currentMissionText.color = newColor;
			yield return null;
		}
		StartCoroutine ("FadeToTransprent");
	}

	/*public void RedeemedButtonClicked ()
	{
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
		SceneManager.LoadSceneAsync ("level");
		CloseRedeemMenu ();
	}*/

	void SetCurrentMission ()
	{
		switch (PlayerPrefs.GetInt ("CurrentMissionID")) {
		case 0:
			PlayerPrefs.SetInt ("Mission_DistanceCount", 0);
			break;
		case 1:
			PlayerPrefs.SetInt ("Mission_BalloonCount", 0);
			break;
		case 2:
			PlayerPrefs.SetInt ("Mission_JumpCount", 0);
			break;
		case 3:
			PlayerPrefs.SetInt ("Mission_CoinCount", 0);
			break;
		default:
			break;
		}
	}

	void DisplayCurrentMission ()
	{
		switch (PlayerPrefs.GetInt ("CurrentMissionID")) {
		case 0:
			currentMissionText.text = mission [PlayerPrefs.GetInt ("CurrentMissionID")] + " [" + PlayerPrefs.GetInt ("Mission_DistanceCount") + "/100]";		
			break;
		case 1:
			currentMissionText.text = mission [PlayerPrefs.GetInt ("CurrentMissionID")] + " [" + PlayerPrefs.GetInt ("Mission_BalloonCount") + "/2]";
			break;
		case 2:
			currentMissionText.text = mission [PlayerPrefs.GetInt ("CurrentMissionID")] + " [" + PlayerPrefs.GetInt ("Mission_JumpCount") + "/100]";
			break;
		case 3:
			currentMissionText.text = mission [PlayerPrefs.GetInt ("CurrentMissionID")] + " [" + PlayerPrefs.GetInt ("Mission_CoinCount") + "/20]";
			break;
		default:
			currentMissionText.text = "Invalid";
			break;
		}
	}

	public bool CheckIfMissionCompleted ()
	{
		//print (PlayerPrefs.GetInt ("CurrentMissionID"));
		bool retunStatus = false;
		switch (PlayerPrefs.GetInt ("CurrentMissionID")) {
		case 0:			
			retunStatus = CheckDistanceMission ();
			break;	
		case 1:			
			retunStatus = CheckBalloonMission ();
			break;	
		case 2:			
			retunStatus = CheckJumpMission ();
			break;		
		case 3:			
			retunStatus = CheckCoinMission ();
			break;	
		default:
			currentMissionText.text = "Invalid";
			break;
		}
		return retunStatus;
	}

	bool CheckJumpMission ()
	{
		if (PlayerPrefs.GetInt ("Mission_JumpCount") >= 100) {
			return true;
		}
		return false;
	}

	bool CheckDistanceMission ()
	{
		if (PlayerPrefs.GetInt ("Mission_DistanceCount") >= 100) {
			return true;
		}
		return false;
	}

	bool CheckBalloonMission ()
	{
		if (PlayerPrefs.GetInt ("Mission_BalloonCount") >= 2) {
			return true;
		}
		return false;
	}

	bool CheckCoinMission ()
	{
		if (PlayerPrefs.GetInt ("Mission_CoinCount") >= 20) {
			return true;
		}
		return false;
	}

	public void MissionCompleted ()
	{
		PlayerPrefs.SetInt ("Token", PlayerPrefs.GetInt ("Token") + 5);
		CoinCalculation.m_instance.UpdateCurrencyOnUI ();
		PlayerPrefs.SetInt ("CurrentMissionID", (PlayerPrefs.GetInt ("CurrentMissionID") + 1));
		if (PlayerPrefs.GetInt ("CurrentMissionID") >= mission.Length) {
			PlayerPrefs.SetInt ("CurrentMissionID", 0);
		}
		SetCurrentMission ();
	}
}
