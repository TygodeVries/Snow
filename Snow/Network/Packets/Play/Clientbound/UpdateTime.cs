using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Clientbound
{
    public class UpdateTime : ClientboundPacket
    {
        public override void Create(PacketWriter packetWriter)
        {
            packetWriter.WriteVarInt(0x62);

            packetWriter.WriteLong(1000); // World time
            packetWriter.WriteLong(0); // Time of day
        }
    }
}
