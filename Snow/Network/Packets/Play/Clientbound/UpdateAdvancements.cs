using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Clientbound
{
    public class UpdateAdvancements : ClientboundPacket
    {
        public override void Create(PacketWriter packetWriter)
        {
            packetWriter.WritePacketID(this);

            packetWriter.WriteBool(true);

            packetWriter.WriteVarInt(0);
            packetWriter.WriteVarInt(0);
            packetWriter.WriteVarInt(0);
        }
    }
}
