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

	private User () {

	}

	public User (int id, string login, string pwd, string fio, string birthDate = "") {
		this.id = id;
		this.login = login;
		password = pwd;
		this.fio = fio;
		this.birthDate = birthDate;
	}
}
