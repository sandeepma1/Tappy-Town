using UnityEngine;
using System.Collections;

public class PigJump : MonoBehaviour
{
		//public float gravityMultiplier = 6.0F;
		public float jumpSpeed = 20.0F;
		public float gravity = 100.0F;
		private Vector3 moveDirection = Vector3.zero;
		CharacterController controller;
		//public float pushPower = 2.0F;
		public GameObject man;
		public GameObject touchControl;
		public GameObject UI;
		
		void Start ()
		{
				controller = GetComponent<CharacterController> ();
				UI.gameObject.SetActive (true);
		}
		void Update ()
		{		
				float g = gravity;
				if (controller.isGrounded) {			
						#if UNITY_ANDROID			
						if (Input.touchCount > 0) {
								Touch touch = Input.GetTouch (0);
								if (touch.position.x < Screen.height / 2) {
										man.GetComponent<CharacterController> ().enabled = false;
										man.GetComponent<ManJump> ().enabled = false;
										man.GetComponent<BoxCollider> ().enabled = false;
										moveDirection.y = jumpSpeed;
										g = gravity;
								} 
						}
						#endif				
						#if UNITY_EDITOR
						if (Input.GetKey ("space")) {
								man.GetComponent<CharacterController> ().enabled = false;
								man.GetComponent<ManJump> ().enabled = false;
								man.GetComponent<BoxCollider> ().enabled = false;
								moveDirection.y = jumpSpeed;
								g = gravity;
						}
						#endif
						//http://docs.unity3d.com/ScriptReference/CharacterController.Move.html
						if (controller.velocity.normalized == Vector3.down) {
								g = gravity / 2;// * gravityMultiplier;
								man.GetComponent<CharacterController> ().enabled = true;
								man.GetComponent<ManJump> ().enabled = true;
								man.GetComponent<BoxCollider> ().enabled = true;
						}			
				}
				moveDirection.y -= gravity * Time.deltaTime;
				controller.Move (moveDirection * Time.deltaTime);	
		}	
		void OnTriggerEnter (Collider other)
		{
				if (other.gameObject.tag == "death") {
						//transform.root.gameObject.GetComponent<MovingPlatform> ().ResetPosition ();
						Application.LoadLevel (0);
				}
				if (other.gameObject.tag == "pickable_coin") {
						//transform.root.gameObject.GetComponent<MovingPlatform> ().AddCoins (1);
						Destroy (other.gameObject);
				}
		}
}