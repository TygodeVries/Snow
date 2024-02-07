using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Clientbound
{
    public class SetHeldItem : ClientboundPacket
    {
        byte slot;
        public SetHeldItem(byte slot) { 
            this.slot = slot;
        }

        public override void Create(PacketWriter packetWriter)
        {
            packetWriter.WritePacketID(this);

            packetWriter.WriteByte(slot);
        }
    }
}
