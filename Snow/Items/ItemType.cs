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

        public ItemType(string id, int networkId, int customModelData, string defaultName, bool isBlock)
        {
            this.id = id;
            this.networkId = networkId;
            this.customModelData = customModelData;
            this.defaultName = defaultName;
            IsBlock = isBlock;
        }
    }
}
