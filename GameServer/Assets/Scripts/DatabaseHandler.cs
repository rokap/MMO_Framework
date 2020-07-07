using UnityEngine;
using System;
using System.Data;
using System.Text;

using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

using MySql.Data;
using MySql.Data.MySqlClient;

public class DatabaseHandler : MonoBehaviour
{
    public static DatabaseHandler instance;
    public string host, database, user, password;
    public bool pooling = true;

    private string connectionString;
    private MySqlConnection con = null;
    private MySqlCommand cmd = null;
    private MySqlDataReader rdr = null;

    private MD5 _md5Hash;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }

        DontDestroyOnLoad(this.gameObject);
        connectionString = "Server=" + host + ";Database=" + database + ";User=" + user + ";Password=" + password + ";Pooling=";
        if (pooling)
        {
            connectionString += "True";
        }
        else
        {
            connectionString += "False";
        }
        try
        {
            con = new MySqlConnection(connectionString);
            con.Open();
            Debug.Log("Mysql state: " + con.State);
     

        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    void onApplicationQuit()
    {
        if (con != null)
        {
            if (con.State.ToString() != "Closed")
            {
                con.Close();
                Debug.Log("Mysql connection closed");
            }
            con.Dispose();
        }
    }

    public string GetShops()
    {
        DataTable data = new DataTable();
        string sql = "SELECT * FROM accounts";
        cmd = new MySqlCommand(sql, con);
        data.Load(cmd.ExecuteReader());

        string result = "";
        foreach (DataRow row in data.Rows)
        {
            result += row["id"] + " - " + row["username"] + " - " + row["email"] + "\n";
        }
        return result;
    }

    public bool CreateAccount(string username, string password, string email)
    {
        // Check if User Exists

        Debug.Log("Checking " + username);
        DataTable data = new DataTable();
        string sql = "SELECT username FROM accounts WHERE username = '"+username+"'";
        cmd = new MySqlCommand(sql, con);
        data.Load(cmd.ExecuteReader());

        Debug.Log("Result " + data.Rows.Count);
        if (data.Rows.Count == 0 || data.Rows == null)
        {
            // Create User
            cmd = null;
            string cmdString = "";
            cmdString = "insert into accounts (username,password,email) values ('" + username + "','" + password + "','" + email + "')";

            cmd = new MySqlCommand(cmdString, con);
            cmd.ExecuteNonQuery();
            return true;
        }

        return false;        

    }

    public string GetConnectionState()
    {
        return con.State.ToString();
    }
}
