using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using Mono.Data.Sqlite;
using UnityEngine;

public class DBWorker {
	/*
	static string DBUrl = ( Application.platform != RuntimePlatform.Android ? "URI=file:" + Application.dataPath + "/StreamingAssets/db.bytes" 
		: "jar:file://" + Application.dataPath + "!/assets/db.bytes" ); // Android: "jar:file://" + Application.dataPath + "!/assets/db.bytes"
*/
	static string DBUrl = ( Application.platform != RuntimePlatform.Android ? "URI=file:" + Application.dataPath + "/StreamingAssets/db.bytes" 
		: "jar:file://" + Application.dataPath + "!/assets/db.bytes" ); // Android: "jar:file://" + Application.dataPath + "!/assets/db.bytes"

	//static string DBUrl = "jar:file://" + Application.dataPath + "!/assets/db.bytes";

	private static IDbConnection dbcon;
	private static readonly string _dbName = "db.bytes";

	public static bool OpenDatabase()
	{
		if (dbcon == null)
		{
			string filePath = "";

			if (Application.platform == RuntimePlatform.WindowsEditor)
			{
				filePath = "file://" + Application.streamingAssetsPath + "/" + _dbName;
			}
			else if (Application.platform == RuntimePlatform.Android)
			{
				filePath = Application.persistentDataPath + "/" + _dbName;
				if (!File.Exists(filePath))
				{
					var streamFilePath = "jar:file://" + Application.dataPath + "!/assets/" + _dbName;
					WWW loadDB = new WWW(streamFilePath);
					while (!loadDB.isDone)
					{
					}
					File.WriteAllBytes(filePath, loadDB.bytes);
				}
			}
			else
			{
				Debug.LogError("You not supposed to be here...");
			}

			dbcon = new SqliteConnection("URI=" + filePath);
		}

		dbcon.Open();
		Debug.Log("Connection state: " + (dbcon.State == ConnectionState.Open ? "Open" : "Error"));
		return dbcon.State == ConnectionState.Open;
	}

	public static bool CloseDatabase()
	{
		dbcon.Close();
		return dbcon.State == ConnectionState.Closed;
	}

	public static Task loadTask (int taskId) {
		OpenDatabase ();
		Task task;
		using (IDbCommand dbcmd = dbcon.CreateCommand()) {
			dbcmd.CommandText = SELECT_TASK.Replace("{task_id}", "" + taskId);
			using (IDataReader reader = dbcmd.ExecuteReader()) {
				byte [] buf = (byte[]) reader["img"];
				task = new Task ((string) reader ["title"], (string) reader ["description"], loadImg(buf), taskId, int.Parse (reader.GetValue (3).ToString()));
			}
			task.setGamePictures(loadPictures(dbcmd, taskId));
		}
		CloseDatabase ();
		return task;
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
		OpenDatabase ();
		using (IDbCommand dbcmd = dbcon.CreateCommand()) {
			{
				dbcmd.CommandText = INSERT_TASK.Replace("{background_id}", "" + task.BackgroundId).Replace("{title}", task.Name).Replace("{description}", task.Description).Replace("{type}", "" + task.Type);
				dbcmd.ExecuteNonQuery();

				id = getLastId (dbcmd, "Tasks");

				GamePictureInfo[] gpis = task.getGamePictures ();
				for (int i = 0; i < gpis.Length; i++) {
					addPictureToTask (dbcmd, id, gpis [i]);
				}
			}
		}
		CloseDatabase ();

		if (task.DBId >= 0) {
			removeTask (task.DBId);
		}

		return id;
	}

	public static int saveChild (User child) {
		if (child.IsTeacher)
			return -1;

		int id = child.Id;
		if (id == -1) {
			OpenDatabase ();
			using (IDbCommand dbcmd = dbcon.CreateCommand ()) {
				{
					dbcmd.CommandText = INSERT_CHILD.Replace ("{fio}", child.FIO).Replace ("{login}", child.Login).Replace ("{password}", child.Password).Replace ("{birth_date}", child.BirthDate);
					dbcmd.ExecuteNonQuery ();

					id = getLastId (dbcmd, "Children");
				}
			}
			CloseDatabase ();
		} else {
			OpenDatabase ();
			using (IDbCommand dbcmd = dbcon.CreateCommand ()) {
				{
					dbcmd.CommandText = UPDATE_CHILD.Replace ("{fio}", child.FIO).Replace ("{login}", child.Login).Replace ("{password}", child.Password).Replace ("{birth_date}", child.BirthDate).Replace("{id}", "" + child.Id);
					dbcmd.ExecuteNonQuery ();
				}
			}
			CloseDatabase ();
		}
		return id;
	}

	public static int addCurrentTask (User child,  Task task) {
		if (child.IsTeacher || child.Id == -1 || task.DBId == -1)
			return -1;

		OpenDatabase ();
		using (IDbCommand dbcmd = dbcon.CreateCommand ()) {
			{
				dbcmd.CommandText = INSERT_CURRENT_TASK.Replace ("{child_id}", "" + child.Id).Replace ("{task_id}", "" + task.DBId);
				dbcmd.ExecuteNonQuery ();
			}
		}
		CloseDatabase ();
		return 1;
	}

	public static int addHistory (User child, History history) {
		if (child.IsTeacher || child.Id == -1)
			return -1;

		OpenDatabase ();
		using (IDbCommand dbcmd = dbcon.CreateCommand ()) {
			{
				dbcmd.CommandText = INSERT_HISTORY.Replace ("{child_id}", "" + child.Id).Replace ("{date}", history.Date).Replace ("{task_title}", history.TaskTitle)
					.Replace ("{misstake_number}", "" + history.Misstakes).Replace ("{time}", "" + history.Time);
				dbcmd.ExecuteNonQuery ();
			}
		}
		CloseDatabase ();
		return 1;
	}

	private static void addPictureToTask (IDbCommand dbcmd, int taskId, GamePictureInfo gpi) {
		dbcmd.CommandText = INSERT_GAME_PICTURE.Replace("{picture_id}", "" + gpi.PictureId).Replace("{size}", "" + gpi.Size).Replace("{angle}", "" + gpi.Angle)
			.Replace("{x}", "" + gpi.Position.x).Replace("{y}", "" + gpi.Position.y).Replace("{flip_x}", "" + gpi.FlipX_int).Replace("{flip_y}", "" + gpi.FlipY_int).Replace("{task_id}", "" + taskId);
		dbcmd.ExecuteNonQuery();
	}

	public static Task loadLastAddedTask () {
		int id = -1;
		OpenDatabase ();
		using (IDbCommand dbcmd = dbcon.CreateCommand ()) {
			dbcmd.CommandText = SELECT_LAST_ID.Replace ("{table_name}", "Tasks");

			using (IDataReader reader = dbcmd.ExecuteReader ()) {
				id = int.Parse (reader.GetValue (0).ToString ());
			}
		}
		CloseDatabase ();
		return loadTask (id);
	}

	private static int getLastId (IDbCommand dbcmd, string tableName) {
		dbcmd.CommandText = SELECT_LAST_ID.Replace("{table_name}", tableName);
		using (IDataReader reader = dbcmd.ExecuteReader ()) {
			return int.Parse (reader.GetValue (0).ToString ());
		}
	}

	public static Task[] loadAllTasks() {
		OpenDatabase();
		List <Task> tasks = new List<Task>();
		Task[] res;
		using (IDbCommand dbcmd = dbcon.CreateCommand()) {
			dbcmd.CommandText = SELECT_ALL_TASKS;
			using (IDataReader reader = dbcmd.ExecuteReader()) {
				while (reader.Read ()) {
					byte [] buf = (byte[]) reader["img"];
					tasks.Add (new Task ((string) reader ["title"], (string) reader ["description"], loadImg(buf), int.Parse (reader.GetValue (0).ToString()), int.Parse (reader.GetValue (1).ToString()), int.Parse (reader ["type"].ToString())));
				}
			}
			res = tasks.ToArray ();
			for (int i = 0; i < res.Length; i++){
				res[i].setGamePictures(loadPictures(dbcmd, res[i].DBId));
			}
		}
		CloseDatabase ();
		return res;
	}

	public static Pair<int, Texture2D>[] loadAllPictures() {
		OpenDatabase();
		List <Pair<int, Texture2D>> pictures = new List<Pair<int, Texture2D>>();
		using (IDbCommand dbcmd = dbcon.CreateCommand()) {
			dbcmd.CommandText = SELECT_ALL_PICTURES;
			using (IDataReader reader = dbcmd.ExecuteReader()) {
				while (reader.Read ()) {
					byte [] buf = (byte[]) reader["img"];
					pictures.Add (new Pair <int, Texture2D> (int.Parse (reader.GetValue (0).ToString()), loadImg(buf)));
				}
			}
		}
		CloseDatabase ();
		return pictures.ToArray();
	}

	public static Pair<int, Texture2D>[] loadAllBackgrounds() {
		OpenDatabase();
		List <Pair<int, Texture2D>> pictures = new List<Pair<int, Texture2D>>();
		using (IDbCommand dbcmd = dbcon.CreateCommand()) {
			dbcmd.CommandText = SELECT_ALL_BACKGROUDS;
			using (IDataReader reader = dbcmd.ExecuteReader()) {
				while (reader.Read ()) {
					byte [] buf = (byte[]) reader["img"];
					pictures.Add (new Pair <int, Texture2D> (int.Parse (reader.GetValue (0).ToString()), loadImg(buf)));
				}
			}
		}
		CloseDatabase ();
		return pictures.ToArray();
		return null;
	}

	public static void removeTask (Task task) {
		removeTask (task.DBId);
	}

	public static void removeChild (User child) {
		removeChild (child.Id);
	}

	private static void removeTask (int id) {
		OpenDatabase ();
		using (IDbCommand dbcmd = dbcon.CreateCommand ()) {
			dbcmd.CommandText = DELETE_ROW_FROM_TABLE_BY_ID.Replace ("{table}", "Tasks").Replace ("{id}", "" + id);
			dbcmd.ExecuteNonQuery();

			dbcmd.CommandText = DELETE_GAMEPICTURES_BY_TASK_ID.Replace ("{task_id}", "" + id);
			dbcmd.ExecuteNonQuery();
		}
		CloseDatabase ();
	}

	private static void removeChild (int id) {
		OpenDatabase ();
		using (IDbCommand dbcmd = dbcon.CreateCommand ()) {
			dbcmd.CommandText = DELETE_ROW_FROM_TABLE_BY_ID.Replace ("{table}", "Children").Replace ("{id}", "" + id);
			dbcmd.ExecuteNonQuery();

			//dbcmd.CommandText = DELETE_GAMEPICTURES_BY_TASK_ID.Replace ("{task_id}", "" + id);
			//dbcmd.ExecuteNonQuery();
		}
		CloseDatabase ();
	}

	public static void removeCurrentTask (User child, Task task) {
		OpenDatabase ();
		using (IDbCommand dbcmd = dbcon.CreateCommand ()) {
			dbcmd.CommandText = DELETE_CURRENT_TASK.Replace ("{child_id}", "" + child.Id).Replace ("{task_id}", "" + task.DBId);
			dbcmd.ExecuteNonQuery();
		}

		CloseDatabase ();
	}

	public static void removeHistory (User child) {
	OpenDatabase ();
		using (IDbCommand dbcmd = dbcon.CreateCommand ()) {
			dbcmd.CommandText = DELETE_HISTORY.Replace ("{child_id}", "" + child.Id);
			dbcmd.ExecuteNonQuery();
		}

		CloseDatabase ();
	}

	public static User getUser (string login, string pwd) {
		User res = null;
		OpenDatabase();
		using (IDbCommand dbcmd = dbcon.CreateCommand()) {
			dbcmd.CommandText = SELECT_TEACHER.Replace ("{login}",login).Replace ("{password}", pwd);
			using (IDataReader reader = dbcmd.ExecuteReader()) {
				if (reader.Read ()) {
					res = new User (true, int.Parse (reader.GetValue (0).ToString ()), login, pwd, (string)reader ["fio"]); 
				}
				//return null;
			}

			dbcmd.CommandText = SELECT_CHILD.Replace ("{login}",login).Replace ("{password}", pwd);
			using (IDataReader reader = dbcmd.ExecuteReader()) {
				if (reader.Read ()) {
					res = new User (false, int.Parse (reader.GetValue (0).ToString ()), login, pwd, (string)reader ["fio"], (string)reader ["birth_year"]); 
				}
			}

			if (res != null && !res.IsTeacher) {
				dbcmd.CommandText = SELECT_CURRENT_TASKS.Replace ("{child_id}", "" + res.Id);
				using (IDataReader reader = dbcmd.ExecuteReader()) {
					while (reader.Read ()) {
						res.currentTasks.Add(int.Parse (reader.GetValue (0).ToString ())); 
					}
				}
			}
		}
		CloseDatabase ();
		return res;
	}

	public static User[] loadChildren () {
		List <User> children = new List <User> ();
		User[] res = children.ToArray();
		OpenDatabase();
		using (IDbCommand dbcmd = dbcon.CreateCommand()) {
			dbcmd.CommandText = SELECT_CHILDREN;
			using (IDataReader reader = dbcmd.ExecuteReader()) {
				while (reader.Read ()) {
					User child = new User (false, int.Parse (reader.GetValue (0).ToString ()), (string)reader ["login"], (string)reader ["password"], (string)reader ["fio"], (string)reader ["birth_year"]);	
					children.Add (child);
				}
			}
			res = children.ToArray ();
			dbcmd.CommandText = SELECT_ALL_CURRENT_TASKS;
			using (IDataReader reader = dbcmd.ExecuteReader ()) {
				while (reader.Read ()) {
					int child_id = int.Parse (reader.GetValue (0).ToString ());
					int task_id = int.Parse (reader.GetValue (1).ToString ());
					bool added = false;
					for (int i = 0; i < res.Length && !added; i++) {
						if (res [i].Id == child_id) {
							res [i].currentTasks.Add (task_id);
							added = true;
						}
					}
				}
			}
		}
		CloseDatabase ();
		return res; 
	}

	public static History[] loadHistory (User child) {
		List <History> history = new List <History> ();
		OpenDatabase();
		using (IDbCommand dbcmd = dbcon.CreateCommand()) {
			dbcmd.CommandText = SELECT_HISTORY.Replace("{child_id}", "" + child.Id);
			using (IDataReader reader = dbcmd.ExecuteReader()) {
				while (reader.Read ()) {
					History h = new History(child.Id,(string)reader ["task_title"], int.Parse (reader.GetValue (0).ToString ()), int.Parse (reader.GetValue (1).ToString ()), (string)reader ["date"]);	
					history.Add (h);
				}
			}
		}
		CloseDatabase ();
		return history.ToArray(); 
	}

	public static bool loginConsists (string login, string curId = null) {
		bool res = false;
		OpenDatabase();
		using (IDbCommand dbcmd = dbcon.CreateCommand()) {
			if (curId == null) {
				dbcmd.CommandText = SELECT_LOGIN.Replace ("{login}", login).Replace ("{login}", login);
			} else {
				dbcmd.CommandText = SELECT_LOGIN_WITHOUT_ID.Replace ("{login}", login).Replace ("{login}", login).Replace ("{id}", curId);
			}
			using (IDataReader reader = dbcmd.ExecuteReader()) {
				if (reader.Read ()) {
					res = true;
				}
			}
		}
		CloseDatabase ();
		return res; 
	}

	const string SELECT_TASK = "SELECT img, title, description, type FROM Backgrounds, Tasks WHERE Backgrounds.id = (SELECT background_id FROM Tasks WHERE id = {task_id}) and Tasks.id = {task_id}";
	const string SELECT_PICTURES = "SELECT Game_pictures.id, picture_id, size, angle, x, y, flip_x, flip_y, img FROM Game_pictures, Pictures WHERE Game_pictures.task_id = {task_id} and Pictures.id = Game_pictures.picture_id";
	const string SELECT_TASK_ID = "SELECT id FROM Tasks WHERE background_id = {background_id} and title = '{title}' and description = '{description}'";
	const string INSERT_TASK = "INSERT INTO Tasks (background_id, title, description, type) VALUES ({background_id}, \'{title}\', \'{description}\', '{type}')";
	const string INSERT_GAME_PICTURE = "INSERT INTO Game_pictures (picture_id, size, angle, x, y, flip_x, flip_y, task_id) VALUES ({picture_id}, {size}, {angle}, {x}, {y}, {flip_x}, {flip_y}, {task_id})";
	const string SELECT_ALL_TASKS = "SELECT Tasks.id, Backgrounds.id, img, title, description, type FROM Backgrounds, Tasks WHERE Backgrounds.id = Tasks.background_id ORDER BY title";
	const string SELECT_ALL_PICTURES = "SELECT id, img FROM Pictures";
	const string SELECT_ALL_BACKGROUDS = "SELECT id, img FROM Backgrounds";
	const string SELECT_LAST_ID = "SELECT last_insert_rowid() FROM {table_name}";
	const string DELETE_ROW_FROM_TABLE_BY_ID = "DELETE FROM {table} WHERE id = {id}";
	const string DELETE_GAMEPICTURES_BY_TASK_ID = "DELETE FROM Game_pictures WHERE task_id = {task_id}";
	const string SELECT_TEACHER = "SELECT id, fio FROM Teachers WHERE login = \'{login}\' AND password = \'{password}\'";
	const string SELECT_CHILD = "SELECT id, fio, birth_year FROM Children WHERE login = \'{login}\' AND password = \'{password}\'";
	const string SELECT_CHILDREN = "SELECT id, fio, login, password, birth_year FROM Children";
	const string SELECT_ALL_CURRENT_TASKS = "SELECT child_id, task_id FROM Current_tasks";
	const string SELECT_CURRENT_TASKS = "SELECT task_id FROM Current_tasks WHERE child_id = {child_id}";
	const string INSERT_CHILD = "INSERT INTO Children (fio, login, password, birth_year) VALUES (\'{fio}\', \'{login}\', \'{password}\', \'{birth_date}\')";
	const string UPDATE_CHILD = "UPDATE Children SET fio = '{fio}', login = '{login}', password = '{password}', birth_year = '{birth_date}' WHERE id = \'{id}\'";
	const string INSERT_CURRENT_TASK = "INSERT INTO Current_tasks (child_id, task_id) VALUES (\'{child_id}\', \'{task_id}\')";
	const string DELETE_CURRENT_TASK = "DELETE FROM Current_tasks WHERE child_id = {child_id} AND task_id = {task_id}";
	const string SELECT_HISTORY = "SELECT time, misstake_number, task_title, date FROM Statistic WHERE child_id = \'{child_id}\'";
	const string INSERT_HISTORY = "INSERT INTO Statistic (task_title, time, misstake_number, date, child_id) VALUES (\'{task_title}\', {time}, {misstake_number}, \'{date}\', \'{child_id}\')";
	const string SELECT_LOGIN = "SELECT * FROM Children, Teachers WHERE Children.login = \'{login}\' OR Teachers.login = '{login}'";
	const string SELECT_LOGIN_WITHOUT_ID = "SELECT * FROM Children, Teachers WHERE (Children.login = \'{login}\' OR Teachers.login = '{login}') AND Children.id <> {id}";
	const string DELETE_HISTORY = "DELETE FROM Statistic WHERE child_id = {child_id}";
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
