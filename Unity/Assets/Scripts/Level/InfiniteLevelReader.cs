using UnityEngine;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class InfiniteLevelReader : MonoBehaviour
{
	public GameObject[] a, b, c, d, g, p, h;
	public Light mainLight, mainLightWithShadow, nightLight;
	public GameObject lastBestBoard;
	public GameObject defaultWorld, world2;
	public Transform startPointPosition;
	string[] text;
	int totalSections = 0;
	static int sectionHeight = 10;
	int count = 0, lineCount = 0;
	string[] lines;
	string[] chars;
	int xPos = 0, yPos = 0, zPos = 0, zPosTemp = 0;
	TextAsset[] levelData;
	GameObject[] objToSpawn;
	static public int numberOfFiles = 4;
	GameObject[] blocks;

	float setLastBestBoardPosition;
	int worldIndex = 0;


	/*	void CreateEmptyGOs (int nos, string name)
    {
      objToSpawn = new GameObject[nos];
      for (int i = 0; i < nos; i++) {
        objToSpawn [i] = new GameObject (name + i);
        objToSpawn [i].transform.position = new Vector3 (0, 0, zPosTemp);
        objToSpawn [i].AddComponent<blockDisableAtPeriod> ();
        zPosTemp = zPosTemp + sectionHeight;
      }
    }*/

	void CreateAllEmptyGOs ()
	{
		objToSpawn = new GameObject[sectionHeight * numberOfFiles];
		int ctr = 0;
		for (int i = 0; i < numberOfFiles; i++) {
			for (int j = 0; j < sectionHeight; j++) {
				objToSpawn [ctr] = new GameObject (i.ToString () + j.ToString ());
				objToSpawn [ctr].transform.position = new Vector3 (0, 0, zPosTemp);
				zPosTemp = zPosTemp + sectionHeight;
				ctr++;
			}
		}
	}

	void SetupGameEnvironment ()
	{
		if ((PlayerPrefs.GetString ("currentCharacterSelected")) == "") {
			PlayerPrefs.SetString ("currentCharacterSelected", "chr_raver3");
		}
		/****************************************************************************
worldIndex = 0 //Default World
worldIndex = 1 //Alien World


		/****************************************************************************/

		switch (PlayerPrefs.GetString ("currentCharacterSelected")) {		
		case "chr_bananaman":// Dark world Default Index
			NightModeON (true);
			StartItemsMaker (defaultWorld);
			break;
		case "alien_engi3":// Dark world Default Index
			worldIndex = 1;
			StartItemsMaker (defaultWorld);
			NightModeON (false);
			break;	
		case "chr_bridget":
			worldIndex = 1;
			StartItemsMaker (world2);
			NightModeON (false);
			break;
		default:
			NightModeON (false);
			worldIndex = 0;
			StartItemsMaker (defaultWorld);
			break;
		}
	}

	void StartItemsMaker (GameObject startItem)
	{
		Instantiate (startItem, startPointPosition.position, Quaternion.identity);
		//startPointPosition.position = startItem.transform.position;
	}

	void NightModeON (bool active)
	{
		mainLight.gameObject.SetActive (!active);
		mainLightWithShadow.gameObject.SetActive (!active);
		nightLight.gameObject.SetActive (active);
	}
	void Awake ()
	{
		SetupGameEnvironment ();
		yPos = sectionHeight - 1;// *********** adjusted value
		levelData = new TextAsset[numberOfFiles];
		text = new string[numberOfFiles];
		lines = new string[100];
		objToSpawn = new GameObject[numberOfFiles];
		CreateAllEmptyGOs ();
		for (int i = 0; i < levelData.Length; i++) {
			levelData [i] = Resources.Load ("Levels/" + worldIndex + "/" + i.ToString ()) as TextAsset;
			text [i] = levelData [i].text;
			lines = Regex.Split (text [i], "\r\n");
			totalSections = lines.Length / sectionHeight;
			//CreateEmptyGOs (10, i.ToString ());
			foreach (string line in lines) {
				if (line != "") {// Skip all blank lines
					lineCount++;
					if (lineCount % sectionHeight == 0) {
						zPos = sectionHeight + zPos;
						count++;
						yPos = sectionHeight;
					}
					chars = Regex.Split (line, ",");
					yPos--;
					//yPos = 1;
					foreach (string chars1 in chars) {
						xPos++;
						switch (chars1) {
						case "a0":
							AutoInstantiate (a [0], new Vector3 (xPos, yPos, zPos));
							break;
						case "a1":
							AutoInstantiate (a [1], new Vector3 (xPos, yPos, zPos));
							break;
						case "a2":
							AutoInstantiate (a [2], new Vector3 (xPos, yPos, zPos));
							break;
						case "b0":
							AutoInstantiate (b [0], new Vector3 (xPos, yPos, zPos));
							break;
						case "b1":
							AutoInstantiate (b [1], new Vector3 (xPos, yPos, zPos));
							break;
/*						case "d0":
							int coins = Random.Range (0, 4);
							if (coins >= 2) {
								AutoInstantiate (d [0], new Vector3 (xPos, yPos, zPos));
							}
							break;*/
						case "d0":
							AutoInstantiate (d [0], new Vector3 (xPos, yPos, zPos));
							break;
						case "c":
							int cars = Random.Range (0, 4);
							AutoInstantiate (c [cars], new Vector3 (xPos, yPos, zPos));
							break;
						case "g0":
							AutoInstantiate (g [0], new Vector3 (xPos, yPos, zPos));
							break;
						case "g1":
							AutoInstantiate (g [1], new Vector3 (xPos, yPos, zPos));
							break;
						case "p0":
							AutoInstantiate (p [0], new Vector3 (xPos, yPos, zPos));
							break;
						case "p1":
							AutoInstantiate (p [1], new Vector3 (xPos, yPos, zPos));
							break;
						case "h0":
							AutoInstantiate (h [0], new Vector3 (xPos, yPos, zPos));
							break;
						case "h1":
							AutoInstantiate (h [1], new Vector3 (xPos, yPos, zPos));
							break;
						default:
							break;
						}
					}
					xPos = 0;
				}
			}
		}
		SetLastBestMarker ();
	}
	void SetLastBestMarker ()
	{
		if (PlayerPrefs.GetInt ("lastBestScore") <= 30) {
			lastBestBoard.transform.localPosition = new Vector3 (-100, 0, 0);
		} else {
			lastBestBoard.transform.localPosition = new Vector3 (PlayerPrefs.GetInt ("lastBestScore"), 0, 0);
		}
	}
	void AutoInstantiate (GameObject aa, Vector3 posaa)
	{
		GameObject objectInstance;
		objectInstance = Instantiate (aa, posaa, aa.transform.rotation) as GameObject;
		objectInstance.name = aa.name;
		objectInstance.transform.parent = objToSpawn [count].transform;
	}
}