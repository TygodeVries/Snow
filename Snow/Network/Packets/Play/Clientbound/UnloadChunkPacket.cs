using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Clientbound
{
    public class UnloadChunkPacket : ClientboundPacket
    {
        int x;
        int z;

        public UnloadChunkPacket(int x, int z)
        {
            this.x = x;
            this.z = z;
        }

        public override void Create(PacketWriter packetWriter)
        {
            packetWriter.WritePacketID(this);
            
            packetWriter.WriteInt(this.x);
            packetWriter.WriteInt(this.z);
        }
    }
}
