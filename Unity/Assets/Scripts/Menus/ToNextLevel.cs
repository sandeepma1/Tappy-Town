using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ToNextLevel : MonoBehaviour
{
	//public Image fade;
	//public float fadeSpeed = 0.01f;
	public Animator splashScreenAnim;

	void Start ()
	{
		Application.targetFrameRate = 60;
		StartCoroutine ("WaitAndStartlevel");
	}

	IEnumerator WaitAndStartlevel ()
	{		
		yield return new WaitForSeconds (3f);
		splashScreenAnim.PlayInFixedTime ("FadeIn");
		yield return new WaitForSeconds (1f);
		print ("nextlevel");
		SceneManager.LoadSceneAsync ("level");
	}
}

