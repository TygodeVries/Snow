using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Clientbound
{
    public class SetTickingState : ClientboundPacket
    {
        public override void Create(PacketWriter packetWriter)
        {
            packetWriter.WriteVarInt(0x6E);

            packetWriter.WriteFloat(20);
            packetWriter.WriteBool(false);
        }
    }
}
