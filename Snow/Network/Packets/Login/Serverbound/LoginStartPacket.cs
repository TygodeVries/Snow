﻿using Snow.Entities;
using Snow.Events;
using Snow.Formats;
using Snow.Servers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Login.Serverbound
{
    internal class LoginStartPacket : ServerboundPacket
    {
        public override void Use(Connection connection)
        {
            Player player = new Player(connection);

            player.SetName(this.username);

            Server server = connection.GetServer();
            server.GetWorldManager().GetDefaultWorld().SpawnEntity(player);

            Log.Send($"{username} joined the server!");

            foreach(Connection c in connection.GetServer().GetPlayerConnections())
            {
                if(c.GetPlayer() != null)
                    c.GetPlayer().SendSystemMessage(new TextComponent($"{username} joined the server!"));
            }


            connection.Connect(server, player);
            connection.SetConnectionState(ConnectionState.PLAY);

            player.SyncClient();

        }

        string username;
        UUID uuid;

        public override void Decode(PacketReader packetReader)
        {
            username = packetReader.ReadString();
            uuid = packetReader.ReadUUID();
        }
    }
}
