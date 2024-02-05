using Snow.Formats.Nbt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Level
{
    public class ItemStack
    {
        public bool present;

        public int itemID;
        public byte count;

        public NbtCompoundTag nbt = new NbtCompoundTag();

        public ItemStack()
        {
            present = false;
            itemID = 0;
            count = 0;
        }

        public ItemStack(int itemId, byte count)
        {
            present = true;
            this.itemID = itemId;
            this.count = count;
        }
    }
}
