using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Clientbound
{
    public class GameEvent : ClientboundPacket
    {
        byte eventByte;
        float value;

        public GameEvent(byte eventByte, float value)
        {
            this.eventByte = eventByte;
            this.value = value;
        }

        public override void Create(PacketWriter packetWriter)
        {
            packetWriter.WriteVarInt(0x20);

            packetWriter.WriteByte(eventByte);

            packetWriter.WriteFloat(value);
        }
    }
}
