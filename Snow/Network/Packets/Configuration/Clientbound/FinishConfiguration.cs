using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Configuration.Clientbound
{
    public class FinishConfiguration : ClientboundPacket
    {
        public FinishConfiguration()
        {

        }

        public override void Create(PacketWriter packetWriter)
        {
            packetWriter.WriteVarInt(0x02);
        }
    }
}
