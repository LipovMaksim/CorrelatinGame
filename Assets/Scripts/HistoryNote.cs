using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HistoryNote : MonoBehaviour {

	[SerializeField]
	private Text title;
	[SerializeField]
	private Text time;
	[SerializeField]
	private Text misstakes;
	[SerializeField]
	private Text date;

	public void setData (string _title, string _time, string _misstakes, string _date) {
		title.text = _title;
		time.text = _time;
		misstakes.text = _misstakes;
		date.text = _date;
	}

	public void setData (History h) {
		title.text = h.TaskTitle;
		time.text = h.Time + " сек.";
		misstakes.text = "" + h.Misstakes;
		date.text = h.Date;
	}
}
