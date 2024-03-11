using Snow.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Clientbound
{
    public class UpdateEntityPositionPacket : ClientboundPacket
    {
        Entity entity;

        short deltaX;
        short deltaY;
        short deltaZ;

        public UpdateEntityPositionPacket(Entity entity, short deltaX, short deltaY, short deltaZ)
        {
            this.entity = entity;
            this.deltaX = deltaX;
            this.deltaY = deltaY;
            this.deltaZ = deltaZ;
        }

        public override void Create(PacketWriter packetWriter)
        {
            packetWriter.WritePacketID(this);

            packetWriter.WriteVarInt(entity.GetId());

            packetWriter.WriteShort(deltaX);
            packetWriter.WriteShort(deltaY);
            packetWriter.WriteShort(deltaZ);

            packetWriter.WriteBool(false);
        }
    }
}
