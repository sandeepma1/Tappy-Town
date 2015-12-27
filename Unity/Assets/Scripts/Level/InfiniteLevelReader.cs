using UnityEngine;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class InfiniteLevelReader : MonoBehaviour
{
	public GameObject[] a, b, c, d, g, p, h, cs, lo;
	public Light mainLightWithShadow, nightLight, spotLight;
	public GameObject lastBestBoard;
	public GameObject world0, world1, world2, world3;
	//string[] text;
	int totalSections = 0;
	static int sectionHeight = 10;
	int count = 0, lineCount = 0;
	string[] lines;
	string[] chars;
	int xPos = 0, yPos = 0, zPos = 0, zPosTemp = 0;
	GameObject[] objToSpawn;
	public int numberOfFiles = 11;
	GameObject[] blocks;
	public TextAsset[] levelData;
	float setLastBestBoardPosition;
	string worldName = "";
	public static GameObject[] myObjects;
	//I used this to keep track of the number of objects I spawned in the scene.
	public static int numSpawned = 0;
	public static InfiniteLevelReader m_instance = null;

	public Text debugText;

	void CreateAllEmptyGOs ()
	{
		objToSpawn = new GameObject[sectionHeight * numberOfFiles];
		int ctr = 0;
		for (int i = 0; i < numberOfFiles; i++) {
			for (int j = 0; j < sectionHeight; j++) {
				objToSpawn [ctr] = new GameObject (levelData [i].name + j.ToString ());
				objToSpawn [ctr].transform.position = new Vector3 (0, 0, zPosTemp);
				zPosTemp = zPosTemp + sectionHeight;
				ctr++;
			}
		}
	}

	void SetupGameEnvironment ()
	{
		if ((PlayerPrefs.GetString ("currentCharacterSelected")) == "") {
			PlayerPrefs.SetString ("currentCharacterSelected", "chr_mailman");
		}		
		switch (PlayerPrefs.GetString ("currentCharacterSelected")) {		
		case "chr_goth1": // Dark world
		case "chr_thief":
			NightModeON (true);
			worldName = "town";
			StartItemsMaker (world0);
			break;
		case "chr_bridget":// City World
			NightModeON (false);
			worldName = "city";
			StartItemsMaker (world1);
			break;
		case "alien_engi3":// Alien World
		case "alien_eye1c":
			NightModeON (false);
			worldName = "alien";
			StartItemsMaker (world2);
			break;			
		case "chr_riotcop":// Cop World
			NightModeON (false);
			worldName = "cop";
			StartItemsMaker (world3);
			break;
		default:			// Town World - DEFAULT GREEN
			NightModeON (false); 
			worldName = "town";
			StartItemsMaker (world0);
			break;
		}
	}

	void StartItemsMaker (GameObject startItem)
	{
		Instantiate (startItem, Vector3.zero, Quaternion.identity);
	}

	void NightModeON (bool active)
	{
		if (active) {
			GameEventManager.isNightMode = true;
		} else {
			GameEventManager.isNightMode = false;
		}
		mainLightWithShadow.gameObject.SetActive (!active);
		nightLight.gameObject.SetActive (active);
		spotLight.gameObject.SetActive (active);
	}

	void BlocksMaker (string value)
	{
		for (int i = 0; i < value.Length; i++) {
			
		}
	}

	void Awake ()
	{	
		m_instance = this;
		SetupGameEnvironment ();
		levelData = Resources.LoadAll <TextAsset> ("Levels/" + worldName);
		numberOfFiles = levelData.Length;
		yPos = sectionHeight - 1;// *********** adjusted value
		lines = new string[100];
		objToSpawn = new GameObject[numberOfFiles];
		CreateAllEmptyGOs ();
		for (int i = 0; i < levelData.Length; i++) {
			lines = Regex.Split (levelData [i].text, "\r\n");
			totalSections = lines.Length / sectionHeight;
			foreach (string line in lines) {
				if (line != "") {
					lineCount++;
					if (lineCount % sectionHeight == 0) {
						zPos = sectionHeight + zPos;
						count++;
						yPos = sectionHeight;
					}
					chars = Regex.Split (line, ",");
					yPos--;

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
						case "a3":
							AutoInstantiate (a [3], new Vector3 (xPos, yPos, zPos));
							break;
						case "a4":
							AutoInstantiate (a [4], new Vector3 (xPos, yPos, zPos));
							break;
						case "a5":
							AutoInstantiate (a [5], new Vector3 (xPos, yPos, zPos));
							break;
						case "a6":
							AutoInstantiate (a [6], new Vector3 (xPos, yPos, zPos));
							break;
						case "a7":
							AutoInstantiate (a [7], new Vector3 (xPos, yPos, zPos));
							break;
						case "a8":
							AutoInstantiate (a [8], new Vector3 (xPos, yPos, zPos));
							break;
						case "a9":
							AutoInstantiate (a [9], new Vector3 (xPos, yPos, zPos));
							break;
						case "b0":
							AutoInstantiate (b [0], new Vector3 (xPos, yPos, zPos));
							break;
						case "b1":
							AutoInstantiate (b [1], new Vector3 (xPos, yPos, zPos));
							break;
						case "b2":
							AutoInstantiate (b [2], new Vector3 (xPos, yPos, zPos));
							break;
						case "d0":
							AutoInstantiate (d [0], new Vector3 (xPos, yPos, zPos));
							break;
						case "c":
							int cars = Random.Range (0, 2);
							AutoInstantiate (c [cars], new Vector3 (xPos, yPos, zPos));
							break;
						case "c2":							
							AutoInstantiate (c [2], new Vector3 (xPos, yPos, zPos));
							break;
						case "c3":							
							AutoInstantiate (c [3], new Vector3 (xPos, yPos, zPos));
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
						case "p2":
							AutoInstantiate (p [2], new Vector3 (xPos, yPos, zPos));
							break;
						case "p3":
							AutoInstantiate (p [3], new Vector3 (xPos, yPos, zPos));
							break;
						case "p4":							
							AutoInstantiate (p [4], new Vector3 (xPos, yPos, zPos));
							break;
						case "p5":							
							AutoInstantiate (p [5], new Vector3 (xPos, yPos, zPos));
							break;
						case "p6":							
							AutoInstantiate (p [6], new Vector3 (xPos, yPos, zPos));
							break;
						case "p7":							
							AutoInstantiate (p [7], new Vector3 (xPos, yPos, zPos));
							break;
						case "p8":							
							AutoInstantiate (p [8], new Vector3 (xPos, yPos, zPos));
							break;
						case "h0":
							AutoInstantiate (h [0], new Vector3 (xPos, yPos, zPos));
							break;
						case "h1":
							AutoInstantiate (h [1], new Vector3 (xPos, yPos, zPos));
							break;
						case "cb0":
							AutoInstantiate (cs [0], new Vector3 (xPos, yPos, zPos));
							break;						
						case "lo0":
							AutoInstantiate (lo [0], new Vector3 (xPos, yPos, zPos));
							break;
						case "lo1":
							AutoInstantiate (lo [1], new Vector3 (xPos, yPos, zPos));
							break;
						case "lo2":
							AutoInstantiate (lo [2], new Vector3 (xPos, yPos, zPos));
							break;
						case "lo3":
							AutoInstantiate (lo [3], new Vector3 (xPos, yPos, zPos));
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
			lastBestBoard.transform.localPosition = new Vector3 (PlayerPrefs.GetInt ("lastBestScore"), 0, 1.25f);
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