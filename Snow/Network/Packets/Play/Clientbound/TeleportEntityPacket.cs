﻿using Snow.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Clientbound
{
    public class TeleportEntityPacket : ClientboundPacket
    {
        Entity entity;

        double x;
        double y;
        double z;

        float yaw;
        float pitch;

        public TeleportEntityPacket(Entity entity, double x, double y, double z, float yaw, float pitch)
        {
            this.entity = entity;
            this.x = x;
            this.y = y;
            this.z = z;
            this.yaw = yaw;
            this.pitch = pitch;
        }

        public override void Create(PacketWriter packetWriter)
        {
            packetWriter.WritePacketID(this);

            packetWriter.WriteVarInt(entity.GetId());

            packetWriter.WriteDouble(x);
            packetWriter.WriteDouble(y);
            packetWriter.WriteDouble(z);

            packetWriter.WriteByte((byte) (yaw * 255f / 360f));
            packetWriter.WriteByte((byte) (pitch * 255f / 360f));



            packetWriter.WriteBool(false);
        }
    }
}
