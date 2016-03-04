using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MovingPlatform : MonoBehaviour
{
	public Vector3 moveDirection = new Vector3 (2, 0, 0);
	Vector3 tempMoveDirection;
	public float moveInSeconds = 0.0f;
	public static Vector3 playerPos, manPos;
	public GameObject man;
	public GameObject IGM_GO;
	//public Slider progressBarSlider;
	//float lastBest = 0;
	//public Text percentText;
	public Text Score;
	public static MovingPlatform m_instance = null;

	/*	void OnDisable ()
	{
		this.gameObject.SetActive (true);
	}*/

	void Start ()
	{
		m_instance = this;
		tempMoveDirection = moveDirection;
		playerPos = this.transform.position;
		manPos = man.transform.position;
	}

	public void SpeedStopper (float speed)
	{
		moveInSeconds = speed;
		if (moveInSeconds <= 0) {
			moveInSeconds = 0;
		}
	}

	void Update ()
	{
		if (GameEventManager.GetState () == GameEventManager.E_STATES.e_game) {
			transform.Translate (moveDirection * (Time.deltaTime * moveInSeconds));
		}
	}

	void LateUpdate ()
	{		
		Score.text = transform.position.x.ToString ("F0");

		/*if (transform.position.x >= 500) {
			Social.ReportProgress ("CgkIqM2wutYIEAIQBw", 500, (bool success) => {
			});
		}*/
	}

	public bool isHighScore ()
	{
		SaveLastBestRunScore ();
		if (June.LocalStore.Instance.GetInt ("lastBestScore") < (int)transform.position.x) {
			June.LocalStore.Instance.SetInt ("lastBestScore", (int)transform.position.x);
			return true;
		} 
		return false;
	}

	public void SaveLastBestRunScore ()
	{
		June.LocalStore.Instance.SetInt ("PlayerDistanceCovered", June.LocalStore.Instance.GetInt ("PlayerDistanceCovered") + (int)transform.position.x);
		June.LocalStore.Instance.SetInt ("Mission_DistanceCount", June.LocalStore.Instance.GetInt ("Mission_DistanceCount") + (int)transform.position.x);
		June.LocalStore.Instance.SetInt ("Progression_DistanceCount", June.LocalStore.Instance.GetInt ("Progression_DistanceCount") + (int)transform.position.x);
		Progression.m_instance.UpdatePlayerXP ();
	}

	public void ResetPosition ()
	{
		this.transform.position = playerPos;
		man.transform.position = manPos;
	}

	/*public void lastBestFun ()
	{
		print (lastBest);
		if (lastBest > June.LocalStore.Instance.GetFloat ("lastBest")) {
			June.LocalStore.Instance.SetFloat ("lastBest", lastBest);
		}
	}*/

	public void SpeedUp ()
	{
		StartCoroutine ("SpeedUpPlayer");
	}

	IEnumerator SpeedUpPlayer ()
	{
		moveDirection = new Vector3 (10, 0, 0);
		yield return new WaitForSeconds (0.3f);
		moveDirection = tempMoveDirection;

	}
}
