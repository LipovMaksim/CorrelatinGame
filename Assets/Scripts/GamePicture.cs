using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePicture : DraggableObject {

	public Vector3 positionInToolBar;
	private SpriteRenderer spriteRenderer;

	private Vector2 targetPosition;
	public float wUnitsPicture = 2.5f;
	public float hUnitsPicture = 2.5f;

	public string url = "";

	void Awake(){
		spriteRenderer = GetComponent <SpriteRenderer> ();
		positionInToolBar = transform.position;
	}

	public void setTargetPosition (Vector2 tp) {
		targetPosition = tp;
	}

	public bool onTargetPosition (float esp = 1) {
		if (targetPosition == null)
			return false;
		float a = Mathf.Abs (targetPosition.x - transform.position.x);
		float b = Mathf.Abs (targetPosition.y - transform.position.y);
		return Mathf.Sqrt (a * a + b * b) < esp;
	}

	public void initiatePicture(string path){
		if (setPicture (path))
			activate ();
	}

	public bool setPicture (string path) {
		Texture2D newTex = FileWorker.readImage (path);
		if (newTex != null) {
			url = path;
			Sprite s;
			//Побольшей стороне
			s = Sprite.Create (newTex, new Rect (0, 0, newTex.width, newTex.height), new Vector2 (0.5f, 0.5f), 
				(newTex.width / wUnitsPicture > newTex.height / hUnitsPicture ? newTex.width / wUnitsPicture : newTex.height / hUnitsPicture)); //Расчет количества пикселей в юните по большему отношению сторон к соответствующему количеству юнитов
			setSprite(s);
			return true;
		}
		return false;
	}

	public void reset(){
		transform.position = positionInToolBar;
		removeSprite ();
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
}
