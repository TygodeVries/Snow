using Snow.Commands;
using Snow.Network.Packets.Play.Clientbound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Serverbound
{
    internal class CommandSuggestionsRequestPacket : ServerboundPacket
    {
        int transactionId;
        string text;

        public override void Decode(PacketReader packetReader)
        {
            transactionId = packetReader.ReadVarInt();
            text = packetReader.ReadString();
        }

        public override void Use(Connection connection)
        {
            Log.Send(text);
        }
    }
}
