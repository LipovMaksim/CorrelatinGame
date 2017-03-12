using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slider : MonoBehaviour {

	[SerializeField]
	private float x = 681;
	[SerializeField]
	private float y = 273;
	[SerializeField]
	private float w = 100;
	[SerializeField]
	private float leftVal = 0.3f;
	[SerializeField]
	private float rightVal = 1f;


	private float value = 1f;
	private float lastValue = 1f;
	private bool frize = false;

	void Awake () {
		value = rightVal;
		lastValue = rightVal;
	}
	void OnGUI () {
		value = GUI.HorizontalSlider (new Rect (x, y, w, 10), value, leftVal, rightVal);
	}

	void Update () {
		if (!frize && lastValue != value) {
			if (valueChanged != null)
				valueChanged (value);
			lastValue = value;
		}
	}

	public delegate void ValueChanged (float val);
	public ValueChanged valueChanged;

	public void setValue (float val) {
		frize = true;
		value = val;	
		lastValue = val;
		frize = false;
	}
}
