using UnityEngine;
using System.Collections;

public class TutorialManager : MonoBehaviour
{

	public static TutorialManager m_instance = null;
	public GameObject balloonTutorial;
	// Use this for initialization
	void Awake ()
	{
		m_instance = this;
	}

	public void ShowBalloonTutorial ()
	{
		if (Bronz.LocalStore.Instance.GetInt ("balloonTutorial") <= 2) {
			Bronz.LocalStore.Instance.Increment ("balloonTutorial");
			IGMLogic.m_instance.PauseGame ();
			balloonTutorial.SetActive (true);
		}
	}

	public void CloseBalloonTutorial ()
	{
		IGMLogic.m_instance.ResumeGame ();
		balloonTutorial.SetActive (false);
	}

}
