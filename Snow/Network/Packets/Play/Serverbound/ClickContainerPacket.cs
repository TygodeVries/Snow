using Snow.Entities;
using Snow.Events.Arguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Serverbound
{
    internal class ClickContainerPacket : ServerboundPacket
    {
        byte windowId;
        int stateId;
        short slot;
        byte button;
        int mode;
        

        public override void Decode(PacketReader packetReader)
        {
            windowId = packetReader.ReadByte();
            int stateId = packetReader.ReadVarInt();
            slot = packetReader.ReadShort();
            button = packetReader.ReadByte();
            mode = packetReader.ReadVarInt();
        }

        public override void Use(Connection connection)
        {
            Player player = connection.GetPlayer();
            player.UpdateInventory();

            OnInventoryClickArgs inventoryClickArgs = new OnInventoryClickArgs(player, windowId, stateId, slot, button, mode);
            player.OnInventoryClick?.Invoke(player, inventoryClickArgs);
        }
    }
}
