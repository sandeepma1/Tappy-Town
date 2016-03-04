using UnityEngine;
using System.Collections;

public class MoveTowarsPlayer : MonoBehaviour
{

	public float time = 1f;
	public Vector3 moveDirection = new Vector3 (0, 0, 0);
	public float maxMove = 0;
	Vector3 iniPos, iniRot;
	//Transform iniMoverTransform;
	public GameObject mover;
	bool isMoving = false;

	void Start ()
	{
		//iniMoverTransform = mover.transform;
		iniPos = mover.transform.localPosition;
		iniRot = mover.transform.localEulerAngles;			
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.name == "man") {
			isMoving = true;
			mover.transform.localEulerAngles = new Vector3 (iniRot.x, 90, iniRot.z);
		}
	}

	void LateUpdate ()
	{
		if (isMoving) {
			mover.transform.Translate (moveDirection * (Time.deltaTime * time));
			if (mover.transform.localPosition.x <= maxMove) {
				mover.transform.localPosition = iniPos;
				mover.transform.localEulerAngles = iniRot;
				isMoving = false;
			}
		}
	}
}
