using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CoinCalculation : MonoBehaviour
{
	public Text coinsText;
	int coinAchievement = 0;
	//public Animation coinScaler;

	void Start ()
	{
		coinsText.text = PlayerPrefs.GetInt ("Coins").ToString ();
	}

	public void UpdateCoinsOnUI ()
	{
		coinsText.text = PlayerPrefs.GetInt ("Coins").ToString ();
	}

	public void AddCoins (int nos)
	{
		coinAchievement++;
		/*if (coinAchievement >= 10) {
			Social.ReportProgress ("CgkIqM2wutYIEAIQAg", 10, (bool success) => {
			});
		}*/
		PlayerPrefs.SetInt ("Coins", PlayerPrefs.GetInt ("Coins") + nos);
		coinsText.text = PlayerPrefs.GetInt ("Coins").ToString ();
		//coinsText.GetComponent<Animation> ().Play ("CoinScale");
	}

	IEnumerator ScaleCoinText ()
	{
		//coinsText.rectTransform.sizeDelta = new Vector2 (1.2f, 1.2f);
		yield return new WaitForSeconds (0.1f);				
	}

	public void add100Coins ()
	{
		AddCoins (100);
		UpdateCoinsOnUI ();
	}
}
