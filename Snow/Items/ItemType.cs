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

        private string id;
        public string GetId()
        {
            return id;
        }

        private int networkId = 1;
        private int customModelData;
        private string defaultName = "Unnamed Item";

        public bool IsBlock { get; protected set; }
        public BlockType blockType;

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
        public ItemType(string id, int networkId, int customModelData, string defaultName)
        {
            this.id = id;
            this.networkId = networkId;
            this.customModelData = customModelData;
            this.defaultName = defaultName;
            IsBlock = false;
        }

        public ItemType(string id, int networkId, int customModelData, string defaultName, BlockType blockType)
        {
            this.id = id;
            this.networkId = networkId;
            this.customModelData = customModelData;
            this.defaultName = defaultName;
            this.blockType = blockType;
            IsBlock = true;
        }
    }
}
