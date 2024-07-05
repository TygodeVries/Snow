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
    public class PaintingVariantRegistry : Registry
    {
        public List<PaintingVariant> paintingVariants = new List<PaintingVariant>();

        public override void SendPacketToConnection(Connection connection)
        {
            List<RegistryDataPacketEntry> entries = new List<RegistryDataPacketEntry>();

            foreach (PaintingVariant entry in paintingVariants)
            {
                NbtCompoundTag tag = new NbtCompoundTag();
                tag.AddField("asset_id", new NbtStringTag(entry.assetId));
                tag.AddField("width", new NbtIntTag(entry.width));
                tag.AddField("height", new NbtIntTag(entry.height));

                entries.Add(new RegistryDataPacketEntry(entry.identifier, true, tag));
            }

            connection.SendPacket(new RegistryDataPacket(new Identifier("minecraft", "painting_variant"), entries));
        }
    }

    public class PaintingVariant
    {
        public Identifier identifier;

        public string assetId;
        public int width;
        public int height;

        public PaintingVariant(Identifier identifier, string assetId, int width, int height)
        {
            this.identifier = identifier;
            this.assetId = assetId;
            this.width = width;
            this.height = height;
        }
    }
}
