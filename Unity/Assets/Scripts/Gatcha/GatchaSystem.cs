using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

//using UnityEngine.SceneManagement;

public class GatchaSystem : MonoBehaviour
{
	int coinsToSpin = 0;
	public Button redeemButton, backButton;
	public GameObject tapToContinue;
	public GameObject spinBox, gatchaMenu;
	public Animator anim;
	public GameObject prizeMesh;
	public Text prizeInfo, spinValueText;
	public GameObject[] objectMesh;
	string prizeString = "";
	public float[] weights;
	int result = 0;
	public static GatchaSystem m_instance = null;

	void Awake ()
	{		
		m_instance = this;
		spinValueText.text = GameEventManager.gatchaSpinValue.ToString ();
		coinsToSpin = GameEventManager.gatchaSpinValue;
		anim = spinBox.GetComponent<Animator> ();
		tapToContinue.gameObject.SetActive (false);
		weights = new float[GatchaManager.AllGatchas.Count];
		for (int i = 0; i < GatchaManager.AllGatchas.Count; i++) {
			weights [i] = float.Parse (GatchaManager.AllGatchas [i].Probability);
		}

		//TODO-Pooch-Developer
		spinValueText.text = coinsToSpin.ToString ();
	}

	public void PerformSpin ()
	{
		if (June.LocalStore.Instance.GetInt ("coins") >= coinsToSpin) {
			June.LocalStore.Instance.SetInt ("coins", June.LocalStore.Instance.GetInt ("coins") - coinsToSpin);
			CoinCalculation.m_instance.UpdateCurrencyOnUI ();
			StartCoroutine ("ShowTapToContinuePrompt");
		} else {
			IGMLogic.m_instance.ShowInGameStoreMenu ();
		}
	}

	IEnumerator ShowTapToContinuePrompt ()
	{
		prizeInfo.text = "";
		tapToContinue.gameObject.SetActive (false);
		redeemButton.gameObject.SetActive (true);
		backButton.gameObject.SetActive (true);
		anim.PlayInFixedTime ("OpenBox");
		result = (int)(CalculateProbability (weights));
		RandomWeighted (result);
		yield return new WaitForSeconds (3.0f);
		prizeInfo.text = prizeString;
		CoinCalculation.m_instance.UpdateCurrencyOnUI ();
		yield return new WaitForSeconds (1.0f);
		redeemButton.gameObject.SetActive (true);
		tapToContinue.gameObject.SetActive (true);
		backButton.gameObject.SetActive (true);
	}

	float CalculateProbability (float[] probs)
	{		
		redeemButton.gameObject.SetActive (false);
		backButton.gameObject.SetActive (false);
		float total = GatchaManager.AllGatchas.Count;
		foreach (float elem in probs) {
			total += elem;
		}
		float randomPoint = Random.value * total;
		print (randomPoint);
		for (int i = 0; i < probs.Length; i++) {
			if (randomPoint < probs [i]) {
				return i;
			} else {
				randomPoint -= probs [i];
			}
		}
		return probs.Length - 1;
	}

	void RandomWeighted (int probResult)
	{
		print (result + " " + probResult);
		if (result == 0) {
			ShowCoinsUnlocked ();
		} else if (result == 1) {
			ShowTokensUnlocked ();
		} else if (result >= 2 && result <= 16) {
			ShowCharacterUnlocked (result);
		} else if (result >= 17 && result <= 21) {
			ShowCharTokenUnlocked (result);
		}
	}

	void ShowCharTokenUnlocked (int resultNo)
	{	
		if (!SaveStringArray.isCharacterTokenFull (GatchaManager.AllGatchas [result].ID)) {			
			SaveStringArray.AddCharTokenIDtoUnlock (GatchaManager.AllGatchas [resultNo].ID);
			prizeString = "New Character Token Unlocked";
		} else {
			prizeString = "Try Again";
		}
		ChangePrizeMesh ();
	}

	void ShowCharacterUnlocked (int resultNo)
	{		
		if (SaveStringArray.IsCharacterUnlocked (GatchaManager.AllGatchas [result].ID) == false) {
			SaveStringArray.AddCharIDtoUnlock (GatchaManager.AllGatchas [result].ID);
			prizeString = "New Character Unlocked";
		} else {
			prizeString = "Try Again";
		}

		ChangePrizeMesh ();
	}

	void ShowCoinsUnlocked ()
	{
		June.LocalStore.Instance.SetInt ("coins", June.LocalStore.Instance.GetInt ("coins") + 100);
		prizeString = "You win " + 100 + " Coins";
		ChangePrizeMesh ();
	}

	void ShowTokensUnlocked ()
	{
		June.LocalStore.Instance.SetInt ("tokens", June.LocalStore.Instance.GetInt ("tokens") + 10);
		prizeString = "You win " + 10 + " Tokens";
		ChangePrizeMesh ();
	}

	void ChangePrizeMesh ()
	{
		prizeMesh.GetComponent<MeshFilter> ().sharedMesh = objectMesh [result].GetComponent<MeshFilter> ().sharedMesh;
		prizeMesh.GetComponent<MeshRenderer> ().sharedMaterial.mainTexture = objectMesh [result].GetComponent<MeshRenderer> ().sharedMaterial.mainTexture;
	}

	public void OpenGatchaMenu ()
	{
		IGMLogic.m_instance.mainCanvas.renderMode = RenderMode.ScreenSpaceCamera;
		GameEventManager.SetState (GameEventManager.E_STATES.e_pause);
		redeemButton.gameObject.SetActive (true);
		prizeMesh.GetComponent<MeshFilter> ().mesh = null;
		tapToContinue.gameObject.SetActive (false);
		prizeInfo.text = "";
		backButton.gameObject.SetActive (true);
		//SetMainCameraCanvas (false);
		gatchaMenu.SetActive (true);
		IGMLogic.m_instance.isTextMeshesVisible (false);
		IGMLogic.m_instance.anim.CrossFadeInFixedTime ("rotate", 0.1f);

	}

	public void CloseBackGatchaMenu ()
	{
		gatchaMenu.SetActive (false);
		IGMLogic.m_instance.isTextMeshesVisible (true);
		IGMLogic.m_instance.mainCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
	}

	public void CloseGatchaMenu ()
	{
		tapToContinue.gameObject.SetActive (false);
		gatchaMenu.SetActive (false);
		IGMLogic.m_instance.isTextMeshesVisible (true);
	}

}
