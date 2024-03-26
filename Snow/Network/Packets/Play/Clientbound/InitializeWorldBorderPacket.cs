using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Clientbound
{
    public class InitializeWorldBorderPacket : ClientboundPacket
    {
        double x;
        double z; 
        double oldSize; 
        double newSize; 
        long speed; 
        int warningBlocks; 
        int warningTime;

        public InitializeWorldBorderPacket(double x, double z, double oldSize, double newSize, long speed, int warningBlocks, int warningTime)
        {
            this.x = x;
            this.z = z;
            this.oldSize = oldSize;
            this.newSize= newSize;
            this.speed = speed;
            this.warningBlocks = warningBlocks;
            this.warningTime = warningTime;
        }
        public override void Create(PacketWriter packetWriter)
        {
            packetWriter.WritePacketID(this);

            packetWriter.WriteDouble(x);
            packetWriter.WriteDouble(z);

            packetWriter.WriteDouble(oldSize);
            packetWriter.WriteDouble(newSize);

            packetWriter.WriteVarLong(speed);

            packetWriter.WriteVarInt(29999984);
            packetWriter.WriteVarInt(warningBlocks);
            packetWriter.WriteVarInt(warningTime);
        }
    }
}
