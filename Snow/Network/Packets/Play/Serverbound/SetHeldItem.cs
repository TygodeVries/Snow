using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Serverbound
{
    internal class SetHeldItem : ServerboundPacket
    {
        public short slot;

        public override void Decode(PacketReader packetReader)
        {
            slot = packetReader.ReadShort();
        }
    }
}
