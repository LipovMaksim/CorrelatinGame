using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditionModel : MonoBehaviour {

	[SerializeField]
	private ButtonController btnController;
	[SerializeField]
	private GameField field;
	[SerializeField]
	private GamePicture [] pictures;
	[SerializeField]
	private Transform basket;
	[SerializeField]
	private Slider sizeSlider;
	[SerializeField]
	private Slider rotationSlider;
	[SerializeField]
	private GameObject imagesCanvas;
	[SerializeField]
	private GameObject backgroundsCanvas;
	[SerializeField]
	private ImageData pictureImageDataPrefab;
	[SerializeField]
	private ImageData backgroundImageDataPrefab;
	[SerializeField]
	private Transform imagesLayout;
	[SerializeField]
	private Transform backgroundsLayout;

	private GamePicture currentPcture = null;
	private Vector3 [] pictureToolBarPositions; 

	void Awake() {

		btnController.backgroundImgChoised += openBackgroundImg;
		btnController.pictureToPanelChoised += openNewPicture;
		btnController.saveLevel += saveLevel;
		btnController.openLevel += openLevel;
		btnController.mirrorVertButtnPresed += mirrorVertCurrentPicture;
		btnController.mirrorHorButtnPresed += mirrorHorCurrentPicture;
		sizeSlider.valueChanged += sizeSliderValueChanged;
		rotationSlider.valueChanged += rotationSliderValueChanged;

		pictureToolBarPositions = new Vector3[pictures.Length];
		for (int i = 0; i < pictures.Length; i++) {
			pictures[i].draggingStart += outFromPanel;
			pictures[i].draggingEnd += pictureDroped;
			pictureToolBarPositions [i] = pictures [i].transform.position;
		}

		Pair<int, Texture2D>[] texs = DBWorker.loadAllPictures ();
		for (int i = 0; i < texs.Length; i++) {
			ImageData id = (ImageData)Instantiate (pictureImageDataPrefab, imagesLayout);
			id.transform.localScale = new Vector3 (1, 1, 1);
			id.setImage (texs [i].second, texs[i].first);
		}

		texs = DBWorker.loadAllBackgrounds ();
		for (int i = 0; i < texs.Length; i++) {
			ImageData id = (ImageData)Instantiate (backgroundImageDataPrefab, backgroundsLayout);
			id.transform.localScale = new Vector3 (1, 1, 1);
			id.setImage (texs [i].second, texs[i].first);
		}
	}

	private void outFromPanel (DraggableObject obj){
		
	}

	void pictureDroped (DraggableObject obj, Vector3 position){
		
		GamePicture gp = (GamePicture)obj;
		setCurrentPicture (gp);
		if (gp != null) {
			//Перемещение в карзину
			Collider2D[] colliders = Physics2D.OverlapCircleAll (new Vector2 (basket.position.x, basket.position.y), 0.2f);
			if (contanes(colliders, gp.gameObject)) {
				gp.reset (); 
				setCurrentPicture (null);
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
			pictures [i].initiatePicture (path);
		}	
	}

	public void openNewPicture(ImageData imgData){

		int i = 0;
		for (; i < pictures.Length && pictures[i].GetComponent<SpriteRenderer>().sprite != null; i++);
		if (i < pictures.Length) {
			pictures [i].setPicture (imgData);
		}	
		imagesCanvas.SetActive (false);
	}

	public void setBackgroundImage (ImageData imgData) {
		field.setBackgroundImg (imgData);
		backgroundsCanvas.SetActive (false);
	}

	public void showAddPicture () {
		imagesCanvas.SetActive (true);
	}

	public void showSetBackground () {
		backgroundsCanvas.SetActive (true);
	}

	private void saveLevel (string path){

		FileWorker.writeLevelInFile (path, field, pictures, "Level name", "Level description");
	}

	public void saveTask () {
		Task task = new Task ("Задание 3", "", null, field.BackgroundId);
		DBWorker.saveTask (task);
	}

	private void openLevel (string path){
		string name = "";
		string description = "";
		FileWorker.readLevelFromFileForEdition (path, ref field, ref pictures, ref name, ref description);
	}

	public void setCurrentPicture (GamePicture gp) {
		if (gp == null) {
			currentPcture.resetCurrent ();
			return;
		}
		if (currentPcture != gp) {
			if (currentPcture != null) {
				currentPcture.resetCurrent ();
			}
			currentPcture = gp;
			currentPcture.setCurrent ();
			sizeSlider.setValue (currentPcture.getSize ());
			rotationSlider.setValue (currentPcture.getRotationAngle());
		}
	}

	private void mirrorVertCurrentPicture () {
		if (currentPcture != null)
			currentPcture.flipX ();
	}

	private void mirrorHorCurrentPicture () {
		if (currentPcture != null)
			currentPcture.flipY ();
	}

	private void sizeSliderValueChanged (float val) {
		if (currentPcture != null)
			currentPcture.setSize (val);
	}

	private void rotationSliderValueChanged (float val) {
		if (currentPcture != null)
			currentPcture.setRotation (val);
	}
}
