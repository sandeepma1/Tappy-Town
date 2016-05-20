using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
	public string levelToLoad;
	//public Text text;
	public GameObject progressBar;
	public Scrollbar bar;
	private int loadProgress = 0;
	public Animator splashScreenAnim;

	void Start ()
	{
		StartCoroutine (DisplayLoadingScreen ("level"));
	}

	IEnumerator DisplayLoadingScreen (string level)
	{
		yield return new WaitForSeconds (3f);
		splashScreenAnim.PlayInFixedTime ("FadeIn");
		//yield return new WaitForSeconds (1f);
		//print ("nextlevel");
		//SceneManager.LoadSceneAsync ("level");

		progressBar.transform.localScale = new Vector3 (loadProgress, progressBar.transform.localScale.y, progressBar.transform.localScale.z);

		//text.text = "Loading Progress " + loadProgress + "%";

		AsyncOperation async = Application.LoadLevelAsync (level);
		while (!async.isDone) {
			loadProgress = (int)(async.progress * 100);
			//text.text = loadProgress + "%";
			bar.size = loadProgress;
			progressBar.transform.localScale = new Vector3 (async.progress, progressBar.transform.localScale.y, progressBar.transform.localScale.z);
			yield return null;
		}
	}
}

/*public IEnumerator wait()
{
	Debug.Log("Now its called");
	yield return new WaitForSeconds(1);

	bar.value = 1;

}*/