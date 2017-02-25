using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {

	private Color forSprits = new Color(0.8f, 0.8f, 0.8f, 1f);
	private Color tmColor;
	private Color srColor;
	private TextMesh tm;
	private SpriteRenderer sr;
	void Awake () {
		tm = GetComponent<TextMesh> ();
		if (tm != null)
			tmColor = tm.color;

		sr = GetComponent <SpriteRenderer> ();
		if (sr != null) {
			srColor = sr.color;
		}
	}

	private void OnMouseEnter () {
		if (tm != null)
			tm.color = Color.gray;
		if (sr != null)
			sr.color = forSprits;
	}

	private void OnMouseExit () {
		if (tm != null)
			tm.color = tmColor;
		if (sr != null)
			sr.color = srColor;
	}

	private void OnMouseUp () {
		if (buttonPresed != null) {
			buttonPresed ();
		}
	}
	public delegate void ButtonPresed ();
	public ButtonPresed buttonPresed;
}
