using UnityEngine;
using System.Collections;

public class ObjectPoolLight : MonoBehaviour
{

	public GameObject cube;
	GameObject[] ground = null;
	public int numberOfGround = 0;
	// Use this for initialization
	void Start ()
	{
		ground = new GameObject[numberOfGround];
		InstantiateProjectiles ();
		StartCoroutine ("groundMaker");
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetMouseButtonDown (0)) {
			//StartCoroutine ("groundMaker");
		}
	}
	IEnumerator groundMaker ()
	{
		ActivateProjectile ();
		yield return new WaitForSeconds (0.14f);
		StartCoroutine ("groundMaker");
	}

	private void InstantiateProjectiles ()
	{
		for (int i = 0; i < numberOfGround; i++) {
			ground [i] = Instantiate (cube, new Vector3 (-20, 0, 0), new Quaternion (0, 0, 0, 0)) as GameObject;
			ground [i].transform.position = Vector3.zero;
			ground [i].SetActive (false);
		}
	}

	private void ActivateProjectile ()
	{
		for (int i = 0; i < numberOfGround; i++) {
			if (ground [i].activeSelf == false) {
				ground [i].SetActive (true);
				ground [i].GetComponent<movingGround> ().Activate ();
				return;
			}
		}
	}
}
