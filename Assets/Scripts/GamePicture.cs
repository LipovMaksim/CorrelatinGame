using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePicture : DraggableObject {
	[SerializeField]
	private SpriteRenderer frame;
	[SerializeField]
	private bool edition = false;

	private Color trueColor = new Color (255, 255, 255, 255);


	private SpriteRenderer _spriteRenderer;
	private SpriteRenderer spriteRenderer{
		set { _spriteRenderer = value;} 
		get { 
			while (_spriteRenderer == null) {
				_spriteRenderer = GetComponent <SpriteRenderer> ();
			}
			return _spriteRenderer;
		}
	}

	private GamePictureInfo gamePictureInfo = null;
	public GamePictureInfo GamePictureInfo{
		get {
			if (edition) {
				gamePictureInfo.Angle = getRotationAngle ();
				gamePictureInfo.Size = getSize ();
				gamePictureInfo.Position = getPositionOnField ();
				gamePictureInfo.FlipX = getFlipX ();
				gamePictureInfo.FlipY = getFlipY ();
			}
			return gamePictureInfo;
		}
	}
	private GamePicture target;
	private Vector2 targetPosition;
	public float wUnitsPicture = 2f;
	public float hUnitsPicture = 2f;

	private int id = -1;
	public int Id { get { return id; } set { id = value; } }

	public string url = "";

	private Vector3 fieldPosition = new Vector3 (0, 0, 0);
	public Vector3 FieldPosition { get { return fieldPosition;} set { fieldPosition = value;}}

	void Awake(){
		
	}

	public void setTargetPosition (Vector2 tp) {
		targetPosition = tp;
	}
	public void setTarget (GamePicture t) {
		target = t;
	}

	public bool onTargetPosition (float esp = 1) {
		if (targetPosition == null)
			return false;
		float a = Mathf.Abs (targetPosition.x - transform.position.x);
		float b = Mathf.Abs (targetPosition.y - transform.position.y);
		if (Mathf.Sqrt (a * a + b * b) < esp) {
			return true;
		} else {
			return false;
		}
	}

	/*
	public void initiatePicture(string path){
		if (setPicture (path))
			activate ();
	}*/

	/*
	public bool setPicture (string path) {
		Texture2D newTex = FileWorker.readImage (path);
		if (newTex != null) {
			url = path;
			setPicture (newTex);
			return true;
		}
		return false;
	}*/

	public void setPicture (ImageData imgData) {
		gamePictureInfo = new GamePictureInfo (-1, imgData.Id, imgData.Texture, new Vector2 ());
		setPicture (imgData.Texture);
		activate ();
	}

	public void setPicture (GamePictureInfo gpi) {
		gamePictureInfo = gpi;
		id = gpi.Id;
		setPicture (gpi.Img);
		activate ();
	}

	public void setPicture (Texture2D tex) {
		Sprite s;
		//По большей стороне
		s = Sprite.Create (tex, new Rect (0, 0, tex.width, tex.height), new Vector2 (0.5f, 0.5f), 
			(tex.width / wUnitsPicture > tex.height / hUnitsPicture ? tex.width / wUnitsPicture : tex.height / hUnitsPicture)); //Расчет количества пикселей в юните по большему отношению сторон к соответствующему количеству юнитов
		setSprite(s);
	}

	public void reset(){
		//transform.position = positionInToolBar;
		removeSprite ();
		setSize (1f);
		spriteRenderer.flipX = false;
		spriteRenderer.flipY = false;
	}

	public void returnToPreviousPosition (){
		transform.position = previousPosition;
	}

	public void setSprite(Sprite sprite){
		SpriteRenderer sr = GetComponent <SpriteRenderer> ();
		if (sr.sprite != null)
			Destroy(sr.sprite);
		sr.sprite = sprite;
	}

	public void activate (bool f = true) {
		GetComponent<CircleCollider2D> ().enabled = f;
		//transform.position = positionInToolBar;
	}

	public void removeSprite(){
		SpriteRenderer sr = GetComponent<SpriteRenderer> ();
		if (sr.sprite != null)
			DestroyImmediate (sr.sprite, true);

		GetComponent<CircleCollider2D> ().enabled = false;
	}

	public bool isActive(){
		return GetComponent <CircleCollider2D> ().enabled;
	}

	public void setCurrent () {
		frame.enabled = true;
	}

	public void resetCurrent () {
		frame.enabled = false;
	}

	public void flipX () {
		spriteRenderer.flipX = !spriteRenderer.flipX;
	}

	public void flipY () {
		spriteRenderer.flipY = !spriteRenderer.flipY;
	}

	public void setSize (float scale) {
		transform.localScale = new Vector3 (scale, scale, 1);
	}

	public float getSize () {
		return transform.localScale.x;
	}

	public void setRotation (float angle) {
		transform.localRotation = new Quaternion(0, 0, angle, 1);
	}

	public float getRotationAngle () {
		return transform.localRotation.z;
	}

	public void setFlipX (bool val) {
		spriteRenderer.flipX = val;
	}

	public void setFlipY (bool val) {
		spriteRenderer.flipY = val;
	}

	public bool getFlipX () {
		return spriteRenderer.flipX;
	}

	public bool getFlipY () {
		return spriteRenderer.flipY;
	}

	public void setSpriteColor (Color c) {
		spriteRenderer.color = c;
	}

	public void setFrom (GamePictureInfo gpi) {
		setPicture (gpi.Img);
		setRotation (gpi.Angle);
		setSize (gpi.Size);
		setTargetPosition (gpi.Position);
		setFlipX (gpi.FlipX);
		setFlipY (gpi.FlipY);
		gamePictureInfo = gpi;
		activate ();
	}

	public void setTrueColor () {
		setSpriteColor (trueColor);
	}

	public void setPositionOnField (Vector2 pos) {
		transform.position = new Vector3 (fieldPosition.x + pos.x, fieldPosition.y + pos.y, fieldPosition.z - 1);
	}

	public Vector2 getPositionOnField () {
		return new Vector2 (transform.position.x - fieldPosition.x, transform.position.y - fieldPosition.y);
	}
}
