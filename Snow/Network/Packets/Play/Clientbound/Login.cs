using Snow.Formats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Clientbound
{
    internal class Login : ClientboundPacket
    {
       
        public override void Create(PacketWriter packetWriter)
        {
            packetWriter.WriteVarInt(0x29);

            packetWriter.WriteInt(0);
            packetWriter.WriteBool(false);

            Identifier[] dimensionNames = new Identifier[1] { new Identifier("minecraft", "overworld") };

            packetWriter.WriteArrayOfIdentifier(dimensionNames);

            packetWriter.WriteVarInt(10);
            packetWriter.WriteVarInt(5);
            packetWriter.WriteVarInt(5);

            packetWriter.WriteBool(false);
            packetWriter.WriteBool(false);
            packetWriter.WriteBool(false);

            packetWriter.WriteIdentifier(new Identifier("minecraft", "overworld"));
            packetWriter.WriteIdentifier(new Identifier("minecraft", "overworld"));
            packetWriter.WriteByteArray(new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 });
            packetWriter.WriteByte(0x00);
            packetWriter.WriteByte(0x00);

            packetWriter.WriteBool(false);
            packetWriter.WriteBool(true);

            packetWriter.WriteBool(false);
            
            packetWriter.WriteVarInt(0);

        }
    }
}
