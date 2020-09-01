using System.Collections.Generic;
using UnityEngine;

public static class Game
{
    public static Dictionary<uint, Account> Accounts = new Dictionary<uint, Account>();
    public static Dictionary<uint, Character> Characters = new Dictionary<uint, Character>();
    public static Dictionary<uint, Npc> Npcs = new Dictionary<uint, Npc>();
    public static Dictionary<uint, NpcSpawn> NpcSpawns = new Dictionary<uint, NpcSpawn>();

    public static void Load()
    {
        // Load Accounts;
        foreach (Account account in ActiveRecord.Load<Account>())
        {
            Accounts.Add(account.id, account);
        }

        // Load Characters;
        foreach (Character character in ActiveRecord.Load<Character>())
        {
            Characters.Add(character.id, character);
        }

        // Load Npcs;
        foreach (Npc npc in ActiveRecord.Load<Npc>())
        {
            Npcs.Add(npc.id, npc);
        }

        // Load Npcs In World;
        foreach (NpcSpawn npcSpawn in ActiveRecord.Load<NpcSpawn>())
        {
            NpcSpawns.Add(npcSpawn.id, npcSpawn);
        }
    }

    public static void Save()
    {

        // Load Npcs In World;
        foreach (NpcSpawn npcSpawn in NpcSpawns.Values)
        {
            npcSpawn.Update();
            Debug.Log(npcSpawn.npc);
        }
    }

}