using UnityEngine;
using System.Collections;

public class CarMover : MonoBehaviour
{
	Vector3 moveDirection;
	float moveInSeconds;

	void OnEnable ()
	{		
		moveInSeconds = Random.Range (13f, 15f);
		moveDirection = new Vector3 (0, -1, 0);
	}

	void LateUpdate ()
	{		
		if (GameEventManager.GetState () == GameEventManager.E_STATES.e_game) {
			transform.Translate (moveDirection * (Time.deltaTime * moveInSeconds));		
			if (transform.localPosition.x > 100) {
				Destroy (this.gameObject);
			}
		}
	}
}
