﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PromoStripsManager : MonoBehaviour
{
	public Text missionStatusText;
	public static PromoStripsManager m_instance = null;
	public Animator animTop, animMiddle;

	void Start ()
	{
		m_instance = this;
	}

	public void ShowPromoStrips ()
	{
		StripTop ();
		StripMiddle ();
	}

	void StripTop ()
	{
		if (June.LocalStore.Instance.GetBool ("CurrentMissionCompleted")) {			
			animTop.PlayInFixedTime ("MissionClaim");
			print ("Mission Completed");
			return;
		} 

		int ran = Random.Range (0, 3);

		if (ran >= 0) {
			MissionLogic.m_instance.DisplayCurrentMission ();
			missionStatusText.text = MissionLogic.m_instance.currentMissionText.text;
			animTop.PlayInFixedTime ("MissionStatus");
		}

		if (ran == 1) {
			animTop.PlayInFixedTime ("CharBuy");
		}

		if (ran == 2) {
			if (June.LocalStore.Instance.GetInt ("coins") > GameEventManager.gatchaSpinValue) {
				animTop.PlayInFixedTime ("UnlockCharacter");
				return;
			}
		}
	}

	void StripMiddle ()
	{		
		if (June.LocalStore.Instance.GetBool ("isReady")) {
			animMiddle.PlayInFixedTime ("FreeGiftClaim");
			return;
		}

		int ran = Random.Range (0, 4);

		if (ran == 0 || ran == 1) {
			animMiddle.PlayInFixedTime ("WatchAds");
		}

		if (ran == 2) {		
			animMiddle.PlayInFixedTime ("FreeGiftStatus");
		}
	}
}

/*		if (June.LocalStore.Instance.GetInt ("coins") >= GameEventManager.gatchaSpinValue) {
			print ("Use Gatch spin");
			animBottom.PlayInFixedTime ("UseGatcha");
			return;
		}*/
