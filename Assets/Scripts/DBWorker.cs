using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Data;
using Mono.Data.Sqlite;


public class DBWorker {

	static string DBUrl = "URI=file:" + Application.dataPath + "/StreamingAssets/db.bytes"; // Android: "jar:file://" + Application.dataPath + "!/assets/db.bytes"

	public static Task loadTask (int taskId) {
		using (IDbConnection dbcon = (IDbConnection) new SqliteConnection(DBUrl)) {
			Task task;
			dbcon.Open ();
			using (IDbCommand dbcmd = dbcon.CreateCommand()) {
				dbcmd.CommandText = SELECT_TASK.Replace("{task_id}", "" + taskId);
				using (IDataReader reader = dbcmd.ExecuteReader()) {
					byte [] buf = (byte[]) reader["img"];
					task = new Task ((string) reader ["title"], (string) reader ["description"], loadImg(buf), taskId);
				}
				task.setGamePictures(loadPictures(dbcmd, taskId));
			}
			dbcon.Close ();
			return task;
		}
		return null;
	}

	public static GamePictureInfo[] loadPictures (IDbCommand dbcmd, int taskId) {
		dbcmd.CommandText = SELECT_PICTURES.Replace("{task_id}", "" + taskId);
		using (IDataReader reader = dbcmd.ExecuteReader()) {
			GamePictureInfo[] gpis = new GamePictureInfo[Task.PICTURES_SIZE];
			for (int i = 0; i < Task.PICTURES_SIZE && reader.Read (); i++) {
				byte[] buf = (byte[])reader ["img"];
				Texture2D tex = loadImg (buf);
				Vector2 position = new Vector2 ((float)reader ["x"], (float)reader ["y"]);
				float size = (float)reader ["size"];
				float angle = (float)reader ["angle"];
				bool flipX = reader ["flip_x"].ToString() == "1";
				bool flipY = reader ["flip_y"].ToString() == "1";
				gpis [i] = new GamePictureInfo (tex, position, size, angle, flipX, flipY);
			}
			return gpis;
		}
		return null;
	}

	public static Texture2D loadImg (byte [] buf) {
		Texture2D tex = new Texture2D (5, 5, TextureFormat.DXT1, false);
		tex.LoadImage (buf);
		return tex;
	}

	//НЕ ПРОВЕРЕН
	public static void saveTask (Task task) {
		using (IDbConnection dbcon = (IDbConnection) new SqliteConnection(DBUrl)) {
			dbcon.Open ();
			using (IDbCommand dbcmd = dbcon.CreateCommand()) {
				if (task.DBId < 0) {
					dbcmd.CommandText = INSERT_TASK.Replace("{background_id}", "" + task.BackgroundId).Replace("{title}", task.Name).Replace("{description}", task.Description);
					IDataReader reader = dbcmd.ExecuteReader ();
				}
			}
			dbcon.Close ();
		}
	}

	public static Task[] loadAllTasks() {
		using (IDbConnection dbcon = (IDbConnection) new SqliteConnection(DBUrl)) {
			List <Task> tasks = new List<Task>();
			Task[] res;
			dbcon.Open ();
			using (IDbCommand dbcmd = dbcon.CreateCommand()) {
				dbcmd.CommandText = SELECT_ALL_TASKS;
				using (IDataReader reader = dbcmd.ExecuteReader()) {
					while (reader.Read ()) {
						byte [] buf = (byte[]) reader["img"];
						tasks.Add (new Task ((string) reader ["title"], (string) reader ["description"], loadImg(buf), int.Parse (reader.GetValue (0).ToString())));
					}
				}
				res = tasks.ToArray ();
				for (int i = 0; i < res.Length; i++){
					res[i].setGamePictures(loadPictures(dbcmd, res[i].DBId));
				}
			}
			dbcon.Close ();
			return res;
		}
		return null;
	}

	const string SELECT_TASK = "SELECT img, title, description FROM Backgrounds, Tasks WHERE Backgrounds.id = (SELECT background_id FROM Tasks WHERE id = {task_id}) and Tasks.id = {task_id}";
	const string SELECT_PICTURES = "SELECT size, angle, x, y, flip_x, flip_y, img FROM Game_pictures, Pictures WHERE Game_pictures.task_id = {task_id} and Pictures.id = Game_pictures.picture_id";
	const string SELECT_TASK_ID = "SELECT id FROM Tasks WHERE background_id = {background_id} and title = '{title}' and description = '{description}'";
	const string INSERT_TASK = "INSERT INTO Tasks (background_id, title, description) VALUES ({background_id}, \'{title}\', \'{description}\')";
	const string INSERT_GAME_PICTURE = "INSERT INTO Game_pictures (picture_id, size, angle, x, y, flip_x, flip_y, task_id) VALUES ({picture_id}, {size}, {angle}, {x}, {y}, {flip_x}, {flip_y}, {task_id})";
	const string SELECT_ALL_TASKS = "SELECT Tasks.id, img, title, description FROM Backgrounds, Tasks WHERE Backgrounds.id = Tasks.background_id";
}
