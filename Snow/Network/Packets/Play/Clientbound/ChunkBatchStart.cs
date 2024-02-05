﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Clientbound
{
    public class ChunkBatchStart : ClientboundPacket
    {
        public override void Create(PacketWriter packetWriter)
        {
            packetWriter.WriteVarInt(0x0D);
        }
    }
}
