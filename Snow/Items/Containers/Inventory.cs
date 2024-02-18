using Snow.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Items.Containers
{
    public class Inventory
    {
        private ItemStack[] content;
        private int size;

        public Inventory(int size)
        {
            this.size = size;
            content = new ItemStack[size];
            for(int i = 0; i < size; i++)
            {
                content[i] = null;
            }
        }

        public void SetItem(int index, ItemStack item)
        {
            content[index] = item;
        }

        public ItemStack GetItem(int index)
        {
            return content[index];
        }

        public ItemStack[] GetContent()
        {
            return content;
        }

        public int GetSize()
        {
            return size;
        }
    }
}
