using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveTaskWindowModel : MonoBehaviour {

	[SerializeField]
	private TaskNote taskNote;

	void Awake () {
		FileWorker.readLevelFromFileForNote ("E:\\Google Drive\\Диплом\\CorrelatinGame\\Components\\Levels\\test.cglvl", ref taskNote);
	}

}
