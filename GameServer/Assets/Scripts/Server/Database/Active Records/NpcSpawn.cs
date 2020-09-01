using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Table("npc_spawn")]
public class NpcSpawn : ActiveRecord
{
    public uint id;
    public uint npc_id;
    public uint npc_spawn_group_id;
    public uint npc_spawn_point_id;
    public float position_x;
    public float position_y;
    public float position_z;
    public float heading;

    // Relationships
    public Npc npc
    {
        get { return Load<Npc>(npc_id); }
    }

    public NpcSpawn() : base() { }

    public NpcSpawn(uint npc_id, uint npc_spawn_group_id, uint npc_spawn_point_id, float position_x, float position_y, float position_z, float heading)
    {
        this.npc_id = npc_id;
        this.npc_spawn_group_id = npc_spawn_group_id;
        this.npc_spawn_point_id = npc_spawn_point_id;
        this.position_x = position_x;
        this.position_y = position_y;
        this.position_z = position_z;
        this.heading = heading;
        this.id = this.Create();
    }
}
