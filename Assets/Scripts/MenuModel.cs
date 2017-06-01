using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuModel : MonoBehaviour {

	[SerializeField]
	private GameObject palyBtn;
	[SerializeField]
	private GameObject tasksBtn;
	[SerializeField]
	private GameObject childrenBtn;


	void Awake () {
		setTeacherMod (DataTransfer.CurrentUser.IsTeacher);
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

	public void toGame () {
		Application.LoadLevel(1);
	}

	public void exit () {
		Application.Quit ();
	}

	public void childMod () {
		setTeacherMod (false);
	}

	public void teacherMod () {
		setTeacherMod (true);
	}

	private void setTeacherMod (bool f) {
		palyBtn.SetActive (!f);
		tasksBtn.SetActive (f);
		childrenBtn.SetActive (f);
	}

}
