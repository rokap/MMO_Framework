using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

    [Table("npcs")]
    public class Npc : ActiveRecord
    {
        public uint id;
        public string name;
        public int health;
        public string prefab;
        public bool merchant;

        public Npc() : base() { }

        public Npc(string name, int health, string prefab, bool merchant)
        {
            this.name = name;
            this.health = health;
            this.prefab = prefab;
            this.merchant = merchant;
            this.id = this.Create();
        }
    }
