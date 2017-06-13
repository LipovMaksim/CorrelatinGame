using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PicturesBar : MonoBehaviour {

	[SerializeField]
	private GamePicture [] pictures;
	[SerializeField]
	private Transform fieldTransform;
	[SerializeField]
	private bool isEditor = false;


	private Vector3[] picturePositions = { new Vector3 (-6.3f, 0, -2), new Vector3 (-4.2f, 0, -2), new Vector3 (-2.1f, 0, -2), new Vector3 (0, 0, -2), new Vector3 (2.1f, 0, -2), new Vector3 (4.2f, 0, -2), new Vector3 (6.3f, 0, -1)};

	void Awake () {

		for (int i = 0; i < pictures.Length; i++) {
			pictures [i].draggingEnd += pictureDropedAction;
			//if (!isEditor)
				pictures [i].transform.localPosition = picturePositions [i];
			pictures [i].FieldPosition = fieldTransform.position;
		}
	}

	void pictureDropedAction (DraggableObject obj, Vector3 position){
		pictureDroped (obj, position);
	}
	public delegate void PictureDroped (DraggableObject obj, Vector3 position);
	public PictureDroped pictureDroped;

	public bool hasActivePictures () {
		for (int i = 0; i < pictures.Length; i++) {
			if (pictures [i].isActive ())
				return true;
		}
		return false;
	}

	public void setTask (Task task) {
		GamePictureInfo[] gps = task.getGamePictures ();
		for (int i = 0; i < gps.Length && gps [i] != null; i++) {
			pictures [i].setFrom (gps [i]);
			if (isEditor) {
				pictures [i].setPositionOnField (gps [i].Position);
			}
		}
	}

	public void addPicture (ImageData imgData) {
		int i = 0;
		for (; i < pictures.Length && pictures[i].GetComponent<SpriteRenderer>().sprite != null; i++);
		if (i < pictures.Length) {
			pictures [i].setPicture (imgData);
			pictures [i].transform.localPosition = picturePositions [i];
		}	
	}

	public GamePictureInfo [] getGamePictures () {
		List <GamePictureInfo> gpis = new List<GamePictureInfo> ();
		for (int i = 0; i < pictures.Length && pictures [i].isActive (); i++) {
			gpis.Add (pictures [i].GamePictureInfo);
		}
		return gpis.ToArray();
	}

	public GamePicture [] getGamePicturesObj () {
		return pictures;
	}
}
