using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginModel : MonoBehaviour {

	[SerializeField]
	UnityEngine.UI.InputField name;
	[SerializeField]
	UnityEngine.UI.InputField pwd;

	public void exit () {
		Application.Quit ();
	}

	public void login () {
		DataTransfer.CurrentUser = DBWorker.getUser (name.text, pwd.text);
		if (DataTransfer.CurrentUser != null) {
			Application.LoadLevel (0);
		}
	}
}
