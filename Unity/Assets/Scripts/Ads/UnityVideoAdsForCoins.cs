using UnityEngine;
using UnityEngine.Advertisements;


public class UnityVideoAdsForCoins : MonoBehaviour
{
	/*public void ShowAd ()
	{
		if (Advertisement.IsReady ()) {
			Advertisement.Show ();
		}
	}

	public void ShowRewardedAd ()
	{
		if (Advertisement.IsReady ("rewardedVideo")) {
			var options = new ShowOptions { resultCallback = HandleShowResult };
			Advertisement.Show ("rewardedVideo", options);
		}
	}

	private void HandleShowResult (ShowResult result)
	{
		switch (result) {
		case ShowResult.Finished:
			Debug.Log ("The ad was successfully shown.");
			RewardPlayer ();
			break;
		case ShowResult.Skipped:
			Debug.Log ("The ad was skipped before reaching the end.");
			break;
		case ShowResult.Failed:
			Debug.LogError ("The ad failed to be shown.");
			break;
		}
	}

	private void RewardPlayer ()
	{
		CoinCalculation.m_instance.AddCoins (30);
	}
*/

}