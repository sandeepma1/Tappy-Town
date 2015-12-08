using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ToNextLevel : MonoBehaviour
{
	public Image fade;
	bool startFading = false;
	public float fadeSpeed = 0.01f;
	public GameObject mainCanvas;
	Animator anim;
	// Use this for initialization
	void Start ()
	{
		Application.targetFrameRate = 60;
		anim = mainCanvas.GetComponent<Animator> ();
		StartCoroutine ("WaitAndStartlevel");
	}
	IEnumerator WaitAndStartlevel ()
	{
		//startFading = true;
		yield return new WaitForSeconds (3f);
		anim.Play ("moveToMainLevel");
		yield return new WaitForSeconds (1f);
		Application.LoadLevel ("level");
	}

	void Update ()
	{
		//if (startFading) {
		//FadeToBlack ();
		//}
		//fade.GetComponent<Animation> ().Play ("PanelFadeOut");
	}

	void FadeToBlack ()
	{
		//fade.GetComponent<Image> ().color = Color.Lerp (fade.GetComponent<Image> ().color, Color.white, fadeSpeed * Time.deltaTime);
	}

}
