﻿using Snow.Formats.Nbt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Configuration.Clientbound
{
    public class RegistryDataPacket : ClientboundPacket
    {
        public override void Create(PacketWriter packetWriter)
        {
            packetWriter.WritePacketID(this);

            NbtCompoundTag tag = NbtConverter.Convert(@"Data/registry_data.json", false);
            packetWriter.WriteByteArray(tag.ToByteArray());
        }
    }
}
