using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Clientbound
{
    public class RemoveEntitiesPacket : ClientboundPacket
    {

        int[] entityIds;
        public RemoveEntitiesPacket(int[] entityIds)
        {
            this.entityIds = entityIds;
        }

        public override void Create(PacketWriter packetWriter)
        {
            packetWriter.WritePacketID(this);
            packetWriter.WriteVarInt(entityIds.Length);

            for(int i = 0; i < entityIds.Length; i++)
            {
                packetWriter.WriteVarInt((ushort)entityIds[i]);
            }
        }
    }
}
