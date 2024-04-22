using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Status.Clientbound
{
    public class PingResponsePacket : ClientboundPacket
    {
        long payload;
        public PingResponsePacket(long payload)
        {
            this.payload = payload;
        }

        public override void Create(PacketWriter packetWriter)
        {
            packetWriter.WritePacketID(this);
            packetWriter.WriteLong(payload);
        }
    }
}
