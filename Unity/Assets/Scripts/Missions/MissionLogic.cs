using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MissionLogic : MonoBehaviour
{
	public static MissionLogic m_instance = null;
	public GameObject missionCompleteGiftMenu, giftParticles, backButton, redeemButton, promoStripMissionClaim;
	public Text gotRewardText;
	public TextMesh currentMissionText;
	public Animator anim;
	float blinkRate = 0.25f;

	void Awake ()
	{
		m_instance = this;
		//ClearSingleRunValues ();
		//****************************  Run Once ************************************************
		if (Bronz.LocalStore.Instance.GetInt ("SetMissionOnce") <= 0) {
			Bronz.LocalStore.Instance.SetInt ("SetMissionOnce", 1);
			Bronz.LocalStore.Instance.SetInt ("CurrentMissionID", 0);
			Bronz.LocalStore.Instance.SetBool ("CurrentMissionCompleted", false);
			Bronz.LocalStore.Instance.SetInt ("MissionAdder", 1);
			ShowMissionBanner (true);
			SetCurrentMission ();
			//DisplayCurrentMission ();
		}//**************************************************************************************

		CheckMissionStatus ();
		if (!Bronz.LocalStore.Instance.GetBool ("showMissionBanner")) {
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
		Bronz.LocalStore.Instance.SetBool ("CurrentMissionCompleted", false);
		ShowMissionBanner (true);
		for (int i = 0; i < MissionManager.AllMissions.Count; i++) {
			Bronz.LocalStore.Instance.SetInt (MissionManager.AllMissions [i].PlayerPrefs, 0);
		}
	}

	void ClearSingleRunValues ()
	{
		for (int i = 0; i < MissionManager.AllMissions.Count; i++) {
			if (MissionManager.AllMissions [i].SingleRun) {
				Bronz.LocalStore.Instance.SetInt (MissionManager.AllMissions [i].PlayerPrefs, 0);
			}
		}
	}

	public void DisplayCurrentMission ()
	{
		string tempString = Bronz.LocalStore.Instance.GetInt (MissionManager.AllMissions [Bronz.LocalStore.Instance.GetInt ("CurrentMissionID")].PlayerPrefs) + "/" + MissionManager.AllMissions [Bronz.LocalStore.Instance.GetInt ("CurrentMissionID")].Value * Bronz.LocalStore.Instance.GetInt ("MissionAdder");
		string currentMissionDescription = MissionManager.AllMissions [Bronz.LocalStore.Instance.GetInt ("CurrentMissionID")].Description.Replace ("*", tempString);
		currentMissionText.text = currentMissionDescription;
	}

	public bool CheckIfMissionCompleted ()
	{
		if (Bronz.LocalStore.Instance.GetInt (MissionManager.AllMissions [Bronz.LocalStore.Instance.GetInt ("CurrentMissionID")].PlayerPrefs) >= MissionManager.AllMissions [Bronz.LocalStore.Instance.GetInt ("CurrentMissionID")].Value * Bronz.LocalStore.Instance.GetInt ("MissionAdder")) {
			Bronz.LocalStore.Instance.SetBool ("CurrentMissionCompleted", true);
			return true;
		} else {			
			Bronz.LocalStore.Instance.SetBool ("CurrentMissionCompleted", false);
			return false;
		}
	}

	void ShowMissionBanner (bool flag)
	{
		Bronz.LocalStore.Instance.SetBool ("showMissionBanner", flag);
	}

	public void CurrentMissionCompleted (bool flag)
	{		
		Bronz.LocalStore.Instance.SetBool ("CurrentMissionCompleted", flag);
	}

	public void MissionGiftisReady (bool flag)
	{		
		Bronz.LocalStore.Instance.SetBool ("MissionGiftisReady", flag);
	}

	public void CargoMissionAdder ()
	{		
		Bronz.LocalStore.Instance.SetInt ("Mission_CargoSequence", Bronz.LocalStore.Instance.GetInt ("Mission_CargoSequence") + 1);
	}

	public void BalloonMissionAdder ()
	{		
		Bronz.LocalStore.Instance.SetInt ("Mission_BalloonCount", Bronz.LocalStore.Instance.GetInt ("Mission_BalloonCount") + 1);
	}

	public void MissionCompletedGiftClaimed ()
	{
		Bronz.LocalStore.Instance.SetInt ("CurrentMissionID", (Bronz.LocalStore.Instance.GetInt ("CurrentMissionID") + 1));
		if (Bronz.LocalStore.Instance.GetInt ("CurrentMissionID") >= MissionManager.AllMissions.Count) {
			Bronz.LocalStore.Instance.SetInt ("CurrentMissionID", 0);
			Bronz.LocalStore.Instance.SetInt ("MissionAdder", Bronz.LocalStore.Instance.GetInt ("MissionAdder") + 1); // *************** Mission adder multiplier X1
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
		IGMLogic.m_instance.scrollingBG.SetActive (false);
		IGMLogic.m_instance.isTextMeshesVisible (true);
		IGMLogic.m_instance.playerShadow.SetActive (true);
	}

	public void OpenMissionCompleteGiftMenu ()
	{
		IGMLogic.m_instance.mainCanvas.renderMode = RenderMode.ScreenSpaceCamera;
		IGMLogic.m_instance.scrollingBG.SetActive (true);
		IGMLogic.m_instance.isTextMeshesVisible (false);
		if (GameEventManager.isNightMode) {
			IGMLogic.m_instance.shadowLight.gameObject.SetActive (true);
		}
		giftParticles.SetActive (true);
		missionCompleteGiftMenu.SetActive (true);
		IGMLogic.m_instance.playerShadow.SetActive (false);
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
		if (MissionManager.AllMissions [Bronz.LocalStore.Instance.GetInt ("CurrentMissionID")].RewardType == "coins") {
			print ("got coins " + Bronz.LocalStore.Instance.GetInt ("CurrentMissionID"));
			Bronz.LocalStore.Instance.SetInt ("coins", Bronz.LocalStore.Instance.GetInt ("coins") + MissionManager.AllMissions [Bronz.LocalStore.Instance.GetInt ("CurrentMissionID")].Reward);
			gotRewardText.text = "+ " + MissionManager.AllMissions [Bronz.LocalStore.Instance.GetInt ("CurrentMissionID")].Reward + "~";
		}
		if (MissionManager.AllMissions [Bronz.LocalStore.Instance.GetInt ("CurrentMissionID")].RewardType == "tokens") {
			print ("got tokens " + Bronz.LocalStore.Instance.GetInt ("CurrentMissionID"));
			Bronz.LocalStore.Instance.SetInt ("tokens", Bronz.LocalStore.Instance.GetInt ("tokens") + MissionManager.AllMissions [Bronz.LocalStore.Instance.GetInt ("CurrentMissionID")].Reward);
			gotRewardText.text = "+ " + MissionManager.AllMissions [Bronz.LocalStore.Instance.GetInt ("CurrentMissionID")].Reward + "$";
		}
		// *****************************************   Gets coins or tokens Ends here
		CoinCalculation.m_instance.UpdateCurrencyOnUI ();
		MissionCompletedGiftClaimed ();
		yield return new WaitForSeconds (2);
		CloseMissionCompleteGiftMenu ();
		promoStripMissionClaim.SetActive (false);
		//GameManagers.m_instance.Restartlevel ();
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