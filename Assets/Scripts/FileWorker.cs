using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FileWorker {

	static public Texture2D readImage(string url){
		Texture2D tex = new Texture2D (5, 5, TextureFormat.DXT1, false);
		WWW www = new WWW ("file:///" + url);
		www.LoadImageIntoTexture (tex);
		return tex;
	}

	static public bool writeLevelInFile(string path, GameField field, GamePicture[] pictures, string name, string description){
		bool res = false;
		StreamWriter file = new StreamWriter (path, false);
		file.WriteLine (name);
		file.WriteLine (description);
		file.WriteLine (field.url);
		for (int i = 0; i < pictures.Length; i++) {
			if (pictures [i].isActive () && field.contanes (pictures [i])) {
				file.WriteLine (pictures [i].url);
				file.WriteLine (pictures [i].getSize());
				file.WriteLine (pictures [i].getRotationAngle());
				file.WriteLine (pictures [i].transform.position.x - field.transform.position.x);
				file.WriteLine (pictures [i].transform.position.y - field.transform.position.y);
				file.WriteLine ((pictures [i].getFlipX() ? 1 : 0));
				file.WriteLine ((pictures [i].getFlipY() ? 1 : 0));
				res = true;
			}
		}
		file.Close ();
		return res;
	}

	/*
	static public void readLevelFromFileForEdition(string path, ref GameField field, ref GamePicture[] pictures, ref string name, ref string description){
		StreamReader file = new StreamReader (path);
		name = file.ReadLine ();
		description = file.ReadLine ();
		field.setBackgroundImg(file.ReadLine ());
		for (int i = 0; i < pictures.Length; i++) {
			if (file.Peek () != -1) {
				pictures [i].initiatePicture (file.ReadLine ());
				pictures [i].setSize (System.Convert.ToSingle (file.ReadLine ()));
				pictures [i].setRotation (System.Convert.ToSingle (file.ReadLine ())); 
				float x = System.Convert.ToSingle (file.ReadLine ());
				float y = System.Convert.ToSingle (file.ReadLine ());
				pictures [i].transform.position = new Vector3 (x, y, pictures [i].transform.position.z);
				bool flipX = System.Convert.ToBoolean (System.Convert.ToSingle (file.ReadLine ()));
				bool flipY = System.Convert.ToBoolean (System.Convert.ToSingle (file.ReadLine ()));
				pictures [i].setFlipX (flipX);
				pictures [i].setFlipY (flipY);
			} else { 
				pictures [i].reset ();
			}
		}
		file.Close ();
	}*/
	/*
	static public void readLevelFromFileForGame(string path, ref GameField field, ref GamePicture[] pictures, ref string name, ref string description){
		StreamReader file = new StreamReader (path);
		name = file.ReadLine ();
		description = file.ReadLine ();
		field.setBackgroundImg(file.ReadLine ());
		for (int i = 0; i < pictures.Length && file.Peek() != -1; i++) {
			string url = file.ReadLine ();
			pictures[i].initiatePicture(url);
			float size = System.Convert.ToSingle (file.ReadLine ());
			float angle = System.Convert.ToSingle (file.ReadLine ());
			pictures [i].setSize (size);
			pictures [i].setRotation (angle); 
			float x = System.Convert.ToSingle(file.ReadLine ()) + field.transform.position.x;
			float y = System.Convert.ToSingle(file.ReadLine ()) + field.transform.position.y;
			//pictures [i].setTargetPosition (new Vector3 (x, y, pictures [i].transform.position.z));
			bool flipX = System.Convert.ToBoolean (System.Convert.ToSingle (file.ReadLine ()));
			bool flipY = System.Convert.ToBoolean (System.Convert.ToSingle (file.ReadLine ()));
			pictures [i].setFlipX (flipX);
			pictures [i].setFlipY (flipY);
			pictures [i].setTarget(field.createPicture (url, new Vector3 (x, y, -1), size, angle, flipX, flipY));
		}
		file.Close ();
	}*/

}
