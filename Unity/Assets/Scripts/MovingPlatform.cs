using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MovingPlatform : MonoBehaviour
{
	Vector3 tempMoveDirection;
	public static Vector3 playerPos, manPos;
	public GameObject man;
	public GameObject IGM_GO;
	public Text Score;
	public static MovingPlatform m_instance = null;
	float moveInSeconds = 0.0f;
	Vector3 moveDirection = new Vector3 (0, 0, 0);
	float gameTime = 0.0f;
	float gameTimeSpeedUp = 10.0f;
	float gameTimeMultiplier = 1.0f;

	void Awake ()
	{
		m_instance = this;
		IniPlayerPosValues ();
		playerPos = this.transform.position;
		manPos = man.transform.position;
	}

	public void IniPlayerPosValues ()
	{
		moveDirection = GameEventManager.playerMoveDirection;
		moveInSeconds = GameEventManager.playerMoveInSeconds;
		tempMoveDirection = moveDirection;
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
			gameTime += Time.deltaTime;
		}
	}

	void LateUpdate ()
	{		
		//Score.text = transform.position.x.ToString ("F0");
		Score.text = gameTime.ToString ("F0");
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
		June.LocalStore.Instance.SetInt ("Mission_DistanceCountSR", June.LocalStore.Instance.GetInt ("Mission_DistanceCountSR") + (int)transform.position.x);
		June.LocalStore.Instance.SetInt ("Progression_DistanceCount", June.LocalStore.Instance.GetInt ("Progression_DistanceCount") + (int)transform.position.x);
		Progression.m_instance.UpdatePlayerXP ();
	}

	public void ResetPosition ()
	{
		this.transform.position = playerPos;
		man.transform.position = manPos;
	}

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
