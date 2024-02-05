using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Clientbound
{
    public class SetExperience : ClientboundPacket
    {
        float bar;
        int level;
        int total;

        public SetExperience(float bar, int level, int total)
        {
            this.bar = bar;
            this.level = level;
            this.total = total;
        }
        public override void Create(PacketWriter packetWriter)
        {
            packetWriter.WriteVarInt(0x5A);

            packetWriter.WriteFloat(bar);
            packetWriter.WriteVarInt(level);
            packetWriter.WriteVarInt(total);

        }
    }
}
