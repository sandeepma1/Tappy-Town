using UnityEngine;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class InfiniteLevelReader : MonoBehaviour
{
	public static InfiniteLevelReader m_instance = null;
	public Light mainLightWithShadow, nightLight, spotLight;
	public GameObject lastBestBoard;
	public int numberOfFiles = 0;
	public TextAsset[] levelData;

	public GameObject[] a, b, c, d, f, p, h, cs, lo;
	TextAsset worldData;
	int countOfBlank = 0;
	//int totalSections = 0;
	static int sectionHeight = 10;
	int count = 0, lineCount = 0;
	string[] lines;
	string[] chars;
	int xPos = 0, yPos = 0, zPos = 0, zPosTemp = 0;
	GameObject[] objToSpawn;
	int ctr = 0;

	void Awake ()
	{	
		m_instance = this;

		SetupGameEnvironment ();

		levelData = Resources.LoadAll <TextAsset> ("LevelBlocks");
		numberOfFiles = levelData.Length;
		yPos = sectionHeight - 1;// *********** adjusted value
		lines = new string[100];
		objToSpawn = new GameObject[numberOfFiles];

		ReadGamePlayElementsFromWorldCSV ();

		CreateAllEmptyGOs ();

		FillGameObjectsInBlocks ();

		SetLastBestMarker ();
	}

	void SetupGameEnvironment ()
	{
		Character currentCharacter = CharacterManager.CurrentCharacterSelected;
		NightModeON (currentCharacter.IsNightModeOn);
		Instantiate (Resources.Load ("Prefabs/StartBlock/" + CharacterManager.CurrentCharacterSelected.WorldName, typeof(GameObject)), Vector3.zero, Quaternion.identity);	
	}

	void NightModeON (bool active)
	{
		if (active) {
			GameEventManager.isNightMode = true;
			RenderSettings.ambientIntensity = 0.3f;
		} else {
			GameEventManager.isNightMode = false;
			RenderSettings.ambientIntensity = 1;
		}
		mainLightWithShadow.gameObject.SetActive (!active);
		nightLight.gameObject.SetActive (active);
		spotLight.gameObject.SetActive (active);

	}

	void ReadGamePlayElementsFromWorldCSV ()
	{
		worldData = Resources.Load ("Worlds/" + CharacterManager.CurrentCharacterSelected.WorldName) as TextAsset;
//		print ("Worlds/" + CharacterManager.CurrentCharacterSelected.WorldName);
		lines = Regex.Split (worldData.text, "\n");
		for (int i = 0; i < lines.Length - 1; i++) {
			if (lines [i] != "") {
				chars = Regex.Split (lines [i], ",");
				switch (chars [0]) {
				case "a":
					FillArray (chars, ref a);
					break;
				case "b":					
					FillArray (chars, ref b);
					break;
				case "c":					
					FillArray (chars, ref c);
					break;
				case "d":					
					FillArray (chars, ref d);
					break;
				case "f":					
					FillArray (chars, ref f);
					break;
				case "p":					
					FillArray (chars, ref p);
					break;
				case "h":					
					FillArray (chars, ref h);
					break;
				case "cs":					
					FillArray (chars, ref cs);
					break;
				case "lo":					
					FillArray (chars, ref lo);
					break;
				default:
					break;
				}
			}
		}
	}

	void FillArray (string[] array, ref GameObject[] elements)
	{
		countOfBlank = 0;
		foreach (var s in array) {
			if (!s.Equals ("")) {
				countOfBlank++;
			}
		}
		elements = new GameObject[countOfBlank - 1];
		for (int x = 0; x < elements.Length; x++) {
			elements [x] = Resources.Load ("Prefabs/" + array [x + 1]) as GameObject;
		}
	}

	void CreateAllEmptyGOs ()
	{
		objToSpawn = new GameObject[sectionHeight * numberOfFiles];
		for (int i = 0; i < numberOfFiles; i++) {
			for (int j = 0; j < sectionHeight; j++) {
				objToSpawn [ctr] = new GameObject (levelData [i].name + j.ToString ());
				objToSpawn [ctr].transform.position = new Vector3 (0, 0, zPosTemp);
				zPosTemp = zPosTemp + sectionHeight;
				ctr++;
			}
		}
	}

	void FillGameObjectsInBlocks ()
	{
		for (int i = 0; i < levelData.Length; i++) {
			lines = Regex.Split (levelData [i].text, "\n");
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
						case "b3":
							AutoInstantiate (b [3], new Vector3 (xPos, yPos, zPos));
							break;
						case "b4":
							AutoInstantiate (b [4], new Vector3 (xPos, yPos, zPos));
							break;
						case "d0":
							AutoInstantiate (d [0], new Vector3 (xPos, yPos, zPos));
							break;
						case "d1":
							AutoInstantiate (d [1], new Vector3 (xPos, yPos, zPos));
							break;
						case "d2":
							AutoInstantiate (d [2], new Vector3 (xPos, yPos, zPos));
							break;
						case "c":
							int cars = Random.Range (0, 5);
							AutoInstantiate (c [cars], new Vector3 (xPos, yPos, zPos));
							break;
						case "c2":							
							AutoInstantiate (c [2], new Vector3 (xPos, yPos, zPos));
							break;
						case "c3":							
							AutoInstantiate (c [3], new Vector3 (xPos, yPos, zPos));
							break;
						case "f0":
							AutoInstantiate (f [0], new Vector3 (xPos, yPos, zPos));
							break;
						case "f1":
							AutoInstantiate (f [1], new Vector3 (xPos, yPos, zPos));
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
						case "p9":							
							AutoInstantiate (p [9], new Vector3 (xPos, yPos, zPos));
							break;
						case "p10":							
							AutoInstantiate (p [10], new Vector3 (xPos, yPos, zPos));
							break;
						case "p11":							
							AutoInstantiate (p [11], new Vector3 (xPos, yPos, zPos));
							break;
						case "p12":							
							AutoInstantiate (p [12], new Vector3 (xPos, yPos, zPos));
							break;
						case "p13":							
							AutoInstantiate (p [13], new Vector3 (xPos, yPos, zPos));
							break;
						case "p14":							
							AutoInstantiate (p [14], new Vector3 (xPos, yPos, zPos));
							break;
						case "p15":							
							AutoInstantiate (p [15], new Vector3 (xPos, yPos, zPos));
							break;
						case "p16":							
							AutoInstantiate (p [16], new Vector3 (xPos, yPos, zPos));
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
						case "lo4":
							AutoInstantiate (lo [4], new Vector3 (xPos, yPos, zPos));
							break;
						default:
							break;
						}
					}
					xPos = 0;
				}
			}
		}
	}

	void AutoInstantiate (GameObject aa, Vector3 posaa)
	{
		GameObject objectInstance;
		objectInstance = Instantiate (aa, posaa, aa.transform.rotation) as GameObject;
		objectInstance.name = aa.name;
		objectInstance.transform.parent = objToSpawn [count].transform;
	}

	void SetLastBestMarker ()
	{
		if (June.LocalStore.Instance.GetInt ("lastBestScore") <= 30) {
			lastBestBoard.transform.localPosition = new Vector3 (-100, 0, 0);
		} else {
			lastBestBoard.transform.localPosition = new Vector3 (June.LocalStore.Instance.GetInt ("lastBestScore"), 0, 1.25f);
		}
	}


}