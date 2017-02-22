using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditionModel : MonoBehaviour {

	[SerializeField]
	private MainMenuElement continueButton;
	[SerializeField]
	private MainMenuElement toMainMenuButton;
	[SerializeField]
	private MainMenuElement menuButton;
	[SerializeField]
	private Transform menu;
	[SerializeField]
	private OpenBackground openBackgroundImgButton;
	[SerializeField]
	private OpenPicture	 openNewPictureImgButton;
	[SerializeField]
	private Transform field;
	[SerializeField]
	private SpriteRenderer fieldSprite;
	[SerializeField]
	private GamePicture [] pictures;
	[SerializeField]
	private Transform basket;

	private const float wUnitsField = 10;
	private const float hUnitsField = 7;
	private const float wUnitsPicture = 2.5f;
	private const float hUnitsPicture = 2.5f;


	private Vector3 [] pictureToolBarPositions; 

	void Awake() {
		toMainMenuButton.toMainMenuButtonPresed += toMainMenu;
		continueButton.continueButtonPresed += continueGame;
		menuButton.showMenuButtonPresed += showMenu;
		openBackgroundImgButton.backgroundImgChoised += openBackgroundImg;
		openNewPictureImgButton.pictureChoised += openNewPicture;

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
			}

			//Перемешение на поле
			colliders = Physics2D.OverlapBoxAll (new Vector2 (field.position.x, field.position.y), new Vector2 (wUnitsField-5f, hUnitsField-4f), 0);
			if (contanes(colliders, gp.gameObject)) {
				return;
			}

		}
		gp.returnToPreviousPosition ();
	}


	private void continueGame(){
		menu.gameObject.SetActive (false);
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

	private void showMenu(){
		menu.gameObject.SetActive (true);
	}

	private void openBackgroundImg(string path){

		Texture2D newTex = FileWorker.readImage (path);

		if (newTex != null){
			Sprite s;
			//Побольшей стороне
			if (false){
				s = Sprite.Create(newTex, new Rect(0,0,newTex.width,newTex.height), new Vector2(0.5f, 0.5f), 
					(newTex.width / wUnitsField > newTex.height / hUnitsField ? newTex.width / wUnitsField : newTex.height / hUnitsField)); //Расчет количества пикселей в юните по большему отношению сторон к соответствующему количеству юнитов
			}
			//Заполнение
			s = Sprite.Create(newTex, zapolnenie (newTex.width,newTex.height, wUnitsField, hUnitsField), new Vector2(0.5f, 0.5f),
				(newTex.width / wUnitsField < newTex.height / hUnitsField ? newTex.width / wUnitsField : newTex.height / hUnitsField));
			if (fieldSprite.sprite != null)
				Destroy (fieldSprite.sprite);
			fieldSprite.sprite = s;
		}
	}

	private Rect zapolnenie(int w, int h, float wu, float hu){
		int ppuw = (int) (w / wu);
		int ppuh = (int) (h / hu);
		int newW = w;
		int newH = h;
		if (ppuh < ppuw) {
			newW = (int) (ppuh * wu);
		} else {
			newH = (int) (ppuw * hu);
		}
		return new Rect ((w - newW) / 2, (h - newH) / 2, newW, newH);
	}

	private void openNewPicture(string path){

		Texture2D newTex = FileWorker.readImage (path);

		if (newTex != null) {
			Sprite s;
			//Побольшей стороне
			s = Sprite.Create (newTex, new Rect (0, 0, newTex.width, newTex.height), new Vector2 (0.5f, 0.5f), 
				(newTex.width / wUnitsPicture > newTex.height / hUnitsPicture ? newTex.width / wUnitsPicture : newTex.height / hUnitsPicture)); //Расчет количества пикселей в юните по большему отношению сторон к соответствующему количеству юнитов
			int i = 0;
			for (; i < pictures.Length && pictures[i].GetComponent<SpriteRenderer>().sprite != null; i++);
			if (i < pictures.Length) {
				pictures [i].setSprite (s);
			}	

		}
	}
}
