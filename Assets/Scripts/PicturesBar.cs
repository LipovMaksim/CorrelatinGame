using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PicturesBar : MonoBehaviour {

	[SerializeField]
	private GamePicture [] pictures;

	void Start () {

		for (int i = 0; i < pictures.Length; i++) {
			pictures [i].draggingEnd += pictureDropedAction;
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
		}
	}
}
