using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Clientbound
{
    public class SetHealth : ClientboundPacket
    {
        public override void Create(PacketWriter packetWriter)
        {
            packetWriter.WritePacketID(this);

            packetWriter.WriteFloat(20);
            packetWriter.WriteVarInt(20);
            packetWriter.WriteFloat(5.0f);
        }
    }
}
