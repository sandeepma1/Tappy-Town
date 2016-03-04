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
		transform.localPosition = new Vector3 (29, 0, 0);
		moveInSeconds = Random.Range (0.5f, 1.5f);
		moveDirection = new Vector3 (Random.Range (-1.0f, -3.0f), 0, 0);
		//transform.localPosition = Vector3.zero;
		//isMoving = true;
	}

	void LateUpdate ()
	{
		if (!isMoving) {
			startDelay += Time.deltaTime;

		}
		if (startDelay > 3 && !isMoving) {
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
