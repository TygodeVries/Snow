using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Items
{
    public class ItemManager
    {
        private List<ItemType> itemsTypes = new List<ItemType>();

        public ItemType GetNamespace(string ns)
        {
            foreach(ItemType type in  itemsTypes)
            {
                if(type.GetId() == ns)
                {
                    return type;
                }
            }

            Log.Err("Item with ID " + ns + " cant be found");
            return null;
        }

        public void RegisterItemType(ItemType itemType)
        {
            itemsTypes.Add(itemType);
        }
    }
}
