using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MissionLogic : MonoBehaviour
{
	public static MissionLogic m_instance = null;
	public GameObject missionCompleteGiftMenu, giftParticles, backButton, redeemButton;
	public Text gotTokenText;
	//, tokenText;
	public TextMesh currentMissionText;
	public Animator anim;
	float blinkRate = 0.25f;
	string[] mission = new string[4];

	void Awake ()
	{
		//currentMissionText.text = "text";
		m_instance = this;
		mission [0] = "Travel";
		mission [1] = "Use Balloon";
		mission [2] = "Jump";
		mission [3] = "Collect Coins";
		//****************************  Run Once ************************************************
		if (June.LocalStore.Instance.GetInt ("SetMissionOnce") <= 0) {
			June.LocalStore.Instance.SetInt ("SetMissionOnce", 1);
			June.LocalStore.Instance.SetInt ("CurrentMissionID", 0);
			June.LocalStore.Instance.SetBool ("showMissionBanner", true);
			SetCurrentMission ();
			DisplayCurrentMission ();
		}//**************************************************************************************
		if (!June.LocalStore.Instance.GetBool ("showMissionBanner")) {
			currentMissionText.text = "";
			this.gameObject.GetComponent<MeshRenderer> ().enabled = false;
		} else {
			DisplayCurrentMission ();
		}
	}

	void Start ()
	{
		
		StartCoroutine ("FadeToTransprent");
	}

	public void CloseMissionCompleteGiftMenu ()
	{
		if (GameEventManager.isNightMode) {
			IGMLogic.m_instance.shadowLight.gameObject.SetActive (false);
		}	
		missionCompleteGiftMenu.SetActive (false);
		IGMLogic.m_instance.mainCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
		IGMLogic.m_instance.isTextMeshesVisible (true);
	}

	public void OpenMissionCompleteGiftMenu ()
	{
		IGMLogic.m_instance.mainCanvas.renderMode = RenderMode.ScreenSpaceCamera;
		IGMLogic.m_instance.isTextMeshesVisible (false);
		if (GameEventManager.isNightMode) {
			IGMLogic.m_instance.shadowLight.gameObject.SetActive (true);
		}
		giftParticles.SetActive (true);
		missionCompleteGiftMenu.SetActive (true);

	}

	public void MissionCompleteGiftButtonClicked ()
	{
		StartCoroutine ("PlayerGetCoinsAnimation");

	}

	IEnumerator PlayerGetCoinsAnimation ()
	{
		giftParticles.SetActive (false);
		backButton.SetActive (false);
		redeemButton.SetActive (false);
		anim.Play ("binRumbleAnimation");
		yield return new WaitForSeconds (2f);
		gotTokenText.text = "+" + GameEventManager.missionCompleteTokenAmount;
		June.LocalStore.Instance.SetInt ("tokens", June.LocalStore.Instance.GetInt ("tokens") + GameEventManager.missionCompleteTokenAmount);
		CoinCalculation.m_instance.UpdateCurrencyOnUI ();
		yield return new WaitForSeconds (2);
		MissionCompleted ();
		GameManagers.m_instance.Restartlevel ();
	}

	public void StopTextBlinking ()
	{
		StopCoroutine ("FadeToOpaque");
		StopCoroutine ("FadeToTransprent");
	}

	IEnumerator FadeToTransprent ()
	{
		if (GameEventManager.GetState () == GameEventManager.E_STATES.e_game) {
			StopTextBlinking ();
		}
		currentMissionText.color = Color.white;
		yield return new WaitForSeconds (blinkRate);
		StartCoroutine ("FadeToOpaque");
	}

	IEnumerator FadeToOpaque ()
	{
		currentMissionText.color = Color.yellow;
		yield return new WaitForSeconds (blinkRate);
		StartCoroutine ("FadeToTransprent");
	}

	void SetCurrentMission ()
	{
		June.LocalStore.Instance.SetBool ("showMissionBanner", true);
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
		June.LocalStore.Instance.SetInt ("CurrentMissionID", (June.LocalStore.Instance.GetInt ("CurrentMissionID") + 1));
		if (June.LocalStore.Instance.GetInt ("CurrentMissionID") >= mission.Length) {
			June.LocalStore.Instance.SetInt ("CurrentMissionID", 0);
		}
		SetCurrentMission ();
		June.LocalStore.Instance.SetBool ("showMissionBanner", false);
	}
}
