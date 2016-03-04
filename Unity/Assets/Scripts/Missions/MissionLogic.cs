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
		if (June.LocalStore.Instance.GetInt ("SetMissionOnce") <= 0) {
			June.LocalStore.Instance.SetInt ("SetMissionOnce", 1);
			June.LocalStore.Instance.SetInt ("CurrentMissionID", 0);
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
		float alpha = currentMissionText.color.b;
		for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / blinkRate) {
			Color newColor = new Color (1, 1, Mathf.Lerp (alpha, 0, t), 1);
			currentMissionText.color = newColor;
			yield return null;
		}
		StartCoroutine ("FadeToOpaque");
	}

	IEnumerator FadeToOpaque ()
	{
		float alpha = currentMissionText.color.b;
		for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / blinkRate) {
			Color newColor = new Color (1, 1, Mathf.Lerp (alpha, 0, t), 1);
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
		June.LocalStore.Instance.SetInt("coins"), coinsInGift + June.LocalStore.Instance.GetInt("coins"));
		gotCoinText.text = "+" + coinsInGift;
		coinText.text = June.LocalStore.Instance.GetInt("coins").ToString ();
		yield return new WaitForSeconds (3);
		SceneManager.LoadSceneAsync ("level");
		CloseRedeemMenu ();
	}*/

	void SetCurrentMission ()
	{
		switch (June.LocalStore.Instance.GetInt ("CurrentMissionID")) {
		case 0:
			June.LocalStore.Instance.SetInt ("Mission_DistanceCount", 0);
			break;
		case 1:
			June.LocalStore.Instance.SetInt ("Mission_BalloonCount", 0);
			break;
		case 2:
			June.LocalStore.Instance.SetInt ("Mission_JumpCount", 0);
			break;
		case 3:
			June.LocalStore.Instance.SetInt ("Mission_CoinCount", 0);
			break;
		default:
			break;
		}
	}

	void DisplayCurrentMission ()
	{
		switch (June.LocalStore.Instance.GetInt ("CurrentMissionID")) {
		case 0:
			currentMissionText.text = mission [June.LocalStore.Instance.GetInt ("CurrentMissionID")] + " [" + June.LocalStore.Instance.GetInt ("Mission_DistanceCount") + "/100]";		
			break;
		case 1:
			currentMissionText.text = mission [June.LocalStore.Instance.GetInt ("CurrentMissionID")] + " [" + June.LocalStore.Instance.GetInt ("Mission_BalloonCount") + "/2]";
			break;
		case 2:
			currentMissionText.text = mission [June.LocalStore.Instance.GetInt ("CurrentMissionID")] + " [" + June.LocalStore.Instance.GetInt ("Mission_JumpCount") + "/100]";
			break;
		case 3:
			currentMissionText.text = mission [June.LocalStore.Instance.GetInt ("CurrentMissionID")] + " [" + June.LocalStore.Instance.GetInt ("Mission_CoinCount") + "/20]";
			break;
		default:
			currentMissionText.text = "Invalid";
			break;
		}
	}

	public bool CheckIfMissionCompleted ()
	{
		//print (June.LocalStore.Instance.GetInt ("CurrentMissionID"));
		bool retunStatus = false;
		switch (June.LocalStore.Instance.GetInt ("CurrentMissionID")) {
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
		if (June.LocalStore.Instance.GetInt ("Mission_JumpCount") >= 100) {
			return true;
		}
		return false;
	}

	bool CheckDistanceMission ()
	{
		if (June.LocalStore.Instance.GetInt ("Mission_DistanceCount") >= 100) {
			return true;
		}
		return false;
	}

	bool CheckBalloonMission ()
	{
		if (June.LocalStore.Instance.GetInt ("Mission_BalloonCount") >= 2) {
			return true;
		}
		return false;
	}

	bool CheckCoinMission ()
	{
		if (June.LocalStore.Instance.GetInt ("Mission_CoinCount") >= 20) {
			return true;
		}
		return false;
	}

	public void MissionCompleted ()
	{
		June.LocalStore.Instance.SetInt ("tokens", June.LocalStore.Instance.GetInt ("tokens") + 5);
		CoinCalculation.m_instance.UpdateCurrencyOnUI ();
		June.LocalStore.Instance.SetInt ("CurrentMissionID", (June.LocalStore.Instance.GetInt ("CurrentMissionID") + 1));
		if (June.LocalStore.Instance.GetInt ("CurrentMissionID") >= mission.Length) {
			June.LocalStore.Instance.SetInt ("CurrentMissionID", 0);
		}
		SetCurrentMission ();
	}
}
