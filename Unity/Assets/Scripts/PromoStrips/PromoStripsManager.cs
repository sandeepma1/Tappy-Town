using UnityEngine;
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
		//FillStripsInfo ();
	}

	/*void FillStripsInfo ()
	{		
	}
*/
	public void ShowPromoStrips ()
	{
		StripTop ();
		StripMiddle ();
	}

	void StripTop ()
	{
		if (MissionLogic.m_instance.CheckIfMissionCompleted ()) {			
			animTop.PlayInFixedTime ("MissionClaim");
			print ("Mission Completed");
			return;
		} 

		int ran = Random.Range (0, 3);

		if (ran >= 0) {
			missionStatusText.text = MissionLogic.m_instance.currentMissionText.text;
			print ("3/4 Train jumps");
			animTop.PlayInFixedTime ("MissionStatus");
		}

		if (ran == 1) {
			print ("Buy new Character");
			animTop.PlayInFixedTime ("CharBuy");
		}

		if (ran == 2) {
			if (June.LocalStore.Instance.GetInt ("coins") > GameEventManager.gatchaSpinValue) {
				print ("Unlock New Character");
				animTop.PlayInFixedTime ("UnlockCharacter");
				return;
			}
		}
	}

	void StripMiddle ()
	{		
		if (June.LocalStore.Instance.GetBool ("isReady")) {
			print ("Gift is ready");
			animMiddle.PlayInFixedTime ("FreeGiftClaim");
			return;
		}

		int ran = Random.Range (0, 4);

		if (ran == 0 || ran == 1) {
			print ("Watch video Ad");
			animMiddle.PlayInFixedTime ("WatchAds");
		}

		if (ran == 2) {			
			print ("Gift in 2h 20m");
			animMiddle.PlayInFixedTime ("FreeGiftStatus");
		}
	}
}

/*		if (June.LocalStore.Instance.GetInt ("coins") >= GameEventManager.gatchaSpinValue) {
			print ("Use Gatch spin");
			animBottom.PlayInFixedTime ("UseGatcha");
			return;
		}*/
