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
    public class BiomeRegistry : Registry
    {
        public List<Biome> biomes = new List<Biome>();

        public override void SendPacketToConnection(Connection connection)
        {
            List<RegistryDataPacketEntry> entries = new List<RegistryDataPacketEntry>();

            foreach (Biome entry in biomes)
            {
                NbtCompoundTag tag = new NbtCompoundTag();
    
                tag.AddField("temperature", new NbtFloatTag(entry.temperature));
                tag.AddField("downfall", new NbtFloatTag(entry.downfall));
                tag.AddField("has_precipitation", new NbtByteTag(entry.hasRain));

                BiomeEffects biomeEffects = entry.biomeEffects;
                NbtCompoundTag effects = new NbtCompoundTag();
                effects.AddField("fog_color", new NbtIntTag(biomeEffects.fogColor));
                effects.AddField("water_color", new NbtIntTag(biomeEffects.waterColor));
                effects.AddField("water_fog_color", new NbtIntTag(biomeEffects.waterFogColor));
                effects.AddField("sky_color", new NbtIntTag(biomeEffects.skyColor));
                effects.AddField("foliage_color", new NbtIntTag(biomeEffects.foliageColor));
                effects.AddField("grass_color", new NbtIntTag(biomeEffects.grassColor));
                effects.AddField("grass_color_modifier", new NbtStringTag(biomeEffects.grassColorModifier));
                
                tag.AddField("effects", effects);

                entries.Add(new RegistryDataPacketEntry(entry.identifier, true, tag));
            }

            connection.SendPacket(new RegistryDataPacket(new Identifier("minecraft", "worldgen/biome"), entries));
        }
    }

    public class Biome
    {
        public Identifier identifier;

        public byte hasRain;
        public float temperature;
        public float downfall;
        public BiomeEffects biomeEffects;

        public Biome(Identifier identifier, byte hasRain, float temperature, float downfall, BiomeEffects biomeEffects)
        {
            this.identifier = identifier;
            this.hasRain = hasRain;
            this.temperature = temperature;
            this.downfall = downfall;
            this.biomeEffects = biomeEffects;
        }
    }

    public class BiomeEffects
    {
        public int fogColor;
        public int waterColor;
        public int waterFogColor;
        public int skyColor;
        public int foliageColor; // Leaves
        public int grassColor;
        // Add particle
        public string grassColorModifier = "none";

        public BiomeEffects(int fogColor, int waterColor, int waterFogColor, int skyColor, int foliageColor, int grassColor)
        {
            this.fogColor = fogColor;
            this.waterColor = waterColor;
            this.waterFogColor = waterFogColor;
            this.skyColor = skyColor;
            this.foliageColor = foliageColor;
            this.grassColor = grassColor;
        }


        // Add sounds

    }
}
