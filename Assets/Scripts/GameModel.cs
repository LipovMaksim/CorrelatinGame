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
	private PicturesBar picturesBar;
	[SerializeField]
	private Transform menu;

	[SerializeField]
	private GameObject continueBtn;
	[SerializeField]
	private GameObject repeatBtn;
	[SerializeField]
	private GameObject toMainMenuBtn;

	//private Dictionary <GamePicture, Vector3> gamePicturePlaces = new Dictionary<GamePicture, Vector3>();
	private bool gameIsActive = true;
	private Task currentTask;
	private string gameUrl = "Components\\Levels\\test.cglvl";
	private System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch ();
	private int misstakes = 0;

	void Awake () {
		//DataTransfer.CurrentUser = DBWorker.getUser ("child", "");
		currentTask = DBWorker.loadTask (DataTransfer.CurrentUser.currentTasks.ToArray ()[0]);
		gameField.setTask (currentTask);
		picturesBar.setTask (currentTask);
		picturesBar.pictureDroped += pictureDroped;
		timer.Start ();
		misstakes = 0;
	}

	void pictureDroped (DraggableObject obj, Vector3 position){
		const float R = 0.01F; // Радиус зоны попадания на картинку
		GamePicture gp = (GamePicture)obj;
		if (gp != null) {
			GamePicture target = gameField.onTargetPositon (gp);
			if (target != null) {
				//картинка сопоставлена правильно (музыка, эфекты)
				audioController.playBubblesSound ();
				//gamePicturePlaces.Remove(gp);
				//Destroy (gp.gameObject);
				target.setTrueColor ();
				gp.reset ();
				checkForGameOver ();
				return;
			} else {
				misstakes++;
			}
		}
		gp.returnToPreviousPosition ();
	}

	private void checkForGameOver() {
		if (!picturesBar.hasActivePictures()) {
			viewController.showCangratulations ();
			audioController.playVictorySound ();
			timer.Stop ();
			gameIsActive = false;
			repeatBtn.active = true;
			StartCoroutine (showContinueBtn (3));

		}
	}

	private IEnumerator showContinueBtn (int sec) {
		yield return new WaitForSeconds (sec);
		continueBtn.active = true;
	}

	void OnMouseUp(){
		if (!gameIsActive) {
			gameIsActive = true;
			viewController.hideCangratulations ();
			Application.LoadLevel (4);
		}
	}

	private void continueGame(){
		menu.gameObject.SetActive (false);
	}

	public void toMainMenu(){
		Application.LoadLevel (4);
	}

	private void showMenu(){
		menu.gameObject.SetActive (true);
	}

	public void repeatGame () {
		Application.LoadLevel (1);
	}

	public void nextTask () {

		System.DateTime cd = System.DateTime.Now;
		DBWorker.addHistory(DataTransfer.CurrentUser, new History(DataTransfer.CurrentUser.Id, currentTask.Name, (int)timer.ElapsedMilliseconds/1000, misstakes, 
			(cd.Day < 10 ? "0" : "") + cd.Day + "." + (cd.Month < 10 ? "0" : "") + cd.Month + "." + cd.Year));
		DBWorker.removeCurrentTask (DataTransfer.CurrentUser, currentTask); 
		DataTransfer.CurrentUser.currentTasks.Remove (currentTask.DBId);

		if (DataTransfer.CurrentUser.currentTasks.Count > 0) {
			repeatGame ();
		} else {
			toMainMenu ();
		}
	}

	void Update () {
		
	}
}
