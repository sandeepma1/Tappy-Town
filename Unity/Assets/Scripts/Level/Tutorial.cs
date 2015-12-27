using UnityEngine;
using System.Collections;

public class Tutorial : MonoBehaviour
{
	public GameObject carsLooping;

	// Use this for initialization
	/*void Awake ()
		{
				if (GameEventManager.currentPlayingLevel != 1) {
						this.gameObject.SetActive (false);
				}
		}*/
	void Start ()
	{
		carsLooping.SetActive (false);
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
