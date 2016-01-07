using UnityEngine;
using System.Collections;

public class EnemyPatrol : MonoBehaviour
{
	//http://answers.unity3d.com/questions/14279/make-an-object-move-from-point-a-to-point-b-then-b.html
	public Vector3 pointB;
	bool side = false;
	public float speed = 0.1f;

	IEnumerator Start ()
	{
		Vector3 pointA = transform.position;
		while (true) {
			yield return StartCoroutine (MoveObject (transform, pointA, pointB, 1));
			//yield return StartCoroutine (RotateObject (transform));
			yield return StartCoroutine (MoveObject (transform, pointB, pointA, 1));
			//yield return StartCoroutine (RotateObject (transform));
		}
	}

	IEnumerator MoveObject (Transform thisTransform, Vector3 startPos, Vector3 endPos, float time)
	{
		float i = 0.0f;
		//speed = 1.0f / time;
		while (i < 1.0f) {
			i += Time.deltaTime * (speed / time);
			thisTransform.position = Vector3.Lerp (startPos, endPos, i);
			yield return null; 
		}
	}

	IEnumerator RotateObject (Transform thisTransform)
	{
		side = !side;
		if (side) {
			thisTransform.Rotate (thisTransform.eulerAngles.x, thisTransform.eulerAngles.y, -180, Space.Self);
			//transform.RotateAround (transform.position, transform.up, -90);
			//transform.rotation = Quaternion.Slerp (Quaternion.identity, Quaternion.identity, 0);
		} else {
			thisTransform.Rotate (thisTransform.eulerAngles.x, thisTransform.eulerAngles.y, 0, Space.Self);
			//transform.RotateAround (transform.position, transform.up, 90f);
			//transform.rotation = Quaternion.Slerp (Quaternion.identity, Quaternion.identity, 0);
		}
		yield return null;
	}
}
