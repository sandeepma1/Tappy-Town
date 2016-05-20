using UnityEngine;
using System.Collections;

public class moveCars : MonoBehaviour
{
	bool isMoving = false;
	Vector3 moveDirection;
	float moveInSeconds;
	float startDelay = 0;
	// Use this for initialization
	void OnEnable ()
	{		
		transform.localPosition = new Vector3 (0, 200, 0);
		moveInSeconds = Random.Range (2f, 5f);
		moveDirection = new Vector3 (Random.Range (2.0f, 5.0f), 0, 0);
		//transform.localPosition = Vector3.zero;
		//isMoving = true;
	}

	void LateUpdate ()
	{
		if (!isMoving) {
			startDelay += Time.deltaTime;

		}
		if (startDelay > 6 && !isMoving) {
			isMoving = true;
		}
		if (GameEventManager.GetState () == GameEventManager.E_STATES.e_game && isMoving) {
			transform.Translate (moveDirection * (Time.deltaTime * moveInSeconds));
		}
	}

	void OnDisable ()
	{
		isMoving = false;
	}
}
