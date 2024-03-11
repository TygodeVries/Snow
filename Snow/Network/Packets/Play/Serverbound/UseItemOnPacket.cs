using Snow.Entities;
using Snow.Events.Args;
using Snow.Formats;
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
            connection.GetServer().GetEventManager().ExecuteRightClickBlock(new RightClickBlockEventArgs(connection.GetPlayer(), location, face, hand, insideBlock));
        }
    }
}
