using UnityEngine;
using System.Collections;
using System.IO;

public class ShareApp : MonoBehaviour
{
	//Grant the Writing Access to External (SDCard) in PlayerSettings ;-) Otherwise you won't be able to share pictures!!!!
	private bool isProcessing = false;
	string subject = "Tappy Town";
	string body = "";
	public GameObject playerPosition;
	public GameObject mainCanvas;

	public void ShareButtonTap ()
	{
		body = playerPosition.transform.position.x.ToString ("F0") + " on #tappytown. My top is " + June.LocalStore.Instance.GetInt ("lastBestScore").ToString ();
		print (body);
		StartCoroutine (ShareScreenshot ());
	}

	public IEnumerator ShareScreenshot ()
	{
		isProcessing = true;
		mainCanvas.SetActive (false);
		// wait for graphics to render
		yield return new WaitForEndOfFrame ();
		//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- PHOTO
		// create the texture
		Texture2D screenTexture = new Texture2D (Screen.width, Screen.height, TextureFormat.RGB24, true);
		// put buffer into texture
		screenTexture.ReadPixels (new Rect (0f, 0f, Screen.width, Screen.height), 0, 0);
		// apply
		screenTexture.Apply ();
		mainCanvas.SetActive (true);
		//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- PHOTO
		byte[] dataToSave = screenTexture.EncodeToPNG ();
		string destination = Path.Combine (Application.persistentDataPath, System.DateTime.Now.ToString ("yyyy-MM-dd-HHmmss") + ".png");
		File.WriteAllBytes (destination, dataToSave);
		if (!Application.isEditor) {
			// block to open the file and share it ------------START
			AndroidJavaClass intentClass = new AndroidJavaClass ("android.content.Intent");
			AndroidJavaObject intentObject = new AndroidJavaObject ("android.content.Intent");
			intentObject.Call<AndroidJavaObject> ("setAction", intentClass.GetStatic<string> ("ACTION_SEND"));
			AndroidJavaClass uriClass = new AndroidJavaClass ("android.net.Uri");
			AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject> ("parse", "file://" + destination);
			intentObject.Call<AndroidJavaObject> ("putExtra", intentClass.GetStatic<string> ("EXTRA_STREAM"), uriObject);
			intentObject.Call<AndroidJavaObject> ("putExtra", intentClass.GetStatic<string> ("EXTRA_TEXT"), body);
			intentObject.Call<AndroidJavaObject> ("putExtra", intentClass.GetStatic<string> ("EXTRA_SUBJECT"), subject);
			intentObject.Call<AndroidJavaObject> ("setType", "image/jpeg");
			AndroidJavaClass unity = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
			AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject> ("currentActivity");
			// option one WITHOUT chooser:
			currentActivity.Call ("startActivity", intentObject);
			// option two WITH chooser:
			//AndroidJavaObject jChooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, "YO BRO! WANNA SHARE?");
			//currentActivity.Call("startActivity", jChooser);
			// block to open the file and share it ------------END
		}
		isProcessing = false;
		//GetComponent<GUITexture> ().enabled = true;
	}
}

/*string subject = "WORD-O-MAZE";
string body = "PLAY THIS AWESOME GAME. GET IT ON THE PLAYSTORE AT LINK";

public void shareText(){
	//execute the below lines if being run on a Android device
	#if UNITY_ANDROID
	AndroidJavaClass intentClass = new AndroidJavaClass ("android.content.Intent");
	AndroidJavaObject intentObject = new AndroidJavaObject ("android.content.Intent");
	intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
	intentObject.Call<AndroidJavaObject>("setType", "text/plain");
	intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), subject);
	intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), body);
	AndroidJavaClass unity = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
	AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
	//start the activity by sending the intent data
	currentActivity.Call ("startActivity", intentObject);
	#endif
}*/
