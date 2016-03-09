using UnityEngine;
using System.Collections;

public class PromoStripsManager : MonoBehaviour
{
	public static PromoStripsManager m_instance = null;
	public GameObject promoTemp;
	public GameObject videoAds, gatchaSpin, charBuy;
	public GameObject missionProgress, missionClaim, giftProgress, giftClaim;
	public GameObject rateUs, fbConnect, googlePlusConnect;
	public Animator animTop, animMiddle, animBottom;

	void Start ()
	{
		m_instance = this;
	}

	public void ShowPromoStrips ()
	{
		StripTop ();
		StripMiddle ();
		StripBottom ();
		//StartCoroutine ("Play");
	}

	IEnumerator Play ()
	{
		StripTop ();
		yield return new WaitForSeconds (1);
		StripMiddle ();
		yield return new WaitForSeconds (1);
		StripBottom ();
	}

	void StripTop ()
	{
		int ran = Random.Range (0, 4);

		if (ran == 0 || ran == 1 || ran == 2) {
			print ("Watch video Ad");
			animTop.PlayInFixedTime ("WatchAds");
		}

		if (ran == 3) {
			print ("Buy new Character");
			animTop.PlayInFixedTime ("CharBuy");
		}
	}

	void StripMiddle ()
	{		
		int ran = Random.Range (0, 2);
		if (ran == 0) {
			if (June.LocalStore.Instance.GetBool ("isReady")) {
				print ("Gift is ready");
				animMiddle.PlayInFixedTime ("FreeGiftClaim");
			} else {
				print ("Gift in 2h 20m");
				animMiddle.PlayInFixedTime ("FreeGiftStatus");
			}
		}

		if (ran == 1) {
			if (MissionLogic.m_instance.CheckIfMissionCompleted ()) {
				print ("Mission is Complete");
				animMiddle.PlayInFixedTime ("MissionClaim");
			} else {
				print ("3/4 Train jumps");
				animMiddle.PlayInFixedTime ("MissionStatus");
			}
		}

		if (ran >= 4) {
			print ("  ");
		}
	}

	void StripBottom ()
	{			
		if (June.LocalStore.Instance.GetBool ("isUserRatedGame")) {
			print ("Rate Us");
		}

		if (June.LocalStore.Instance.GetBool ("isUserFBConnected")) {
			print ("Connect FB");
		}

		if (June.LocalStore.Instance.GetBool ("isUserGooglePlusConnected")) {
			print ("Connect G+");
		}

		if (June.LocalStore.Instance.GetInt ("coins") >= GameEventManager.gatchaSpinValue) {
			print ("Use Gatch spin");
			animBottom.PlayInFixedTime ("UseGatcha");
		}

	}
}
