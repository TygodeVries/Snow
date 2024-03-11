using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Clientbound
{
    public class ChunkBatchFinishedPacket : ClientboundPacket
    {

        int batchSize;
        public ChunkBatchFinishedPacket(int batchSize)
        {
            this.batchSize = batchSize;
        }

        public override void Create(PacketWriter packetWriter)
        {
            packetWriter.WritePacketID(this);
            packetWriter.WriteVarInt(batchSize);
        }
    }
}
