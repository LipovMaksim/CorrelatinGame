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

	public DraggableObject fish;

	private const float wUnitsField = 10;
	private const float hUnitsField = 7;

	void Awake() {
		toMainMenuButton.toMainMenuButtonPresed += toMainMenu;
		continueButton.continueButtonPresed += continueGame;
		menuButton.showMenuButtonPresed += showMenu;
		openBackgroundImgButton.backgroundImgChoised += openBackgroundImg;

		fish.draggingStart += outFromPanel;
		fish.draggingEnd += pictureDroped;
	}

	private void outFromPanel (DraggableObject obj){
		
	}

	void pictureDroped (DraggableObject obj, Vector3 position){
		GamePicture gp = (GamePicture)obj;
		if (gp != null) {
			Collider2D[] colliders = Physics2D.OverlapBoxAll (new Vector2 (field.position.x, field.position.y), new Vector2 (wUnitsField-6f, hUnitsField-5f), 0);
			if (colliders.Length > 0) {
				bool contane = false;
				for (int i = 0; i < colliders.Length; i++) {
					if (colliders [i].gameObject == gp.gameObject) {
						contane = true;
					}
				}
				if (contane) {
					
					return;
				}
			}
		}
		gp.returnToPreviousPosition ();
	}


	private void continueGame(){
		menu.gameObject.SetActive (false);
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
			if (false) {
				s = Sprite.Create (newTex, new Rect (0, 0, newTex.width, newTex.height), new Vector2 (0.5f, 0.5f), 
					(newTex.width / wUnitsField > newTex.height / hUnitsField ? newTex.width / wUnitsField : newTex.height / hUnitsField)); //Расчет количества пикселей в юните по большему отношению сторон к соответствующему количеству юнитов
			}
			//Заполнение
			s = Sprite.Create (newTex, zapolnenie (newTex.width, newTex.height, wUnitsField, hUnitsField), new Vector2 (0.5f, 0.5f),
				(newTex.width / wUnitsField < newTex.height / hUnitsField ? newTex.width / wUnitsField : newTex.height / hUnitsField));
			if (fieldSprite.sprite != null)
				Destroy (fieldSprite.sprite);
			fieldSprite.sprite = s;
		}
	}
}
