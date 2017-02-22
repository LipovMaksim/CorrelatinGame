using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuElement : MonoBehaviour {

	[SerializeField]
	private ButtonType buttonType;


	private Color color;

	public enum ButtonType
	{
		PLAY,
		FIELD_EDIT,
		OPTIONS,
		EXIT,
		CONTINUE,
		TO_MAIN_MENU,
		MENU
	}

	void Start () {
		color = GetComponent<TextMesh> ().color;

	}

	private void OnMouseEnter () {
		GetComponent<TextMesh> ().color = Color.gray;
	}

	private void OnMouseExit () {
		GetComponent<TextMesh> ().color = color;
	}

	private void OnMouseUp () {
		switch (buttonType) {
		case ButtonType.PLAY:
			//Перейти к окну игры
			Application.LoadLevel(1);
			break;
		case ButtonType.FIELD_EDIT:
			//Перейти к окну редактирования игрового поля
			Application.LoadLevel(2);
			break;
		case ButtonType.OPTIONS:

			break;
		case ButtonType.EXIT:
			//Выйти из игры
			Application.Quit ();
			Debug.Log ("Exit!!!");
			break;
		case ButtonType.CONTINUE:
			if (continueButtonPresed != null)
				continueButtonPresed ();
			break;
		case ButtonType.TO_MAIN_MENU:
			if (toMainMenuButtonPresed != null)
				toMainMenuButtonPresed ();
			break;
		case ButtonType.MENU:
			if (showMenuButtonPresed != null)
				showMenuButtonPresed ();
			break;
		}
				

	}

	public delegate void ContinueButtonPresed ();
	public ContinueButtonPresed continueButtonPresed;

	public delegate void ToMainMenuButtonPresed ();
	public ToMainMenuButtonPresed toMainMenuButtonPresed;

	public delegate void ShowMenuButtonPresed ();
	public ShowMenuButtonPresed showMenuButtonPresed;
}
