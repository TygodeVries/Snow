using Snow.Network.Packets.Status.Clientbound;
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

            if (nextState == 1)
            {
                // #TODO implement status
                connection.SetConnectionState(ConnectionState.STATUS);

                Servers.Configuration config = connection.GetServer().GetSettings();

                StatusResponsePacket packet = new StatusResponsePacket("1.20.4", "765", config.GetString("max-players"), config.GetString("online-players"), connection.GetServer().GetLanguage().GetString("motd"), "data:image/png;base64,<data>", "false", "false", connection.GetServer().GetLanguage().GetString("motd_tooltip"));


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
