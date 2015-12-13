using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{

	// Use this for initialization
	GameObject[] charUnlocked;
	public Text selectedCharacterNameText, charStatusText;
	string charNameTemp;
	public Button selectCharButton;
	public GameObject charPreviewPos;
	public GameObject charTemp;
	public Text charNumberText;
	int charNumber;
	Mesh initialMesh;
	Hashtable optional;
	float charIndex = 0;
	public ScrollRect charScrollRect;
	public static CharacterSelection m_instance = null;
	public Vector2 scrollValue = new Vector2 (0, 0);

	public void Awake ()
	{
		m_instance = this;
		charUnlocked = GameObject.FindGameObjectsWithTag ("charUnlocked");
		optional = new Hashtable ();
		optional.Add ("ease", LeanTweenType.notUsed);
		charScrollRect.GetComponent<ScrollRect> ().horizontalNormalizedPosition = PlayerPrefs.GetFloat ("lastScrollValue");

	}

	public void ScanAllChars ()
	{
		charUnlocked = GameObject.FindGameObjectsWithTag ("charUnlocked");
	}

	public void SetCharName ()
	{
		PlayerPrefs.SetString ("currentCharacterSelected", charNameTemp);
	}
	
	// Update is called once per frame
	void Update ()
	{
		foreach (GameObject chars in charUnlocked) {
			chars.transform.Rotate (0, 0, 60 * Time.deltaTime);
		}
		charTemp.transform.Rotate (0, 0, 60 * Time.deltaTime);
	}

	void OnTriggerEnter (Collider other)
	{
		
		charNumber++;
		charNumberText.text = charNumber.ToString () + "/100";

		charTemp.SetActive (true);
		charTemp.tag = other.gameObject.transform.GetChild (0).gameObject.tag;

		charTemp.gameObject.transform.localScale = new Vector3 (125, 125, 125);

		LeanTween.scale (charTemp.gameObject, new Vector3 (300, 300, 300), 0.15f, optional);		
		
		charTemp.GetComponent<MeshFilter> ().mesh = other.gameObject.transform.GetChild (0).GetComponent<MeshFilter> ().mesh;
		charTemp.GetComponent<MeshRenderer> ().material.mainTexture = other.gameObject.transform.GetChild (0).GetComponent<MeshRenderer> ().material.mainTexture;

		selectedCharacterNameText.text = other.name;
		switch (other.gameObject.transform.GetChild (0).transform.tag) {
		case "charUnlocked":
			charStatusText.text = "";
			charNameTemp = other.gameObject.transform.GetChild (0).transform.name;
			selectCharButton.interactable = true;
			break;
		case "charLocked":
			charStatusText.text = "Locked";
			selectCharButton.interactable = false;
			break;
		case "charHidden":
			charStatusText.text = "???";
			selectCharButton.interactable = false;
			break;
		}		
		other.gameObject.transform.GetChild (0).GetComponent<MeshRenderer> ().enabled = false;
	}

	void OnTriggerExit (Collider other)
	{
		other.gameObject.transform.GetChild (0).GetComponent<MeshRenderer> ().enabled = true;

		charNumberText.text = charNumber.ToString () + "/100";

		charTemp.gameObject.transform.localScale = new Vector3 (300, 300, 300);

		//LeanTween.cancel (charTemp.gameObject);
		LeanTween.scale (charTemp.gameObject, new Vector3 (125, 125, 125), 0.15f, optional);
		other.gameObject.SetActive (true);
	}

	void OnDisable ()
	{
		charTemp.SetActive (false);
	}

	public void ScrollValueChange (Vector2 scrollRectValue)
	{	
		scrollValue = scrollRectValue;
		charIndex = scrollRectValue.x * 100;
		charNumberText.text = charIndex.ToString ("F0") + "/100";
	}
}
