using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModel : MonoBehaviour {

	[SerializeField]
	private GameField gameField;
	[SerializeField]
	private AudioClip bubbles;
	[SerializeField]
	private AudioClip victory;
	[SerializeField]
	private ViewController viewController;
	[SerializeField]
	private MainMenuElement continueButton;
	[SerializeField]
	private MainMenuElement toMainMenuButton;
	[SerializeField]
	private MainMenuElement menuButton;
	[SerializeField]
	private Transform menu;

	public GamePicture fish; //на время
	public GamePicture fish2; //на время

	private Dictionary <GamePicture, Vector3> gamePicturePlaces = new Dictionary<GamePicture, Vector3>();
	private bool gameIsActive = true;

	void Awake () {
		MonoBehaviour[] b = gameField.GetComponents<MonoBehaviour> ();
		gamePicturePlaces.Add (fish, new Vector3 (-3.07F, 1.8F, 0F));
		gamePicturePlaces.Add (fish2, new Vector3 (-3.07F, 1.8F, 0F));

		fish.draggingEnd += pictureDroped;
		fish2.draggingEnd += pictureDroped;
		toMainMenuButton.toMainMenuButtonPresed += toMainMenu;
		continueButton.continueButtonPresed += continueGame;
		menuButton.showMenuButtonPresed += showMenu;
	}

	void pictureDroped (DraggableObject obj, Vector3 position){
		const float R = 0.01F; // Радиус зоны попадания на картинку
		GamePicture gp = (GamePicture)obj;
		if (gp != null) {
			Collider2D[] colliders = Physics2D.OverlapCircleAll(gameField.transform.position + gamePicturePlaces[gp], R);
			if (colliders.Length > 0) {
				bool contane = false;
				for (int i = 0; i < colliders.Length; i++) {
					if (colliders [i].gameObject == gp.gameObject) {
						contane = true;
					}
				}
				if (contane) {
					//картинка сопоставлена правильно (музыка, эфекты)
					AudioSource.PlayClipAtPoint(bubbles, transform.position); //Звук
					gamePicturePlaces.Remove(gp);
					Destroy (gp.gameObject);
					checkForGameOver ();
						
					return;
				}
			}
		}
		gp.returnToPreviousPosition ();
	}

	private void checkForGameOver() {
		if (gamePicturePlaces.Count == 0) {
			viewController.showCangratulations ();
			AudioSource.PlayClipAtPoint(victory	, transform.position);
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
