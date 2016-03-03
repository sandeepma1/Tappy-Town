// Because there's no UNITY_VERSION symbol :facepalm:
#if UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6
#define PRE_UNITY_5
#endif

using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.XCodeEditor;



public class SponsorPayPostProcess
{

	[PostProcessBuild(500)]
	public static void OnPostProcessBuild( BuildTarget target, string path )
	{

#if PRE_UNITY_5
		if (target == BuildTarget.iPhone)
#else
		if (target == BuildTarget.iOS)
#endif
		{
			UnityEditor.XCodeEditor.SponsorPay.XCProject project = new UnityEditor.XCodeEditor.SponsorPay.XCProject(path);
			
			// Find and run through all projmods files to patch the project
			string projModPath = System.IO.Path.Combine(Application.dataPath, "SponsorPay/iOS");
			string[] files = System.IO.Directory.GetFiles(projModPath, "*.projmods", System.IO.SearchOption.AllDirectories);
			foreach( var file in files ) 
			{
				project.ApplyMod(Application.dataPath, file);
			}
			project.Save();
			
		}
	}
}

