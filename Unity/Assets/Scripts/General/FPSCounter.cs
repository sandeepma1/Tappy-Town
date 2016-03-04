/* **************************************************************************
 * FPS COUNTER
 * **************************************************************************
 * Written by: Annop "Nargus" Prapasapong
 * Created: 7 June 2012
 * *************************************************************************/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
/* **************************************************************************
 * CLASS: FPS COUNTER
 * *************************************************************************/ 
[RequireComponent(typeof(GUIText))]
public class FPSCounter : MonoBehaviour
{
		/* Public Variables */
		public float frequency = 0.5f;
		public Text fpsText;
		public Text debugText;

		/* **********************************************************************
	 * PROPERTIES
	 * *********************************************************************/
		public int FramesPerSec { get; protected set; }
	
		/* **********************************************************************
	 * EVENT HANDLERS
	 * *********************************************************************/
		/*
	 * EVENT: Start
	 */
		private void Start ()
		{
				StartCoroutine (FPS ());				
		}
		/*
	 * EVENT: FPS
	 */
		private IEnumerator FPS ()
		{
				for (;;) {
						// Capture frame-per-second
						int lastFrameCount = Time.frameCount;
						float lastTime = Time.realtimeSinceStartup;
						yield return new WaitForSeconds (frequency);
						float timeSpan = Time.realtimeSinceStartup - lastTime;
						int frameCount = Time.frameCount - lastFrameCount;
			
						// Display it
						FramesPerSec = Mathf.RoundToInt (frameCount / timeSpan);
						fpsText.text = FramesPerSec.ToString ();
				}
		}
}