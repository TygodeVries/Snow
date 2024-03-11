using Snow.Formats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Login.Clientbound
{
    internal class LoginSuccessPacket : ClientboundPacket
    {
        UUID uuid;
        string username;
        public LoginSuccessPacket(UUID uuid, string username)
        {
            this.uuid = uuid;
            this.username = username;
        }

        public override void Create(PacketWriter packetWriter)
        {
            packetWriter.WritePacketID(this);
            packetWriter.WriteUUID(uuid);
            packetWriter.WriteString(username);
            packetWriter.WriteVarInt(0); // Not providing any elements for now, Should be implemented later.
        }
    }
}
