using UnityEngine;
using System.Collections;

public class CarSpwaner : MonoBehaviour
{
	public GameObject enemy;
	float spawnTime = 5f;

	void Start ()
	{	
		//InvokeRepeating ("Spawn", spawnTime, spawnTime);
	}

	void Spawn ()
	{
		/*if(playerHealth.currentHealth <= 0f)
		{
			return;
		}*/

		Instantiate (enemy, this.transform.position, enemy.transform.localRotation);

	}

	void LateUpdate ()
	{		
		if (GameEventManager.GetState () == GameEventManager.E_STATES.e_game) {
			spawnTime -= Time.deltaTime;
			if (spawnTime <= 0) {
				spawnTime = Random.Range (4, 9);
				Spawn ();
			}
		}
	}
}


