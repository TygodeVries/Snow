using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Clientbound
{
    internal class SetHeldItem : ClientboundPacket
    {
        byte slot;
        public SetHeldItem(byte slot) { 
            this.slot = slot;
        }

        public override void Create(PacketWriter packetWriter)
        {
            packetWriter.WriteVarInt(0x51);

            packetWriter.WriteByte(slot);
        }
    }
}
