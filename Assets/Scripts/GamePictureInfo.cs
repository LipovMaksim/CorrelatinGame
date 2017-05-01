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
	public int FlipX_int { get { return (flipX ? 1 : 0);} set {flipX = value != 0;} }
	private bool flipY;
	public bool FlipY { get { return flipY;} set {flipY = value;} }
	public int FlipY_int { get { return (flipY ? 1 : 0);} set {flipY = value != 0;} }
	private int id;
	public int Id {get { return id; } set { id = value; }}
	private int pictureId;
	public int PictureId {get { return pictureId; } set { pictureId = value; }}

	public GamePictureInfo (int id, int pictureId, Texture2D tex, Vector2 pos, float s = 1, float a = 0, bool flX = false, bool flY = false) {
		this.id = id;
		this.pictureId = pictureId;
		img = tex;
		position = pos;
		size = s;
		angle = a;
		flipX = flX;
		flipY = flY;
	}

	public bool equals (GamePictureInfo other) {
		return pictureId == other.pictureId && size == other.size && angle == other.angle && flipX == other.flipX && flipY == other.flipY;
	}

}
