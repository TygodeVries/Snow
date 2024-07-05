using Snow.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Servers.Registries
{
    public class RegistryManager
    {
        private ArmorTrimMaterialRegistry armorTrimMaterialRegistry;
        public void AddArmorTrimMaterial(ArmorTrimMaterial armorTrimMaterial)
        {
            armorTrimMaterialRegistry.armorTrimMaterials.Add(armorTrimMaterial);
        }

        private ArmorTrimPatternRegistry armorTrimPatternRegistry;
        public void AddArmorTrimPattern(ArmorTrimPattern armorTrimPattern)
        {
            armorTrimPatternRegistry.armorTrimPatterns.Add(armorTrimPattern);
        }

        private BannerPatternRegistry bannerPatternRegistry;
        public void AddBannerPattern(BannerPattern bannerPattern)
        {
            bannerPatternRegistry.bannerPattern.Add(bannerPattern);
        }
        
        private BiomeRegistry biomeRegistry;
        public void AddBiome(Biome biome)
        {
            biomeRegistry.biomes.Add(biome);
        }

        private ChatTypeRegistry chatTypeRegistry;
        public void AddChatType(ChatType chatType)
        {
            chatTypeRegistry.chatTypes.Add(chatType);
        }

        private DamageTypeRegistry damageTypeRegistry;
        public void AddDamageType(DamageType damage)
        {
            damageTypeRegistry.damageTypes.Add(damage);
        }

        private DimensionTypeRegistry dimensionTypeRegistry;
        public void AddDimensionType(DimensionType type)
        {
            dimensionTypeRegistry.dimensionTypes.Add(type);
        }
        
        private PaintingVariantRegistry paintingVariantRegistry;
        public void AddPaintingVariant(PaintingVariant painting)
        {
            paintingVariantRegistry.paintingVariants.Add(painting);
        }

        private WolfVariantRegistry wolfVariantRegistry;
        public void AddWolfVariant(WolfVariant wolfVariant)
        {
            wolfVariantRegistry.wolfVariants.Add(wolfVariant);
        }

        public void CreateRegistries()
        {
            armorTrimMaterialRegistry = new ArmorTrimMaterialRegistry();
            armorTrimPatternRegistry = new ArmorTrimPatternRegistry();
            bannerPatternRegistry = new BannerPatternRegistry();
            biomeRegistry = new BiomeRegistry();
            chatTypeRegistry = new ChatTypeRegistry();
            damageTypeRegistry = new DamageTypeRegistry();
            dimensionTypeRegistry = new DimensionTypeRegistry();
            paintingVariantRegistry = new PaintingVariantRegistry();
            wolfVariantRegistry = new WolfVariantRegistry();
        }

        public void SendAll(Connection connection)
        {
            armorTrimMaterialRegistry.SendPacketToConnection(connection);
            armorTrimPatternRegistry.SendPacketToConnection(connection);
            bannerPatternRegistry.SendPacketToConnection(connection);
            biomeRegistry.SendPacketToConnection(connection);
            chatTypeRegistry.SendPacketToConnection(connection);
            damageTypeRegistry.SendPacketToConnection(connection);
            dimensionTypeRegistry.SendPacketToConnection(connection);
            paintingVariantRegistry.SendPacketToConnection(connection);
            wolfVariantRegistry.SendPacketToConnection(connection);
        }
    }
}
