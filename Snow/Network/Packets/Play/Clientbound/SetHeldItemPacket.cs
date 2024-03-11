using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Clientbound
{
    public class SetHeldItemPacket : ClientboundPacket
    {
        byte slot;
        public SetHeldItemPacket(byte slot) { 
            this.slot = slot;
        }

        public override void Create(PacketWriter packetWriter)
        {
            packetWriter.WritePacketID(this);

            packetWriter.WriteByte(slot);
        }
    }
}
