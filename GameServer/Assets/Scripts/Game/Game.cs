using Database;
using System.Collections.Generic;
using UnityEngine;

public static class Game
{
    public static Dictionary<uint, Database.Account> Accounts = new Dictionary<uint, Database.Account>();
    public static Dictionary<uint, Database.Character> Characters = new Dictionary<uint, Database.Character>();
    public static Dictionary<uint, Database.Npc> Npcs = new Dictionary<uint, Database.Npc>();
    public static Dictionary<uint, Database.NpcSpawn> NpcSpawns = new Dictionary<uint, Database.NpcSpawn>();
    public static Dictionary<uint, Database.Scenery> Scenery = new Dictionary<uint, Database.Scenery>();

    public static void Load()
    {
        // Load Accounts;
        foreach (Database.Account account in Database.ActiveRecord.Load<Database.Account>())
        {
            Accounts.Add(account.id, account);
        }

        // Load Characters;
        foreach (Database.Character character in Database.ActiveRecord.Load<Database.Character>())
        {
            Characters.Add(character.id, character);
        }

        // Load Npcs;
        foreach (Database.Npc npc in Database.ActiveRecord.Load<Database.Npc>())
        {
            Npcs.Add(npc.id, npc);
        }

        // Load Npcs In World;
        foreach (NpcSpawn npcSpawn in Database.ActiveRecord.Load<NpcSpawn>())
        {
            NpcSpawns.Add(npcSpawn.id, npcSpawn);
        }

        // Load Scenery In World;
        foreach (Scenery scenery in ActiveRecord.Load<Scenery>())
        {
            scenery.Instantiate();
        }
    }

    public static void Save()
    {

        // Save Npcs In World;
        foreach (NpcSpawn npcSpawn in NpcSpawns.Values)
        {
            npcSpawn.Save();
        }

        // Save Scenery In World;
        foreach (Scenery scenery in GameObject.FindObjectsOfType<Scenery>())
        {
            scenery.Save();
        }
    }

    public static T Instantiate<T>(string resource, Vector3 position, Quaternion rotation, Transform parent = null) where T : Object
    {
        return GameObject.Instantiate<T>(Resources.Load<T>(resource), position, rotation, parent);
    }
}