using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Clientbound
{
    public class SetCameraPacket : ClientboundPacket
    {
        int entityId;
        public SetCameraPacket(int entityId)
        {
            this.entityId = entityId;
        }

        public override void Create(PacketWriter packetWriter)
        {
            packetWriter.WritePacketID(this);
            packetWriter.WriteVarInt(this.entityId);
        }
    }
}
