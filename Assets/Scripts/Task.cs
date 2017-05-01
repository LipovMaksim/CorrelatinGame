using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task {

	private int dbId;
	public int DBId { get { return dbId;} set { dbId = value;}}
	private string name;
	public string Name { get { return name;} set { name = value;}}
	private string description;
	public string Description { get { return description;} set { description = value;}}
	public const int PICTURES_SIZE = 7;
	private Texture2D backgroundImg;
	public Texture2D BackgroundImg { get { return backgroundImg;} set { backgroundImg = value;}}
	private int backgroundId;
	public int BackgroundId { get { return backgroundId; } set { backgroundId = value; } }
	private GamePictureInfo[] gamePictures = new GamePictureInfo[PICTURES_SIZE];

	public Task (string n, string d = "", Texture2D bi = null, int id = -1, int bgId = -1) {
		name = n;
		description = d;
		backgroundImg = bi;
		dbId = id;
		backgroundId = bgId;
	}

	public Task (string n, string d, int bgId, int id = -1) {
		name = n;
		description = d;
		backgroundId = bgId;
		dbId = id;
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

	public void setGamePictures (GamePictureInfo[] gpis) {
		gamePictures = gpis;
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

	public ImageData getBackgroundData () {
		return new ImageData (backgroundImg, backgroundId);
	}

}

