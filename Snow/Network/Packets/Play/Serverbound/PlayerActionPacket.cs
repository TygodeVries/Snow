using Snow.Formats;
using Snow.Network.Packets.Play.Clientbound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Serverbound
{
    public class PlayerActionPacket : ServerboundPacket
    {
        int status;
        Position location;
        byte face;
        int sequence;

        public override void Decode(PacketReader packetReader)
        {
            status = packetReader.ReadVarInt();
            location = packetReader.ReadPosition();
            face = packetReader.ReadByte();
            sequence = packetReader.ReadVarInt();
        }

        public override void Use(Connection connection)
        {
            if(status == (int) PlayerActionPacketStatus.START_DIGGING)
            {
                connection.GetPlayer().StartDigging(location);
            }



            AcknowledgeBlockChangePacket packet = new AcknowledgeBlockChangePacket(sequence);
            connection.SendPacket(packet);
        }
    }

    enum PlayerActionPacketStatus {
        START_DIGGING,
        CANCELLED_DIGGING
    }
}
