using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Clientbound
{
    public class PlayerAbilities : ClientboundPacket
    {
        public override void Create(PacketWriter packetWriter)
        {
            packetWriter.WriteVarInt(0x36);

            packetWriter.WriteByte(0x04); // flying
            packetWriter.WriteFloat(0.05f);
            packetWriter.WriteFloat(0.1f);
        }
    }
}
