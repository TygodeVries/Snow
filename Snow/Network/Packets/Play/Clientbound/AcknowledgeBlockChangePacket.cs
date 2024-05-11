using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Clientbound
{
    public class AcknowledgeBlockChangePacket : ClientboundPacket
    {
        int sequenceId;
        public AcknowledgeBlockChangePacket(int sequenceId)
        {
            this.sequenceId = sequenceId;
        }

        public override void Create(PacketWriter packetWriter)
        {
            packetWriter.WritePacketID(this);
            packetWriter.WriteVarInt(sequenceId);
        }
    }
}
