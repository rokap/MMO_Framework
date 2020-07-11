using UnityEngine;
using System;
using System.Data;
using System.Text;

using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

using MySql.Data;
using MySql.Data.MySqlClient;


[Serializable]
public class Database
{
    public string host, database, user, password;
    public bool pooling = true;

    private string connectionString;
    private MySqlConnection con = null;
    private MySqlCommand cmd = null;

    public Database(string host, string database, string user, string password, bool pooling)
    {
        // The following requires: using Castle.ActiveRecord.Framework.Config;

        this.host = host;
        this.database = database;
        this.user = user;
        this.password = password;
        this.pooling = pooling;
    }

    public void Connect()
    {
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
            Debug.Log("Connecting to Mysql" + ((con.State == ConnectionState.Open)?"....Connected":"....Error"));


        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public void Disconnect()
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

    public bool CreateAccount(string username, string password, string email)
    {
        // Check if User Exists

        DataTable data = new DataTable();
        string sql = "SELECT username FROM accounts WHERE username = '" + username + "'";
        cmd = new MySqlCommand(sql, con);
        data.Load(cmd.ExecuteReader());

        if (data.Rows.Count == 0 || data.Rows == null)
        {
            // Create User
            cmd = null;
            string cmdString = "insert into accounts (username,password,email) values ('" + username + "','" + password + "','" + email + "')";
            cmd = new MySqlCommand(cmdString, con);
            cmd.ExecuteNonQuery();
            return true;
        }

        return false;

    }
    
}
