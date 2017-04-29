using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskNote : MonoBehaviour {
	[SerializeField]
	public Image icon;
	[SerializeField]
	private Text title;
	[SerializeField]
	private Text description;
	[SerializeField]
	private Image[] pictures;
	[SerializeField]
	private float sizeUnitsPicture = 20f;
	[SerializeField]	
	private Image background;
	[SerializeField]
	private Color trueColor;
	[SerializeField]
	private Color currentColor;

	public const float iconW = 100f;
	public const float iconH = 70f;


	public const float trueIconSize = 2f;

	public const string objectCountString = "Количество элементов: ";

	private Task task = null;


	public Task getTask () {
		return task;
	}

	public void setTask (Task t) {
		task = t;
		icon.overrideSprite = Sprite.Create(t.BackgroundImg, new Rect(0,0,t.BackgroundImg.width, t.BackgroundImg.height), new Vector2(0.5f, 0.5f), 
			(t.BackgroundImg.width / iconW > t.BackgroundImg.height / iconH ? t.BackgroundImg.width / iconW : t.BackgroundImg.height / iconH));
		icon.enabled = true;
		if (title != null)
			title.text = t.Name;
		if (description != null)
			description.text = t.Description;
		GamePictureInfo [] ps = task.getGamePictures ();
		for (int i = 0; i < pictures.Length; i++) {
			if (ps [i] != null)
				setPicture (ps [i], i);
			else
				pictures [i].enabled = false;
		}
	}

	private void setPicture (GamePictureInfo pi, int index) {
		pictures [index].enabled = true;
		Sprite s = Sprite.Create (pi.Img, new Rect (0, 0, pi.Img.width, pi.Img.height), new Vector2 (0.5f, 0.5f), 
			(pi.Img.width / sizeUnitsPicture > pi.Img.height / sizeUnitsPicture ? pi.Img.width / sizeUnitsPicture : pi.Img.height / sizeUnitsPicture));
		
		pictures [index].overrideSprite = s;
		pictures [index].transform.localPosition = new Vector3 (pi.Position.x * sizeUnitsPicture / trueIconSize, pi.Position.y * sizeUnitsPicture / trueIconSize, 0);
		pictures [index].transform.localScale = new Vector3 (pi.Size * (pi.FlipX ? -1 : 1), pi.Size * (pi.FlipY ? -1 : 1), 1); //В Image нет флипов, поэтому приходится импровизировать
		pictures [index].transform.localRotation = new Quaternion(0, 0, pi.Angle, 1);
	}

	public void resetPictures () {
		for (int i = 0; i < pictures.Length; i++) {
			pictures [i].enabled = false;
		}
	}

	public void setCurrent (bool f = true) {
		background.color = (f ? currentColor : trueColor);
	}
}
