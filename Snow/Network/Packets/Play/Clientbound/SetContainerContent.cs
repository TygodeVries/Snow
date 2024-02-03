using Snow.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Clientbound
{
    internal class SetContainerContent : ClientboundPacket
    {
        public override void Create(PacketWriter packetWriter)
        {
            packetWriter.WriteVarInt(0x13);

            packetWriter.WriteByte(0x00);

            packetWriter.WriteVarInt(0);

            packetWriter.WriteVarInt(44);


            Slot[] slots = new Slot[44];
            
            for(int i = 0; i < slots.Length; i++)
            {
                slots[i] = new Slot();
            }

            packetWriter.WriteSlotArray(slots);

            packetWriter.WriteSlot(new Slot());
        }
    }
}
