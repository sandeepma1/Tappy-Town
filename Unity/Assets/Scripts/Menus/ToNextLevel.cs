using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ToNextLevel : MonoBehaviour
{
	public Image fade;
	public float fadeSpeed = 0.01f;
	public GameObject mainCanvas;
	Animator anim;

	void Start ()
	{
		Application.targetFrameRate = 60;
		anim = mainCanvas.GetComponent<Animator> ();
		StartCoroutine ("WaitAndStartlevel");
	}

	IEnumerator WaitAndStartlevel ()
	{		
		yield return new WaitForSeconds (3f);
		anim.Play ("moveToMainLevel");
		yield return new WaitForSeconds (1f);
		SceneManager.LoadSceneAsync ("level");		

	}

	/*IEnumerator WaitAndStartlevel ()
	{

		yield return new WaitForSeconds (3f);
		anim.Play ("moveToMainLevel");
		yield return new WaitForSeconds (1f);
		SceneManager.LoadSceneAsync ("level");
	}*/

}
