using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuModel : MonoBehaviour {

	void Awake () {

	}

	public void toTaskList () {
		Application.LoadLevel(3);
	}

	public void toChildrenList () {
		//Application.LoadLevel(5);
	}

	public void toLogin () {
		Application.LoadLevel(4);
	}

	public void exit () {
		Application.Quit ();
	}

}
