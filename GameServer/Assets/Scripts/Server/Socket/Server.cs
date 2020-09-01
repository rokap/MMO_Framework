using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using UnityEngine;


public partial class Server
{
    public static int MaxPlayers { get; private set; }
    public static int Port { get; private set; }
    public static Dictionary<int, Client> clients = new Dictionary<int, Client>();
    public static Dictionary<int, Packet.Receiver> packetReceivers;

    public static Database.Connection database = null;

    private static TcpListener tcpListener;
    private static UdpClient udpListener;

    public static bool TcpReady
    {
        get { return (tcpListener != null) ? true : false; }
    }

    public static bool UdpReady
    {
        get { return (udpListener != null) ? true : false; }
    }

    public static bool PacketsReady
    {
        get { return (packetReceivers != null) ? true : false; }
    }

    public static bool ClientsReady
    {
        get { return (clients.Count > 0) ? true : false; }
    }

    public static bool DatabaseReady
    {
        get { return (database != null) ? true : false; }
    }

    public static bool IsReady
    {
        get; private set;
    }

    public static void Start(int _maxPlayers, int _port)
    {

        Util.IniFile MyIni = new Util.IniFile("Settings.ini");
        string section = "MySQL";
        string host = MyIni.Read("Host", section);
        string user = MyIni.Read("Username", section);
        string pass = MyIni.Read("Password", section);
        string db = MyIni.Read("Database", section);

        database = new Database.Connection(host, db, user, pass, true);
        Development.Log("Starting Server");

        MaxPlayers = _maxPlayers;
        Port = _port;

        // Setup Server Packets
        InitializeServerData();

        Development.Divider();
    }

    private static void TCPConnectCallback(IAsyncResult _result)
    {
        TcpClient _client = tcpListener.EndAcceptTcpClient(_result);
        tcpListener.BeginAcceptTcpClient(TCPConnectCallback, null);
        Debug.Log($"Incoming connection from {_client.Client.RemoteEndPoint}...");

        for (int i = 1; i <= MaxPlayers; i++)
        {
            if (clients[i].tcp.socket == null)
            {
                clients[i].tcp.Connect(_client);
                return;
            }
        }

        Debug.Log($"{_client.Client.RemoteEndPoint} failed to connect: Server full!");
    }

    private static void UDPReceiveCallback(IAsyncResult _result)
    {
        // If we lost our connection
        if (_result.AsyncState == null)
            return;

        try
        {

            IPEndPoint _clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
            byte[] _data = udpListener.EndReceive(_result, ref _clientEndPoint);
            udpListener.BeginReceive(UDPReceiveCallback, null);

            if (_data.Length < 4)
            {
                return;
            }

            using (Packet _packet = new Packet(_data))
            {
                int _clientId = _packet.ReadInt();

                if (_clientId == 0)
                {
                    return;
                }

                if (clients[_clientId].udp.endPoint == null)
                {
                    // If this is a new connection
                    clients[_clientId].udp.Connect(_clientEndPoint);
                    return;
                }

                if (clients[_clientId].udp.endPoint.ToString() == _clientEndPoint.ToString())
                {
                    // Ensures that the client is not being impersonated by another by sending a false clientID
                    clients[_clientId].udp.HandleData(_packet);
                }
            }
        }
        catch (Exception _ex)
        {
            Debug.LogError($"Error receiving UDP data: {_ex}");
        }
    }

    public static void SendUDPData(IPEndPoint _clientEndPoint, Packet _packet)
    {
        try
        {
            if (_clientEndPoint != null)
            {
                udpListener.BeginSend(_packet.ToArray(), _packet.Length(), _clientEndPoint, null, null);
            }
        }
        catch (Exception _ex)
        {
            Debug.Log($"Error sending data to {_clientEndPoint} via UDP: {_ex}");
        }
    }

    private static void InitializeServerData()
    {

        // Connect to MySQL
        Development.Log("Connecting to MySQL");
        if (!database.Connect())
        {
            Debug.LogError("No Database Connection... Shutting Down");
            return;
        }

        // Setup Max Clients
        Development.Log("Initializing Client Connections");
        for (int i = 1; i <= MaxPlayers; i++)
        {
            clients.Add(i, new Client(i));
        }

        // Init Packet Receivers.
        Development.Log("Initializing Packet Receivers");
        InitPacketReceiver(Client.Packets.Welcome, Receive.Welcome);
        InitPacketReceiver(Client.Packets.PlayerMovement, Receive.PlayerMovement);
        InitPacketReceiver(Client.Packets.PlayerShoot, Receive.PlayerShoot);
        InitPacketReceiver(Client.Packets.PlayerThrowItem, Receive.PlayerThrowItem);
        InitPacketReceiver(Client.Packets.PlayerInspect, Receive.PlayerThrowItem);
        InitPacketReceiver(Client.Packets.Registration, Receive.Registration);

        // Startup TCP Listener
        Development.Log("Starting TCP Listener");
        tcpListener = new TcpListener(IPAddress.Any, Port);
        tcpListener.Start();
        tcpListener.BeginAcceptTcpClient(TCPConnectCallback, null);

        // Startup UDP Listener
        Development.Log("Starting UDP Listener");
        udpListener = new UdpClient(Port);
        udpListener.BeginReceive(UDPReceiveCallback, null);

        Development.Log($"Server Started ( {Port} )");
        IsReady = true;

        // Delegate to a new Game()
        Game.Load();
    }

    private static void InitPacketReceiver(Client.Packets _clientPacket, Packet.Receiver _serverPacketHandler)
    {
        if (packetReceivers == null)
        {
            //Debug.Log("Initializing Packet Receivers....");
            packetReceivers = new Dictionary<int, Packet.Receiver>();
        }

        //Debug.Log(" - ( " + (int)_clientPacket + " ) " + _clientPacket + " Receiver Initialized... ");
        packetReceivers.Add((int)_clientPacket, _serverPacketHandler);

    }

    public static void Stop()
    {
        Development.Log("Stopping Server");

        if (TcpReady)
        {
            Development.Log("Shutting Down TCP Listener");
            tcpListener.Stop();
            tcpListener = null;
        }

        if (UdpReady)
        {
            Development.Log("Shutting Down UDP Listener");
            udpListener.Close();
            udpListener = null;
        }


        if (PacketsReady)
        {
            Development.Log("Clearing Packet Receivers");
            packetReceivers.Clear();
            packetReceivers = null;
        }

        if (ClientsReady)
        {
            // Setup Max Clients
            Development.Log("Clearing Client Connections");
            clients.Clear();
        }

        if (DatabaseReady)
        {
            Game.Save();
            Development.Log("Disconnected from MySQL");
            database.Disconnect();
            database = null;
        }

        Development.Log("Server Shutdown");
        IsReady = false;
        Development.Divider();
    }
}
