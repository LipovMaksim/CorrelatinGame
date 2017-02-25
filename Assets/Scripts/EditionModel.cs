using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditionModel : MonoBehaviour {

	[SerializeField]
	private ButtonController btnController;
	[SerializeField]
	private GameField field;
	[SerializeField]
	private SpriteRenderer fieldSprite;
	[SerializeField]
	private GamePicture [] pictures;
	[SerializeField]
	private Transform basket;




	private Vector3 [] pictureToolBarPositions; 

	void Awake() {

		btnController.backgroundImgChoised += openBackgroundImg;
		btnController.pictureToPanelChoised += openNewPicture;

		pictureToolBarPositions = new Vector3[pictures.Length];
		for (int i = 0; i < pictures.Length; i++) {
			pictures[i].draggingStart += outFromPanel;
			pictures[i].draggingEnd += pictureDroped;
			pictureToolBarPositions [i] = pictures [i].transform.position;
		}
	}

	private void outFromPanel (DraggableObject obj){
		
	}

	void pictureDroped (DraggableObject obj, Vector3 position){
		
		GamePicture gp = (GamePicture)obj;
		if (gp != null) {
			//Перемещение в карзину
			Collider2D[] colliders = Physics2D.OverlapCircleAll (new Vector2 (basket.position.x, basket.position.y), 0.2f);
			if (contanes(colliders, gp.gameObject)) {
				gp.reset (); 
				return;
			}

			//Перемешение на поле
			if (field.contanes (gp)) {
				return;
			}

		}
		gp.returnToPreviousPosition ();
	}



	private static bool contanes (Collider2D[] colliders, GameObject go){
		
		bool contane = false;
		if (colliders.Length > 0) {
			for (int i = 0; i < colliders.Length; i++) {
				if (colliders [i].gameObject == go) {
					contane = true;
				}
			}
		}
		return contane;
	}

	private void toMainMenu(){
		Application.LoadLevel (0);
	}
		

	private void openBackgroundImg(string path){
		
		field.setBackgroundImg (path);
	}



	private void openNewPicture(string path){

		int i = 0;
		for (; i < pictures.Length && pictures[i].GetComponent<SpriteRenderer>().sprite != null; i++);
		if (i < pictures.Length) {
			pictures [i].setPicture (path);
		}	
	}
}
