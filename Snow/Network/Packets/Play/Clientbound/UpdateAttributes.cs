using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Clientbound
{
    internal class UpdateAttributes : ClientboundPacket
    {
        public override void Create(PacketWriter packetWriter)
        {
            packetWriter.WriteVarInt(0x71);

            packetWriter.WriteVarInt(0);
            packetWriter.WriteVarInt(0);
        }
    }
}
