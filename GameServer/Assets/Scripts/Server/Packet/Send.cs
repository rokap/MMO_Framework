using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Server
{
    public class Send
    {

        private static void SendTCPData(int _toClient, Packet _packet)
        {
            _packet.WriteLength();
            Debug.Log("Sending [" + _packet.packetType + "] to Client[" + _toClient + "]");
            Server.clients[_toClient].tcp.SendData(_packet);
        }

        private static void SendUDPData(int _toClient, Packet _packet)
        {
            _packet.WriteLength();
            Server.clients[_toClient].udp.SendData(_packet);
        }

        private static void SendTCPDataToAll(Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                if (Server.clients[i].tcp.socket != null)
                    Server.clients[i].tcp.SendData(_packet);
            }
        }

        private static void SendTCPDataToAll(int _exceptClient, Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                if (i != _exceptClient)
                {
                    Server.clients[i].tcp.SendData(_packet);
                }
            }
        }

        private static void SendUDPDataToAll(Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                Server.clients[i].udp.SendData(_packet);
            }
        }

        private static void SendUDPDataToAll(int _exceptClient, Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                if (i != _exceptClient)
                {
                    Server.clients[i].udp.SendData(_packet);
                }
            }
        }

        #region Packets

        public static void Welcome(int _toClient, string _msg)
        {
            using (Packet _packet = new Packet((int)Server.Packets.Welcome))
            {
                _packet.Write(_msg);
                _packet.Write(_toClient);

                SendTCPData(_toClient, _packet);
            }
        }

        public static void SpawnPlayer(int _toClient, Player _player)
        {
            using (Packet _packet = new Packet((int)Server.Packets.SpawnPlayer))
            {
                _packet.Write(_player.id);
                _packet.Write(_player.username);
                _packet.Write(_player.transform.position);
                _packet.Write(_player.transform.rotation);

                SendTCPData(_toClient, _packet);
            }
        }

        public static void PlayerPosition(Player _player)
        {
            using (Packet _packet = new Packet((int)Server.Packets.PlayerPosition))
            {
                _packet.Write(_player.id);
                _packet.Write(_player.transform.position);

                SendUDPDataToAll(_packet);
            }
        }

        public static void PlayerRotation(Player _player)
        {
            using (Packet _packet = new Packet((int)Server.Packets.PlayerRotation))
            {
                _packet.Write(_player.id);
                _packet.Write(_player.transform.rotation);

                SendUDPDataToAll(_player.id, _packet);
            }
        }

        public static void PlayerDisconnected(int _playerId)
        {
            using (Packet _packet = new Packet((int)Server.Packets.PlayerDisconnected))
            {
                _packet.Write(_playerId);

                SendTCPDataToAll(_packet);
            }
        }

        public static void PlayerHealth(Player _player)
        {
            using (Packet _packet = new Packet((int)Server.Packets.PlayerHealth))
            {
                _packet.Write(_player.id);
                _packet.Write(_player.health);

                SendTCPDataToAll(_packet);
            }
        }

        public static void PlayerRespawned(Player _player)
        {
            using (Packet _packet = new Packet((int)Server.Packets.PlayerRespawned))
            {
                _packet.Write(_player.id);

                SendTCPDataToAll(_packet);
            }
        }

        public static void CreateItemSpawner(int _toClient, int _spawnerId, Vector3 _spawnerPosition, bool _hasItem)
        {
            using (Packet _packet = new Packet((int)Server.Packets.CreateItemSpawner))
            {
                _packet.Write(_spawnerId);
                _packet.Write(_spawnerPosition);
                _packet.Write(_hasItem);

                SendTCPData(_toClient, _packet);
            }
        }

        public static void ItemSpawned(int _spawnerId)
        {
            using (Packet _packet = new Packet((int)Server.Packets.ItemSpawned))
            {
                _packet.Write(_spawnerId);

                SendTCPDataToAll(_packet);
            }
        }

        public static void ItemPickedUp(int _spawnerId, int _byPlayer)
        {
            using (Packet _packet = new Packet((int)Server.Packets.ItemPickedUp))
            {
                _packet.Write(_spawnerId);
                _packet.Write(_byPlayer);

                SendTCPDataToAll(_packet);
            }
        }

        public static void SpawnProjectile(Projectile _projectile, int _thrownByPlayer)
        {
            using (Packet _packet = new Packet((int)Server.Packets.SpawnProjectile))
            {
                _packet.Write(_projectile.myNetworkId);
                _packet.Write(_projectile.transform.position);
                _packet.Write(_thrownByPlayer);

                SendTCPDataToAll(_packet);
            }
        }

        public static void ProjectilePosition(Projectile _projectile)
        {
            using (Packet _packet = new Packet((int)Server.Packets.ProjectilePosition))
            {
                _packet.Write(_projectile.myNetworkId);
                _packet.Write(_projectile.transform.position);

                SendUDPDataToAll(_packet);
            }
        }

        public static void ProjectileExploded(Projectile _projectile)
        {
            using (Packet _packet = new Packet((int)Server.Packets.ProjectileExploded))
            {
                _packet.Write(_projectile.myNetworkId);
                _packet.Write(_projectile.transform.position);

                SendTCPDataToAll(_packet);
            }
        }

        public static void SpawnEnemy(Enemy _enemy)
        {
            using (Packet _packet = new Packet((int)Server.Packets.SpawnEnemy))
            {
                SendTCPDataToAll(SpawnEnemy_Data(_enemy, _packet));
            }
        }

        public static void SpawnEnemy(int _toClient, Enemy _enemy)
        {
            using (Packet _packet = new Packet((int)Server.Packets.SpawnEnemy))
            {
                SendTCPData(_toClient, SpawnEnemy_Data(_enemy, _packet));
            }
        }

        private static Packet SpawnEnemy_Data(Enemy _enemy, Packet _packet)
        {
            _packet.Write(_enemy.id);
            _packet.Write(_enemy.transform.position);
            return _packet;
        }

        public static void EnemyPosition(Enemy _enemy)
        {
            using (Packet _packet = new Packet((int)Server.Packets.EnemyPosition))
            {
                _packet.Write(_enemy.id);
                _packet.Write(_enemy.transform.position);

                SendUDPDataToAll(_packet);
            }
        }

        public static void EnemyHealth(Enemy _enemy)
        {
            using (Packet _packet = new Packet((int)Server.Packets.EnemyHealth))
            {
                _packet.Write(_enemy.id);
                _packet.Write(_enemy.health);

                SendTCPDataToAll(_packet);
            }
        }

        public static void SpawnEntity(Entity _entity)
        {
            using (Packet _packet = new Packet((int)Server.Packets.SpawnEntity))
            {

                _packet.Write(_entity.id);
                _packet.Write(_entity.name);
                _packet.Write(_entity.transform.position);

                SendTCPDataToAll(_packet);
                Debug.Log("Sending " + _entity.name + " To All Clients");
            }
        }

        public static void SpawnEntity(int _toClient, Entity _entity)
        {
            using (Packet _packet = new Packet((int)Server.Packets.SpawnEntity))
            {

                _packet.Write(_entity.id);
                _packet.Write(_entity.name);
                _packet.Write(_entity.transform.position);

                SendTCPData(_toClient, _packet);
            }
        }

        public static void RegistrationAccountExists(int _toClient)
        {
            using (Packet _packet = new Packet((int)Server.Packets.RegistrationAccountExists))
            {
                SendTCPData(_toClient, _packet);
            }
        }

        public static void SendToCharacterSelection(int _toClient)
        {
            using (Packet _packet = new Packet((int)Server.Packets.SendToCharacterSelection))
            {
                SendTCPData(_toClient, _packet);
            }
        }

        #endregion
    }
}