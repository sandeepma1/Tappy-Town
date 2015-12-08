using UnityEngine;
using System.Collections;

public class blockDisableAtPeriod : MonoBehaviour
{

	/*void OnEnable ()
	{
		if (GameEventManager.GetState () == GameEventManager.E_STATES.e_game) {
			this.gameObject.SetActive (true);
			//StopCoroutine ("DisableAfterSeconds");
			Invoke ("DisableAfterSeconds_I", 8);
			//StartCoroutine ("DisableAfterSeconds");
		}
	}

	void Start ()
	{
		if (GameEventManager.GetState () == GameEventManager.E_STATES.e_game) {
			this.gameObject.SetActive (true);
			Invoke ("DisableAfterSeconds_I", 8);
		}
	}

	void DisableAfterSeconds_I ()
	{
		if (GameEventManager.GetState () == GameEventManager.E_STATES.e_game) {
			this.gameObject.SetActive (false);
		} else
			Invoke ("DisableAfterSeconds_I", 8);
	}
	IEnumerator DisableAfterSeconds ()
	{
		yield return new WaitForSeconds (8);
		if (GameEventManager.GetState () == GameEventManager.E_STATES.e_game) {
			this.gameObject.SetActive (false);
		}

	}*/

	void OnDisable ()
	{
		print (gameObject.name + " was removed");
	}
}
