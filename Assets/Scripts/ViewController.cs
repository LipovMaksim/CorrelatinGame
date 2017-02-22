using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewController : MonoBehaviour {

	[SerializeField]
	private Transform cangratulationsObj;

	public void showCangratulations(){
		cangratulationsObj.gameObject.SetActive (true);
	}

	public void hideCangratulations(){
		cangratulationsObj.gameObject.SetActive (false);
	}
}
