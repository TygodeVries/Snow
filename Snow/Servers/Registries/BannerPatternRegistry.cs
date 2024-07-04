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
    public class BannerPatternRegistry : Registry
    {
        public List<BannerPattern> bannerPattern = new List<BannerPattern>();

        public override void SendPacketToConnection(Connection connection)
        {
            List<RegistryDataPacketEntry> entries = new List<RegistryDataPacketEntry>();

            foreach (BannerPattern entry in bannerPattern)
            {
                NbtCompoundTag tag = new NbtCompoundTag();
                tag.AddField("asset_id", new NbtStringTag(entry.assetId));
                tag.AddField("translation_key", new NbtStringTag(entry.translationKey));
            }

            connection.SendPacket(new RegistryDataPacket(new Identifier("minecraft", "banner_pattern"), entries));
        }
    }

    public class BannerPattern
    {
        public string assetId { get; set; }
        public string translationKey { get; set; }

        public BannerPattern(string assetId, string translationKey)
        {
            this.assetId = assetId;
            this.translationKey = translationKey;
        }
    }
}
