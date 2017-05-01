using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenTaskWindowModel : MonoBehaviour {

	[SerializeField]
	private TaskNote currentTaskView;
	[SerializeField]
	private TaskNote taskNotePrefab;
	[SerializeField]
	private Transform tasksLayout;

	private TaskNote currentTask;

	void Awake () {
		//FileWorker.readLevelFromFileForNote ("Components\\Levels\\test.cglvl", ref taskNote);
		Task[] tasks = DBWorker.loadAllTasks ();
		//currentTask.setTask (tasks [0]);
		for (int i = 0; i < tasks.Length; i++) {
			TaskNote taskNote = (TaskNote)Instantiate (taskNotePrefab, tasksLayout);
			taskNote.transform.localScale = new Vector3 (1, 1, 1);
			taskNote.setTask (tasks [i]);
			//taskNote.GetComponent <Button> ().buttonPresed += taskChoised;
		}
	}

	public void toMainMenu () {
		Application.LoadLevel(0);
	}

	public void openTaskEditor (bool createNewTask) {
		if (createNewTask) {
			DataTransfer.Task = null;
			Application.LoadLevel (2);
		} else {
			if (currentTask != null) {
				DataTransfer.Task = (createNewTask ? null : currentTask.getTask ());
				Application.LoadLevel (2);
			}
		}
	}

	public void openGame () {
		if (currentTask != null) {
			DataTransfer.Task = currentTask.getTask ();
			Application.LoadLevel (1);
		}

	}
		
	public void removeCurrentTask () {
		DBWorker.removeTask (currentTask.getTask ());
		Destroy (currentTask.gameObject);
		taskChoised (null);

	}

	public void taskChoised (TaskNote taskNote) {
		currentTaskView.setTask ( ( taskNote == null ? null : taskNote.getTask() ) );
		if (currentTask != null) {
			currentTask.setCurrent (false);
		}
		currentTask = taskNote;
		if (currentTask != null)
			currentTask.setCurrent ();
	}
}
