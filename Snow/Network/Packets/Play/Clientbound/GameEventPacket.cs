using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Clientbound
{
    public class GameEventPacket : ClientboundPacket
    {
        byte eventByte;
        float value;

        public GameEventPacket(byte eventByte, float value)
        {
            this.eventByte = eventByte;
            this.value = value;
        }

        public override void Create(PacketWriter packetWriter)
        {
            packetWriter.WritePacketID(this);

            packetWriter.WriteByte(eventByte);

            packetWriter.WriteFloat(value);
        }
    }
}
