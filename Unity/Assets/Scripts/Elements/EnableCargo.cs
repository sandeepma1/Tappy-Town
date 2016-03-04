using UnityEngine;
using System.Collections;

public class EnableCargo : MonoBehaviour
{
	void OnTriggerEnter (Collider other)
	{
		transform.FindChild ("cargo").gameObject.SetActive (true);
		Invoke ("DisableObject", 3);
	}

	void DisableObject ()
	{
		if (GameEventManager.GetState () == GameEventManager.E_STATES.e_game) {
			transform.FindChild ("cargo").gameObject.SetActive (false);
		}
	}
}
