using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System;

public class PostProcessBuild {
	[MenuItem( "Utils/Build and Run IOS")]
	public static void BuildAndRunIOS() {
		// Store build settings from user editor
		BuildTarget currentBuildTarget = EditorUserBuildSettings.activeBuildTarget;
		string currentBuildLocation = EditorUserBuildSettings.GetBuildLocation(currentBuildTarget);

		// Only IOS
		if(currentBuildTarget != BuildTarget.iPhone)
			return;

		// Store scene list
		string[] sceneNames = GetSceneList();

		// Perform build
		BuildPipeline.BuildPlayer(sceneNames, currentBuildLocation, currentBuildTarget, BuildOptions.None);

		// OnPostProcessBuild will automatically run here
	}

	[MenuItem( "Utils/Test Build PostProcess")]
	public static void TestPostProcess() {
		OnPostProcessBuild(EditorUserBuildSettings.activeBuildTarget, EditorUserBuildSettings.GetBuildLocation(EditorUserBuildSettings.activeBuildTarget));
	}

	[PostProcessBuild]
	public static void OnPostProcessBuild(BuildTarget target, string pathToBuiltProject) {
		// Only iPhone build post processing for now
		if(target != BuildTarget.iPhone)
			return;
		
		string[] bundleID = PlayerSettings.iPhoneBundleIdentifier.Split('.');

		// Run post process shell script, this script will modify and build the project and deploy it
		string scriptPath = Application.dataPath + "/Editor/PostProcessShellScript.rb";
		RunInShell("ruby", "\"" + scriptPath + "\" \"" + pathToBuiltProject + "\"" + " " + bundleID[bundleID.Length - 1], false);
	}
	
	// Utility function returns a list of scenes, used by BuildPipeLine
	static string[] GetSceneList() {
		EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
		string[] sceneNames = new string[ scenes.Length ];
		// Get scene names from editor settings
		for(int i = 0; i < scenes.Length; i++) {
			sceneNames[i] = scenes[i].path;
		}
		return sceneNames;
	}

	// Utility function to run a shell script given a file and arguments
	public static void RunInShell(string file, string args, bool waitForExit = true) {
		Debug.Log("Running In Shell: " + file + " " + args);
		System.Diagnostics.Process ppScriptProcess = new System.Diagnostics.Process();
		ppScriptProcess.StartInfo.FileName = file;
		ppScriptProcess.StartInfo.Arguments = args;
		ppScriptProcess.StartInfo.UseShellExecute = false;
		ppScriptProcess.StartInfo.CreateNoWindow = true;
		ppScriptProcess.Start();
	}
}
