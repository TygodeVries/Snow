using Snow.Containers;
using Snow.Level;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Clientbound
{
    public class SetContainerContent : ClientboundPacket
    {

        byte windowID;
        Inventory inventory;

        public SetContainerContent(byte windowID, Inventory inventory)
        {
            this.windowID = windowID;
            this.inventory = inventory;
        }

        public override void Create(PacketWriter packetWriter)
        {
            packetWriter.WriteVarInt(0x13);
            
            packetWriter.WriteByte(windowID);
            packetWriter.WriteVarInt(0);
            packetWriter.WriteVarInt(inventory.size);

            for (int i = 0; i < inventory.size; i++)
            {
                packetWriter.WriteItemStack(inventory.content[i]);
            }

            packetWriter.WriteItemStack(new ItemStack());
        }
    }
}
