using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

public class ObjectPlacer : MonoBehaviour
{
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
	//****************** Player's XP Vars
	public int playerXP = 0;
	TextAsset levelData;
	string[] lines;
	string[] chars;
	int charAdder = 0;

	//***********************
	Hashtable optional;

	void ReadPlayerXPFile (int xp)
	{
		lines = new string[100];
		levelData = Resources.Load ("PlayerXP/xp") as TextAsset;
		lines = Regex.Split (levelData.text, "\r\n");
		chars = Regex.Split (lines [xp], ",");
	}

	void Start ()
	{		
		ReadPlayerXPFile (playerXP);
		cargoTruckIniPosition = cargoTruck.transform.localPosition;
		cargoTruckIniRotation = cargoTruck.transform.localEulerAngles;
//		print (cargoTruckIniPosition);
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
				print ("Cargo Start");
				CargoTruckSequenceStart ();
			}
			break;
		case "csE":
			if (isCargoTruckStarted) {
				print ("Cargo End");
				CargoTruckSequenceEnd ();
			}
			break;
		default:			
			calDigit2 ();
			digit2 = digitQ.Dequeue ();
//			print (chars [charAdder] + digit2);
			blocks [FindArrayIndex (chars [charAdder] + digit2)].transform.position = new Vector3 (posAdder, 0);
			blocks [FindArrayIndex (chars [charAdder] + digit2)].SetActive (true);
			break;
		}

		if (charAdder >= chars.Length - 1) {
			print ("char ended" + Random.Range ((chars.Length / 2), chars.Length));
			charAdder = Random.Range (1, (chars.Length - 1));
		}
		charAdder++;
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
		//StopAllCoroutines ();
		StartCoroutine ("CargoTruckEndingAnimation");

	}

	void CargoTruckSequenceStart ()
	{
		blocks [FindArrayIndex ("d2cs0")].transform.position = new Vector3 (posAdder, 0);
		blocks [FindArrayIndex ("d2cs0")].SetActive (true);
		isCargoTruckStarted = true;
		//StopAllCoroutines ();
		StartCoroutine ("CargoTruckStartingAnimation");

		/*cargoDigit2 = cargoDigit2 + 1;
		if (cargoDigit2 > 3) {
			cargoDigit2 = 1;
		}*/
		/*blocks [int.Parse (cargoDigit1 + "" + cargoDigit2)].transform.position = new Vector3 (posAdder, 0);
		blocks [int.Parse (cargoDigit1 + "" + cargoDigit2)].SetActive (true);*/
	}

	/*IEnumerator CargoTruckEndingAnimation ()
	{
		LeanTween.moveLocalX (cargoTruck, 26, 1f, optional);
		yield return new WaitForSeconds (1f);
		cargoTruck.transform.localPosition = new Vector3 (-10, cargoTruck.transform.localPosition.y, cargoTruck.transform.localPosition.z);
	}

	IEnumerator CargoTruckStartingAnimation ()
	{
		yield return new WaitForSeconds (0f);
		LeanTween.moveLocalX (cargoTruck, 16, 2.5f, optional);

	}*/

	IEnumerator CargoTruckStartingAnimation ()
	{
		yield return new WaitForSeconds (2f);
		LeanTween.moveLocalZ (cargoTruck, 5, 1.5f, optional);
		LeanTween.moveLocalX (cargoTruck, 13, 1.5f, optional);
		yield return new WaitForSeconds (1f);
		LeanTween.rotateY (cargoTruck, 90, 0.65f, optional);
	}

	IEnumerator CargoTruckEndingAnimation ()
	{
		LeanTween.moveLocalX (cargoTruck, 26, 0.5f, optional);
		yield return new WaitForSeconds (1f);
		cargoTruck.transform.localPosition = cargoTruckIniPosition;
		cargoTruck.transform.localEulerAngles = cargoTruckIniRotation;
	}


	void CargoTruckStartThrowingObjects ()
	{
		if (GameEventManager.GetState () == GameEventManager.E_STATES.e_game) {
			Instantiate (cargoPrefab, cargoInstantiatePosition.position, Quaternion.identity);
		}
	}

}
