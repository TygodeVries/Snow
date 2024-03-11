using Snow.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Clientbound
{
    public class SpawnEntityPacket : ClientboundPacket
    {
        Entity entity;
        public SpawnEntityPacket(Entity entity)
        {
            this.entity = entity;
        }

        public override void Create(PacketWriter packetWriter)
        {
            packetWriter.WritePacketID(this);
            packetWriter.WriteVarInt(entity.GetId());
            packetWriter.WriteUUID(entity.GetUUID());

            packetWriter.WriteVarInt(entity.type);

            packetWriter.WriteDouble(entity.GetLocation().x);
            packetWriter.WriteDouble(entity.GetLocation().y);
            packetWriter.WriteDouble(entity.GetLocation().z);

            packetWriter.WriteAngle(0);
            packetWriter.WriteAngle(0);
            packetWriter.WriteAngle(0);

            packetWriter.WriteVarInt(0);

            packetWriter.WriteShort(0);
            packetWriter.WriteShort(0);
            packetWriter.WriteShort(0);
        }
    }
}
