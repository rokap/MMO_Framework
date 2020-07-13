
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;
    public int maxPlayers = 5;
    public int port = 26950;
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject projectilePrefab;

    protected GUIStyle myGUIStyle = new GUIStyle();


    private void Awake()
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
    }

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;
        myGUIStyle.normal.textColor = Color.black;
        myGUIStyle.normal.background = MakeTex(600, 1, new Color(0f, 0f, 0f, 0.5f));

    }
    private Texture2D MakeTex(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];

        for (int i = 0; i < pix.Length; i++)
            pix[i] = col;

        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();

        return result;
    }

    private void OnGUI()
    {

        GUILayout.BeginHorizontal();
        GUILayout.BeginArea(new Rect(15, 15, 200, 200), myGUIStyle);
        if (Server.IsReady)
        {
            if (GUILayout.Button("Stop Server"))
            {
                Server.Stop();
            }
        }
        else
        {
            if (GUILayout.Button("Start Server"))
            {
                Server.Start(maxPlayers, port);
            }
        }
        if (Server.IsReady)
            GUILayout.Label("Server: Ready");
        else
            GUILayout.Label("Server: Not Ready");

        if (Server.database != null)
            GUILayout.Label("Database: Ready");
        else
            GUILayout.Label("Database: Not Ready");

        if (Server.ClientsReady)
            GUILayout.Label("Client Connections : Ready");
        else
            GUILayout.Label("Client Connections : Not Ready");

        if (Server.PacketsReady)
            GUILayout.Label("Packets : Ready");
        else
            GUILayout.Label("Packets : Not Ready");

        if (Server.TcpReady)
            GUILayout.Label("TCP Socket : Ready");
        else
            GUILayout.Label("TCP Socket : Not Ready");

        if (Server.UdpReady)
            GUILayout.Label("UDP Socket : Ready");
        else
            GUILayout.Label("UDP Socket : Not Ready");

        GUILayout.EndArea();
        GUILayout.BeginArea(new Rect(230, 15, 200, 400), myGUIStyle);

        GUILayout.Label("Loaded Listener");
        if (Server.PacketsReady)
        {
            foreach (KeyValuePair<int, Packet.Receiver> receiver in Server.packetReceivers)
            {
                GUILayout.Label(" - " + receiver.Value.Method.Name + " ( " + receiver.Key + " )");
            }
        }
        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect(445, 15, 300, 600), myGUIStyle);
        GUILayout.Label("Loaded Packets");
        if (Server.PacketsReady)
        {
            foreach (var item in System.Enum.GetNames(typeof(Server.Packets)))
            {
                GUILayout.Label(" - " + item + " ( " + (int)System.Enum.Parse(typeof(Server.Packets), item) + " )");
            }
        }
        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect(760, 15, 300, 600), myGUIStyle);
        GUILayout.Label("Database Models");
        if (Server.DatabaseReady)
        {
            string path = Application.dataPath + "/Scripts/Server/Database/Active Records";

            if (File.Exists(path))
            {
                // This path is a file
                ProcessFile(path);
            }
            else if (Directory.Exists(path))
            {
                // This path is a directory
                ProcessDirectory(path);
            }
            else
            {
                Debug.LogError(path + " is not a valid file or directory.");
            }
        }
        GUILayout.EndArea();
        GUILayout.EndHorizontal();
    }

    private void OnApplicationQuit()
    {
        Server.Stop();
    }

    public Player InstantiatePlayer()
    {
        return Instantiate(playerPrefab, new Vector3(0f, 0.5f, 0f), Quaternion.identity).GetComponent<Player>();
    }

    public void InstantiateEnemy(Vector3 _position)
    {
        Instantiate(enemyPrefab, _position, Quaternion.identity);
    }

    public Projectile InstantiateProjectile(Transform _shootOrigin)
    {
        return Instantiate(projectilePrefab, _shootOrigin.position + _shootOrigin.forward * 0.7f, Quaternion.identity).GetComponent<Projectile>();
    }


    // Process all files in the directory passed in, recurse on any directories
    // that are found, and process the files they contain.
    public static void ProcessDirectory(string targetDirectory)
    {
        // Process the list of files found in the directory.
        string[] fileEntries = Directory.GetFiles(targetDirectory);
        foreach (string fileName in fileEntries)
            ProcessFile(Path.GetFileName(fileName));

        // Recurse into subdirectories of this directory.
        string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
        foreach (string subdirectory in subdirectoryEntries)
            ProcessDirectory(subdirectory);
    }

    // Insert logic for processing found files here.
    public static void ProcessFile(string path)
    {
        if (!path.Contains(".meta"))
        {
            GUILayout.Label(" - " + path.Replace(".cs", ""));
        }
    }
}
