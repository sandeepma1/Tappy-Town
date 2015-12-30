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
	public Slider progressBarSlider;
	//float lastBest = 0;
	public Text percentText;
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
		//lastBestText.text = PlayerPrefs.GetFloat ("lastBest" + GameEventManager.currentPlayingLevel).ToString ("F0") + "%";
	}

	void Update ()
	{
		if (GameEventManager.GetState () == GameEventManager.E_STATES.e_game) {
			transform.Translate (moveDirection * (Time.deltaTime * moveInSeconds));
		}
	}

	void LateUpdate ()
	{
		//progressBarSlider.value = transform.position.x;
		//percentText.text = (101 - (((progressBarSlider.maxValue - transform.position.x) / progressBarSlider.maxValue) * 100)).ToString ("F0");
		Score.text = transform.position.x.ToString ("F0");
		/*if (transform.position.x >= 500) {
			Social.ReportProgress ("CgkIqM2wutYIEAIQBw", 500, (bool success) => {
			});
		}*/
	}

	public void SaveLastBestRunScore ()
	{
		PlayerPrefs.SetInt ("PlayerDistanceCovered", PlayerPrefs.GetInt ("PlayerDistanceCovered") + (int)transform.position.x);
		if (PlayerPrefs.GetInt ("lastBestScore") <= (int)transform.position.x) {
			PlayerPrefs.SetInt ("lastBestScore", (int)transform.position.x);
			/*Social.ReportScore (PlayerPrefs.GetInt ("lastBestScore"), "CgkIqM2wutYIEAIQBg", (bool success) => {
				// handle success or failure
			});*/
		}
	}

	public void ResetPosition ()
	{
		this.transform.position = playerPos;
		man.transform.position = manPos;
	}

	/*public void lastBestFun ()
	{
		print (lastBest);
		if (lastBest > PlayerPrefs.GetFloat ("lastBest")) {
			PlayerPrefs.SetFloat ("lastBest", lastBest);
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
