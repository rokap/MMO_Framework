﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
public partial class Database
{
    [Table("accounts")]
    public class Account : ActiveRecord
    {
        public int id;

        public string username;

        public string password;

        public string email;

        public Account() : base() { }

        public Account(string username, string password, string email)
        {

            this.username = username;
            this.password = password;
            this.email = email;
            this.id = this.Create();
        }
    }
}