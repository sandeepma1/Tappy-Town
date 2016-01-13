using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class SpinBox : MonoBehaviour
{
	public Button redeemButton, tapToContinue, backButton;
	public GameObject spinBox;
	public Animator anim;
	public GameObject prizeMesh;
	public Text prizeInfo;
	public GameObject[] objectMesh;
	string prizeString = "";
	int[] weights;

	int weightTotal;
	int result = 0, total = 0;

	struct things
	{
		public const int characters = 0;
		public const int coins = 1;
		public const int token = 2;
		//public const int enemyShield = 3;		//public const int doubleBalloon = 4;

	}

	void Awake ()
	{
		weights = new int[3];
		weights [things.characters] = 13;
		weights [things.coins] = 25;
		weights [things.token] = 2;		//weights [things.doubleBalloon] = 30;		//weights [things.enemyShield] = 30;
		weightTotal = 0;

		foreach (int w in weights) {
			weightTotal += w;
		}
		anim = spinBox.GetComponent<Animator> ();
		tapToContinue.gameObject.SetActive (false);
	}

	public void PerformSpin ()
	{
		if (PlayerPrefs.GetInt ("Coins") >= 100) {
			PlayerPrefs.SetInt ("Coins", PlayerPrefs.GetInt ("Coins") - 100);

			StartCoroutine ("ShowTapToContinuePrompt");
		} else {
			//Show Buy Coins menu
		}
	}

	IEnumerator ShowTapToContinuePrompt ()
	{
		redeemButton.gameObject.SetActive (true);
		backButton.gameObject.SetActive (true);
		anim.PlayInFixedTime ("OpenBox");
		RandomWeighted ();
		yield return new WaitForSeconds (3.0f);
		prizeInfo.text = prizeString;
		yield return new WaitForSeconds (1.0f);
		tapToContinue.gameObject.SetActive (true);
	}


	void RandomWeighted ()
	{
		redeemButton.gameObject.SetActive (false);
		backButton.gameObject.SetActive (false);
		IGMLogic.m_instance.unlockNewCharacterButton.gameObject.SetActive (false);
		result = 0;
		total = 0;
		int randVal = Random.Range (0, weightTotal + 1);
		for (result = 0; result < weights.Length; result++) {
			total += weights [result];
			if (total >= randVal)
				break;
		}
		print (result);
		switch (result) {
		case 0:
			ShowCharacterUnlocked ();
			break;
		case 1:
			ShowCoinsUnlocked ();
			break;
		case 2:			
			ShowTokensUnlocked ();
			break;
		case 3:
			ShowEnemyShieldUnlocked ();
			break;
		case 4:
			ShowDoubleBalloonUnlocked ();
			break;			
		default:
			break;
		}
	}

	void ChangePrizeMesh ()
	{
		prizeMesh.GetComponent<MeshFilter> ().sharedMesh = objectMesh [result].GetComponent<MeshFilter> ().sharedMesh;
		prizeMesh.GetComponent<MeshRenderer> ().sharedMaterial.mainTexture = objectMesh [result].GetComponent<MeshRenderer> ().sharedMaterial.mainTexture;
	}

	void ShowCharacterUnlocked ()
	{
		prizeString = "New Character Unlocked";
		ChangePrizeMesh ();
	}

	void ShowCoinsUnlocked ()
	{
		int i = 0;
		i = 100 * Random.Range (1, 6);
		PlayerPrefs.SetInt ("Coins", PlayerPrefs.GetInt ("Coins") + i);
		CoinCalculation.m_instance.UpdateCurrencyOnUI ();
		prizeString = "You win " + i + " Coins";
		ChangePrizeMesh ();
	}

	void ShowTokensUnlocked ()
	{
		int i = 0;
		i = Random.Range (2, 6);
		PlayerPrefs.SetInt ("Token", PlayerPrefs.GetInt ("Token") + i);
		CoinCalculation.m_instance.UpdateCurrencyOnUI ();
		prizeString = "You win " + i + " Tokens";
		ChangePrizeMesh ();
	}

	void ShowDoubleBalloonUnlocked ()
	{
		print ("BalloonUnlocked");
	}

	void ShowEnemyShieldUnlocked ()
	{
		print ("ShiledUnlocked");
	}

}
