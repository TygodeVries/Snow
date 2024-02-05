using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Serverbound
{
    public class SetHeldItem : ServerboundPacket
    {
        public short slot;

        public override void Decode(PacketReader packetReader)
        {
            slot = packetReader.ReadShort();

        }

        public override void Use(PlayerConnection connection)
        {
            connection.GetEntityPlayer().selectedHotbarSlot = slot;
        }
    }
}
