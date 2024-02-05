using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Clientbound
{
    public class InitializeWorldBorder : ClientboundPacket
    {
        public override void Create(PacketWriter packetWriter)
        {
            packetWriter.WriteVarInt(0x23);

            packetWriter.WriteDouble(0);
            packetWriter.WriteDouble(0);

            packetWriter.WriteDouble(100);
            packetWriter.WriteDouble(100);

            packetWriter.WriteVarLong(0);

            packetWriter.WriteVarInt(100);
            packetWriter.WriteVarInt(5);
            packetWriter.WriteVarInt(1);
        }
    }
}
