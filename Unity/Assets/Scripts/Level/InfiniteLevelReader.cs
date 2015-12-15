using UnityEngine;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class InfiniteLevelReader : MonoBehaviour
{
	public GameObject[] a, b, c, d, g, p, h;
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
	TextAsset[] levelData;
	GameObject[] objToSpawn;
	static public int numberOfFiles = 5;
	GameObject[] blocks;
	
	float setLastBestBoardPosition;
	int worldIndex = 0;
	public static GameObject[] myObjects;
	//I used this to keep track of the number of objects I spawned in the scene.
	public static int numSpawned = 0;
	public static InfiniteLevelReader m_instance = null;

	void test ()
	{		
		Object[] veh = (Object[])Resources.LoadAll ("Vehicles");
		for (int i = 0; i < veh.Length; i++) {
			//print (veh [i].name);
		}
	}

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
			PlayerPrefs.SetString ("currentCharacterSelected", "chr_mailman");
		}		
		switch (PlayerPrefs.GetString ("currentCharacterSelected")) {		
		case "chr_goth1": // Dark world
		case "chr_thief":
			NightModeON (true);
			worldIndex = 0;
			StartItemsMaker (world0);
			break;
		case "chr_bridget":// City World
			NightModeON (false);
			worldIndex = 1;
			StartItemsMaker (world1);
			break;
		case "alien_engi3":// Alien World
		case "alien_eye1c":
			NightModeON (false);
			worldIndex = 2;
			StartItemsMaker (world2);
			break;			
		case "chr_riotcop":// Cop World
			NightModeON (false);
			worldIndex = 3;
			StartItemsMaker (world3);
			break;
		//case "chr_paramedic2":
		//	break;
		
		default:			// Town World - DEFAULT GREEN
			NightModeON (false); 
			worldIndex = 0;
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
		//mainLight.gameObject.SetActive (!active);
		mainLightWithShadow.gameObject.SetActive (!active);
		nightLight.gameObject.SetActive (active);
		spotLight.gameObject.SetActive (active);
	}

	void Awake ()
	{
		m_instance = this;
		SetupGameEnvironment ();
		yPos = sectionHeight - 1;// *********** adjusted value
		levelData = new TextAsset[numberOfFiles];
		//text = new string[numberOfFiles];
		lines = new string[100];
		objToSpawn = new GameObject[numberOfFiles];
		CreateAllEmptyGOs ();

		for (int i = 0; i < levelData.Length; i++) {
			levelData [i] = Resources.Load ("Levels/" + worldIndex + "/" + i.ToString ()) as TextAsset;
			//text [i] = levelData [i].text;
			lines = Regex.Split (levelData [i].text, "\r\n");
			totalSections = lines.Length / sectionHeight;
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