﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Clientbound
{
    public class SynchronizePlayerPositionPacket : ClientboundPacket
    {
        double x;
        double y;
        double z;

        float yaw;
        float pitch;

        public SynchronizePlayerPositionPacket(double x, double y, double z, float yaw, float pitch)
        {
            this.x = x;
            this.y = y;
            this.z = z;

            this.yaw = yaw;
            this.pitch = pitch;
        }

        public override void Create(PacketWriter packetWriter)
        {
            packetWriter.WritePacketID(this);

            packetWriter.WriteDouble(x);
            packetWriter.WriteDouble(y);
            packetWriter.WriteDouble(z);

            packetWriter.WriteFloat(yaw);
            packetWriter.WriteFloat(pitch);

            packetWriter.WriteByte(0x00);
            packetWriter.WriteVarInt(3);
        }
    }
}
