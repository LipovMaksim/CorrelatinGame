using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePicture : DraggableObject {

	public Vector3 positionInToolBar;
	private SpriteRenderer spriteRenderer;

	public float wUnitsPicture = 2.5f;
	public float hUnitsPicture = 2.5f;

	void Awake(){
		spriteRenderer = GetComponent <SpriteRenderer> ();
		positionInToolBar = transform.position;
	}

	public void setPicture(string path){
		Texture2D newTex = FileWorker.readImage (path);

		if (newTex != null) {
			Sprite s;
			//Побольшей стороне
			s = Sprite.Create (newTex, new Rect (0, 0, newTex.width, newTex.height), new Vector2 (0.5f, 0.5f), 
				(newTex.width / wUnitsPicture > newTex.height / hUnitsPicture ? newTex.width / wUnitsPicture : newTex.height / hUnitsPicture)); //Расчет количества пикселей в юните по большему отношению сторон к соответствующему количеству юнитов
			setSprite(s);
		}
	}

	public void reset(){
		transform.position = positionInToolBar;
		removeSprite ();
	}

	public void returnToPreviousPosition (){
		transform.position = previousPosition;
	}

	public void setSprite(Sprite sprite){
		if (spriteRenderer.sprite != null)
			Destroy(spriteRenderer.sprite);
		spriteRenderer.sprite = sprite;
		GetComponent<CircleCollider2D> ().enabled = true;
		transform.position = positionInToolBar;
	}

	public void removeSprite(){
		SpriteRenderer sr = GetComponent<SpriteRenderer> ();
		if (sr.sprite != null)
			DestroyImmediate (sr.sprite, true);

		GetComponent<CircleCollider2D> ().enabled = false;
	}
}
