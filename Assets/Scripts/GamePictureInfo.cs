using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePictureInfo {

	private Texture2D img;
	public Texture2D Img { get { return img;} set {img = value;} }
	private Vector2 position;
	public Vector2 Position { get { return position;} set {position = value;} }
	private float size;
	public float Size { get { return size;} set {size = value;} }
	private float angle;
	public float Angle { get { return angle;} set {angle = value;} }
	private bool flipX;
	public bool FlipX { get { return flipX;} set {flipX = value;} }
	private bool flipY;
	public bool FlipY { get { return flipY;} set {flipY = value;} }

	public GamePictureInfo (Texture2D tex, Vector2 pos, float s = 1, float a = 0, bool flX = false, bool flY = false) {
		img = tex;
		position = pos;
		size = s;
		angle = a;
		flipX = flX;
		flipY = flY;
	}

}
