using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System;

public class MAMBC_PostProcessBuild {
	[MenuItem( "Utils/Test Build PostProcess")]
	public static void TestPostProcess() {
		OnPostprocessBuild(EditorUserBuildSettings.activeBuildTarget, EditorUserBuildSettings.GetBuildLocation(EditorUserBuildSettings.activeBuildTarget));
	}

	[PostProcessBuild]
	public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject) {
		// Only iPhone build post processing for now
		if(target != BuildTarget.iPhone) return;

		string scriptPath = Application.dataPath + "/Editor/PostProcessShellScript.rb";
		Debug.Log("Starting Build Processing with Script: " + scriptPath);
		
		System.Diagnostics.Process ppScriptProcess = new System.Diagnostics.Process();
		ppScriptProcess.StartInfo.FileName = "ruby";
		ppScriptProcess.StartInfo.Arguments = "\"" + scriptPath + "\" " + pathToBuiltProject;
		ppScriptProcess.StartInfo.UseShellExecute = false;
		ppScriptProcess.StartInfo.RedirectStandardOutput = true;
		ppScriptProcess.StartInfo.RedirectStandardError = true;
		ppScriptProcess.StartInfo.CreateNoWindow = true;
		ppScriptProcess.Start();

		// Output
		while(! ppScriptProcess.StandardOutput.EndOfStream) {
			Debug.Log("OUTPUT: " + ppScriptProcess.StandardOutput.ReadLine());
		}
		while(! ppScriptProcess.StandardError.EndOfStream) {
			Debug.Log("ERROR: " + ppScriptProcess.StandardError.ReadLine());
		}
		
		Debug.Log("Finished Build Processing.");
	}
}