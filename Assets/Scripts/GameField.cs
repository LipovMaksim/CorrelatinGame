using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameField : MonoBehaviour {

	[SerializeField]
	private GamePicture gamePicturePrefab;
	[SerializeField]
	private Color pictureShadowColor;

	public static float wUnitsField = 10;
	public static float hUnitsField = 7;

	public string url = "";

	private List <GamePicture> gamePictures = new List <GamePicture> ();
	private SpriteRenderer spriteRenderer;

	void Awake (){
		spriteRenderer = GetComponentInChildren <SpriteRenderer> ();
	}
	public void setBackgroundImg (string path) {
		Texture2D newTex = FileWorker.readImage (path);

		if (newTex != null){
			url = path;
			Sprite s;
			//Побольшей стороне
			if (false){
				s = Sprite.Create(newTex, new Rect(0,0,newTex.width,newTex.height), new Vector2(0.5f, 0.5f), 
					(newTex.width / wUnitsField > newTex.height / hUnitsField ? newTex.width / wUnitsField : newTex.height / hUnitsField)); //Расчет количества пикселей в юните по большему отношению сторон к соответствующему количеству юнитов
			}
			//Заполнение
			s = Sprite.Create(newTex, zapolnenie (newTex.width,newTex.height, wUnitsField, hUnitsField), new Vector2(0.5f, 0.5f),
				(newTex.width / wUnitsField < newTex.height / hUnitsField ? newTex.width / wUnitsField : newTex.height / hUnitsField));
			if (spriteRenderer == null)
				spriteRenderer = GetComponentInChildren <SpriteRenderer> ();
			if (spriteRenderer.sprite != null)
				Destroy (spriteRenderer.sprite);
			spriteRenderer.sprite = s;
		}
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
	}

	public float getScale () {
		return transform.localScale.x;
	}
}
