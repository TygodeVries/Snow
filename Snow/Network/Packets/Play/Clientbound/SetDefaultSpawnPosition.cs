using Snow.Formats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Clientbound
{
    public class SetDefaultSpawnPosition : ClientboundPacket
    {
        public override void Create(PacketWriter packetWriter)
        {
            packetWriter.WriteVarInt(0x54);

            packetWriter.WritePosition(new Position(0, 0, 0));
            packetWriter.WriteFloat(0);
        }
    }
}
