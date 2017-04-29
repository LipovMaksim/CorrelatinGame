using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataTransfer : MonoBehaviour {

	static private Task task;
	static public Task Task {get { return task; } set {task = value;}}

	void Awake () {
		DontDestroyOnLoad (this);
	}
}
