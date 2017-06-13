using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginModel : MonoBehaviour {

	[SerializeField]
	UnityEngine.UI.InputField name;
	[SerializeField]
	UnityEngine.UI.InputField pwd;
	[SerializeField]
	UnityEngine.UI.Text errorText;

	public void exit () {
		Application.Quit ();
	}

	public void login () {
		DataTransfer.CurrentUser = DBWorker.getUser (name.text, pwd.text);
		if (DataTransfer.CurrentUser != null) {
			Application.LoadLevel (4);
		} else {
			errorText.gameObject.active = true;
		}
	}
}
