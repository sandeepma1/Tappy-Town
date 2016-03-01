using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;



namespace Prime31
{
	public class Etcetera2GUIManager : Prime31.MonoBehaviourGUI
	{
#if UNITY_IOS
		void OnGUI()
		{
			beginColumn();


			if( GUILayout.Button( "Register for Notifications" ) )
			{
				EtceteraTwoBinding.registerForNotifications();
			}


			if( GUILayout.Button( "Schedule Local Notification" ) )
			{
				EtceteraTwoBinding.scheduleLocalNotification( 20, "Check me out!  I've got some stuff for you to see!", "Check it Out" );
			}


			endColumn( true );


			if( GUILayout.Button( "Play Remote Video" ) )
			{
				string url = "http://prime31.com/_dump/demoVid.mp4";
				EtceteraTwoBinding.playMovie( url, true, true, false );
			}


			if( GUILayout.Button( "Play Local Video" ) )
			{
				string url = Path.Combine( Application.dataPath, "Raw/demoVid.mp4" );
				EtceteraTwoBinding.playMovie( url, true, true, false );
			}


			if( GUILayout.Button( "Piracy Checks" ) )
			{
				var isJB = EtceteraTwoBinding.isJailBroken();
				var isCracked = EtceteraTwoBinding.isCracked();
				EtceteraTwoBinding.logInfoPlistFileSize();

				Debug.Log( "is jailbroken? " + isJB + ", is cracked? " + isCracked );
			}


			if( GUILayout.Button( "Get Info.Plist Value" ) )
			{
				var val = EtceteraTwoBinding.getInfoPlistValue( "CFBundleVersion" );

				Debug.Log( "CFBundleVersion from Info.plist is: " + val );
			}


			if( GUILayout.Button( "Skip Backup of File" ) )
			{
				// create a dummy file to play with
				var filePath = Application.persistentDataPath + "/dummfile.txt";
		        StreamWriter sw = new StreamWriter( filePath );
		        sw.Write( "just writing some junk to a file for testing" );
		        sw.Close();

				var didSet = EtceteraTwoBinding.addSkipBackupAttribute( filePath );
				Debug.Log( "did set skip backup: " + didSet );
			}


			endColumn( false );
		}
#endif
	}

}
