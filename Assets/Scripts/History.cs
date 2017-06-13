using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class History {

	private int childId;
	public int ChildId { get { return childId; } set { childId = value; } }
	private string taskTitle;
	public string TaskTitle { get { return taskTitle; } set { taskTitle = value; } }
	private int time;
	public int Time { get { return time; } set { time = value; } }
	private int misstakes;
	public int Misstakes { get { return misstakes; } set { misstakes = value; } }
	private string date;
	public string Date { get { return date; } set { date = value; } }

	public History (int _childId, string _taskTitle, int _time, int _misstakes, string _date) {
		childId = _childId;
		taskTitle = _taskTitle;
		time = _time;
		misstakes = _misstakes;
		date = _date;
	}
}
