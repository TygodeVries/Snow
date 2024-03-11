using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Configuration.Clientbound
{
    public class FinishConfigurationPacket : ClientboundPacket
    {
        public FinishConfigurationPacket()
        {

        }

        public override void Create(PacketWriter packetWriter)
        {
            packetWriter.WritePacketID(this);
        }
    }
}
