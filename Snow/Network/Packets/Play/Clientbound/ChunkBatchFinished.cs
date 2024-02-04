using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Clientbound
{
    internal class ChunkBatchFinished : ClientboundPacket
    {

        int batchSize;
        public ChunkBatchFinished(int batchSize)
        {
            this.batchSize = batchSize;
        }

        public override void Create(PacketWriter packetWriter)
        {
            packetWriter.WriteVarInt(0x0C);
            packetWriter.WriteVarInt(batchSize);
        }
    }
}
