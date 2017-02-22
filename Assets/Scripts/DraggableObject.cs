using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableObject : MonoBehaviour {

	private bool dragging = false;
	protected Vector3 previousPosition;
	private Vector3 mousePositionOnObject;

	void OnMouseDown()
	{
		previousPosition = transform.position;
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePosition.z = 0;
		mousePositionOnObject = mousePosition - previousPosition;
		dragging = true;
		if (draggingStart != null)
			draggingStart (this);
	}
	public delegate void DraggingStartEvent (DraggableObject obj);
	public DraggingStartEvent draggingStart;

	void OnMouseUp()
	{
		dragging = false;
		if (draggingEnd != null)
			draggingEnd (this, transform.position);
	}
	public delegate void DraggingEndEvent (DraggableObject obj, Vector3 position);
	public DraggingEndEvent draggingEnd;

	void Update () {
		if (dragging)
		{
			Vector3 mousePoint = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			mousePoint.z = 0;
			transform.position = mousePoint - mousePositionOnObject;
		}
	}
}
