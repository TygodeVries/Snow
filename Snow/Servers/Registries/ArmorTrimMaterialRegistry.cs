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
    public class ArmorTrimMaterialRegistry : Registry
    {
        public List<ArmorTrimMaterial> armorTrimMaterials = new List<ArmorTrimMaterial>();

        public override void SendPacketToConnection(Connection connection)
        {
            List<RegistryDataPacketEntry> entries = new List<RegistryDataPacketEntry>();


            foreach(ArmorTrimMaterial entry in armorTrimMaterials)
            {
                NbtCompoundTag tag = new NbtCompoundTag();
                tag.AddField("asset_name", new NbtStringTag(entry.assetName));
                tag.AddField("ingredient", new NbtStringTag(entry.ingredient));
                tag.AddField("item_model_index", new NbtFloatTag(entry.itemModelIndex));
                tag.AddField("description", new NbtStringTag(entry.description));
            }

            connection.SendPacket(new RegistryDataPacket(new Identifier("minecraft", "trim_material"), entries));
        }
    }

    public class ArmorTrimMaterial
    {
        /// <summary>
        /// The trim color model to render
        /// Located at trims/color_palettes
        /// </summary>
        public string assetName { get; set; }
        public string ingredient { get; set; }
        public float itemModelIndex { get; set; }
        // add override_armor_materials later.
        public string description { get; set; }

        public ArmorTrimMaterial(string assetName, string ingredient, float itemModelIndex, string description)
        {
            this.assetName = assetName;
            this.ingredient = ingredient;
            this.itemModelIndex = itemModelIndex;
            this.description = description;
        }
    }
}
