using UnityEngine;
using System.Collections;

public class CoinRotation : MonoBehaviour
{
	
	void LateUpdate ()
	{
		transform.Rotate (Vector3.forward, 360.0f * Time.deltaTime);
	}
}
