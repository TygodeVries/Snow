using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Clientbound
{
    public class SetTitleAnimationTimesPacket : ClientboundPacket
    {
        int fadein;
        int stay;
        int fadeout;

        public SetTitleAnimationTimesPacket(int fadein, int stay, int fadeout)
        {
            this.fadein = fadein;
            this.stay = stay;
            this.fadeout = fadeout;
        }

        public override void Create(PacketWriter packetWriter)
        {
            packetWriter.WritePacketID(this);

            packetWriter.WriteInt(fadein);
            packetWriter.WriteInt(stay);
            packetWriter.WriteInt(fadeout);
        }
    }
}
