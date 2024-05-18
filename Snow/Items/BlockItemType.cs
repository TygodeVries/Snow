using Snow.Levels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Items
{
    public class BlockItemType : ItemType
    {
       
        public BlockType blockType;

        public BlockItemType(ItemMaterial material, int customModelData, string defaultName, BlockType blockType) : base(material, defaultName, customModelData, null)
        {
            this.blockType = blockType;
        }
    }
}
