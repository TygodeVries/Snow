using Snow.Formats.Nbt.Values;
using Snow.Formats.Nbt;
using Snow.Formats;
using Snow.Network;
using Snow.Network.Packets.Configuration.Clientbound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Servers.Registries
{
    public class ArmorTrimPatternRegistry : Registry
    {
        public List<ArmorTrimPattern> armorTrimPatterns = new List<ArmorTrimPattern>();

        public override void SendPacketToConnection(Connection connection)
        {
            List<RegistryDataPacketEntry> entries = new List<RegistryDataPacketEntry>();

            foreach (ArmorTrimPattern entry in armorTrimPatterns)
            {
                NbtCompoundTag tag = new NbtCompoundTag();
                tag.AddField("asset_id", new NbtStringTag(entry.assetId));
                tag.AddField("template_item", new NbtStringTag(entry.templateItem));
                tag.AddField("description", new NbtStringTag(entry.description));
                tag.AddField("decal", new NbtByteTag(entry.decal));
            }

            connection.SendPacket(new RegistryDataPacket(new Identifier("minecraft", "trim_pattern"), entries));
        }
    }

    public class ArmorTrimPattern
    {
        public string assetId { get; set; }
        public string templateItem { get; set; }
        public string description { get; set; }
        public byte decal { get; set; }
    }
}
