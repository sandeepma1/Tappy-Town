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
	int finalObjNumber = 0;
	int tempDigit2 = 0;
	//***************** Cargo Truck Vars
	public GameObject cargoTruck;
	public GameObject cargoPrefab;
	public Transform cargoInstantiatePosition;
	int cargoDigit1 = 4;
	int cargoDigit2 = 0;
	bool isCargoTruckStarted = true;
	int cargoTruckSequenceStartCounter = 0;
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
		optional = new Hashtable ();
		optional.Add ("ease", LeanTweenType.notUsed);
		int ran;
		ran = Random.Range (2, 4);
		digitQ.Enqueue (ran);
		digitQ.Enqueue (ran - 1);
		digitQ.Enqueue (ran + 2);
		blocks = new GameObject[InfiniteLevelReader.numberOfFiles * 10];
		int ctr = 0;
		for (int i = 0; i < InfiniteLevelReader.numberOfFiles; i++) {
			for (int j = 0; j < 10; j++) {
				blocks [ctr] = GameObject.Find (i.ToString () + j.ToString ());
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

	void NewEnvGenerator ()
	{
		/*	if (int.Parse (chars [charAdder]) == 4) {			
			finalObjNumber = int.Parse (chars [charAdder] + "" + cargoTruckSequenceStartCounter);
			cargoTruckSequenceStartCounter++;
			CargoTruckSequenceStart ();
			print ("over" + cargoTruckSequenceStartCounter);
			if (cargoTruckSequenceStartCounter >= 4) {
				cargoTruckSequenceStartCounter = 0;
			}
		} else {*/
			
		calDigit2 ();
		digit2 = digitQ.Dequeue ();
		finalObjNumber = int.Parse (chars [charAdder] + "" + digit2);
		//	}

		//print (finalObjNumber);
		blocks [finalObjNumber].transform.position = new Vector3 (posAdder, 0);
		blocks [finalObjNumber].SetActive (true);
		charAdder++;
		if (charAdder > chars.Length) {
			print ("char ended");
			charAdder = Random.Range ((chars.Length / 2), chars.Length);
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

	void CargoTruckSequenceStart ()
	{
		if (isCargoTruckStarted) {
			StartCoroutine ("CargoTruckStartingAnimation");
			isCargoTruckStarted = false;
		}
		cargoDigit2 = cargoDigit2 + 1;
		if (cargoDigit2 > 3) {
			cargoDigit2 = 1;
		}
		blocks [int.Parse (cargoDigit1 + "" + cargoDigit2)].transform.position = new Vector3 (posAdder, 0);
		blocks [int.Parse (cargoDigit1 + "" + cargoDigit2)].SetActive (true);
	}

	IEnumerator CargoTruckStartingAnimation ()
	{
		yield return new WaitForSeconds (3f);
		LeanTween.moveLocalX (cargoTruck, 16, 3f, optional);
		yield return new WaitForSeconds (4f);
		InvokeRepeating ("CargoTruckStartThrowingObjects", 0, 1F);
	}

	void CargoTruckStartThrowingObjects ()
	{
		if (GameEventManager.GetState () == GameEventManager.E_STATES.e_game) {
			Instantiate (cargoPrefab, cargoInstantiatePosition.position, Quaternion.identity);
		}
	}

}
