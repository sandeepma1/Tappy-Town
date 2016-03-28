using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MissionLogic : MonoBehaviour
{
	public static MissionLogic m_instance = null;
	public GameObject missionCompleteGiftMenu, giftParticles, backButton, redeemButton;
	public Text gotRewardText;
	public TextMesh currentMissionText;
	public Animator anim;
	float blinkRate = 0.25f;

	void Awake ()
	{
		m_instance = this;
		//ClearSingleRunValues ();
		//****************************  Run Once ************************************************
		if (June.LocalStore.Instance.GetInt ("SetMissionOnce") <= 0) {
			June.LocalStore.Instance.SetInt ("SetMissionOnce", 1);
			June.LocalStore.Instance.SetInt ("CurrentMissionID", 0);
			June.LocalStore.Instance.SetBool ("CurrentMissionCompleted", false);
			June.LocalStore.Instance.SetInt ("MissionAdder", 1);
			ShowMissionBanner (true);
			SetCurrentMission ();
			//DisplayCurrentMission ();
		}//**************************************************************************************

		CheckMissionStatus ();
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

	void CheckMissionStatus ()
	{
		if (CheckIfMissionCompleted ()) {
			CurrentMissionCompleted (true);
			ShowMissionBanner (false);
			CurrentMissionCompleted (true);
			MissionGiftisReady (true);
		} else {
			CurrentMissionCompleted (false);
			ShowMissionBanner (true);
			CurrentMissionCompleted (false);
			MissionGiftisReady (false);
			ClearSingleRunValues ();
		}
	}

	void SetCurrentMission ()
	{
		June.LocalStore.Instance.SetBool ("CurrentMissionCompleted", false);
		ShowMissionBanner (true);
		for (int i = 0; i < MissionManager.AllMissions.Count; i++) {
			June.LocalStore.Instance.SetInt (MissionManager.AllMissions [i].PlayerPrefs, 0);
		}
	}

	void ClearSingleRunValues ()
	{
		for (int i = 0; i < MissionManager.AllMissions.Count; i++) {
			if (MissionManager.AllMissions [i].SingleRun) {
				June.LocalStore.Instance.SetInt (MissionManager.AllMissions [i].PlayerPrefs, 0);
			}
		}
	}

	public void DisplayCurrentMission ()
	{
		string tempString = June.LocalStore.Instance.GetInt (MissionManager.AllMissions [June.LocalStore.Instance.GetInt ("CurrentMissionID")].PlayerPrefs) + "/" + MissionManager.AllMissions [June.LocalStore.Instance.GetInt ("CurrentMissionID")].Value * June.LocalStore.Instance.GetInt ("MissionAdder");
		string currentMissionDescription = MissionManager.AllMissions [June.LocalStore.Instance.GetInt ("CurrentMissionID")].Description.Replace ("*", tempString);
		currentMissionText.text = currentMissionDescription;
	}

	public bool CheckIfMissionCompleted ()
	{
		if (June.LocalStore.Instance.GetInt (MissionManager.AllMissions [June.LocalStore.Instance.GetInt ("CurrentMissionID")].PlayerPrefs) >= MissionManager.AllMissions [June.LocalStore.Instance.GetInt ("CurrentMissionID")].Value * June.LocalStore.Instance.GetInt ("MissionAdder")) {
			June.LocalStore.Instance.SetBool ("CurrentMissionCompleted", true);
			return true;
		} else {			
			June.LocalStore.Instance.SetBool ("CurrentMissionCompleted", false);
			return false;
		}
	}

	void ShowMissionBanner (bool flag)
	{
		June.LocalStore.Instance.SetBool ("showMissionBanner", flag);
	}

	public void CurrentMissionCompleted (bool flag)
	{		
		June.LocalStore.Instance.SetBool ("CurrentMissionCompleted", flag);
	}

	public void MissionGiftisReady (bool flag)
	{		
		June.LocalStore.Instance.SetBool ("MissionGiftisReady", flag);
	}

	public void CargoMissionAdder ()
	{		
		June.LocalStore.Instance.SetInt ("Mission_CargoSequence", June.LocalStore.Instance.GetInt ("Mission_CargoSequence") + 1);
	}

	public void BalloonMissionAdder ()
	{		
		June.LocalStore.Instance.SetInt ("Mission_BalloonCount", June.LocalStore.Instance.GetInt ("Mission_BalloonCount") + 1);
	}

	public void MissionCompletedGiftClaimed ()
	{
		June.LocalStore.Instance.SetInt ("CurrentMissionID", (June.LocalStore.Instance.GetInt ("CurrentMissionID") + 1));
		if (June.LocalStore.Instance.GetInt ("CurrentMissionID") >= MissionManager.AllMissions.Count) {
			June.LocalStore.Instance.SetInt ("CurrentMissionID", 0);
			June.LocalStore.Instance.SetInt ("MissionAdder", June.LocalStore.Instance.GetInt ("MissionAdder") + 1); // *************** Mission adder multiplier X1
		}
		SetCurrentMission ();
		ShowMissionBanner (false);
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
		ShowMissionBanner (true);
		StartCoroutine ("PlayerGetCoinsAnimation");
	}

	IEnumerator PlayerGetCoinsAnimation ()
	{
		giftParticles.SetActive (false);
		backButton.SetActive (false);
		redeemButton.SetActive (false);
		anim.Play ("binRumbleAnimation");
		yield return new WaitForSeconds (2f);
		// *****************************************   Gets coins or tokens
		if (MissionManager.AllMissions [June.LocalStore.Instance.GetInt ("CurrentMissionID")].RewardType == "coins") {
			print ("got coins " + June.LocalStore.Instance.GetInt ("CurrentMissionID"));
			June.LocalStore.Instance.SetInt ("coins", June.LocalStore.Instance.GetInt ("coins") + MissionManager.AllMissions [June.LocalStore.Instance.GetInt ("CurrentMissionID")].Reward);
			gotRewardText.text = "+ " + MissionManager.AllMissions [June.LocalStore.Instance.GetInt ("CurrentMissionID")].Reward + "~";
		}
		if (MissionManager.AllMissions [June.LocalStore.Instance.GetInt ("CurrentMissionID")].RewardType == "tokens") {
			print ("got tokens " + June.LocalStore.Instance.GetInt ("CurrentMissionID"));
			June.LocalStore.Instance.SetInt ("tokens", June.LocalStore.Instance.GetInt ("tokens") + MissionManager.AllMissions [June.LocalStore.Instance.GetInt ("CurrentMissionID")].Reward);
			gotRewardText.text = "+ " + MissionManager.AllMissions [June.LocalStore.Instance.GetInt ("CurrentMissionID")].Reward + "$";
		}
		// *****************************************   Gets coins or tokens Ends here
		CoinCalculation.m_instance.UpdateCurrencyOnUI ();
		MissionCompletedGiftClaimed ();
		yield return new WaitForSeconds (2);
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
}



/*Travel [*] distance
Use Baloon [*] times
Collect [*] coins
Complete [*] cargo sections
Collect [*] tokens
Cross [*] trains
Travel [*] distance in single run
Use Baloon [*] times in single run
Cross [*] trains in single run
Complete [*] cargo sections in single run
Collect [*] coins in single run
*/