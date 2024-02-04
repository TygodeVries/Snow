using Snow.Formats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Clientbound
{
    internal class BlockUpdate : ClientboundPacket
    {
        Position location;
        int blockId;

        public BlockUpdate(Position location, int blockId)
        {
            this.location = location;
            this.blockId = blockId;
        }

        public override void Create(PacketWriter packetWriter)
        {
            packetWriter.WriteVarInt(0x09);

            packetWriter.WritePosition(location);
            packetWriter.WriteVarInt(blockId);
        }
    }
}
