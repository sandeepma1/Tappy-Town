using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//http://answers.unity3d.com/questions/904296/how-to-randomly-instantiate-cubes-that-dont-overla.html

public class Instantiate : MonoBehaviour
{
	[SerializeField]
	private GameObject obj;
	[SerializeField]
	private int NumOfCubes = 10;
	[SerializeField]
	private GameObject SpawnBox;
	private float sizeOfObj = 1f;
	// SET THIS
	List<Vector3> objPositions = new List<Vector3> ();

	private Vector3 SetBoxBoundaries ()
	{
		Vector3 LocalPosition;
		BoxCollider Boundaries = SpawnBox.GetComponent<BoxCollider> ();
		float random = Random.value;
		LocalPosition.x = Mathf.Lerp (Boundaries.center.x + SpawnBox.transform.position.x - Boundaries.extents.x * SpawnBox.transform.localScale.x,
			Boundaries.center.x + SpawnBox.transform.position.x + Boundaries.extents.x * SpawnBox.transform.localScale.x, random);
		random = Random.value;
		LocalPosition.y = Mathf.Lerp (Boundaries.center.y + SpawnBox.transform.position.y - Boundaries.extents.y * SpawnBox.transform.localScale.y,
			Boundaries.center.y + SpawnBox.transform.position.y + Boundaries.extents.y * SpawnBox.transform.localScale.y, random);
		random = Random.value;
		LocalPosition.z = Mathf.Lerp (Boundaries.center.z + SpawnBox.transform.position.z - Boundaries.extents.z * SpawnBox.transform.localScale.z,
			Boundaries.center.z + SpawnBox.transform.position.z + Boundaries.extents.z * SpawnBox.transform.localScale.z, random);
		if (isAvailable (LocalPosition)) {
			objPositions.Add (LocalPosition);
			return LocalPosition;
		} else
			return SetBoxBoundaries ();
	}

	private bool isAvailable (Vector3 pos)
	{
		for (int i = 0; i < objPositions.Count; i++) {
			if (Vector3.Distance (pos, objPositions [i]) < sizeOfObj)
				return false;
		}
		return true;
	}

	void Start ()
	{
		sizeOfObj = obj.transform.localScale.x; // OR WHATEVER YOU WANT
		Generate ();
	}

	void Generate (int i = 0)
	{
		for (i = 0; i < NumOfCubes; i++) {
			//Instantiate (obj, SetBoxBoundaries (), Quaternion.identity);

			GameObject objClone = (GameObject)Instantiate (obj, SetBoxBoundaries (), Quaternion.identity);
			float ranScale = Random.Range (1, 2);
			objClone.transform.localScale = new Vector3 (ranScale, ranScale, ranScale);
			objClone.transform.SetParent (this.transform);
		}
	}
}
