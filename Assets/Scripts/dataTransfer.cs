using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataTransfer : MonoBehaviour {

	static private Task task;
	static public Task Task {get { return task; } set {task = value;}}
	static private User currentUser;
	static public User CurrentUser {get { return currentUser; } set {currentUser = value;}}
	static private User child;
	static public User Child {get { return child; } set {child = value;}}

	void Awake () {
		DontDestroyOnLoad (this);
	}
}
