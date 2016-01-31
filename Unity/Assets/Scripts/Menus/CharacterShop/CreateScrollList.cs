using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Item
{
	public GameObject charMesh;
	public string charName;
	public string charDesc;
	public string charID;
	public string currType;
	public string currValue;
	public string currID;
	public bool isUnlocked;
	public bool isSelected;

	public Item (GameObject mesh, string name, string desc, string id, string cType, string cValue, string cID, bool isUnlck, bool isSelect)
	{
		charMesh = mesh;
		charName = name;
		charDesc = desc;
		charID = id;
		currType = cType;
		currValue = cValue;
		currID = cID;
		isUnlocked = isUnlck;
		isSelected = isSelect;
	}
}

public class CreateScrollList : MonoBehaviour
{
	public List<Sprite> iconBank;
	public GameObject sampleCharacter;
	public Transform contentPanel;
	public Text characterName;
	public Text characterDescription;
	//public GameObject characterPreviewParent;
	public List<Item> itemList;
	GameObject[] unlockedCharactersMesh = new GameObject[100];
	SampleCharacter currentCharacterVisible;
	public GameObject mesh;

	//Unlock Button
	public Text value;
	public Text itemProgress;
	public Image icon;
	public Text select;
	public Button selectButton;
	public Sprite blueSelectButtonTexture, orangeSelectButtonTexture;

	//Unlock Button Variables
	bool isCharacterUnlocked = false;
	string currValueToAsk = "";
	string currTypeToAsk = "";
	string currentCharID = "";

	//Menus
	public GameObject messageMenuGO, newCharacterUnlockedMenuGO;

	//others
	Hashtable optional;
	Vector3 charMeshSize = new Vector3 (0.75f, 0.75f, 0.75f);
	Vector3 charBigSize = new Vector3 (250, 250, 250);
	Vector3 charSmallSize = new Vector3 (75, 75, 75);

	public static CreateScrollList m_instance = null;
	//public string[] unlockedCharacters;
	//string[] unlockedCharsIDs = new string[10];
	void Awake ()
	{
		StartLogic ();
	}

	void StartLogic ()
	{
		m_instance = this;
		optional = new Hashtable ();
		optional.Add ("ease", LeanTweenType.notUsed);
		FillList ();
		PopulateList ();
//		characterPreviewParent.transform.localRotation = new Quaternion (1, 0, 0, 0);
		StartCoroutine ("EnableCollider");			
	}

	IEnumerator EnableCollider () // This is for issue where character fails to zoom at start
	{
		yield return new WaitForSeconds (0.1f);
		gameObject.GetComponent<BoxCollider> ().enabled = true;
	}

	void LateUpdate ()
	{	
		if (isCharacterUnlocked) {			
			mesh.transform.Rotate (0, 0, 60 * Time.deltaTime);
		} else {
			mesh.transform.localRotation = new Quaternion (270, 0, 0, 0);
		}

		for (int i = 0; i < unlockedCharactersMesh.Length; i++) {
			if (unlockedCharactersMesh [i] != null) {
				unlockedCharactersMesh [i].transform.Rotate (0, 0, 60 * Time.deltaTime);
			}
		}
	}

	void FillList ()
	{		
		itemList = new List<Item> ();
		for (int i = 0; i < CharacterManager.AllCharacters.Count; i++) {
			string pathName = "Characters/" + CharacterManager.AllCharacters [i].PrefabName;		
			GameObject mesh = Resources.Load (pathName) as GameObject;

			itemList.Add (new Item (mesh, 
				CharacterManager.AllCharacters [i].Name, 
				CharacterManager.AllCharacters [i].Description, 
				CharacterManager.AllCharacters [i].Id,
				CharacterManager.AllCharacters [i].Currency ["ct"].ToString (), 
				CharacterManager.AllCharacters [i].Currency ["val"].ToString (), 
				CharacterManager.AllCharacters [i].Currency ["id"].ToString (), 
				SaveStringArray.CheckIfIDContains (CharacterManager.AllCharacters [i].Id),
				SaveStringArray.CheckIfIsSelected (CharacterManager.AllCharacters [i].Id))
			);
		}
	}

	void PopulateList ()
	{	
		for (int i = 0; i < itemList.Count; i++) {
			
			GameObject character = Instantiate (sampleCharacter) as GameObject;
			SampleCharacter c = character.GetComponent <SampleCharacter> ();
			character.transform.SetParent (contentPanel);
			character.transform.localScale = charMeshSize;
			character.transform.localPosition = new Vector3 (character.transform.localPosition.x, character.transform.localPosition.y, -278);
			character.transform.localRotation = new Quaternion (0, 0, 0, 0);
					
			c.transform.GetChild (0).GetComponent<MeshFilter> ().sharedMesh = itemList [i].charMesh.GetComponent<MeshFilter> ().sharedMesh;
			c.transform.GetChild (0).GetComponent<MeshRenderer> ().sharedMaterial = itemList [i].charMesh.GetComponent<MeshRenderer> ().sharedMaterial;

			c.isUnlocked = itemList [i].isUnlocked;
			c.isSelected = itemList [i].isSelected;
			c.charName = itemList [i].charName;
			c.charID = itemList [i].charID;

			if (c.isUnlocked == false) {				
				c.transform.GetChild (0).GetComponent<MeshRenderer> ().sharedMaterial.SetColor ("_Color", new Color32 (50, 50, 50, 255));
				c.charDesc = itemList [i].charDesc;
				c.currValue = itemList [i].currValue;
				c.currType = itemList [i].currType;
				c.currID = itemList [i].currID;
			} else {
				c.transform.GetChild (0).GetComponent<MeshRenderer> ().sharedMaterial.SetColor ("_Color", new Color32 (203, 203, 203, 255));
				c.charDesc = "";
				c.currValue = "";
				c.currType = "";
				c.currID = "";
				unlockedCharactersMesh [i] = character.transform.Find ("mesh").gameObject;
				if (c.isSelected) {
					c.currType = "selected";
				}
			}
		}

	}

	void OnTriggerEnter (Collider other)
	{		
		currentCharacterVisible = other.transform.GetComponent<SampleCharacter> ();

		mesh.GetComponent<MeshFilter> ().sharedMesh =	other.transform.FindChild ("mesh").gameObject.GetComponent<MeshFilter> ().sharedMesh;
		mesh.GetComponent<MeshRenderer> ().sharedMaterial = other.transform.FindChild ("mesh").gameObject.GetComponent<MeshRenderer> ().sharedMaterial;

		characterName.text = currentCharacterVisible.charName;
		characterDescription.text = currentCharacterVisible.charDesc;
		isCharacterUnlocked = currentCharacterVisible.isUnlocked;
		currValueToAsk = currentCharacterVisible.currValue;
		currTypeToAsk = currentCharacterVisible.currType;
		currentCharID = currentCharacterVisible.charID;
		FillButtonDetails (currentCharacterVisible.currValue.ToString (), currentCharacterVisible.currType.ToString ());

		other.transform.FindChild ("mesh").gameObject.GetComponent<MeshRenderer> ().enabled = false;
		//mesh.gameObject.transform.localScale = charSmallSize;
		//LeanTween.scale (mesh, charBigSize, 0.05f, optional);	
	}

	void OnTriggerExit (Collider other)
	{
		other.transform.FindChild ("mesh").gameObject.GetComponent<MeshRenderer> ().enabled = true;
		//mesh.gameObject.transform.localScale = charBigSize;
		//LeanTween.scale (mesh, charSmallSize, 0.05f, optional);

	}

	void FillButtonDetails (string t_Value, string t_Type)
	{		
		switch (t_Type) {
		case "coins":			
			FillButtonValues (t_Value, "", "", iconBank [0]);
			break;
		case "tokens":			
			FillButtonValues (t_Value, "", "", iconBank [1]);
			break;
		case "fb":
			FillButtonValues ("Log In", "", "", iconBank [2]);
			break;
		case "usd":			
			FillButtonValues ("$" + t_Value, "", "Buy Now", iconBank [3]);
			break;
		case "collectibles":
			FillButtonValues ("Find", "", "0/" + t_Value, iconBank [4]);
			break;		
		case "":
			FillButtonValues ("", "Select", "", iconBank [3]);
			break;
		case "selected":
			FillButtonValues ("", "Selected", "", iconBank [3]);
			break;
		case "blank":			
			FillButtonValues ("", "", "", iconBank [3]);
			break;
		default:
			break;
		}
	}

	void FillButtonValues (string s_value, string s_select, string s_itemProgress, Sprite s_icon)
	{
		value.text = s_value;
		select.text = s_select;
		itemProgress.text = s_itemProgress;	
		icon.sprite = s_icon;
		if (s_select == "Selected") {
			selectButton.GetComponent<Image> ().sprite = blueSelectButtonTexture;
		} else {
			selectButton.GetComponent<Image> ().sprite = orangeSelectButtonTexture;
		}
	}

	public void SelectButtonPressed ()
	{
		switch (currTypeToAsk) {
		case "coins":
			CheckIfPlayerHaveResources (currTypeToAsk, int.Parse (currValueToAsk));
			break;
		case "tokens":
			CheckIfPlayerHaveResources (currTypeToAsk, int.Parse (currValueToAsk));
			break;
		case "usd":
			IGMLogic.m_instance.ShowinAppStoreMenu ();
			break;
		default:
			break;
		}

		if (selectButton.transform.Find ("Select").transform.GetComponent<Text> ().text == "Select") {
			PlayerPrefs.SetString ("currentCharacterSelectedID", CharacterManager.GetCharacterWithId (currentCharID).PrefabName);
			ResetCharacterSelectionLogic ();
		}
	}

	void CheckIfPlayerHaveResources (string currTypePrefs, int currencyToAsk)
	{
		if (June.LocalStore.Instance.GetInt (currTypePrefs) >= currencyToAsk) {
			June.LocalStore.Instance.SetInt (currTypePrefs, June.LocalStore.Instance.GetInt (currTypePrefs) - currencyToAsk);
			CoinCalculation.m_instance.UpdateCurrencyOnUI ();
			SetContentPanel (false);
			newCharacterUnlockedMenuGO.SetActive (true);
			newCharacterUnlockedMenuGO.transform.Find ("UnlockedCharText").transform.GetComponent<Text> ().text = currentCharacterVisible.charName;
			SaveStringArray.AddCharIDtoUnlock (currentCharID);
		} else {
			SetContentPanel (false);
			messageMenuGO.SetActive (true);
			messageMenuGO.transform.Find ("UnlockedCharText").transform.GetComponent<Text> ().text = "Don't have enough " + currTypePrefs;
		}
	}

	public void CloseMessageMenuGo ()
	{
		messageMenuGO.SetActive (false);
		SetContentPanel (true);
	}

	public void CloseMessageMenu ()
	{
		messageMenuGO.SetActive (false);
		SetContentPanel (true);
	}

	public void CloseNewCharacterUnlockedMessageBox ()
	{
		newCharacterUnlockedMenuGO.SetActive (false);
		ResetCharacterSelectionLogic ();
		SetContentPanel (true);
	}

	void SetContentPanel (bool flag)
	{
		contentPanel.gameObject.SetActive (flag);
		mesh.gameObject.SetActive (flag);
	}

	void ResetCharacterSelectionLogic ()
	{		
		//mesh.gameObject.transform.localScale = charBigSize;
		foreach (Transform child in contentPanel) {
			Destroy (child.gameObject);
		}
		StartLogic ();
	}

}