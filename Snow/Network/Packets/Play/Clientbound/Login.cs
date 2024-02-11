using Snow.Formats;
using Snow.Level.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Clientbound
{
    public class Login : ClientboundPacket
    {
        public Login(Player entityPlayer)
        {
            this.player = entityPlayer;
        }
       
        Player player;

        public override void Create(PacketWriter packetWriter)
        {
            packetWriter.WritePacketID(this);

            packetWriter.WriteInt(player.EntityID);
            packetWriter.WriteBool(false);

            
            packetWriter.WriteVarInt(1);
            packetWriter.WriteString("minecraft:overworld");

            packetWriter.WriteVarInt(10);
            packetWriter.WriteVarInt(5);
            packetWriter.WriteVarInt(5);

            packetWriter.WriteBool(false);
            packetWriter.WriteBool(false);
            packetWriter.WriteBool(false);

            packetWriter.WriteString("minecraft:overworld");
            packetWriter.WriteString("minecraft:overworld");
            packetWriter.WriteByteArray(new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 });
          
            packetWriter.WriteByte(0x01);
            packetWriter.WriteByte(0x01);

            packetWriter.WriteBool(false);
            packetWriter.WriteBool(true);

            packetWriter.WriteBool(false);
            
            packetWriter.WriteVarInt(0);

        }
    }
}
