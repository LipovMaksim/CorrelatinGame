using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameField : MonoBehaviour {

	[SerializeField]
	private GamePicture gamePicturePrefab;
	[SerializeField]
	private Color pictureShadowColor;
	[SerializeField]
	private Color pictureHalfColor;

	public static float wUnitsField = 10;
	public static float hUnitsField = 7;

	public string url = "";

	private List <GamePicture> gamePictures = new List <GamePicture> ();
	private SpriteRenderer spriteRenderer;
	private int backgroundId = -1;
	public int BackgroundId { get{return backgroundId;} set{backgroundId = value;}}

	void Awake (){
		spriteRenderer = GetComponentInChildren <SpriteRenderer> ();
	}
	public void setBackgroundImg (string path) {
		Texture2D newTex = FileWorker.readImage (path);
		if (newTex != null) {
			url = path;
			setBackgroundImg (newTex);
		}

	}

	public void setBackgroundImg (ImageData imgData) {
		setBackgroundImg (imgData.Texture);
		backgroundId = imgData.Id;
	}

	public void setBackgroundImg (Texture2D tex) {
		Sprite s;
		//Побольшей стороне
		if (false){
			s = Sprite.Create(tex, new Rect(0,0,tex.width,tex.height), new Vector2(0.5f, 0.5f), 
				(tex.width / wUnitsField > tex.height / hUnitsField ? tex.width / wUnitsField : tex.height / hUnitsField)); //Расчет количества пикселей в юните по большему отношению сторон к соответствующему количеству юнитов
		}
		//Заполнение
		s = Sprite.Create(tex, zapolnenie (tex.width,tex.height, wUnitsField, hUnitsField), new Vector2(0.5f, 0.5f),
			(tex.width / wUnitsField < tex.height / hUnitsField ? tex.width / wUnitsField : tex.height / hUnitsField));
		if (spriteRenderer == null)
			spriteRenderer = GetComponentInChildren <SpriteRenderer> ();
		if (spriteRenderer.sprite != null)
			Destroy (spriteRenderer.sprite);
		spriteRenderer.sprite = s;
	}

	private Rect zapolnenie(int w, int h, float wu, float hu){
		int ppuw = (int) (w / wu);
		int ppuh = (int) (h / hu);
		int newW = w;
		int newH = h;
		if (ppuh < ppuw) {
			newW = (int) (ppuh * wu);
		} else {
			newH = (int) (ppuw * hu);
		}
		return new Rect ((w - newW) / 2, (h - newH) / 2, newW, newH);
	}

	public bool contanes (GamePicture gp){
		return (transform.position.x - wUnitsField / 2 <= gp.transform.position.x - gp.wUnitsPicture / 2)
			&& (transform.position.x + wUnitsField / 2 >= gp.transform.position.x + gp.wUnitsPicture / 2)
			&& (transform.position.y - hUnitsField / 2 <= gp.transform.position.y - gp.hUnitsPicture / 2)
			&& (transform.position.y + hUnitsField / 2 >= gp.transform.position.y + gp.hUnitsPicture / 2);
	}

	/*
	public GamePicture createPicture (string url, Vector3 position, float size, float angle, bool flipX = false, bool flipY = false, bool shadow = true) {
		GamePicture gp = Instantiate (gamePicturePrefab, position, transform.rotation, transform) as GamePicture;
		gamePictures.Add (gp);
		gp.setPicture (url);
		gp.setSize (size);
		gp.setRotation (angle);
		gp.setFlipX (flipX);
		gp.setFlipY (flipY);
		if (shadow)
			gp.setSpriteColor (pictureShadowColor);
		return gp;
	}*/

	public GamePicture createPicture (GamePictureInfo gpi, int type = 2) {
		GamePicture gp = Instantiate (gamePicturePrefab, new Vector3 (gpi.Position.x, gpi.Position.y, -1f), transform.rotation, transform) as GamePicture;
		gp.transform.localPosition = new Vector3 (gpi.Position.x, gpi.Position.y, -1);
		gamePictures.Add (gp);
		gp.setPicture(gpi);
		gp.setSize (gpi.Size);
		gp.setRotation (gpi.Angle);
		gp.setFlipX (gpi.FlipX);
		gp.setFlipY (gpi.FlipY);
		if (type == 1) {
			gp.setSpriteColor (pictureShadowColor);
		} else if (type == 0) {
			gp.setSpriteColor (pictureHalfColor);
		}
		gp.activate (false);
		return gp;
	}

	public float getScale () {
		return transform.localScale.x;
	}

	public void setTask (Task task) {
		setBackgroundImg (task.BackgroundImg);
		GamePictureInfo[] gps = task.getGamePictures ();
		for (int i = 0; i < gps.Length && gps [i] != null; i++) {
			createPicture (gps [i], task.Type);
		}
	}

	public void setTrueColorTo (GamePictureInfo gpi) {
		foreach (GamePicture gp in gamePictures) {
			if (gp.GamePictureInfo == gpi) {
				gp.setTrueColor();
			}
		}
	}

	public GamePicture onTargetPositon (GamePicture gp, float esp = 1f) {
		foreach (GamePicture gamePicture in gamePictures) {
			float a = Mathf.Abs (gamePicture.transform.position.x - gp.transform.position.x);
			float b = Mathf.Abs (gamePicture.transform.position.y - gp.transform.position.y);
			if (Mathf.Sqrt (a * a + b * b) < esp) {
				if (gp.GamePictureInfo.equals (gamePicture.GamePictureInfo)) {
					return gamePicture;
				}
			}
		}
		return null;
	}
}
