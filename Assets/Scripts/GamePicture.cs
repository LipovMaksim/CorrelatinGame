using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePicture : DraggableObject {

	public void returnToPreviousPosition (){
		transform.position = previousPosition;
	}
}
