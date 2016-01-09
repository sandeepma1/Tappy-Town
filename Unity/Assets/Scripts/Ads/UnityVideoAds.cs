using UnityEngine;
using UnityEngine.Advertisements;
using System.Collections;
using UnityEngine.SceneManagement;

public class UnityVideoAds : MonoBehaviour
{
	/*public void ShowVideoAds ()
	{
		if (Advertisement.IsReady ()) {
			Advertisement.Show ();
		}
		print ("test");
	}

	public void ShowRewardedAd ()
	{
		if (Advertisement.IsReady ("rewardedVideoZone")) {
			var options = new ShowOptions { resultCallback = HandleShowResult };
			Advertisement.Show ("rewardedVideoZone", options);
		}
	}

	private void HandleShowResult (ShowResult result)
	{
		switch (result) {
		case ShowResult.Finished:
			Debug.Log ("The ad was successfully shown.");
			SceneManager.LoadSceneAsync ("level");
			// YOUR CODE TO REWARD THE GAMER
			// Give coins etc.
			break;
		case ShowResult.Skipped:
			Debug.Log ("The ad was skipped before reaching the end.");
			break;
		case ShowResult.Failed:
			Debug.LogError ("The ad failed to be shown.");
			break;
		}
	}*/
}