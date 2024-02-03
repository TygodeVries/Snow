using Snow.Formats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Clientbound
{
    internal class ServerData : ClientboundPacket
    {
        public override void Create(PacketWriter packetWriter)
        {
            throw new NotImplementedException();

            packetWriter.WriteVarInt(0x49);

            packetWriter.WriteChat(new Chat("Hello World!"));
            packetWriter.WriteBool(false);
            packetWriter.WriteBool(false);
        }
    }
}
