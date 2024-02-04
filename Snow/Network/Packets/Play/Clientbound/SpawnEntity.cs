﻿using Snow.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Clientbound
{
    internal class SpawnEntity : ClientboundPacket
    {
        Entity entity;
        public SpawnEntity(Entity entity)
        {
            this.entity = entity;
        }

        public override void Create(PacketWriter packetWriter)
        {
            packetWriter.WriteVarInt(0x01);
            packetWriter.WriteVarInt(entity.Id);
            packetWriter.WriteUUID(entity.uuid);

            packetWriter.WriteVarInt(entity.type);

            packetWriter.WriteDouble(entity.x);
            packetWriter.WriteDouble(entity.y);
            packetWriter.WriteDouble(entity.z);

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