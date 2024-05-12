using Snow.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Items
{
    /// <summary>
    /// ItemBlockType is an item type that can be placed down to create a block
    /// </summary>
    public class ItemBlockType : ItemType
    {
        public BlockMaterial blockMaterial;

        public ItemBlockType(ItemMaterial material, BlockMaterial blockMaterial, string defaultName, int customModelData) : base(material, defaultName, customModelData)
        {
            this.blockMaterial = blockMaterial;
        }
    }
}
