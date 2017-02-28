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
				file.WriteLine (pictures [i].hUnitsPicture);
				file.WriteLine (pictures [i].wUnitsPicture);
				file.WriteLine (pictures [i].transform.position.x);
				file.WriteLine (pictures [i].transform.position.y);
				res = true;
			}
		}
		file.Close ();
		return res;
	}

	static public void readLevelFromFileForEdition(string path, ref GameField field, ref GamePicture[] pictures, ref string name, ref string description){
		StreamReader file = new StreamReader (path);
		name = file.ReadLine ();
		description = file.ReadLine ();
		field.setBackgroundImg(file.ReadLine ());
		for (int i = 0; i < pictures.Length && file.Peek() != -1; i++) {
			pictures[i].initiatePicture(file.ReadLine ());
			pictures [i].hUnitsPicture = System.Convert.ToSingle(file.ReadLine ()); //Пока не реализовано
			pictures [i].wUnitsPicture = System.Convert.ToSingle(file.ReadLine ()); //Пока не реализовано
			float x = System.Convert.ToSingle(file.ReadLine ());
			float y = System.Convert.ToSingle(file.ReadLine ());
			pictures [i].transform.position = new Vector3 (x, y, pictures [i].transform.position.z);
		}
		file.Close ();
	}

	static public void readLevelFromFileForGame(string path, ref GameField field, ref GamePicture[] pictures, ref string name, ref string description){
		StreamReader file = new StreamReader (path);
		name = file.ReadLine ();
		description = file.ReadLine ();
		field.setBackgroundImg(file.ReadLine ());
		for (int i = 0; i < pictures.Length && file.Peek() != -1; i++) {
			string url = file.ReadLine ();
			pictures[i].initiatePicture(url);
			pictures [i].hUnitsPicture = System.Convert.ToSingle(file.ReadLine ()); //Пока не реализовано
			pictures [i].wUnitsPicture = System.Convert.ToSingle(file.ReadLine ()); //Пока не реализовано
			float x = System.Convert.ToSingle(file.ReadLine ());
			float y = System.Convert.ToSingle(file.ReadLine ());
			pictures [i].setTargetPosition (new Vector3 (x, y, pictures [i].transform.position.z));
			field.createPicture (url, new Vector3 (x, y, -1));
		}
		file.Close ();
	}

}
