﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
public partial class Database
{
    [Table("items")]
    public class Item : ActiveRecord
    {
        public int id;
        public string name;

        public Item() : base() { }

        public Item(string name)
        {
            this.name = name;
            this.id = this.Create();
        }
    }
}