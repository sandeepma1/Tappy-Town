using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPlacer : MonoBehaviour
{
	Queue<int> digitQ = new Queue<int> ();
	int posAdder = 0;
	GameObject[] blocks;
	//*****************
	int dCount = 0;
	public int difficulty = 3;
	int digit1 = 0;
	int digit2 = 0;
	int finalObjNumber = 0;
	int tempDigit2 = 0;
	//*****************
	bool isReliefMoment = false;
	int reliefMomentCounter = 0;

	void Start ()
	{
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
				GenerateEnvSection ();
			}
			if (MovingPlatform.m_instance.transform.position.x >= 300) {
				
			}
		}
	}

	void GenerateEnvSection ()
	{
		if (dCount >= difficulty) {
			dCount = 0;
			digit1++;
			isReliefMoment = true;
		}
		if (digit1 >= InfiniteLevelReader.numberOfFiles) {
			digit1 = Random.Range (1, InfiniteLevelReader.numberOfFiles);
			print ("digit1 over, new digit1: " + digit1);
		}
		dCount++;
		calDigit2 ();
		digit2 = digitQ.Dequeue ();
		finalObjNumber = int.Parse (digit1 + "" + digit2);
		//************************************************************************************
		if (isReliefMoment) {
			calDigit2 ();
			digit2 = digitQ.Dequeue ();
			finalObjNumber = int.Parse (digit1 + "" + digit2);
			reliefMomentCounter++;
			if (reliefMomentCounter > 1) {
				reliefMomentCounter = 0;
				isReliefMoment = false;
			}
		}
		//*************************************************************************************
//		print (blocks [finalObjNumber]);
		blocks [finalObjNumber].transform.position = new Vector3 (posAdder, 0);
		blocks [finalObjNumber].SetActive (true);
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
}
