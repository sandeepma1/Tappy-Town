using UnityEngine;
using System.Collections;

public class RandomSpwan : MonoBehaviour
{
	public GameObject ObjectToSpawn;

	// Use this for initialization
	void RandomPos ()
	{
		Vector3 rndPosWithin;
		rndPosWithin = new Vector3 (Random.Range (-1f, 1f), 0, Random.Range (-1f, 1f));
		rndPosWithin = transform.TransformPoint (rndPosWithin * 10);
		print (rndPosWithin);
		for (int i = 0; i < 10; i++) {
			Instantiate (ObjectToSpawn, rndPosWithin, new Quaternion (0, 0, 0, 0)); 
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetMouseButtonDown (1)) {
			RandomPos ();
		}
	}
}
