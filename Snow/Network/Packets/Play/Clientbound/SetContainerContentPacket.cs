using Snow.Items;
using Snow.Items.Containers;
using Snow.Levels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Clientbound
{
    public class SetContainerContentPacket : ClientboundPacket
    {

        byte windowID;
        Inventory inventory;
        ItemStack carriedItem;

        public SetContainerContentPacket(byte windowID, Inventory inventory, ItemStack carriedItem)
        {
            this.windowID = windowID;
            this.inventory = inventory;
            this.carriedItem = carriedItem;
        }

        public override void Create(PacketWriter packetWriter)
        {
            packetWriter.WritePacketID(this);

            packetWriter.WriteByte(windowID);
            packetWriter.WriteVarInt(0);
            packetWriter.WriteVarInt(inventory.GetSize());

            for (int i = 0; i < inventory.GetSize(); i++)
            {
                packetWriter.WriteItemStack(inventory.GetContent()[i]);
            }

            packetWriter.WriteItemStack(carriedItem);
        }
    }
}
