using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenTaskWindowModel : MonoBehaviour {

	[SerializeField]
	private TaskNote taskNote;

	void Awake () {
		//FileWorker.readLevelFromFileForNote ("Components\\Levels\\test.cglvl", ref taskNote);
		Task t = DBWorker.loadTask (1);
		taskNote.setTask (t);
	}

}
