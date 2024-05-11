using Snow.Entities;
using Snow.Events;
using Snow.Events.Arguments;
using Snow.Formats;
using Snow.Network.Packets.Play.Clientbound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Serverbound
{
    internal class UseItemOnPacket : ServerboundPacket
    {

        int hand;
        Position location;

        int face;

        float cursorx;
        float cursory;
        float cursorz;

        bool insideBlock;
        int sequence;

        DateTime start;

        public override void Decode(PacketReader packetReader)
        {
            hand = packetReader.ReadVarInt();
            location = packetReader.ReadPosition();

            face = packetReader.ReadVarInt();

            cursorx = packetReader.ReadFloat();
            cursory = packetReader.ReadFloat();
            cursorz = packetReader.ReadFloat();

            insideBlock = packetReader.ReadBool();
            sequence = packetReader.ReadVarInt();

        }

        public override void Use(Connection connection)
        {
            connection.GetPlayer().OnUseItem?.Invoke(this,
                new OnUseItemsArgs(connection.GetPlayer(), location, face, insideBlock));

            AcknowledgeBlockChangePacket packet = new AcknowledgeBlockChangePacket(sequence);
            connection.SendPacket(packet);
        }
    }
}
