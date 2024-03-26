using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Clientbound
{
    public class KeepAlivePacket : ClientboundPacket
    {
        public KeepAlivePacket(long keepAliveId)
        {
            this.keepAliveId = keepAliveId;
        }

        public long keepAliveId;

        public override void Create(PacketWriter packetWriter)
        {
            packetWriter.WritePacketID(this);

            packetWriter.WriteLong(keepAliveId);
        }
    }
}
