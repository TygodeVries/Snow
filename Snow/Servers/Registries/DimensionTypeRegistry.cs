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
    public class DimensionTypeRegistry : Registry
    {
        public List<DimensionType> dimensionTypes = new List<DimensionType>();
        public override void SendPacketToConnection(Connection connection)
        {
            List<RegistryDataPacketEntry> entries = new List<RegistryDataPacketEntry>();

            foreach (DimensionType entry in dimensionTypes)
            {
                NbtCompoundTag tag = new NbtCompoundTag();
                tag.AddField("has_skylight", new NbtByteTag(entry.hasSkylight));
                tag.AddField("has_ceiling", new NbtByteTag(entry.hasCeiling));
                tag.AddField("ultrawarm", new NbtByteTag(entry.ultrawarm));
                tag.AddField("natural", new NbtByteTag(entry.natural));
                tag.AddField("coordinate_scale", new NbtDoubleTag(entry.coordinateScale));
                tag.AddField("bed_works", new NbtByteTag(entry.bedWorks));
                tag.AddField("respawn_anchor_works", new NbtByteTag(entry.respawnAnchorWorks));

                tag.AddField("min_y", new NbtIntTag(entry.minY));
                tag.AddField("height", new NbtIntTag(entry.height));
                tag.AddField("logical_height", new NbtIntTag(entry.logicalHeight));
                tag.AddField("infiniburn", new NbtStringTag(entry.infiniburn));
                tag.AddField("effects", new NbtStringTag(entry.effects));
                tag.AddField("ambient_light", new NbtFloatTag(entry.ambientLight));
                tag.AddField("piglin_safe", new NbtByteTag(entry.piglinSafe));

                tag.AddField("has_raids", new NbtByteTag(entry.hasRaids));
                tag.AddField("monster_spawn_light_level", new NbtIntTag(entry.monsterSpawnLightLevel));
                tag.AddField("monster_spawn_block_light_limit", new NbtIntTag(entry.monsterSpawnBlockLightLimit));

                entries.Add(new RegistryDataPacketEntry(entry.identifier, true, tag));
            }

            connection.SendPacket(new RegistryDataPacket(new Identifier("minecraft", "dimension_type"), entries));
        }
    }

    public class DimensionType
    {
        public Identifier identifier;

        public byte hasSkylight;
        public byte hasCeiling;
        public byte ultrawarm;
        public byte natural;
        public double coordinateScale;
        public byte bedWorks;
        public byte respawnAnchorWorks;
        public int minY;
        public int height;
        public int logicalHeight;
        public string infiniburn;
        public string effects;
        public float ambientLight;
        public byte piglinSafe;
        public byte hasRaids;
        public int monsterSpawnLightLevel;
        public int monsterSpawnBlockLightLimit;

        public DimensionType(Identifier identifier, byte hasSkylight, byte hasCeiling, byte ultrawarm, byte natural, double coordinateScale, byte bedWorks, byte respawnAnchorWorks, int minY, int height, int logicalHeight, string infiniburn, string effects, float ambientLight, byte piglinSafe, byte hasRaids, int monsterSpawnLightLevel, int monsterSpawnBlockLightLimit)
        {
            this.identifier = identifier;
            this.hasSkylight = hasSkylight;
            this.hasCeiling = hasCeiling;
            this.ultrawarm = ultrawarm;
            this.natural = natural;
            this.coordinateScale = coordinateScale;
            this.bedWorks = bedWorks;
            this.respawnAnchorWorks = respawnAnchorWorks;
            this.minY = minY;
            this.height = height;
            this.logicalHeight = logicalHeight;
            this.infiniburn = infiniburn;
            this.effects = effects;
            this.ambientLight = ambientLight;
            this.piglinSafe = piglinSafe;
            this.hasRaids = hasRaids;
            this.monsterSpawnLightLevel = monsterSpawnLightLevel;
            this.monsterSpawnBlockLightLimit = monsterSpawnBlockLightLimit;
        }
    }
}
