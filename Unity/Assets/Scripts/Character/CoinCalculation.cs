using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CoinCalculation : MonoBehaviour
{
	public Text coinsText, tokenText;
	int coinAchievement = 0;
	public static CoinCalculation m_instance = null;
	//public Animation coinScaler;

	void Awake ()
	{
		m_instance = this;
	}

	void Start ()
	{
		coinsText.text = PlayerPrefs.GetInt ("Coins").ToString ();
		tokenText.text = PlayerPrefs.GetInt ("Token").ToString ();
	}

	public void UpdateCurrencyOnUI ()
	{
		coinsText.text = PlayerPrefs.GetInt ("Coins").ToString ();
		tokenText.text = PlayerPrefs.GetInt ("Token").ToString ();
	}

	public void AddCoins (int nos)
	{
		/*coinAchievement++;
		if (coinAchievement >= 10) {
			Social.ReportProgress ("CgkIqM2wutYIEAIQAg", 10, (bool success) => {
			});
		}*/
		PlayerPrefs.SetInt ("Coins", PlayerPrefs.GetInt ("Coins") + nos);
		PlayerPrefs.SetInt ("Mission_CoinCount", PlayerPrefs.GetInt ("Mission_CoinCount") + nos);
		coinsText.text = PlayerPrefs.GetInt ("Coins").ToString ();
	}

	public void AddToken (int nos)
	{
		/*coinAchievement++;
		if (coinAchievement >= 10) {
			Social.ReportProgress ("CgkIqM2wutYIEAIQAg", 10, (bool success) => {
			});
		}*/
		PlayerPrefs.SetInt ("Token", PlayerPrefs.GetInt ("Token") + nos);
		tokenText.text = PlayerPrefs.GetInt ("Token").ToString ();
	}

	IEnumerator ScaleCoinText ()
	{
		//coinsText.rectTransform.sizeDelta = new Vector2 (1.2f, 1.2f);
		yield return new WaitForSeconds (0.1f);				
	}

	public void add100Coins ()
	{
		AddCoins (1000);
		UpdateCurrencyOnUI ();
	}

	public void add100Token ()
	{
		AddToken (100);
		UpdateCurrencyOnUI ();
	}
}
