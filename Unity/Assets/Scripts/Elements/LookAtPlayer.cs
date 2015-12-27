using UnityEngine;
using System.Collections;

public class LookAtPlayer : MonoBehaviour
{
	Transform player;
	bool isChasing = false;
	public Vector3 moveDirection = new Vector3 (5, 0, 0);
	Transform child;
	// Use this for initialization
	void Start ()
	{
		child = transform.GetChild (0);
		player = GameObject.FindGameObjectWithTag ("Player").transform;
	}
	
	// Update is called once per frame
	void Update ()
	{
		child.transform.LookAt (player);
		child.transform.localEulerAngles = new Vector3 (0, child.transform.localEulerAngles.y, 0);
		if (isChasing) {
//			print ("chasing");
			transform.Translate (moveDirection * (Time.deltaTime * 1));
		}

		//child.localPosition = new Vector3 (player.localPosition.x, child.transform.position.y, player.localPosition.z);
		//child.transform.LookAt (player.localPosition);
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.name == "man") {
			isChasing = true;
		}
	}

	void OnTriggerExit (Collider other)
	{
		if (other.name == "man") {
			isChasing = false;
		}
	}

}
