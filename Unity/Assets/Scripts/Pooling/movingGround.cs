using UnityEngine;
using System.Collections;

public class movingGround : MonoBehaviour
{

	public float life = 3;
	public float speed = 9;
	private float timeToDie = 0;

	// Update is called once per frame
	void Update ()
	{
		CountDown ();
		Automove ();
	}

	public void Activate ()
	{
		timeToDie = Time.time + life;
		transform.position = Vector3.zero;
	}

	private void Deactivate ()
	{
		this.gameObject.SetActive (false);
		transform.position = Vector3.zero;
	}

	private void CountDown ()
	{
		if (timeToDie < Time.time) {
			Deactivate ();
		}
	}

	private void Automove ()
	{
		Vector3 velocity = speed * Time.deltaTime * new Vector3 (-1, 0, 0);
		transform.Translate (velocity);
	}
}
