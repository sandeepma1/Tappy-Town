using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class BackgroundAudio : MonoBehaviour
{
		int pauseCnt = 0, playCnt = 0;
		AudioSource audioBG;
		void Start ()
		{
				audioBG = GetComponent<AudioSource> ();
				//audio.Play ();
		}

		void LateUpdate ()
		{
				if (GameEventManager.GetState () == GameEventManager.E_STATES.e_game && playCnt <= 1) {
						playCnt++;
						pauseCnt = 0;
						audioBG.Play ();
						return;
				}
				if (GameEventManager.GetState () == GameEventManager.E_STATES.e_pause && pauseCnt <= 1) {
						pauseCnt++;
						playCnt = 0;
						audioBG.Pause ();
						return;
				}

		}
}
