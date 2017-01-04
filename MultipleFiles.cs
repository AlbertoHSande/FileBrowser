using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class ConfigRunBrowser : MonoBehaviour {

	private Rect WindowRect;
	public static bool browserCalled;
	private int write;
	private string path, file;
	private Vector2 list1, list2, sceneList;
	public int selected1, selected2, selectedFile;
	GUIStyle label = new GUIStyle();
	List<string> files = new List<string>();

	void Start(){
		label.alignment = TextAnchor.MiddleCenter;
		label.fontStyle = FontStyle.Bold;
		label.fontSize = 20;
		browserCalled = true;
		path = Environment.CurrentDirectory;
		selected1 = selected2 = -1;
		if (file == null)
			file = "File name";
	}

	public static bool MainBrowser(ref string path, ref Rect WindowRect, ref Vector2 list1, ref Vector2 list2, ref Vector2 sceneList, 
		ref int selected1, ref int selected2, ref int selectedFile, ref List<string> files, ref string file, ref GUIStyle label){
		DirectoryInfo directory;
		bool done = false;

		directory = new DirectoryInfo(path);

		GUI.Label (new Rect (20, 70, WindowRect.width/1.2f, WindowRect.height), path);
		if (GUI.Button (new Rect (20, WindowRect.height/1.2f, 50, 50), "Back")) {
			SceneManager.LoadScene("Main");
		}
		if (path != "/" && GUI.Button (new Rect (WindowRect.width/1.75f, 70, 30, 20), "Up")) {
			path = Directory.GetParent(path).ToString();
		}

		string[] directoryList = Directory.GetDirectories (path);
		string[] fileList = Directory.GetFiles (path);
		int i = 0;

		foreach (string s in Directory.GetDirectories (path)) {
			directoryList[i++] = s.Remove (0, path.Length);
		}

		i = 0;

		if (Application.platform == RuntimePlatform.LinuxPlayer | Application.platform == RuntimePlatform.LinuxEditor) {
			foreach (string s in Directory.GetFiles (path)) {
				fileList [i++] = s.Remove (0, path.Length + 1);
			}
		} else if (Application.platform == RuntimePlatform.WindowsPlayer | Application.platform == RuntimePlatform.WindowsEditor) {
			foreach (string s in Directory.GetFiles (path)) {
				fileList [i++] = s.Remove (0, path.Length);
			}
		}

		GUI.Label (new Rect (20, 50, WindowRect.width / 3.5f * 2, 20), "File Browser", label);
		float scrollheight = (WindowRect.height / 8f)*directoryList.Length;
		list1 = GUI.BeginScrollView (new Rect(20, 100, WindowRect.width / 3.5f, WindowRect.height / 1.6f), list1, new Rect(0, 0, 0, scrollheight));
		selected1 = GUI.SelectionGrid (new Rect(0, 0, WindowRect.width / 3.65f, scrollheight), selected1, directoryList, 1);
		GUI.EndScrollView ();

		scrollheight = (WindowRect.height / 8f)*fileList.Length;
		list2 = GUI.BeginScrollView (new Rect((WindowRect.width/3.5f) + 30, 100, WindowRect.width / 3.5f, WindowRect.height / 1.6f), list2, new Rect(0, 0, 0, scrollheight));
		selected2 = GUI.SelectionGrid (new Rect(0, 0, WindowRect.width / 3.65f, scrollheight), selected2, fileList, 1);
		GUI.EndScrollView ();

		GUI.Label (new Rect ((WindowRect.width / 3.5f * 2) + 160, 50, WindowRect.width / 3.5f, 20), "Selected", label);
		scrollheight = (WindowRect.height / 8f)*files.Count;
		sceneList = GUI.BeginScrollView (new Rect((WindowRect.width/3.5f * 2) + 160, 100, WindowRect.width / 3.5f, WindowRect.height / 1.6f), sceneList, new Rect(0, 0, 0, scrollheight));
		selectedFile = GUI.SelectionGrid (new Rect(0, 0, WindowRect.width / 3.65f, scrollheight), selectedFile, files.ToArray(), 1);
		GUI.EndScrollView ();

		if (selected2 >= 0) {
			files.Add(path + "/" + fileList [selected2]);
			Debug.Log (fileList [selected2]);
			Debug.Log (files.Count);
			selected2 = -1;
		}

		if (GUI.Button (new Rect ((WindowRect.width / 3.5f * 2)+50, WindowRect.height/3f, 100, 50), "Move Up")) {
			if (selectedFile >= 1) {
				string old = files[selectedFile - 1];
				string moved = files[selectedFile];
				files.Remove (old);
				files.Insert (selectedFile - 1, moved);
				files.Remove (files[selectedFile - 1]);
				files.Insert (selectedFile, old);
				selectedFile--;
			}
		}

		if (GUI.Button (new Rect ((WindowRect.width / 3.5f * 2)+50, WindowRect.height/2.4f, 100, 50), "Remove")) {
			files.RemoveAt (selectedFile);
		}

		if (GUI.Button (new Rect ((WindowRect.width / 3.5f * 2)+50, WindowRect.height/2f, 100, 50), "Move Down")) {
			if (selectedFile <= files.Count) {
				string old = files[selectedFile + 1];
				string moved = files[selectedFile];
				files.Remove (old);
				files.Insert (selectedFile + 1, moved);
				files.Remove (files[selectedFile + 1]);
				files.Insert (selectedFile, old);
				selectedFile++;
			}
		}

		if (selected1 >= 0) {
			path = path + directoryList [selected1];
			selected1 = -1;
		}
			if (GUI.Button (new Rect (WindowRect.width / 1.12f, WindowRect.height / 1.2f, 50, 50), "Run")) {
				// Do something
			}

		return done;
	}

	public void Browser(int WindowID){
		if (MainBrowser(ref path, ref WindowRect, ref list1, ref list2, ref sceneList, ref selected1, ref selected2, ref selectedFile, 
			ref files, ref file, ref label)) {
			browserCalled = false;
		}
	}

	void OnGUI(){
		if (browserCalled) {
			WindowRect = new Rect (0, Screen.height / 6f, Screen.width, Screen.height - (Screen.height / 6f));
			GUI.Window (0, WindowRect, Browser, "File Browser");
		}
	}
}

