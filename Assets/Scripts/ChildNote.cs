using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChildNote : MonoBehaviour {

	[SerializeField]
	private Text name;
	[SerializeField]
	private Text birthDate;

	[SerializeField]
	Image background;
	[SerializeField]
	private Color trueColor;
	[SerializeField]
	private Color currentColor;

	private User child;

	public void setChild (User user) {
		if (!user.IsTeacher) {
			child = user;
			name.text = user.FIO;
			birthDate.text = user.BirthDate;
		}
	}

	public User getChild () {
		return child;
	}

	public void setCurrent (bool cur = true) {
		background.color = (cur ? currentColor : trueColor);
	}
}
