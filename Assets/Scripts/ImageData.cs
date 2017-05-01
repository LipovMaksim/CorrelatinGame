using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageData : MonoBehaviour {

	[SerializeField]
	private Image image; 
	[SerializeField]
	private float sizeUnitsPicture = 100f;

	private int id;
	public int Id {get { return id; } set { id = value;} }

	private Texture2D texture;
	public Texture2D Texture {get { return texture; } set { texture = value;} }

	public ImageData (Texture2D tex, int id) {
		setImage (tex, id);
	}

	public void setImage (Texture2D tex, int id) {
		texture = tex;
		this.id = id;

		if (image != null) {
			image.overrideSprite = Sprite.Create (tex, new Rect (0, 0, tex.width, tex.height), new Vector2 (0.5f, 0.5f), 
				(tex.width / sizeUnitsPicture > tex.height / sizeUnitsPicture ? tex.width / sizeUnitsPicture : tex.height / sizeUnitsPicture));
		}
	}
	/*
	public GamePictureInfo gpi () {
		return new GamePictureInfo (-1, id, texture, null);
	}*/
}
