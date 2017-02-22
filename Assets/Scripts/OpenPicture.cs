using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class OpenPicture : MonoBehaviour {

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
			Debug.Log ("Picture img choised: " + path);
			if (pictureChoised != null) {
				pictureChoised (path);
			}
		}
	}

	public delegate void PictureChoisedEvent (string path);
	public PictureChoisedEvent pictureChoised;

	// Update is called once per frame
	void Update () {

	}
}
