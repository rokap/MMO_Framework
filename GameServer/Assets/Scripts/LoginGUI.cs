using UnityEngine;
using System.Collections;

public class LoginGUI : MonoBehaviour {
	public DatabaseHandler _mysqlHolder;

	void OnGUI(){
		GUILayout.Label ("MYSQL Connection state："+_mysqlHolder.GetConnectionState ());
		GUILayout.Label ("GetShops:\n"+_mysqlHolder.GetShops());
	}
}
