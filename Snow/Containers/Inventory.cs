using Snow.Level;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Containers
{
    internal class Inventory
    {
        public ItemStack[] content;
        public int size;

        public Inventory(int size)
        {
            this.size = size;
            content = new ItemStack[size];
            for(int i = 0; i < size; i++)
            {
                content[i] = new ItemStack();
            }
        }

        public void SetItem(int index, ItemStack item)
        {
            content[index] = item;
        }
    }
}
