using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildrenListModel : MonoBehaviour {

	[SerializeField]
	private ChildNote childNotePrefab;
	[SerializeField]
	private Transform childrenLayout;
	[SerializeField]
	private TaskNote taskNotePrefab;
	[SerializeField]
	private TaskNote fullTaskNotePrefab;
	[SerializeField]
	private Transform tasksLayout;
	[SerializeField]
	private GameObject addChildMenu;
	[SerializeField]
	private GameObject pall;
	[SerializeField]
	private Transform allTasksLayout;
	[SerializeField]
	private GameObject tasksToAdd;

	[SerializeField]
	private UnityEngine.UI.InputField fio;
	[SerializeField]
	private UnityEngine.UI.InputField login;
	[SerializeField]
	private UnityEngine.UI.InputField pwd;
	[SerializeField]
	private UnityEngine.UI.Dropdown day;
	[SerializeField]
	private UnityEngine.UI.Dropdown month;
	[SerializeField]
	private UnityEngine.UI.Dropdown year;
	[SerializeField]
	private GameObject nameError;
	[SerializeField]
	private GameObject loginError;
	[SerializeField]
	private GameObject loginConsistsError;

	private Task [] taskList;
	private ChildNote currentChild = null;
	private TaskNote currentTask = null;
	private ChildNote editingChild = null;

	const int MONTH_COUNT = 12;
	const int YEAR_PERIOD = 18;
	const int DAYS_IN_MONTH = 31;


	void Awake () {
		User[] children = DBWorker.loadChildren ();
		for (int i = 0; i < children.Length; i++) {
			ChildNote childNote = (ChildNote)Instantiate (childNotePrefab, childrenLayout);
			childNote.transform.localScale = new Vector3 (1, 1, 1);
			childNote.setChild (children [i]);
		}

		taskList = DBWorker.loadAllTasks ();

		for (int i = 0; i < taskList.Length; i++) {
			TaskNote taskNote = (TaskNote)Instantiate (fullTaskNotePrefab, allTasksLayout);
			taskNote.transform.localScale = new Vector3 (1, 1, 1);
			taskNote.setTask (taskList [i]);
		}

		List <string> monthes = new List<string> ();
		for (int i = 0; i < MONTH_COUNT; i++) {
			monthes.Add ((i + 1).ToString());
		}
		month.AddOptions (monthes);

		List <string> years = new List<string> ();
		//System.DateTime curDateTime = System.DateTime.Now;
		//for (int i = curDateTime.Year - YEAR_PERIOD + 1; i <= curDateTime.Year; i++) {
		for (int i = 2017 - YEAR_PERIOD + 1; i <= 2017; i++) {
			years.Add (i.ToString ());
		}
		year.AddOptions (years);

		List <string> days = new List<string> ();
		for (int i = 1; i <= DAYS_IN_MONTH; i++) {
			days.Add (i.ToString ());
		}
		day.AddOptions (days);
	}

	public void showAddChild (bool show) {
		addChildMenu.active = show;
		pall.active = show;
		if (!show) {
			fio.text = "";
			pwd.text = "";
			login.text = "";
			day.value = 0;
			month.	value = 0;
			year.value = 0;
		}
	}

	public void showAddTask (bool show) {
		if (currentChild != null) {
			tasksToAdd.active = show;
			pall.active = show;
		}
	}

	public void editChild () {
		if (currentChild != null) {
			pall.active = true;
			addChildMenu.active = true;
			editingChild = currentChild;
			fio.text = editingChild.getChild ().FIO;
			pwd.text = editingChild.getChild ().Password;
			login.text = editingChild.getChild ().Login;

			char[] separators = { '.' };
			string[] date = editingChild.getChild ().BirthDate.Split (separators, 3);
			day.value = int.Parse (date [0]) - 1;
			month.value = int.Parse (date [1]) - 1;
			year.value = int.Parse (date [2]) - int.Parse (year.options.ToArray () [year.value].text);
		}
	}

	public void saveChild () {
		if (fio.text.Length == 0) {
			nameError.active = true;
		} else if (login.text.Length == 0) {
			loginError.active = true;
		} else if (DBWorker.loginConsists(login.text, (editingChild == null ? null : editingChild.getChild().Id.ToString()))) {
			loginConsistsError.active = true;
		} else {
			string date = (day.value + 1 < 10 ? "0" : "") + (day.value + 1).ToString ();
			date += "." + (month.value + 1 < 10 ? "0" : "") + (month.value + 1).ToString ();
			date += "." + year.options.ToArray () [year.value].text;

			if (editingChild == null) {
				User child = new User (false, -1, login.text, pwd.text, fio.text, date);

				child.Id = DBWorker.saveChild (child);

				ChildNote childNote = (ChildNote)Instantiate (childNotePrefab, childrenLayout);
				childNote.transform.localScale = new Vector3 (1, 1, 1);
				childNote.setChild (child);
				showAddChild (false);
			} else {
				User c = editingChild.getChild ();
				c.FIO = fio.text;
				c.Login = login.text;
				c.Password = pwd.text;
				c.BirthDate = date;
				DBWorker.saveChild (c);
				editingChild.setChild (c);
				showAddChild (false);
				editingChild = null;
			}
		}
	}

	public void hideLoginErrors () {
		loginError.active = false;
		loginConsistsError.active = false;
	}

	public void hideFIOErrors () {
		nameError.active = false;
	}

	public void remomeChild () {
		if (currentChild != null) {
			DBWorker.removeChild (currentChild.getChild ());
			Destroy (currentChild.gameObject);
			currentChild = null;
		}
	}

	public void remomeCurrentTask () {
		if (currentChild != null && currentTask != null) {
			DBWorker.removeCurrentTask (currentChild.getChild (), currentTask.getTask());
			currentChild.getChild ().currentTasks.Remove (currentTask.getTask ().DBId);
			Destroy (currentTask.gameObject);
			currentTask = null;
		}
	}

	public void addTaskToCurrent (TaskNote tn) {
		TaskNote taskNote = (TaskNote)Instantiate (taskNotePrefab, tasksLayout);
		taskNote.transform.localScale = new Vector3 (1, 1, 1);
		taskNote.setTask (tn.getTask());
		currentChild.getChild ().currentTasks.Add (taskNote.getTask ().DBId);

		DBWorker.addCurrentTask (currentChild.getChild (), tn.getTask ());

		showAddTask (false);
	}

	public void updateDays (int m) {
		List <UnityEngine.UI.Dropdown.OptionData> options = day.options;
		int dayCount = dayCountInMonth (m + 1, int.Parse (year.options.ToArray()[year.value].text));
		if (dayCount != options.Count) {
			if (day.value >= dayCount - 1) {
				day.value = dayCount - 1;
			}
			while (dayCount < options.Count) {
				options.RemoveAt (options.Count - 1);
			}
			while (dayCount > options.Count) {
				options.Add (new UnityEngine.UI.Dropdown.OptionData((options.Count + 1).ToString()));
			}
		}

	}

	private int dayCountInMonth (int m, int y) {
		int dayCount;
		if (m == 2) {
			if (y % 4 == 0) {
				dayCount = 29;
			} else {
				dayCount = 28;
			}
		} else if (m < 8) {
			if (m % 2 == 0) {
				dayCount = 30;
			} else {
				dayCount = 31;
			}
		} else {
			if (m % 2 == 0) {
				dayCount = 31;
			} else {
				dayCount = 30;
			}
		}
		return dayCount;
	}

	public void setTasks (ChildNote child) {
		if (currentChild != null) {
			currentChild.setCurrent (false);
		}
		currentChild = child;
		currentChild.setCurrent ();
		int [] ct = child.getChild ().currentTasks.ToArray ();
		List <Task> tl = new List<Task> ();
		for (int i = 0; i < ct.Length; i++) {
			for (int j = 0; j < taskList.Length; j++) {
				if (taskList [j].DBId == ct [i]) {
					tl.Add (taskList [j]);
				}
			}
		}
		setTaskList (tl.ToArray());
	}

	public void setCurrentTask (TaskNote tn) {
		if (currentTask != null) {
			currentTask.setCurrent (false);
		}
		currentTask = tn;
		currentTask.setCurrent ();
	}

	private void setTaskList (Task[] tasks) {
		TaskNote [] taskNotes = tasksLayout.GetComponentsInChildren<TaskNote> ();
		for (int i = 0; i < taskNotes.Length; i++) {
			DestroyObject (taskNotes [i].gameObject);
		}

		for (int i = 0; i < tasks.Length; i++) {
			TaskNote taskNote = (TaskNote)Instantiate (taskNotePrefab, tasksLayout);
			taskNote.transform.localScale = new Vector3 (1, 1, 1);
			taskNote.setTask (tasks [i]);
		}
	}

	public void toHistory () {
		if (currentChild != null) {
			DataTransfer.Child = currentChild.getChild ();
			Application.LoadLevel (6);
		}
	}

	public void toMainMenu () {
		Application.LoadLevel (4);
	}
}
