using Snow.Formats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Clientbound
{
    public class SystemChatMessagePacket : ClientboundPacket
    {
        TextComponent textComponent;
        bool overlay;

        public SystemChatMessagePacket(TextComponent textComponent, bool overlay)
        {
            this.textComponent = textComponent;
            this.overlay = overlay;
        }

        public override void Create(PacketWriter packetWriter)
        {
            packetWriter.WritePacketID(this);

            packetWriter.WriteTextComponent(textComponent);
            packetWriter.WriteBool(overlay);
        }
    }
}
