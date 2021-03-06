﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditionModel : MonoBehaviour {

	[SerializeField]
	private ButtonController btnController;
	[SerializeField]
	private GameField field;
	[SerializeField]
	PicturesBar picturesBar;
	//private GamePicture [] pictures;
	[SerializeField]
	private Transform basket;
	[SerializeField]
	private UnityEngine.UI.Slider sizeSlider;
	[SerializeField]
	private UnityEngine.UI.Slider rotationSlider;
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
	[SerializeField]
	private GameObject saveMenu;
	[SerializeField]
	private GameObject pall;
	[SerializeField]
	private GameObject canvasPall;
	[SerializeField]
	private UnityEngine.UI.InputField taskTitleImput;
	[SerializeField]
	private UnityEngine.UI.InputField taskDescriptionImput;
	[SerializeField]
	private UnityEngine.UI.Dropdown type;

	[SerializeField]
	private GameObject backgroundError;
	[SerializeField]
	private GameObject noObjectsOnFieldError;
	[SerializeField]
	private GameObject enterTitleError;


	private GamePicture currentPcture = null;
	private string taskTitle = "";
	private string taskDescription = "";

	void Start() {

		btnController.mirrorVertButtnPresed += mirrorVertCurrentPicture;
		btnController.mirrorHorButtnPresed += mirrorHorCurrentPicture;
		//sizeSlider.valueChanged += sizeSliderValueChanged;
		//rotationSlider.valueChanged += rotationSliderValueChanged;
		picturesBar.pictureDroped += pictureDroped;


		Pair<int, Texture2D>[] texs = DBWorker.loadAllPictures ();
		for (int i = 0; i < texs.Length; i++) {
			ImageData imageData = (ImageData)Instantiate (pictureImageDataPrefab, imagesLayout);
			imageData.transform.localScale = new Vector3 (1, 1, 1);
			imageData.setImage (texs [i].second, texs[i].first);
		}

		texs = DBWorker.loadAllBackgrounds ();
		for (int i = 0; i < texs.Length; i++) {
			ImageData imageData = (ImageData)Instantiate (backgroundImageDataPrefab, backgroundsLayout);
			imageData.transform.localScale = new Vector3 (1, 1, 1);
			imageData.setImage (texs [i].second, texs[i].first);
		}

		//DataTransfer.Task = DBWorker.loadTask (1);

		if (DataTransfer.Task != null) {
			field.setBackgroundImg (DataTransfer.Task.getBackgroundData ());
			picturesBar.setTask (DataTransfer.Task);
			picturesBar.pictureDroped += pictureDroped;
		}
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
				noObjectsOnFieldError.active = false;
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
		Application.LoadLevel (4);
	}

	public void toTaskList () {
		Application.LoadLevel (3);
	}

	public void hideBackgroundCanvas() {
		backgroundsCanvas.SetActive (false);
		freez (false);
	}

	public void hideGameObjectCanvas() {
		imagesCanvas.SetActive (false);
		freez (false);
	}

	private void openBackgroundImg(string path){
		
		field.setBackgroundImg (path);
	}


	/*
	private void openNewPicture(string path){

		int i = 0;
		for (; i < pictures.Length && pictures[i].GetComponent<SpriteRenderer>().sprite != null; i++);
		if (i < pictures.Length) {
			pictures [i].initiatePicture (path);
		}	
	}*/

	public void openNewPicture(ImageData imgData){

		picturesBar.addPicture (imgData);
		imagesCanvas.SetActive (false);
		freez (false);
	}

	public void setBackgroundImage (ImageData imgData) {
		field.setBackgroundImg (imgData);
		backgroundsCanvas.SetActive (false);
		freez (false);
		backgroundError.active = false;
	}

	public void showAddPicture () {
		if (!(saveMenu.active || backgroundsCanvas.active || imagesCanvas.active)) {
			imagesCanvas.SetActive (true);
			freez ();
		}
	}

	public void showSetBackground () {
		if (!(saveMenu.active || backgroundsCanvas.active || imagesCanvas.active)) {
			backgroundsCanvas.SetActive (true);
			freez ();
		}
	}

	/*
	private void saveLevel (string path){

		FileWorker.writeLevelInFile (path, field, pictures, "Level name", "Level description");
	}*/

	public void saveTask (bool asNew) {
		if (taskTitle != null && taskTitle.Length > 0) {
			Task task = new Task (taskTitle, taskDescription, field.BackgroundId, type.value);
			task.setGamePictures (picturesBar.getGamePictures ());
			if (!asNew && DataTransfer.Task != null)
				task.DBId = DataTransfer.Task.DBId;
			DataTransfer.Task = task;
			DataTransfer.Task.DBId = DBWorker.saveTask (task);
			//DataTransfer.Task = DBWorker.loadLastAddedTask ();
			showSaveMenu (false);
			freez (false);
		} else {
			enterTitleError.active = true;
		}
	}

	public void hideTaskTitleError () {
		enterTitleError.active = false;
	}

	/*
	private void openLevel (string path){
		string name = "";
		string description = "";
		FileWorker.readLevelFromFileForEdition (path, ref field, ref pictures, ref name, ref description);
	}*/

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
			sizeSlider.value = (currentPcture.getSize ());
			rotationSlider.value =  (currentPcture.getRotationAngle());
		}
	}

	private void mirrorVertCurrentPicture () {
		if (!(saveMenu.active || backgroundsCanvas.active || imagesCanvas.active)) {
			if (currentPcture != null)
				currentPcture.flipX ();
		}
	}

	private void mirrorHorCurrentPicture () {
		if (!(saveMenu.active || backgroundsCanvas.active || imagesCanvas.active)) {
			if (currentPcture != null)
				currentPcture.flipY ();
		}
	}

	public void sizeSliderValueChanged (UnityEngine.UI.Slider slider) {
		if (!(saveMenu.active || backgroundsCanvas.active || imagesCanvas.active)) {
			if (currentPcture != null)
				currentPcture.setSize (slider.value);
		}
	}

	public void rotationSliderValueChanged (UnityEngine.UI.Slider slider) {
		if (!(saveMenu.active || backgroundsCanvas.active || imagesCanvas.active)) {
			if (currentPcture != null)
				currentPcture.setRotation (slider.value);
		}
	}

	public void taskTitleChaged (UnityEngine.UI.Text text) {
		taskTitle = text.text;
	}

	public void taskDecriptionChaged (UnityEngine.UI.Text text) {
		taskDescription = text.text;
	}

	public void showSaveMenu (bool val) {
		if (field.BackgroundId == -1) {
			backgroundError.active = true;
		} else if ( getPicturesOnField().Length == 0 ) {
			noObjectsOnFieldError.active = true;
		} else if (!(backgroundsCanvas.active || imagesCanvas.active)) {
			if (val == true && DataTransfer.Task != null) {
				taskTitle = taskTitleImput.text = DataTransfer.Task.Name;
				taskDescription = taskDescriptionImput.text = DataTransfer.Task.Description;
				type.value = DataTransfer.Task.Type;
			}
			saveMenu.SetActive (val);
			freez (val);
		}
	}

	private void freez (bool f = true) {
		pall.active = f;
		canvasPall.active = f;
	}

	private GamePicture [] getPicturesOnField () {
		List<GamePicture> res = new List<GamePicture>();
		GamePicture[] gps = picturesBar.getGamePicturesObj ();

		for (int i = 0; i < gps.Length; i++) {
			if (gps [i].isActive ()) {
				if (field.contanes(gps[i])) {
					res.Add (gps [i]);
				}
			}
		}
		return res.ToArray ();
	}
}

