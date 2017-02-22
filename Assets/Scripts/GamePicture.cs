using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePicture : DraggableObject {

	public Vector3 positionInToolBar;

	void Awake(){
		positionInToolBar = transform.position;
	}

	public void reset(){
		transform.position = positionInToolBar;
		removeSprite ();
	}

	public void returnToPreviousPosition (){
		transform.position = previousPosition;
	}

	public void setSprite(Sprite sprite){
		SpriteRenderer sr = GetComponent<SpriteRenderer> ();
		if (sr.sprite != null)
			Destroy(sr.sprite);
		sr.sprite = sprite;
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
