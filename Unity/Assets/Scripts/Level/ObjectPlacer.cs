﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

public class ObjectPlacer : MonoBehaviour
{
	public static ObjectPlacer m_instance = null;
	public GameObject cargoTruck;
	public int xpDistance;
	string lastBlockPlaced = "";

	Queue<int> digitQ = new Queue<int> ();
	int posAdder = 30;
	GameObject[] blocks;
	//*****************
	int digit2 = 0;
	int tempDigit2 = 0;
	//***************** Cargo Truck Vars
	Vector3 cargoTruckIniPosition;
	Vector3 cargoTruckIniRotation;
	bool isCargoTruckStarted = false;
	bool isFlappyBird = false;
	bool isMiniCar = false;
	//****************** Player's XP Vars
	TextAsset levelData;
	string[] lines;
	string[] chars;
	int charAdder = 1;
	//***********************
	Hashtable optional;

	void ReadPlayerXPFile (int xp)
	{
		levelData = Resources.Load ("PlayerXP/xp") as TextAsset;
		lines = Regex.Split (levelData.text, "\n");
		chars = Regex.Split (lines [xp], ",");
		xpDistance = int.Parse (chars [0]);
	}

	void Awake ()
	{
		m_instance = this;
		if (Bronz.LocalStore.Instance.GetBool ("useLevelProgress")) {
			ReadPlayerXPFile (Bronz.LocalStore.Instance.GetInt ("PlayerXP"));
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
				blocks [ctr].gameObject.SetActive (false); // disables all GOs
				ctr++;
			}
		}
		NewEnvGenerator ();
	}

	void Update ()
	{
		if (GameEventManager.GetState () == GameEventManager.E_STATES.e_game) {			
			if (MovingPlatform.m_instance.transform.position.x >= posAdder) {
				posAdder = posAdder + GameEventManager.patternLength;
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
		print (chars [charAdder]);
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
			case "mcS":
				if (!isMiniCar) {
					MiniCarSequenceStart ();
				}
				break;
			case "mcE":
				if (isMiniCar) {
					MiniCarSequenceEnd ();
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

	public void CargoTruckSequenceEnd ()
	{
		blocks [FindArrayIndex ("d2cs1")].transform.position = new Vector3 (posAdder, 0);
		blocks [FindArrayIndex ("d2cs1")].SetActive (true);
		isCargoTruckStarted = false;
		MissionLogic.m_instance.CargoMissionAdder ();

	}

	public void CargoTruckSequenceEndAnimation ()
	{
		StartCoroutine ("CargoTruckEndingAnimation");
	}

	public void CargoTruckSequenceStart ()
	{
		blocks [FindArrayIndex ("d2cs0")].transform.position = new Vector3 (posAdder, 0);
		blocks [FindArrayIndex ("d2cs0")].SetActive (true);
		isCargoTruckStarted = true;
	}

	public void CargoTruckSequenceStartAnimation ()
	{		
		StartCoroutine ("CargoTruckStartingAnimation");
	}

	void FlappyBirdSequenceEnd ()
	{
		blocks [FindArrayIndex ("d2fb1")].transform.position = new Vector3 (posAdder, 0);
		blocks [FindArrayIndex ("d2fb1")].SetActive (true);
		MissionLogic.m_instance.BalloonMissionAdder ();
		isFlappyBird = false;
	}

	void FlappyBirdSequenceStart ()
	{
		blocks [FindArrayIndex ("d2fb0")].transform.position = new Vector3 (posAdder, 0);
		blocks [FindArrayIndex ("d2fb0")].SetActive (true);
		isFlappyBird = true;
	}

	void MiniCarSequenceEnd ()
	{
		blocks [FindArrayIndex ("d2mc1")].transform.position = new Vector3 (posAdder, 0);
		blocks [FindArrayIndex ("d2mc1")].SetActive (true);
		isMiniCar = false;
	}

	void MiniCarSequenceStart ()
	{
		blocks [FindArrayIndex ("d2mc0")].transform.position = new Vector3 (posAdder, 0);
		blocks [FindArrayIndex ("d2mc0")].SetActive (true);
		isMiniCar = true;
	}


	IEnumerator CargoTruckStartingAnimation ()
	{
		IGMLogic.m_instance.pauseButton.SetActive (false);
		yield return new WaitForSeconds (0f);
		if (GameEventManager.GetState () == GameEventManager.E_STATES.e_game) {	
			LeanTween.moveLocalZ (cargoTruck, 5, 1.5f, optional);
			LeanTween.moveLocalX (cargoTruck, 13, 1.5f, optional);
			yield return new WaitForSeconds (1f);
			LeanTween.rotateY (cargoTruck, 90, 0.65f, optional);
		}
		IGMLogic.m_instance.pauseButton.SetActive (true);
	}

	IEnumerator CargoTruckEndingAnimation ()
	{
		LeanTween.moveLocalX (cargoTruck, 35, 0.5f, optional);
		yield return new WaitForSeconds (1f);
		cargoTruck.transform.localPosition = cargoTruckIniPosition;
		cargoTruck.transform.localEulerAngles = cargoTruckIniRotation;
	}
}
