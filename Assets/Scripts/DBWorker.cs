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
				gpis [i] = new GamePictureInfo ( int.Parse (reader.GetValue (0).ToString()),  int.Parse (reader.GetValue (1).ToString()), tex, position, size, angle, flipX, flipY);
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


	public static int saveTask (Task task) {
		int id = -1;
		using (IDbConnection dbcon = (IDbConnection) new SqliteConnection(DBUrl)) {
			dbcon.Open ();
			using (IDbCommand dbcmd = dbcon.CreateCommand()) {
				{
					dbcmd.CommandText = INSERT_TASK.Replace("{background_id}", "" + task.BackgroundId).Replace("{title}", task.Name).Replace("{description}", task.Description);
					dbcmd.ExecuteNonQuery();

					id = getLastId (dbcmd, "Tasks");

					GamePictureInfo[] gpis = task.getGamePictures ();
					for (int i = 0; i < gpis.Length; i++) {
						addPictureToTask (dbcmd, id, gpis [i]);
					}

					if (task.DBId >= 0) {
						removeTask (task.DBId);
					}
				}
			}
			dbcon.Close ();
		}
		return id;
	}

	private static void addPictureToTask (IDbCommand dbcmd, int taskId, GamePictureInfo gpi) {
		dbcmd.CommandText = INSERT_GAME_PICTURE.Replace("{picture_id}", "" + gpi.PictureId).Replace("{size}", "" + gpi.Size).Replace("{angle}", "" + gpi.Angle)
			.Replace("{x}", "" + gpi.Position.x).Replace("{y}", "" + gpi.Position.y).Replace("{flip_x}", "" + gpi.FlipX_int).Replace("{flip_y}", "" + gpi.FlipY_int).Replace("{task_id}", "" + taskId);
		dbcmd.ExecuteNonQuery();
	}

	public static Task loadLastAddedTask () {
		int id = -1;
		using (IDbConnection dbcon = (IDbConnection)new SqliteConnection (DBUrl)) {
			dbcon.Open ();
			using (IDbCommand dbcmd = dbcon.CreateCommand ()) {
				dbcmd.CommandText = SELECT_LAST_ID.Replace ("{table_name}", "Tasks");

				using (IDataReader reader = dbcmd.ExecuteReader ()) {
					id = int.Parse (reader.GetValue (0).ToString ());
				}
			}
			dbcon.Close ();
		}
		return loadTask (id);
	}

	private static int getLastId (IDbCommand dbcmd, string tableName) {
		dbcmd.CommandText = SELECT_LAST_ID.Replace("{table_name}", tableName);
		using (IDataReader reader = dbcmd.ExecuteReader ()) {
			return int.Parse (reader.GetValue (0).ToString ());
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
						tasks.Add (new Task ((string) reader ["title"], (string) reader ["description"], loadImg(buf), int.Parse (reader.GetValue (0).ToString()), int.Parse (reader.GetValue (1).ToString())));
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

	public static Pair<int, Texture2D>[] loadAllPictures() {
		using (IDbConnection dbcon = (IDbConnection) new SqliteConnection(DBUrl)) {
			List <Pair<int, Texture2D>> pictures = new List<Pair<int, Texture2D>>();
			dbcon.Open ();
			using (IDbCommand dbcmd = dbcon.CreateCommand()) {
				dbcmd.CommandText = SELECT_ALL_PICTURES;
				using (IDataReader reader = dbcmd.ExecuteReader()) {
					while (reader.Read ()) {
						byte [] buf = (byte[]) reader["img"];
						pictures.Add (new Pair <int, Texture2D> (int.Parse (reader.GetValue (0).ToString()), loadImg(buf)));
					}
				}
			}
			dbcon.Close ();
			return pictures.ToArray();
		}
		return null;
	}

	public static Pair<int, Texture2D>[] loadAllBackgrounds() {
		using (IDbConnection dbcon = (IDbConnection) new SqliteConnection(DBUrl)) {
			List <Pair<int, Texture2D>> pictures = new List<Pair<int, Texture2D>>();
			dbcon.Open ();
			using (IDbCommand dbcmd = dbcon.CreateCommand()) {
				dbcmd.CommandText = SELECT_ALL_BACKGROUDS;
				using (IDataReader reader = dbcmd.ExecuteReader()) {
					while (reader.Read ()) {
						byte [] buf = (byte[]) reader["img"];
						pictures.Add (new Pair <int, Texture2D> (int.Parse (reader.GetValue (0).ToString()), loadImg(buf)));
					}
				}
			}
			dbcon.Close ();
			return pictures.ToArray();
		}
		return null;
	}

	public static void removeTask (Task task) {
		removeTask (task.DBId);
	}

	private static void removeTask (int id) {
		using (IDbConnection dbcon = (IDbConnection)new SqliteConnection (DBUrl)) {
			dbcon.Open ();
			using (IDbCommand dbcmd = dbcon.CreateCommand ()) {
				dbcmd.CommandText = DELETE_ROW_FROM_TABLE_BY_ID.Replace ("{table}", "Tasks").Replace ("{id}", "" + id);
				dbcmd.ExecuteNonQuery();

				dbcmd.CommandText = DELETE_GAMEPICTURES_BY_TASK_ID.Replace ("{task_id}", "" + id);
				dbcmd.ExecuteNonQuery();
			}

		}
	}

	const string SELECT_TASK = "SELECT img, title, description FROM Backgrounds, Tasks WHERE Backgrounds.id = (SELECT background_id FROM Tasks WHERE id = {task_id}) and Tasks.id = {task_id}";
	const string SELECT_PICTURES = "SELECT Game_pictures.id, picture_id, size, angle, x, y, flip_x, flip_y, img FROM Game_pictures, Pictures WHERE Game_pictures.task_id = {task_id} and Pictures.id = Game_pictures.picture_id";
	const string SELECT_TASK_ID = "SELECT id FROM Tasks WHERE background_id = {background_id} and title = '{title}' and description = '{description}'";
	const string INSERT_TASK = "INSERT INTO Tasks (background_id, title, description) VALUES ({background_id}, \'{title}\', \'{description}\')";
	const string INSERT_GAME_PICTURE = "INSERT INTO Game_pictures (picture_id, size, angle, x, y, flip_x, flip_y, task_id) VALUES ({picture_id}, {size}, {angle}, {x}, {y}, {flip_x}, {flip_y}, {task_id})";
	const string SELECT_ALL_TASKS = "SELECT Tasks.id, Backgrounds.id, img, title, description FROM Backgrounds, Tasks WHERE Backgrounds.id = Tasks.background_id ORDER BY title";
	const string SELECT_ALL_PICTURES = "SELECT id, img FROM Pictures";
	const string SELECT_ALL_BACKGROUDS = "SELECT id, img FROM Backgrounds";
	const string SELECT_LAST_ID = "SELECT last_insert_rowid() FROM {table_name}";
	const string DELETE_ROW_FROM_TABLE_BY_ID = "DELETE FROM {table} WHERE id = {id}";
	const string DELETE_GAMEPICTURES_BY_TASK_ID = "DELETE FROM Game_pictures WHERE task_id = {task_id}";
}

public class Pair <T, U> {
	private T firstVal;
	public T first {get { return firstVal; } set {firstVal = value;}}
	private U secondVal;
	public U second {get { return secondVal; } set {secondVal = value;}}

	public Pair (){
		firstVal = default(T);
		secondVal = default(U);
	}

	public Pair (T f, U s){
		firstVal = f;
		secondVal = s;
	}
}
