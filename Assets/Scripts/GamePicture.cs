using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePicture : DraggableObject {
	[SerializeField]
	private SpriteRenderer frame;


	private Color trueColor = new Color (255, 255, 255, 255);

	public Vector3 positionInToolBar;
	private SpriteRenderer spriteRenderer;

	public GamePictureInfo gamePictureInfo = null;
	private GamePicture target;
	private Vector2 targetPosition;
	public float wUnitsPicture = 2f;
	public float hUnitsPicture = 2f;

	public string url = "";

	void Awake(){
		spriteRenderer = GetComponent <SpriteRenderer> ();
		//frame = GetComponentInChildren <SpriteRenderer> ();
		positionInToolBar = transform.position;
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

	public void initiatePicture(string path){
		if (setPicture (path))
			activate ();
	}

	public bool setPicture (string path) {
		Texture2D newTex = FileWorker.readImage (path);
		if (newTex != null) {
			url = path;
			setPicture (newTex);
			return true;
		}
		return false;
	}

	public void setPicture (Texture2D tex) {
		Sprite s;
		//По большей стороне
		s = Sprite.Create (tex, new Rect (0, 0, tex.width, tex.height), new Vector2 (0.5f, 0.5f), 
			(tex.width / wUnitsPicture > tex.height / hUnitsPicture ? tex.width / wUnitsPicture : tex.height / hUnitsPicture)); //Расчет количества пикселей в юните по большему отношению сторон к соответствующему количеству юнитов
		setSprite(s);
	}

	public void reset(){
		transform.position = positionInToolBar;
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

	public void activate () {
		GetComponent<CircleCollider2D> ().enabled = true;
		transform.position = positionInToolBar;
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
}
