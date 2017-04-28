using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenTaskWindowModel : MonoBehaviour {

	[SerializeField]
	private TaskNote taskNotePrefab;
	[SerializeField]
	private Transform tasksLayout;

	void Awake () {
		//FileWorker.readLevelFromFileForNote ("Components\\Levels\\test.cglvl", ref taskNote);
		Task[] tasks = DBWorker.loadAllTasks ();
		for (int i = 0; i < tasks.Length; i++) {
			TaskNote taskNote = (TaskNote)Instantiate (taskNotePrefab, tasksLayout);
			taskNote.transform.localScale = new Vector3 (1, 1, 1);
			taskNote.setTask (tasks [i]);
		}
	}

}
