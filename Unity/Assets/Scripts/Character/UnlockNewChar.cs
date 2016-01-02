using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnlockNewChar : MonoBehaviour
{
	int[] weights;
	int weightTotal;

	struct things
	{
		//this is just for code-read niceness
		public const int aThing = 0;
		public const int anotherThing = 1;
		public const int something = 2;
		public const int somethingElse = 3;
	}

	void Awake ()
	{
		weights = new int[4]; //number of things

		//weighting of each thing, high number means more occurrance
		weights [things.aThing] = 1;
		weights [things.anotherThing] = 2;
		weights [things.something] = 3;
		weights [things.somethingElse] = 20;

		weightTotal = 0;
		foreach (int w in weights) {
			weightTotal += w;
		}
	}

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.P)) {
			RandomWeighted ();
		}
	}

	void RandomWeighted ()
	{
		int result = 0, total = 0;
		int randVal = Random.Range (0, weightTotal + 1);
		for (result = 0; result < weights.Length; result++) {
			total += weights [result];
			if (total >= randVal)
				break;
		}
		print (result);
	}
}
