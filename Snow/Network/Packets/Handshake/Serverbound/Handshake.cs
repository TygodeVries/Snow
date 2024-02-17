using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Handshake.Serverbound
{
    internal class Handshake : ServerboundPacket
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

            }
            else
            {
                connection.SetConnectionState(ConnectionState.LOGIN);
            }
        }
    }
}
