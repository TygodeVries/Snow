using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Clientbound
{
    public class UpdateRecipeBook : ClientboundPacket
    {
        public override void Create(PacketWriter packetWriter)
        {
            packetWriter.WriteVarInt(0x3F);

            packetWriter.WriteVarInt(0);

            packetWriter.WriteBool(false);
            packetWriter.WriteBool(false);
            packetWriter.WriteBool(false);
            packetWriter.WriteBool(false);
            packetWriter.WriteBool(false);
            packetWriter.WriteBool(false);
            packetWriter.WriteBool(false);

            packetWriter.WriteVarInt(0);
            packetWriter.WriteVarInt(0);
            packetWriter.WriteBool(false);
        }
    }
}
