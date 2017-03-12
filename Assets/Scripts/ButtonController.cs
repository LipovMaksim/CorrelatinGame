using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ButtonController : MonoBehaviour {

	[SerializeField]
	private Button continueBtn;
	[SerializeField]
	private Button toMainMenuBtn;
	[SerializeField]
	private Button showMenuBtn;
	[SerializeField]
	private Button playBtn;
	[SerializeField]
	private Button editFieldBtn;
	[SerializeField]
	private Button optionsBtn;
	[SerializeField]
	private Button exitBtn;
	[SerializeField]
	private Button openBackgroundPictureBtn;
	[SerializeField]
	private Button addPictureToPanelBtn;
	[SerializeField]
	private Button saveLevelBtn;
	[SerializeField]
	private Button openLevelBtn;
	[SerializeField]
	private Button mirrorVert;
	[SerializeField]
	private Button mirrorHor;
	[SerializeField]
	private GameObject menu; //Если не указывать, примется объект на котором лежит скрипт


	void Start () {
		if (continueBtn != null)				continueBtn.buttonPresed 				+= continueAction;
		if (toMainMenuBtn != null)				toMainMenuBtn.buttonPresed 				+= toMainMenuAction;
		if (showMenuBtn != null)				showMenuBtn.buttonPresed				+= showMenuAction;
		if (playBtn != null)					playBtn.buttonPresed					+= playAction;
		if (editFieldBtn != null)				editFieldBtn.buttonPresed				+= editFieldAction;
		if (optionsBtn != null) 				optionsBtn.buttonPresed 				+= optionsAction;
		if (exitBtn != null)					exitBtn.buttonPresed					+= exitAction;
		if (openBackgroundPictureBtn != null)	openBackgroundPictureBtn.buttonPresed	+= openBackgroundPictureAction;
		if (addPictureToPanelBtn != null)		addPictureToPanelBtn.buttonPresed		+= addPictureToPanelAction;
		if (saveLevelBtn != null)				saveLevelBtn.buttonPresed				+= saveLevelAction;
		if (openLevelBtn != null)				openLevelBtn.buttonPresed				+= openLevelAction;
		if (mirrorVert != null)					mirrorVert.buttonPresed					+= mirrorVertAction;
		if (mirrorHor != null)					mirrorHor.buttonPresed					+= mirrorHorAction;

		if (menu == null)	menu = gameObject;
	}

	private void continueAction () {
		menu.SetActive (false);
	}

	private void toMainMenuAction () {
		Application.LoadLevel (0);
	}

	private void showMenuAction () {
		menu.SetActive (true);
	}

	private void playAction () {
		Application.LoadLevel(1);
	}

	private void editFieldAction () {
		Application.LoadLevel(2);
	}

	private void optionsAction () {
		//Пока не реализовано
	}

	private void exitAction () {
		Application.Quit ();
		Debug.Log ("Exit!!!");
	}

	private void openBackgroundPictureAction () {
		string title = "Открыть фоновое изображение";
		string directory = "Components/Backgrounds";
		string extention = "png,jpg";
		if (backgroundImgChoised != null) {
			string path = EditorUtility.OpenFilePanel (title, directory, extention);

			if (path != "") {
				Debug.Log ("Background img choised: " + path);
				backgroundImgChoised (path);
			}
		}
			
	}
	public delegate void BackgroundImgChoised (string path);
	public BackgroundImgChoised backgroundImgChoised;

	private void addPictureToPanelAction () {
		string title = "Добавть объект";
		string directory = "Components/Objects";
		string extention = "png,jpg";
		if (backgroundImgChoised != null) {
			string path = EditorUtility.OpenFilePanel (title, directory, extention);

			if (path != "") {
				Debug.Log ("Picture choised: " + path);
				pictureToPanelChoised (path);
			}
		}
	}
	public delegate void PictureToPanelChoised (string path);
	public PictureToPanelChoised pictureToPanelChoised;

	private void saveLevelAction () {
		string title = "Сохранить уровень";
		string directory = "Components/Levels";
		string defaultName = "Новый уровень";
		string extention = "cglvl";
		if (saveLevel != null) {
			string path = EditorUtility.SaveFilePanel (title, directory, defaultName, extention);

			if (path != "") {
				Debug.Log ("File foe save level choised: " + path);
				saveLevel (path);
			}
		}
	}
	public delegate void SaveLevelEvent (string path);
	public SaveLevelEvent saveLevel;

	private void openLevelAction () {
		string title = "Открыть уровень";
		string directory = "Components/Levels";
		string extention = "cglvl";
		if (openLevel != null) {
			string path = EditorUtility.OpenFilePanel(title, directory, extention);

			if (path != "") {
				Debug.Log ("File foe save level choised: " + path);
				openLevel (path);
			}
		}
	}
	public delegate void OpenLevelEvent (string path);
	public OpenLevelEvent openLevel;

	private void mirrorVertAction () {
		mirrorVertButtnPresed ();
	}
	public delegate void MirrorVertButtnPresedEvent ();
	public MirrorVertButtnPresedEvent mirrorVertButtnPresed;

	private void mirrorHorAction () {
		mirrorHorButtnPresed ();
	}
	public delegate void MirrorHorButtnPresedEvent ();
	public MirrorHorButtnPresedEvent mirrorHorButtnPresed;

		
}
