using Snow.Formats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Serverbound
{
    public class ChatMessagePacket : ServerboundPacket
    {
        string message;

        public override void Decode(PacketReader packetReader)
        {
            message = packetReader.ReadString();
        }

        public override void Use(Connection connection)
        {
            string playerName = connection.GetPlayer().GetName();
            connection.GetServer().BroadcastMessage(new TextComponent($"<{playerName}> {message}"));
        }
    }
}
