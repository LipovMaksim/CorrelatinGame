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
	[SerializeField]
	private Color notActiveColor;

	private bool teacherMod = false;

	void Awake () {
		//DataTransfer.CurrentUser = DBWorker.getUser ("test", "test");
		//DataTransfer.CurrentUser = DBWorker.getUser ("child", "");
		setTeacherMod (DataTransfer.CurrentUser.IsTeacher);

		if (!teacherMod) {
			if (DataTransfer.CurrentUser.currentTasks.Count == 0) {
				palyBtn.GetComponent<UnityEngine.UI.Image> ().color = notActiveColor;
				palyBtn.GetComponent<UnityEngine.UI.Button> ().enabled = false;
			}
		}
	}

	public void toTaskList () {
		Application.LoadLevel(3);
	}

	public void toChildrenList () {
		Application.LoadLevel(5);
	}

	public void toLogin () {
		Application.LoadLevel(0);
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
		
	private void setTeacherMod (bool f) {
		palyBtn.SetActive (!f);
		tasksBtn.SetActive (f);
		childrenBtn.SetActive (f);
		teacherMod = f;
	}

}
