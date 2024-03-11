using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Clientbound
{
    public class PlayerAbilitiesPacket : ClientboundPacket
    {
        public override void Create(PacketWriter packetWriter)
        {
            packetWriter.WritePacketID(this);

            packetWriter.WriteByte(0x04); // flying
            packetWriter.WriteFloat(0.05f);
            packetWriter.WriteFloat(0.1f);
        }
    }
}
