using Snow.Formats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Clientbound
{
    public class SetTitleTextPacket : ClientboundPacket
    {
        TextComponent textComponent;
        public SetTitleTextPacket(TextComponent textComponent)
        {
            this.textComponent = textComponent;
        }

        public override void Create(PacketWriter packetWriter)
        {
            packetWriter.WritePacketID(this);
            packetWriter.WriteTextComponent(textComponent);
        }
    }
}
