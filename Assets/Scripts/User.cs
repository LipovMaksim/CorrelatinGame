using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User{

	private int id; 
	public int Id {get { return id;}}
	private string login; 
	public string Login { get { return login;} set { login = value;}} 
	private string password; 
	public string Password { get { return password;} set { password = value;}}
	private string fio; 
	public string FIO { get { return fio;} set { fio = value;}}
	private string birthDate;
	public string BirthDate { get { return birthDate;} set { birthDate = value;}}
	private bool isTeacher;
	public bool IsTeacher { get { return isTeacher; } set { isTeacher = value; } }

	private User () {

	}

	public User (bool isT, int id, string login, string pwd, string fio, string birthDate = "") {
		isTeacher = isT;
		this.id = id;
		this.login = login;
		password = pwd;
		this.fio = fio;
		this.birthDate = birthDate;
	}
}
