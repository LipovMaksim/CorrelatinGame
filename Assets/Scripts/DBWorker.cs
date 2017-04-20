using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Data;
using Mono.Data.Sqlite;


public class DBWorker {

	static string DBUrl = "URI=file:" + Application.dataPath + "/StreamingAssets/db.bytes"; // Android: "jar:file://" + Application.dataPath + "!/assets/db.bytes"

	public static Task readTask (int taskId) {
		using (IDbConnection dbcon = (IDbConnection) new SqliteConnection(DBUrl)) {
			Task task;
			dbcon.Open ();
			using (IDbCommand dbcmd = dbcon.CreateCommand()) {
				dbcmd.CommandText = SELECT_BACKGROUND.Replace("{task_id}", "" + taskId);
				using (IDataReader reader = dbcmd.ExecuteReader()) {
					byte [] buf = (byte[]) reader["img"];
					task = new Task ((string) reader ["title"], (string) reader ["description"], loadImg(buf), taskId);
				}

				dbcmd.CommandText = SELECT_PICTURES.Replace("{task_id}", "" + taskId);
				using (IDataReader reader = dbcmd.ExecuteReader()) {
					while (reader.Read ()) {
						byte[] buf = (byte[])reader ["img"];
						Texture2D tex = loadImg (buf);
						Vector2 position = new Vector2 ((float)reader ["x"], (float)reader ["y"]);
						float size = (float)reader ["size"];
						float angle = (float)reader ["angle"];
						bool flipX = reader ["flip_x"].ToString() == "1";
						bool flipY = reader ["flip_y"].ToString() == "1";
						task.addGamePicture (new GamePictureInfo (tex, position, size, angle, flipX, flipY));
					}
				}


			}
			dbcon.Close ();
			return task;
		}
		return null;
	}

	public static Texture2D loadImg (byte [] buf) {
		Texture2D tex = new Texture2D (5, 5, TextureFormat.DXT1, false);
		tex.LoadImage (buf);
		return tex;
	}

	const string SELECT_BACKGROUND = "SELECT img, title, description FROM Backgrounds, Tasks WHERE Backgrounds.id = (SELECT background_id FROM Tasks WHERE id = {task_id}) and Tasks.id = {task_id}";
	const string SELECT_PICTURES = "SELECT size, angle, x, y, flip_x, flip_y, img FROM Game_pictures, Pictures WHERE Game_pictures.task_id = {task_id} and Pictures.id = Game_pictures.picture_id";
}
