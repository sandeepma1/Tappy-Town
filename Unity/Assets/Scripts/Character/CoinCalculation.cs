using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CoinCalculation : MonoBehaviour
{
	public Text coinsText, tokenText, coinsText_1, tokenText_1;
	//int coinAchievement = 0;
	public static CoinCalculation m_instance = null;
	//public Animation coinScaler;

	void Awake ()
	{
		m_instance = this;
	}

	void Start ()
	{
		coinsText_1.text = coinsText.text = June.LocalStore.Instance.GetInt ("coins").ToString ();
		tokenText_1.text = tokenText.text = June.LocalStore.Instance.GetInt ("tokens").ToString ();
	}

	public void UpdateCurrencyOnUI ()
	{
		coinsText_1.text = coinsText.text = June.LocalStore.Instance.GetInt ("coins").ToString ();
		tokenText_1.text = tokenText.text = June.LocalStore.Instance.GetInt ("tokens").ToString ();
	}

	public void AddCoins (int nos)
	{
		/*coinAchievement++;
		if (coinAchievement >= 10) {
			Social.ReportProgress ("CgkIqM2wutYIEAIQAg", 10, (bool success) => {
			});
		}*/
		June.LocalStore.Instance.SetInt ("coins", June.LocalStore.Instance.GetInt ("coins") + nos);
		June.LocalStore.Instance.SetInt ("Mission_CoinCount", June.LocalStore.Instance.GetInt ("Mission_CoinCount") + nos);
		coinsText_1.text = coinsText.text = June.LocalStore.Instance.GetInt ("coins").ToString ();
	}

	public void AddToken (int nos)
	{
		/*coinAchievement++;
		if (coinAchievement >= 10) {
			Social.ReportProgress ("CgkIqM2wutYIEAIQAg", 10, (bool success) => {
			});
		}*/
		June.LocalStore.Instance.SetInt ("tokens", June.LocalStore.Instance.GetInt ("tokens") + nos);
		tokenText.text = June.LocalStore.Instance.GetInt ("tokens").ToString ();
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
