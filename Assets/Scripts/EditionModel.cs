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
	private OpenFile openBackgroundImgButton;
	[SerializeField]
	private SpriteRenderer fieldSprite;


	void Awake() {
		toMainMenuButton.toMainMenuButtonPresed += toMainMenu;
		continueButton.continueButtonPresed += continueGame;
		menuButton.showMenuButtonPresed += showMenu;
		openBackgroundImgButton.backgroundImgChoised += openBackgroundImg;
	}

	void Update () {
		
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
		/*Texture2D img = Resources.Load (path, typeof(Sprite)) as Texture2D;
		Sprite s = Resources.Load (path, typeof(Sprite)) as Sprite;
		s.
		fieldSprite.sprite = s;*/
	}
}
