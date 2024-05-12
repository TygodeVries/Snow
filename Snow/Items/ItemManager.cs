using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Items
{
    public class ItemManager
    {
        private Dictionary<string, ItemType> itemsTypes = new Dictionary<string, ItemType>();

        public ItemType GetNamespace(string nameSpace)
        {
            if(itemsTypes.ContainsKey(nameSpace))
            {
                return itemsTypes[nameSpace];
            }
            else
            {
                Log.Err("Cant find item named " + nameSpace);
                return null;
            }
        }

        public void RegisterItemType(string nameSpace, ItemType itemType)
        {
            itemsTypes.Add(nameSpace, itemType);
        }
    }
}
