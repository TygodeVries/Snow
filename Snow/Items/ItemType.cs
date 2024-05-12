using Snow.Levels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Items
{
    public class ItemType
    {
        private int networkId = 1;
        private int customModelData;
        private string defaultName = "Unnamed Item";

        public int GetNetworkId()
        {
            return networkId;
        }

        public string GetDefaultName()
        {
            return defaultName;
        }

        public int GetCustomModelData()
        {
            return customModelData;
        }

        /// <summary>
        /// Create a non-block item
        /// </summary>
        /// <param name="id"></param>
        /// <param name="networkId"></param>
        /// <param name="customModelData"></param>
        /// <param name="defaultName"></param>
        public ItemType(ItemMaterial material, string defaultName, int customModelData)
        {
            this.networkId = (int) material;
            this.customModelData = customModelData;
            this.defaultName = defaultName;
        }
    }
}
