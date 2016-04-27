using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class FacebookConnectScript : MonoBehaviour
{
	public GameObject m_ConnectButton;
	public GameObject m_LogOutButton;
	public GameObject m_ShareButton;

	public Action OnFbTimedOut;
	public Action OnConnectStarted;
	public Action<bool> OnConnectEnded;
	public Action OnLoggedOut;

	public Action<bool> OnSharedComplete;

	void Awake ()
	{
		if (null != m_ConnectButton)
			UIEventListener.Get (m_ConnectButton).onClick += ConnectButtonOnTap;
		if (null != m_LogOutButton)
			UIEventListener.Get (m_LogOutButton).onClick += LogoutButtonOnTap;
		if (null != m_ShareButton)
			UIEventListener.Get (m_ShareButton).onClick += ShareButtonOnTap;
	}

	void OnEnable ()
	{
		if (null != m_ConnectButton) {
			m_ConnectButton.SetActive (!FacebookSDK.Instance.IsLoggedIn);
		}
		if (null != m_LogOutButton) {
			m_LogOutButton.SetActive (FacebookSDK.Instance.IsLoggedIn);
		}
	}

	void OnDisable ()
	{
		if (null != m_ConnectButton)
			m_ConnectButton.SetActive (false);
		if (null != m_ShareButton)
			m_ShareButton.SetActive (false);
		CancelInvoke ("TimeoutFacebook");
	}

	public void Hide ()
	{
		this.gameObject.SetActive (false);
	}


	void ShowLoadingMessage (string message)
	{
		//PopupManager.Instance.ShowLoading (message);
	}

	void HideLoading ()
	{
		//PopupManager.Instance.HideLoading ();
	}

	void TimeoutFacebook ()
	{
		CancelInvoke ("TimeoutFacebook");
		HideLoading ();
		m_ConnectButton.SetActive (true);
		ShowLogoutButton (false);
		if (null != OnFbTimedOut)
			OnFbTimedOut ();
	}

	void ShowShareDialog ()
	{
		FacebookSDK.Instance.ShowShareDialog (this.VerifyCallback<bool> (sts => {
			Debug.Log ("[FACEBOOK] ShowShare Status: " + sts);
			if (OnSharedComplete != null)
				OnSharedComplete (sts);
		}));
	}

	void ShareButtonOnTap (GameObject go)
	{
		if (Application.internetReachability == NetworkReachability.NotReachable) {
			Etcetera.ShowAlert ("No Internet", "Please check the internet connection and try again.", "Ok");
			return;
		}

		m_ShareButton.SetActive (false);

		ShowLoadingMessage ("Connecting...");
		Invoke ("TimeoutFacebook", 60);
		#if UNITY_EDITOR
		if (null != OnSharedComplete)
			OnSharedComplete (true);
		return;
		#endif
		if (false == FacebookSDK.Instance.IsLoggedIn) {
			FacebookSDK.Instance.Login (this.VerifyCallback<bool> (status => {
				Debug.Log ("[FACEBOOK] Login Status: " + status);

				if (null != OnConnectEnded)
					OnConnectEnded (status);

				if (status) {
					CancelInvoke ("TimeoutFacebook");
					Invoke ("TimeoutFacebook", 60);
					ShowShareDialog ();
				} else {
					m_ConnectButton.SetActive (true);

					HideLoading ();

					Etcetera.ShowAlert ("Facebook", "You need to allow Chhota Bheem Rush on Facebook to get your reward.", "OK");
				}
			}));
		} else
			ShowShareDialog ();

		//
		//				Dictionary<string, object> parameters = new Dictionary<string, object>() {
		//						{"via", m_ViaScene}
		//				};
		//
		//				June.MessageBroker.Publish(June.Messages.FacebookShareButtonTap, parameters);

	}

	void LogoutButtonOnTap (GameObject go)
	{
		if (Application.internetReachability == NetworkReachability.NotReachable) {
			Etcetera.ShowAlert ("No Internet", "Please check the internet connection and try again.", "Ok");
			return;
		}

		if (m_LogOutButton != null)
			m_LogOutButton.SetActive (false);
		if (m_ConnectButton != null)
			m_ConnectButton.SetActive (true);
		
		FacebookSDK.Instance.Logout ();
		if (OnLoggedOut != null)
			OnLoggedOut ();

	}

	void ShowLogoutButton (bool value)
	{
		if (m_LogOutButton != null)
			m_LogOutButton.SetActive (value);
	}

	public	void FBConnectButtonOnTap ()
	{
		print ("tap");
		GameObject go = null;
		ConnectButtonOnTap (go);
	}

	void ConnectButtonOnTap (GameObject go)
	{
		if (Application.internetReachability == NetworkReachability.NotReachable) {
			Etcetera.ShowAlert ("No Internet", "Please check the internet connection and try again.", "Ok");
			return;
		}

		m_ConnectButton.SetActive (false);
		if (null != OnConnectStarted)
			OnConnectStarted ();
		ShowLoadingMessage ("Connecting...");
		Invoke ("TimeoutFacebook", 60);
		#if UNITY_EDITOR
		ShowLogoutButton (true);
		if (null != OnConnectEnded)
			OnConnectEnded (true);
		return;
		#endif
		if (false == FacebookSDK.Instance.IsLoggedIn) {
			FacebookSDK.Instance.Login (this.VerifyCallback<bool> (status => {
				Debug.Log ("[FACEBOOK] Login Status: " + status);
				if (null != OnConnectEnded)
					OnConnectEnded (status);

				ShowLogoutButton (status);

				if (status) {
					CancelInvoke ("TimeoutFacebook");
					HideLoading ();
					m_ConnectButton.SetActive (false);

				} else {
					m_ConnectButton.SetActive (true);
					HideLoading ();
					Etcetera.ShowAlert ("Facebook", "Failed to connect to Facebook.Try again", "OK");
				}
			}));
		}

		//				Dictionary<string, object> parameters = new Dictionary<string, object>() {
		//						{"via", m_ViaScene}
		//				};
		//
		//				June.MessageBroker.Publish(June.Messages.FacebookConnectButtonTap, parameters);
	}


}
