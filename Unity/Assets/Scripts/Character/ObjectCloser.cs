using UnityEngine;
using System.Collections;

public class ObjectCloser : MonoBehaviour
{
	void OnCollisionEnter (Collision collision)
	{
		collision.transform.root.gameObject.SetActive (false);
	}
}
