using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManagers : MonoBehaviour
{
	public static GameManagers m_instance = null;
	//public GameObject blankLogo, gameName;
	public Animator splashScreenAnim;

	// Use this for initialization
	void Awake ()
	{
		splashScreenAnim.PlayInFixedTime ("FadeOut");
		m_instance = this;
	}

	public void Restartlevel ()
	{
		IGMLogic.m_instance.mainCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
		StartCoroutine ("LevelRestartWithWait");
	}

	IEnumerator LevelRestartWithWait ()
	{
		splashScreenAnim.PlayInFixedTime ("FadeIn");
		yield return new WaitForSeconds (1.2f);
		SceneManager.LoadSceneAsync ("level");
	}

	public void LoadMainlevel ()
	{
		SceneManager.LoadSceneAsync ("1Loading");
	}


}
