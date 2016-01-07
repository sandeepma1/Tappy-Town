using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MissionLogic : MonoBehaviour
{
	public static MissionLogic m_instance = null;
	public TextMesh currentMissionText;
	//public TextMesh
	public int currentMissionSelected = 0;

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
	}

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
			//isCurrentMissionCompleted = true;
			return true;
		}
		return false;
	}

	bool CheckDistanceMission ()
	{
		if (PlayerPrefs.GetInt ("Mission_DistanceCount") >= 100) {
			//isCurrentMissionCompleted = true;
			return true;
		}
		return false;
	}

	bool CheckBalloonMission ()
	{
		if (PlayerPrefs.GetInt ("Mission_BalloonCount") >= 2) {
			//isCurrentMissionCompleted = true;
			return true;
		}
		return false;
	}

	bool CheckCoinMission ()
	{
		if (PlayerPrefs.GetInt ("Mission_CoinCount") >= 10) {
			//isCurrentMissionCompleted = true;
			return true;
		}
		return false;
	}

	public void MissionCompleted ()
	{
		PlayerPrefs.SetInt ("CurrentMissionID", (PlayerPrefs.GetInt ("CurrentMissionID") + 1));
		if (PlayerPrefs.GetInt ("CurrentMissionID") >= mission.Length) {
			PlayerPrefs.SetInt ("CurrentMissionID", 0);
		}
		SetCurrentMission ();
	}
}
