using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Raycastes : MonoBehaviour
{

		// Use this for initialization

		int x = 0, y = 0, x1 = 0;
		public Text selectedCharacterNameText;		
		GameObject character = null;
		GameObject[] charUnlocked;

		void Start ()
		{
				charUnlocked = GameObject.FindGameObjectsWithTag ("charUnlocked");
		}
		public void ScanAllChars ()
		{
				charUnlocked = GameObject.FindGameObjectsWithTag ("charUnlocked");
		}
		// Update is called once per frame
		void Update ()
		{
				foreach (GameObject chars in charUnlocked) {
						chars.transform.Rotate (0, 0, 60 * Time.deltaTime);
				}
				x = Screen.width / 2;
				y = Screen.height / 2;
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay (new Vector3 (x, y));
				Debug.DrawRay (ray.origin, ray.direction * 250, new Color (1f, 0.922f, 0.016f, 1f));
				if (Physics.Raycast (ray, out hit)) {
						if (hit.collider != null) {								
								hit.collider.gameObject.transform.GetChild (0).transform.localPosition = new Vector3 (0, 1, -0.48f);
								hit.collider.gameObject.transform.GetChild (0).transform.localScale = new Vector3 (1.7f, 1.7f, 1.7f);
								
								character = hit.collider.gameObject;
								selectedCharacterNameText.text = character.name;

								//hit.collider.transform.localScale = new Vector3 (200, 200, 200);
								//	hit.collider.transform.localPosition = new Vector3 (hit.collider.transform.localPosition.x, hit.collider.transform.localPosition.y, -150);
						}
				} 
		}
		void LateUpdate ()
		{
				x = (Screen.width / 2) + 100;
				x1 = (Screen.width / 2) - 100;
				y = Screen.height / 2;
				RaycastHit hitRight, hitLeft;
				Ray rayRight = Camera.main.ScreenPointToRay (new Vector3 (x, y));
				Ray rayLeft = Camera.main.ScreenPointToRay (new Vector3 (x1, y));

				Debug.DrawRay (rayRight.origin, rayRight.direction * 250, new Color (1f, 0.122f, 0.016f, 1f));
				Debug.DrawRay (rayLeft.origin, rayLeft.direction * 250, new Color (1f, 0.322f, 0.016f, 1f));

				if (Physics.Raycast (rayRight, out hitRight)) {
						if (hitRight.collider != null) {
								hitRight.collider.gameObject.transform.GetChild (0).transform.localPosition = new Vector3 (0, 0, -0.48f);
								hitRight.collider.gameObject.transform.GetChild (0).transform.localScale = new Vector3 (1f, 1f, 1f);
						}
				}
				if (Physics.Raycast (rayLeft, out hitLeft)) {
						if (hitLeft.collider != null) {
								hitLeft.collider.gameObject.transform.GetChild (0).transform.localPosition = new Vector3 (0, 0, -0.48f);
								hitLeft.collider.gameObject.transform.GetChild (0).transform.localScale = new Vector3 (1f, 1f, 1f);
						}
				}

		}
		IEnumerator RotateCharacters (GameObject characterMesh)
		{
				yield return new WaitForSeconds (0);
		}
}
