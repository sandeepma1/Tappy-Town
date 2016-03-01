using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;



namespace Prime31
{
	public class EtceteraTwoEventListener : MonoBehaviour
	{
#if UNITY_IOS
		void OnEnable()
		{
			// Listen to all events for illustration purposes
			EtceteraTwoManager.screenMirroringDidStartEvent += screenMirroringDidStartEvent;
			EtceteraTwoManager.screenMirroringDidStopEvent += screenMirroringDidStopEvent;
			EtceteraTwoManager.moviePlayerDidFinishEvent += moviePlayerDidFinishEvent;
		}
	
	
		void OnDisable()
		{
			// Remove all event handlers
			EtceteraTwoManager.screenMirroringDidStartEvent -= screenMirroringDidStartEvent;
			EtceteraTwoManager.screenMirroringDidStopEvent -= screenMirroringDidStopEvent;
			EtceteraTwoManager.moviePlayerDidFinishEvent -= moviePlayerDidFinishEvent;
		}
	
	
	
		void screenMirroringDidStartEvent()
		{
			Debug.Log( "screenMirroringDidStartEvent" );
		}
	
	
		void screenMirroringDidStopEvent()
		{
			Debug.Log( "screenMirroringDidStopEvent" );
		}
		
		
		void moviePlayerDidFinishEvent()
		{
			Debug.Log( "moviePlayerDidFinishEvent" );
		}
	
#endif
	}

}
	
	
