using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {

	[SerializeField]
	private AudioClip bubbles;
	[SerializeField]
	private AudioClip victory;

	//private AudioSource audioSource;

	void Awake () {
		//audioSource = GetComponent <AudioSource> ();
	}

	public void playBubblesSound () {
		AudioSource.PlayClipAtPoint(bubbles, transform.position); //Звук
	}

	public void playVictorySound () {
		AudioSource.PlayClipAtPoint(victory	, transform.position);
	}

}
