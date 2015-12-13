using UnityEngine;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class levelReader : MonoBehaviour
{
	string text;
	string[] lines;
	string[] chars, charsE;
	int limit = 0;
	int counter_Inner = 0, counter_Outer = 0;
	int count = 0;

	public GameObject[] a, b, c, g, j, t, z;
	public GameObject[] aE, bE, cE;
	TextAsset levelData;
// = Resources.Load ("Level2 - Sheet1") as TextAsset;
	public Slider progressBarSlider;
	public GameObject lastBestBoard;
	float setLastBestBoardPosition;
	bool isLevelData = false;
	// Use this for initialization
	void Awake ()
	{
		//PlayerPrefs.SetInt ("lastPlayedLevel", GameEventManager.currentPlayingLevel);
		//GameEventManager.currentPlayingLevel = PlayerPrefs.GetInt ("lastPlayedLevel");
		if (Resources.Load ("Level" + GameEventManager.currentPlayingLevel).name == null) {

			levelData = Resources.Load ("Level1") as TextAsset;
		} else {
			levelData = Resources.Load ("Level" + GameEventManager.currentPlayingLevel) as TextAsset;
		}
	}

	void Start ()
	{
		text = levelData.text;
		lines = Regex.Split (text, "\r\n");
		counter_Outer = lines.Length;
		// print(yPos);

		foreach (string line in lines) {
			limit++;
			//if (limit == 1) { // For first line only
			//  charsE = Regex.Split(line, ",");
			//  yPos--;
			//  foreach (string ch in charsE) {
			//    counter_InnerE++;
			//    switch (ch) {
			//      default:
			//        break;
			//    }
			//  }
			//  counter_InnerE = 0;
			//}

			if (isLevelData) { // For rest lines only
				chars = Regex.Split (line, ",");
				counter_Outer--;
				// print(yPos);
				foreach (string chars1 in chars) {
					counter_Inner++;
					switch (chars1) {
					case "a0":
              //SimplePool.Spawn (a [0], new Vector3 (xPos, yPos, 0), new Quaternion (0, 0, 0, 0));
						AutoInstantiate (a [0], new Vector3 (counter_Inner, counter_Outer, 0));
						break;
					case "a1":
						AutoInstantiate (a [1], new Vector3 (counter_Inner, counter_Outer, 0));
						break;
					case "a2":
						AutoInstantiate (a [2], new Vector3 (counter_Inner, counter_Outer, 0));
						break;
					case "a3":
						AutoInstantiate (a [3], new Vector3 (counter_Inner, counter_Outer, 0));
						break;
					case "a4":
						AutoInstantiate (a [4], new Vector3 (counter_Inner, counter_Outer, 0));
						break;
					case "a5":
						AutoInstantiate (a [5], new Vector3 (counter_Inner, counter_Outer, 0));
						break;
					case "a6":
						AutoInstantiate (a [6], new Vector3 (counter_Inner, counter_Outer, 0));
						break;
					case "a7":
						AutoInstantiate (a [7], new Vector3 (counter_Inner, counter_Outer, 0));
						break;
					case "a8":
						AutoInstantiate (a [8], new Vector3 (counter_Inner, counter_Outer, 0));
						break;
					case "a9":
						AutoInstantiate (a [9], new Vector3 (counter_Inner, counter_Outer, 0));
						break;
					case "a10":
						AutoInstantiate (a [10], new Vector3 (counter_Inner, counter_Outer, 0));
						break;
					case "a11":
						AutoInstantiate (a [11], new Vector3 (counter_Inner, counter_Outer, 0));
						break;
					case "a12":
						AutoInstantiate (a [12], new Vector3 (counter_Inner, counter_Outer, 0));
						break;
					case "b0":
						AutoInstantiate (b [0], new Vector3 (counter_Inner, counter_Outer, 0));
						break;
					case "b1":
						AutoInstantiate (b [1], new Vector3 (counter_Inner, counter_Outer, 0));
						break;
					case "b2":
						AutoInstantiate (b [2], new Vector3 (counter_Inner, counter_Outer, 0));
						break;
					case "c0":
						AutoInstantiate (c [0], new Vector3 (counter_Inner, counter_Outer, 0));
						break;
					case "g0":
						AutoInstantiate (g [0], new Vector3 (counter_Inner, counter_Outer, 0));
						break;
					case "g1":
						AutoInstantiate (g [1], new Vector3 (counter_Inner, counter_Outer, 0));
						break;
					case "j0": //home
						AutoInstantiate (j [0], new Vector3 (counter_Inner, counter_Outer, 0));
						break;
					case "t0": //home
						AutoInstantiate (t [0], new Vector3 (counter_Inner, counter_Outer, 0));
						break;
					case "z0": //home
						AutoInstantiate (z [0], new Vector3 (counter_Inner, counter_Outer, 0));
						break;
					default:
						break;
					}
					count++;
				}
				counter_Inner = 0;
			}
			isLevelData = true;
		}
		progressBarSlider.maxValue = (count / 13);
		SetLastBestMarker ();
	}

	void SetLastBestMarker ()
	{
		if (PlayerPrefs.GetFloat ("lastBest" + GameEventManager.currentPlayingLevel) >= 99 || PlayerPrefs.GetFloat ("lastBest" + GameEventManager.currentPlayingLevel) <= 0) {
			lastBestBoard.transform.localPosition = new Vector3 (-100, 0, 0);
		} else {
			setLastBestBoardPosition = (progressBarSlider.maxValue * PlayerPrefs.GetFloat ("lastBest" + GameEventManager.currentPlayingLevel)) / 100;
			lastBestBoard.transform.localPosition = new Vector3 (setLastBestBoardPosition + 1, 0, 0);
		}
	}

	void AutoInstantiate (GameObject aa, Vector3 posaa)
	{
		GameObject objectInstance;
		objectInstance = Instantiate (aa, posaa, aa.transform.rotation) as GameObject;
		objectInstance.name = aa.name;
		objectInstance.transform.parent = this.transform;
	}
}
