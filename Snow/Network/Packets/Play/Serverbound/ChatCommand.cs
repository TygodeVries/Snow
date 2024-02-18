using Snow.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Serverbound
{
    internal class ChatCommand : ServerboundPacket
    {
        string command;
        long timestamp;

        public override void Decode(PacketReader packetReader)
        {
            command = packetReader.ReadString();
            timestamp = packetReader.ReadLong();
        }

        public override void Use(Connection connection)
        {
            connection.GetLobby().GetCommandManager().RunCommand(connection.GetPlayer(), command);
        }
    }
}
