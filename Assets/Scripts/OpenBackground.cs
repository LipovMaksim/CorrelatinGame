using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class OpenBackground : MonoBehaviour {

	[SerializeField]
	private string title;
	[SerializeField]
	private string directory;
	[SerializeField]
	private string extention;


	void Start () {
		
	}

	private void OnMouseUp() {
		string path = EditorUtility.OpenFilePanel (title, directory, extention);

		if (path != "") {
			Debug.Log ("Background img choised: " + path);
			if (backgroundImgChoised != null) {
				backgroundImgChoised (path);
			}
		}
	}

	public delegate void BackgroundImgChoisedEvent (string path);
	public BackgroundImgChoisedEvent backgroundImgChoised;
	
	// Update is called once per frame
	void Update () {
		
	}
}
