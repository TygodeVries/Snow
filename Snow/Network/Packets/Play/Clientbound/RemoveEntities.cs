using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Clientbound
{
    public class RemoveEntities : ClientboundPacket
    {

        int[] entityIds;
        public RemoveEntities(int[] entityIds)
        {
            this.entityIds = entityIds;
        }

        public override void Create(PacketWriter packetWriter)
        {
            packetWriter.WriteVarInt(0x40);
            packetWriter.WriteVarInt(entityIds.Length);

            for(int i = 0; i < entityIds.Length; i++)
            {
                packetWriter.WriteVarInt((ushort)entityIds[i]);
            }
        }
    }
}
