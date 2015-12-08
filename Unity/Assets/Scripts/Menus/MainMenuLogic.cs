using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenuLogic : MonoBehaviour
{
	public Button playLockButton, charSelcButton;
	public Text heading;
	public Image blankLogo;
	public Text gamename;
	public GameObject settingsMenuGO, warningMenu, charSelcMenu;
	public Image playIcon;
	public Image lockIcon;	
	public GameObject charSelcLogic;
	public GameObject mainCanvas;
	Animator anim;
	int currentIndex = 0;

	void Start ()
	{
		anim = mainCanvas.GetComponent<Animator> ();
		anim.Play ("mmDefault");
		//****************************  Run Once ************************************************
		if (PlayerPrefs.GetInt ("warningMenu") <= 0) {	
			PlayerPrefs.SetInt ("warningMenu", 1);	
			warningMenu.SetActive (true);
			PlayerPrefs.SetString ("currentCharacterSelected", "chr_raver3");
		}//**************************************************************************************
		playIcon.enabled = false;
		lockIcon.enabled = false;
		GameEventManager.currentLevelAttempts = 0;
	}

	public void PlayLockButtonPressed ()
	{
		if (currentIndex != 0 && playIcon.enabled) {
			anim.Play ("mmToLevelmenu");
			StartCoroutine ("StartLevelWait");
		}
	}

	IEnumerator StartLevelWait ()
	{
		yield return new WaitForSeconds (1.2f);
		Application.LoadLevel ("level");
	}

	void Update ()
	{
		if (Input.GetMouseButton (2)) {
			GameEventManager.currentPlayingLevel = 0;
			Application.LoadLevel ("level");
		}
	}

	public void LevelButtonPressed (int index)
	{
		playLockButton.gameObject.SetActive (true);				
		if (PlayerPrefs.GetInt ("unlockedLevels") >= index - 1) {						
			heading.text = "Showing Preview of Level " + index;
			playIcon.enabled = true;
			lockIcon.enabled = false;
			currentIndex = index;
			GameEventManager.currentPlayingLevel = index; 
		} else {
			heading.text = "Complete Level " + (index - 1) + " to unlock this level";
			playIcon.enabled = false;
			lockIcon.enabled = true;
		}
	}

	public void SettingsButtonPressed ()
	{
		settingsMenuGO.SetActive (true);
	}
	public void CloseSettingsmenu ()
	{
		settingsMenuGO.SetActive (false);
	}
	public void CloseWarningMenu ()
	{
		warningMenu.SetActive (false);
	}
	public void CloseCharacterSelectionMenu ()
	{				
		charSelcMenu.SetActive (false);
	}
	public void SelectChar ()
	{
		charSelcLogic.GetComponent<CharacterSelection> ().SetCharName ();
		charSelcMenu.SetActive (false);
	}

	public void ScrollList (Vector2 val)
	{
		print (val);
	}
	public void OpenCharacterSelectionMenu ()
	{
		charSelcMenu.SetActive (true);
		charSelcLogic.GetComponent<CharacterSelection> ().ScanAllChars ();
	}

	IEnumerator BlankScreen ()
	{
		yield return new WaitForSeconds (0.3f);
		gamename.gameObject.SetActive (true);
		yield return new WaitForSeconds (0.4f);
				
	}
	public void ResetGameData ()
	{
		PlayerPrefs.DeleteAll ();
		Application.LoadLevel ("MainMenu");
	}
}