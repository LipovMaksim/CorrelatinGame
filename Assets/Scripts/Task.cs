using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task {

	private int DBId;
	private string name;
	public string Name { get { return name;} set { name = value;}}
	private string description;
	public string Description { get { return description;} set { description = value;}}
	const int PICTURES_SIZE = 7;
	private Texture2D backgroundImg;
	public Texture2D BackgroundImg { get { return backgroundImg;} set { backgroundImg = value;}}
	private GamePictureInfo[] gamePictures = new GamePictureInfo[PICTURES_SIZE];

	public Task (string n, string d = "", Texture2D bi = null, int id = -1) {
		name = n;
		description = d;
		backgroundImg = bi;
		DBId = id;
	}

	public bool addGamePicture (GamePictureInfo gpi) {
		for (int i = 0; i < PICTURES_SIZE; i++) {
			if (gamePictures [i] == null) {
				gamePictures [i] = gpi;
				return true;
			}
		}
		return false;
	}

	public GamePictureInfo[] getGamePictures () {
		return gamePictures;
	}

	public bool resetGamePicture (int i) {
		if (i >= PICTURES_SIZE || i < 0)
			return false;

		gamePictures [i] = null;
		return true;
	}

	public void resetGamePictures () {
		for (int i = 0; i < PICTURES_SIZE; i++) {
			gamePictures [i] = null;
		}
	}

}

public class Pair <T, U> {
	private T firstVal;
	private U secondVal;

	public Pair (){
		firstVal = default(T);
		secondVal = default(U);
	}

	public Pair (T f, U s){
		firstVal = f;
		secondVal = s;
	}

	public void setFirst(T f) {
		firstVal = f;
	}

	public void setSecond(U s) {
		secondVal = s;
	}

	public T first() {
		return firstVal;
	}

	public U second() {
		return secondVal;
	}
}
