using UnityEngine;
using System.Collections;

public class SelfDestruct : MonoBehaviour
{
		//public float dustructIn = 20;
		// Use this for initialization
		void Start ()
		{
				Destroy (gameObject, 15);
		}
	
}
