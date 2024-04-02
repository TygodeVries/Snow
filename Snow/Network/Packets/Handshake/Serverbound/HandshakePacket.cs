﻿using Snow.Network.Packets.Status.Clientbound;
using Snow.Servers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Handshake.Serverbound
{
    internal class HandshakePacket : ServerboundPacket
    {
        int protocolVersion;
        string serverAddress;
        short port;
        int nextState;

        public override void Decode(PacketReader packetReader)
        {
            protocolVersion = packetReader.ReadVarInt();
            serverAddress = packetReader.ReadString();
            port = packetReader.ReadShort();
            nextState = packetReader.ReadVarInt();
        }

        public override void Use(Connection connection)
        {

            Console.WriteLine(nextState);

            if (nextState == 99)
            {
                // #TODO implement status
                connection.SetConnectionState(ConnectionState.STATUS);

                Servers.Configuration config = connection.GetServer().GetConfiguration();

                StatusResponsePacket packet = new StatusResponsePacket("1.20.4", "765", config.GetString("max-players"), config.GetString("online-players"), config.GetString("motd"), "data:image/png;base64,<data>", "false", "false");

                Log.Send("Motd requested.");

                connection.SendPacket(packet);
                return;
            }
            else
            {
                connection.SetConnectionState(ConnectionState.LOGIN);
            }
        }
    }
}
