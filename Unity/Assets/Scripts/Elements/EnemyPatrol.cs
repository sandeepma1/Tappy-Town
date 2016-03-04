using UnityEngine;
using System.Collections;

public class EnemyPatrol : MonoBehaviour
{
	//http://answers.unity3d.com/questions/14279/make-an-object-move-from-point-a-to-point-b-then-b.html
	public Vector3 pointB;
	public GameObject mover;
	Vector3 pointA, iniRot;
	bool isMoving = false;
	bool side = false;
	public float speed = 0.1f;
	float i = 0;

	void Start ()
	{
		pointA = mover.transform.localPosition;
		iniRot = mover.transform.localEulerAngles;
	
		if (pointB.x <= -1) {
			side = true;
			mover.transform.localEulerAngles = new Vector3 (iniRot.x, 90, iniRot.z);
		}
		if (pointB.x >= -1) {
			side = false;
			mover.transform.localEulerAngles = new Vector3 (iniRot.x, -90, iniRot.z);
		}
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.CompareTag ("Player") && !isMoving) {
			isMoving = true;
			StartCoroutine ("MoveObjectA");
			StartCoroutine ("ResetPatrolUnit");
		}
	}

	IEnumerator ResetPatrolUnit ()
	{
		yield return new WaitForSeconds (3);
		if (GameEventManager.GetState () == GameEventManager.E_STATES.e_game) {
			StopCoroutine ("MoveObjectA");
			StopCoroutine ("MoveObjectB");
			StopCoroutine ("RotateObject");
			mover.transform.localPosition = pointA;
			mover.transform.localEulerAngles = iniRot;
			isMoving = false;
		} else {
			StartCoroutine ("ResetPatrolUnit");
		}

	}

	IEnumerator MoveObjectA ()
	{
		i = 0;
		while (i < 1.0f) {
			i += Time.deltaTime * (speed / 1);
			mover.transform.localPosition = Vector3.Lerp (pointA, pointB, i);	
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
			mover.transform.localPosition = Vector3.Lerp (pointB, pointA, i);	
			yield return null;
		}
		StartCoroutine ("RotateObject");
		StartCoroutine ("MoveObjectA");
	}

	IEnumerator RotateObject ()
	{
		side = !side;
		if (side) {
			mover.transform.localEulerAngles = new Vector3 (iniRot.x, 90, iniRot.z);
		} else {
			mover.transform.localEulerAngles = new Vector3 (iniRot.x, -90, iniRot.z);
		}
		yield return null;
	}

}
