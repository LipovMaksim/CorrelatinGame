using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskNote : MonoBehaviour {
	[SerializeField]
	public GameField icon;
	[SerializeField]
	private TextMesh titleMesh;
	[SerializeField]
	private TextMesh objectsCountMesh;
	[SerializeField]
	private TextMesh descriptionMesh;

	public const float iconW = 2f;
	public const float iconH = 1.4f;
	public const string objectCountString = "Количество элементов: ";

	public void setIcon (Texture2D ic) {
		
	}

	public void setTitle (string title) {
		titleMesh.text = title;
	}

	public void setObjectsCount (int c) {
		objectsCountMesh.text = objectCountString + c;
	}

	public void setDescription (string description) {
		descriptionMesh.text = description;
	}
}
