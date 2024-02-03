using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Clientbound
{
    internal class SetCenterChunk : ClientboundPacket
    {
        int x;
        int z;

        public SetCenterChunk(int x, int z)
        {
            this.x = x;
            this.z = z;
        }

        public override void Create(PacketWriter packetWriter)
        {
            packetWriter.WriteVarInt(0x52);

            packetWriter.WriteVarInt(x);
            packetWriter.WriteVarInt(z);
        }
    }
}
