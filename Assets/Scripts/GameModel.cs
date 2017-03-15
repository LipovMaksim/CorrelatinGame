using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModel : MonoBehaviour {

	[SerializeField]
	private GameField gameField;
	[SerializeField]
	private ViewController viewController;
	[SerializeField]
	private AudioController audioController;
	[SerializeField]
	private GamePicture [] pictures;
	[SerializeField]
	private Transform menu;


	//private Dictionary <GamePicture, Vector3> gamePicturePlaces = new Dictionary<GamePicture, Vector3>();
	private bool gameIsActive = true;
	private string gameUrl = "Components\\Levels\\test.cglvl";
	void Awake () {

		string name = "", description = "";
		FileWorker.readLevelFromFileForGame (gameUrl, ref gameField, ref pictures, ref name, ref description); 

		for (int i = 0; i < pictures.Length; i++) {
			pictures [i].draggingEnd += pictureDroped;
		}
		//MonoBehaviour[] b = gameField.GetComponents<MonoBehaviour> ();
		/*gamePicturePlaces.Add (fish, new Vector3 (-3.07F, 1.8F, 0F));
		gamePicturePlaces.Add (fish2, new Vector3 (-3.07F, 1.8F, 0F));

		fish.draggingEnd += pictureDroped;
		fish2.draggingEnd += pictureDroped;*/
		/*toMainMenuButton.toMainMenuButtonPresed += toMainMenu;
		continueButton.continueButtonPresed += continueGame;
		menuButton.showMenuButtonPresed += showMenu;*/
	}

	void pictureDroped (DraggableObject obj, Vector3 position){
		const float R = 0.01F; // Радиус зоны попадания на картинку
		GamePicture gp = (GamePicture)obj;
		if (gp != null) {
			if (gp.onTargetPosition ()) {
				//картинка сопоставлена правильно (музыка, эфекты)
				audioController.playBubblesSound();
				//gamePicturePlaces.Remove(gp);
				//Destroy (gp.gameObject);
				gp.reset ();
				checkForGameOver ();
				return;
			}
		}
		gp.returnToPreviousPosition ();
	}

	private void checkForGameOver() {
		bool empty = true;
		for (int i = 0; i < pictures.Length; i++) {
			if (pictures[i].isActive())
				empty = false;
		}
		if (empty) {
			viewController.showCangratulations ();
			audioController.playVictorySound ();
			gameIsActive = false;
		}
	}

	void OnMouseUp(){
		if (!gameIsActive) {
			gameIsActive = true;
			viewController.hideCangratulations ();
			Application.LoadLevel (0);
		}
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


	void Update () {
		
	}
}
