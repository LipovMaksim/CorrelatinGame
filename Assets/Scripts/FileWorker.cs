using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileWorker {

	static public Texture2D readImage(string url){
		Texture2D tex = new Texture2D (5, 5, TextureFormat.DXT1, false);
		WWW www = new WWW ("file:///" + url);
		www.LoadImageIntoTexture (tex);
		return tex;
	}
}
