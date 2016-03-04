using UnityEngine;
using System.Collections;

public class FlashText : MonoBehaviour
{

		void OnTriggerEnter (Collider other)
		{
				StartCoroutine ("Flash");
		}
		void OnTriggerExit (Collider other)
		{
				StopCoroutine ("Flash");
		}
		IEnumerator Flash ()
		{
				yield return new WaitForSeconds (0.2f);
				gameObject.GetComponent<TextMesh> ().color = Color.grey;
				yield return new WaitForSeconds (0.2f);
				gameObject.GetComponent<TextMesh> ().color = Color.white;
				StartCoroutine ("Flash");
		}
}
