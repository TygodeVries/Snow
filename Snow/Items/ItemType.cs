using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Items
{
    public abstract class ItemType
    {
        protected int itemId = 1;
        protected int customModelData;
        protected string defaultName = "Unnamed Item";

        public int GetItemId()
        {
            return itemId;
        }

        public string GetDefaultName()
        {
            return defaultName;
        }

        public int GetCustomModelData()
        {
            return customModelData;
        }

        public virtual void OnRightClick()
        { }

        public virtual void OnLeftClick()
        { }
    }
}
