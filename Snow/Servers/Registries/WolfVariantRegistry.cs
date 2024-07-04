using Snow.Formats;
using Snow.Formats.Nbt;
using Snow.Formats.Nbt.Values;
using Snow.Network;
using Snow.Network.Packets.Configuration.Clientbound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Servers.Registries
{
    public class WolfVariantRegistry : Registry
    {
        public List<WolfVariant> wolfVariants = new List<WolfVariant>();

        public override void SendPacketToConnection(Connection connection)
        {
            List<RegistryDataPacketEntry> entries = new List<RegistryDataPacketEntry>();

            foreach (WolfVariant entry in wolfVariants)
            {
                NbtCompoundTag tag = new NbtCompoundTag();
                tag.AddField("wild_texture", new NbtStringTag(entry.wildTexture));
                tag.AddField("tame_texture", new NbtStringTag(entry.tameTexture));
                tag.AddField("angry_texture", new NbtStringTag(entry.angryTexture));
                tag.AddField("biomes", new NbtStringTag(entry.biomes));
            }

            connection.SendPacket(new RegistryDataPacket(new Identifier("minecraft", "wolf_variant"), entries));
        }
    }

    public class WolfVariant
    {
        public string wildTexture;
        public string tameTexture;
        public string angryTexture;
        public string biomes;

        public WolfVariant(string wildTexture, string tameTexture, string angryTexture, string biomes)
        {
            this.wildTexture = wildTexture;
            this.tameTexture = tameTexture;
            this.angryTexture = angryTexture;
            this.biomes = biomes;
        }
    }
}
