using UnityEngine;
using System.Collections;

public class TouchControl : MonoBehaviour
{
		public bool isUpButtonDown = false;
		public bool isDownButtonDown = false;
		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
#if UNITY_EDITOR
				if (Input.GetMouseButton (0)) {
						RaycastHit hit;
						Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
						if (Physics.Raycast (ray, out hit)) {
								if (hit.collider != null && hit.collider.name == "UpButton") {
										isUpButtonDown = true;	
								}	
								if (hit.collider != null && hit.collider.name == "DownButton") {
										isDownButtonDown = true;
								}
						}
				} else {
						isUpButtonDown = false;
						isDownButtonDown = false;
				}
				#endif
		
				/*if (Input.touchCount > 0) {
						Touch touch = Input.GetTouch (0);
						if (touch.position.x < Screen.height / 2) {
								isUpButtonDown = true;	
						} else if (touch.position.x > Screen.height / 2) {
								isDownButtonDown = true;
						}
				} else {
						isUpButtonDown = false;
						isDownButtonDown = false;
				}*/
		}
}
