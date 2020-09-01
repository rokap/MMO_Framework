using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Server
{
    public class Receive
    {
        public static void Welcome(int _fromClient, Packet _packet)
        {
            int _clientIdCheck = _packet.ReadInt();
            string _username = _packet.ReadString();

            Debug.Log($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {_fromClient}.");
            if (_fromClient != _clientIdCheck)
            {
                Debug.Log($"Player \"{_username}\" (ID: {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})!");
            }
        }

        public static void PlayerMovement(int _fromClient, Packet _packet)
        {
            bool[] _inputs = new bool[_packet.ReadInt()];
            for (int i = 0; i < _inputs.Length; i++)
            {
                _inputs[i] = _packet.ReadBool();
            }
            Quaternion _rotation = _packet.ReadQuaternion();

            Server.clients[_fromClient].player.SetInput(_inputs, _rotation);
        }

        public static void PlayerShoot(int _fromClient, Packet _packet)
        {
            Vector3 _shootDirection = _packet.ReadVector3();

            Server.clients[_fromClient].player.Shoot(_shootDirection);
        }

        public static void PlayerThrowItem(int _fromClient, Packet _packet)
        {
            Vector3 _throwDirection = _packet.ReadVector3();

            Server.clients[_fromClient].player.ThrowItem(_throwDirection);
        }

        public static void Registration(int _fromClient, Packet _packet)
        {
            string username = _packet.ReadString();
            string password = _packet.ReadString();
            string email = _packet.ReadString();

            Database.Account account = Database.ActiveRecord.Load<Database.Account>(("username", username));

            // Check DB for existing user
            if (account != null)
            {
                // Account Exists / Inform Client
                Server.Send.RegistrationAccountExists(_fromClient);
            }
            else
            {
                // Account Created Successfully
                // Added to Connected Client
                Server.clients[_fromClient].account = new Database.Account(username, password, email);

                // Send Client to Character Selection
                Server.Send.SendToCharacterSelection(_fromClient);
            }
        }

        public static void Login(int _fromClient, Packet _packet)
        {
            string username = _packet.ReadString();
            string password = _packet.ReadString();

            Database.Account account = Database.ActiveRecord.Load<Database.Account>(("username", username), ("password", password));
        }
    }
}
