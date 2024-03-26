using Snow.Formats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Snow.Network.Packets.Play.Clientbound
{
    public class DisconnectPacket : ClientboundPacket
    {
        TextComponent textComponent;
        public DisconnectPacket(TextComponent textComponent)
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
