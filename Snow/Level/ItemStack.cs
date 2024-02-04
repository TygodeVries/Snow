using Snow.Formats.Nbt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Level
{
    internal class ItemStack
    {
        public bool present = false;

        public int itemID = 1;
        public byte count = 1;

        public NbtCompoundTag nbt = new NbtCompoundTag();
    }
}
