using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HelpMenu : MonoBehaviour
{
	public GameObject[] tutGO;
	public Text numberText;
	int i = 0;

	void Start ()
	{
		
		numberText.text = (i + 1).ToString () + "/" + tutGO.Length;
		ShowTutorialImage (tutGO [i]);
		//this.gameObject.SetActive (false);
	}

	public void NextTutorialButtonPressed ()
	{
		i++;
		if (i >= tutGO.Length) {
			i = 0;
		}
		ShowTutorialImage (tutGO [i]);
		numberText.text = (i + 1).ToString () + "/" + tutGO.Length;
	}

	public void BackTutorialButtonPressed ()
	{
		i--;
		if (i < 0) {
			i = tutGO.Length - 1;
		}
		ShowTutorialImage (tutGO [i]);
		numberText.text = (i + 1).ToString () + "/" + tutGO.Length;
	}

	void ShowTutorialImage (GameObject tut)
	{
		foreach (var item in tutGO) {
			item.SetActive (false);
		}
		tut.SetActive (true);
	}
}
