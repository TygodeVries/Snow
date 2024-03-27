using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Clientbound
{
    public class PlayerAbilitiesPacket : ClientboundPacket
    {
        byte flags;
        float flyingspeed;
        float fov;
        public PlayerAbilitiesPacket(byte flags, float flyingspeed, float fov)
        {
            this.flags = flags;
            this.flyingspeed = flyingspeed;
            this.fov = fov;
        }

        public override void Create(PacketWriter packetWriter)
        {
            packetWriter.WritePacketID(this);

            packetWriter.WriteByte(flags); // flying
            packetWriter.WriteFloat(flyingspeed);
            packetWriter.WriteFloat(fov);
        }
    }
}
