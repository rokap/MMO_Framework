using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
public partial class Database
{
    [Table("characters")]
    public class Character : ActiveRecord
    {
        public uint id;
        public uint account_id;
        public string name;
        public int level;

        public Character() : base() { }

        public Character(uint account_id, string name, int level)
        {
            this.account_id = account_id;
            this.name = name;
            this.level = level;
            this.id = this.Create();
        }
    }
}
