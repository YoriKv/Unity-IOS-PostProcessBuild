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
		if(currentBuildTarget != BuildTarget.iPhone) return;

		// Store scene list
		string[] sceneNames = GetSceneList();

		// Perform build
		BuildPipeline.BuildPlayer(sceneNames, currentBuildLocation, currentBuildTarget, BuildOptions.None);
	}

	[MenuItem( "Utils/Test Build PostProcess")]
	public static void TestPostProcess() {
		OnPostprocessBuild(EditorUserBuildSettings.activeBuildTarget, EditorUserBuildSettings.GetBuildLocation(EditorUserBuildSettings.activeBuildTarget));
	}

	[PostProcessBuild]
	public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject) {
		// Only iPhone build post processing for now
		if(target != BuildTarget.iPhone) return;

		// Run post process build script
		string scriptPath = Application.dataPath + "/Editor/PostProcessShellScript.rb";
		RunInShell("ruby", "\"" + scriptPath + "\" \"" + pathToBuiltProject + "\"");
		// Deply onto device, don't wait to exit
		string[] bundleID = PlayerSettings.iPhoneBundleIdentifier.Split('.');
		RunInShell("ios-deploy", "-I -d -b \"" + pathToBuiltProject + "/build/" + bundleID[bundleID.Length - 1] + ".app\"", false);
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
		ppScriptProcess.StartInfo.RedirectStandardError = waitForExit;
		ppScriptProcess.StartInfo.CreateNoWindow = true;
		ppScriptProcess.Start();

		// If we want to not wait, have to ignore error output
		if(waitForExit) {
			while(! ppScriptProcess.StandardError.EndOfStream) {
				Debug.Log("ERROR: " + ppScriptProcess.StandardError.ReadLine());
			}
			ppScriptProcess.WaitForExit();
		}
	}
}
