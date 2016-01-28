using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScreenControl : MonoBehaviour
{
	//bool isPortrait = false;
	//int countP = 0, countL = 0, count = 0;
	bool isChanged;
	public Canvas mainCanvas;
	
	//public Text debug;
	void Awake ()
	{
		/*if (Screen.orientation == ScreenOrientation.Portrait) {
						RotateCamera (0);
				}
				if (Screen.orientation == ScreenOrientation.Landscape) {						
						RotateCamera (1);
				}*/
	}

	void LateUpdate ()
	{
		/*if (!June.LocalStore.Instance.GetBool ("screenRotation")) {		
						ScreenRotateControl (true);
						if (Screen.orientation == ScreenOrientation.Portrait && countP <= 1) {
								countP++;
								countL = 0;
								RotateCamera (0);
								return;
						}
						if (Screen.orientation == ScreenOrientation.Landscape && countL <= 1) {
								countL++;
								countP = 0;
								RotateCamera (1);
								return;    
						}
				} else {
						ScreenRotateControl (false);
				}*/
	}

	void ScreenRotateControl (bool flag)
	{
		Screen.autorotateToLandscapeLeft = flag;
		Screen.autorotateToLandscapeRight = flag;
		Screen.autorotateToPortrait = flag;
	}

	void RotateCamera (int angle)
	{
		if (angle == 0) {
			Camera.main.transform.localPosition = new Vector3 (-21, 27, -56);
			//StartCoroutine ("PauseGameAfterScreenRotate", new Vector3 (-21, 27, -56));
			mainCanvas.GetComponent<CanvasScaler> ().referenceResolution = new Vector2 (1280, 720);
		}
		if (angle == 1) {
			Camera.main.transform.localPosition = new Vector3 (-10, 15, -31);
			//StartCoroutine ("PauseGameAfterScreenRotate", new Vector3 (-10, 15, -30));
			mainCanvas.GetComponent<CanvasScaler> ().referenceResolution = new Vector2 (720, 1280);
		}
	}

	IEnumerator PauseGameAfterScreenRotate (Vector3 pos)
	{
		Camera.main.transform.localPosition = pos;
		GameEventManager.SetState (GameEventManager.E_STATES.e_pause);
		yield return new WaitForSeconds (5f);				
		GameEventManager.SetState (GameEventManager.E_STATES.e_game);
	}
}
