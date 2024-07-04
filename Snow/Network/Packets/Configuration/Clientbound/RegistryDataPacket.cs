using Snow.Formats;
using Snow.Formats.Nbt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Configuration.Clientbound
{
    public class RegistryDataPacket : ClientboundPacket
    {
        Identifier registryId;
        List<RegistryDataPacketEntry> entries;

        public RegistryDataPacket(Identifier registryId, List<RegistryDataPacketEntry> dataEntries)
        {
            this.registryId = registryId;
            this.entries = dataEntries;
        }

        public override void Create(PacketWriter packetWriter)
        {
            packetWriter.WritePacketID(this);

            packetWriter.WriteIdentifier(registryId);
            packetWriter.WriteVarInt(entries.Count);

            foreach(RegistryDataPacketEntry entry in entries)
            {
                packetWriter.WriteIdentifier(entry.entryId);
                packetWriter.WriteBool(entry.hasData);
                packetWriter.WriteCompoundTag(entry.data);
            }
        }
    }

    public class RegistryDataPacketEntry
    {
        public RegistryDataPacketEntry(Identifier entryId, bool hasData, NbtCompoundTag data)
        {
            this.entryId = entryId;
            this.hasData = hasData;
            this.data = data;
        }

        public Identifier entryId { get; }
        public bool hasData { get; }
        public NbtCompoundTag data { get; }
    }
}
