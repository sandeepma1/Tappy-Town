using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using June.Api;

public class FreeGiftPanelScript : MonoBehaviour
{

		public FacebookConnectScript FacebookConnectObject;


		// Use this for initialization
		void Start ()
		{
			FacebookConnectObject.OnConnectEnded += HandleFacebookConnection;
		}

		void OnEnable ()
		{
				
		}

		void CloseButtonTapped (GameObject go)
		{				
				this.gameObject.SetActive (false);
		}	


	void HandleFacebookConnection (bool isConnected)
	{

		if (isConnected) 
		{



		} else 
		{
			Etcetera.ShowAlert("Facebook", "You need to connect to facebook to get your free character.", "OK");

		}


	}
}
