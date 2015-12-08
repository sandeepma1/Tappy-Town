using UnityEngine;
using System.Collections;

public class IniCharactersToUnlock : MonoBehaviour
{
	Quaternion[] chars = new Quaternion[5];
	Quaternion[] a = new Quaternion[5];

	// Use this for initialization
	void Start ()
	{
		for (int i = 0; i < 5; i++) {
			chars [i] = new Quaternion (Random.Range (1, 15), Random.Range (1, 15), Random.Range (1, 15), Random.Range (1, 15));
		}
		InitilizeCharacterValues ();
	}

	void InitilizeCharacterValues ()
	{
		PlayerPrefsX.SetQuaternionArray ("test", chars);
		a = PlayerPrefsX.GetQuaternionArray ("test");

	}
}
