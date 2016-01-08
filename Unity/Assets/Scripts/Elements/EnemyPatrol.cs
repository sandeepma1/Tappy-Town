using UnityEngine;
using System.Collections;

public class EnemyPatrol : MonoBehaviour
{
	//http://answers.unity3d.com/questions/14279/make-an-object-move-from-point-a-to-point-b-then-b.html
	public Vector3 pointB;
	Vector3 pointA, iniRot;
	bool isMoving = false;
	bool side = false;
	public float speed = 0.1f;
	float i = 0;

	void Start ()
	{
		pointA = transform.localPosition;
		iniRot = transform.localEulerAngles;
		StartCoroutine (MoveObjectA ());
		if (pointB.x <= -1) {
			side = true;
		}
		if (pointB.x <= -1) {
			side = false;
		}
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.CompareTag ("Player") && !isMoving) {
			isMoving = true;
			StartCoroutine (MoveObjectA ());
		}
	}

	IEnumerator MoveObjectA ()
	{
		i = 0;
		while (i < 1.0f) {
			i += Time.deltaTime * (speed / 1);
			transform.localPosition = Vector3.Lerp (pointA, pointB, i);	
			yield return null;
		}
		StartCoroutine ("RotateObject");
		StartCoroutine ("MoveObjectB");
	}

	IEnumerator MoveObjectB ()
	{
		i = 0;
		while (i < 1.0f) {
			i += Time.deltaTime * (speed / 1);
			transform.localPosition = Vector3.Lerp (pointB, pointA, i);	
			yield return null;
		}
		StartCoroutine ("RotateObject");
		StartCoroutine ("MoveObjectA");
	}

	IEnumerator RotateObject ()
	{
		side = !side;
		if (side) {
			transform.localEulerAngles = new Vector3 (iniRot.x, 0, iniRot.z);
		} else {
			transform.localEulerAngles = new Vector3 (iniRot.x, 180, iniRot.z);
		}
		yield return null;
	}

}
