using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

public class ObjectPlacer : MonoBehaviour
{
	public static ObjectPlacer m_instance = null;
	Queue<int> digitQ = new Queue<int> ();
	int posAdder = 0;
	GameObject[] blocks;
	//*****************
	int digit2 = 0;
	int tempDigit2 = 0;
	//***************** Cargo Truck Vars
	public GameObject cargoTruck;
	Vector3 cargoTruckIniPosition;
	Vector3 cargoTruckIniRotation;
	public GameObject cargoPrefab;
	public Transform cargoInstantiatePosition;
	bool isCargoTruckStarted = false;
	bool isFlappyBird = false;
	//****************** Player's XP Vars
	public int PlayerXP = 0;
	TextAsset levelData;
	string[] lines;
	string[] chars;
	public int xpDistance;
	int charAdder = 1;

	//***********************
	Hashtable optional;

	void ReadPlayerXPFile (int xp)
	{
		//lines = new string[100];
		levelData = Resources.Load ("PlayerXP/xp") as TextAsset;
		lines = Regex.Split (levelData.text, "\n");
		chars = Regex.Split (lines [xp], ",");
		xpDistance = int.Parse (chars [0]);
	}

	void Awake ()
	{
		m_instance = this;
		if (PlayerPrefsX.GetBool ("useLevelProgress")) {
			ReadPlayerXPFile (PlayerPrefs.GetInt ("PlayerXP"));
		} else {
			ReadPlayerXPFile (49);
		}
	}

	void Start ()
	{			
		cargoTruckIniPosition = cargoTruck.transform.localPosition;
		cargoTruckIniRotation = cargoTruck.transform.localEulerAngles;
		optional = new Hashtable ();
		optional.Add ("ease", LeanTweenType.notUsed);
		int ran;
		ran = Random.Range (2, 7);
		digitQ.Enqueue (ran);
		digitQ.Enqueue (ran - 1);
		digitQ.Enqueue (ran + 2);
		blocks = new GameObject[InfiniteLevelReader.m_instance.numberOfFiles * 10];
		int ctr = 0;
		for (int i = 0; i < InfiniteLevelReader.m_instance.numberOfFiles; i++) {
			for (int j = 0; j < 10; j++) {
				blocks [ctr] = GameObject.Find (InfiniteLevelReader.m_instance.levelData [i].name + j.ToString ());
				blocks [ctr].gameObject.SetActive (false);
				ctr++;
			}
		}
	}

	void Update ()
	{
		if (GameEventManager.GetState () == GameEventManager.E_STATES.e_game) {			
			if (MovingPlatform.m_instance.transform.position.x >= posAdder) {
				posAdder = posAdder + 30;
				NewEnvGenerator ();
			}
		}
	}

	int FindArrayIndex (string name)
	{		
		int i;
		for (i = 0; i < blocks.Length; i++) {
			if (blocks [i].name == name) {				
				break;
			}
		}
		return i;
	}

	void NewEnvGenerator ()
	{
		switch (chars [charAdder]) {
		case "csS":
			if (!isCargoTruckStarted) {
				CargoTruckSequenceStart ();
			}
			break;
		case "csE":
			if (isCargoTruckStarted) {
				CargoTruckSequenceEnd ();
			}
			break;
		case "fbS":
			if (!isFlappyBird) {
				FlappyBirdSequenceStart ();
			}
			break;
		case "fbE":
			if (isFlappyBird) {
				FlappyBirdSequenceEnd ();
			}
			break;
		default:			
			calDigit2 ();
			digit2 = digitQ.Dequeue ();
			blocks [FindArrayIndex (chars [charAdder] + digit2)].transform.position = new Vector3 (posAdder, 0);
			blocks [FindArrayIndex (chars [charAdder] + digit2)].SetActive (true);
			break;
		}
		charAdder++;
		if (charAdder >= chars.Length - 1) {
			print ("char ended going back to 1");
			charAdder = 1;
		}
	}

	void calDigit2 ()
	{
		tempDigit2 = Random.Range (0, 10);
		if (digitQ.Contains (tempDigit2)) {
			calDigit2 ();
		} else {
			digitQ.Enqueue (tempDigit2);
		}
	}

	void CargoTruckSequenceEnd ()
	{
		blocks [FindArrayIndex ("d2cs1")].transform.position = new Vector3 (posAdder, 0);
		blocks [FindArrayIndex ("d2cs1")].SetActive (true);
		isCargoTruckStarted = false;
		StartCoroutine ("CargoTruckEndingAnimation");
	}

	void CargoTruckSequenceStart ()
	{
		blocks [FindArrayIndex ("d2cs0")].transform.position = new Vector3 (posAdder, 0);
		blocks [FindArrayIndex ("d2cs0")].SetActive (true);
		isCargoTruckStarted = true;
		StartCoroutine ("CargoTruckStartingAnimation");
	}

	void FlappyBirdSequenceEnd ()
	{
		blocks [FindArrayIndex ("d2fb1")].transform.position = new Vector3 (posAdder, 0);
		blocks [FindArrayIndex ("d2fb1")].SetActive (true);
		isFlappyBird = false;
	}

	void FlappyBirdSequenceStart ()
	{
		blocks [FindArrayIndex ("d2fb0")].transform.position = new Vector3 (posAdder, 0);
		blocks [FindArrayIndex ("d2fb0")].SetActive (true);
		isFlappyBird = true;
	}

	IEnumerator CargoTruckStartingAnimation ()
	{
		yield return new WaitForSeconds (2f);
		if (GameEventManager.GetState () == GameEventManager.E_STATES.e_game) {	
			LeanTween.moveLocalZ (cargoTruck, 5, 1.5f, optional);
			LeanTween.moveLocalX (cargoTruck, 13, 1.5f, optional);
			yield return new WaitForSeconds (1f);
			LeanTween.rotateY (cargoTruck, 90, 0.65f, optional);
		}
	}

	IEnumerator CargoTruckEndingAnimation ()
	{
		LeanTween.moveLocalX (cargoTruck, 26, 0.5f, optional);
		yield return new WaitForSeconds (1f);
		cargoTruck.transform.localPosition = cargoTruckIniPosition;
		cargoTruck.transform.localEulerAngles = cargoTruckIniRotation;
	}
		
}
