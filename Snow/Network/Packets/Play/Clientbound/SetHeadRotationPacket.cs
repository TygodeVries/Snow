using Snow.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Clientbound
{
    public class SetHeadRotationPacket : ClientboundPacket
    {
        Entity entity;
        float yaw;

        public SetHeadRotationPacket(Entity entity, float yaw)
        {
            this.entity = entity;
            this.yaw = yaw;
        }

        public override void Create(PacketWriter packetWriter)
        {
            packetWriter.WritePacketID(this);
            packetWriter.WriteVarInt(entity.GetId());
            packetWriter.WriteByte((byte)(yaw * 255f / 360f));
        }
    }
}
