using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;

public class FileBrowser : MonoBehaviour {

	private Rect WindowRect;
	private bool browserCalled, write;
	private string path, file;
	private Vector2 list1, list2;
	public int selected1, selected2;

	void Start(){
		browserCalled = false;
		path = Environment.CurrentDirectory;
		selected1 = selected2 = -1;
		if (file == null)
			file = "File name";
	}

	public void OpenFile(){
		browserCalled = true;
		write = false;
	}

	public void SaveFile() {
		browserCalled = true;
		write = true;
	}

	public static bool MainBrowser(ref bool write, ref string path, ref Rect WindowRect, ref Vector2 list1, ref Vector2 list2, ref int selected1, ref int selected2, ref string file){
		DirectoryInfo directory;
		bool done = false;

		directory = new DirectoryInfo(path);

		GUI.Label (new Rect (20, 20, WindowRect.width/1.2f, WindowRect.height), path);
		if (GUI.Button (new Rect (0, 0, 20, 20), "X")) done = true;
		if (path != "/" && GUI.Button (new Rect (WindowRect.width/1.2f, 20, 30, 20), "Up")) {
			path = Directory.GetParent(path).ToString();
		}

		string[] directoryList = Directory.GetDirectories (path);
		string[] fileList = Directory.GetFiles (path);
		int i = 0;

		foreach (string s in Directory.GetDirectories (path)) {
			directoryList[i++] = s.Remove (0, path.Length);
		}
			
		i = 0;

		foreach (string s in Directory.GetFiles (path)) {
			fileList[i++] = s.Remove (0, path.Length+1);
		}

		float scrollheight = (WindowRect.height / 8f)*directoryList.Length;
		list1 = GUI.BeginScrollView (new Rect(20, 60, WindowRect.width / 2.1f, WindowRect.height / 1.6f), list1, new Rect(0, 0, 0, scrollheight));
			selected1 = GUI.SelectionGrid (new Rect(0, 0, WindowRect.width / 2.2f, scrollheight), selected1, directoryList, 1);
		GUI.EndScrollView ();

		scrollheight = (WindowRect.height / 8f)*fileList.Length;
		list2 = GUI.BeginScrollView (new Rect((WindowRect.width/2f) + 10, 60, WindowRect.width / 2.1f, WindowRect.height / 1.6f), list2, new Rect(0, 0, 0, scrollheight));
			selected2 = GUI.SelectionGrid (new Rect(0, 0, WindowRect.width / 2.2f, scrollheight), selected2, fileList, 1);
		GUI.EndScrollView ();

		if (selected2 >= 0) {
			file = fileList [selected2];
			Debug.Log (file);
			selected2 = -1;
		}

		file = GUI.TextField (new Rect (20, WindowRect.height / 1.2f, WindowRect.width/1.6f, WindowRect.height/8f), file);

		if (selected1 >= 0) {
			path = path + directoryList [selected1];
			selected1 = -1;
		}

		if (write) {
			if (GUI.Button(new Rect (WindowRect.width/1.12f, WindowRect.height / 1.2f, 50, 50), "Save")){
				//Save something
				//path+file
			}
		} else {
			if (GUI.Button (new Rect (WindowRect.width / 1.12f, WindowRect.height / 1.2f, 50, 50), "Load")) {
				//Load something
				//path+file
			}
		}

		return done;
	}

	public void Browser(int WindowID){
		if (MainBrowser(ref write, ref path, ref WindowRect, ref list1, ref list2, ref selected1, ref selected2, ref file)) {
			browserCalled = false;
		}
	}

	void OnGUI(){
		if (browserCalled) {
			WindowRect = new Rect (Screen.width / 6f, Screen.height / 6f, Screen.width / 1.5f, Screen.height / 1.5f);
			GUI.Window (0, WindowRect, Browser, "File Browser");
		}
	}
}
