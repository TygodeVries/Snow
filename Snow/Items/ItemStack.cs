using Snow.Formats.Nbt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Items
{
    public class ItemStack
    {
        private ItemType itemType;
        public ItemType GetItemType()
        {
            return itemType;
        }

        byte amount;
        public void SetAmount(int amount)
        {
            this.amount = (byte) amount;
        }

        public int GetAmount()
        {
            return amount;
        }

        public NbtCompoundTag GetNbtData()
        {
            return new NbtCompoundTag();
        }

        public ItemStack(ItemType itemType)
        {
            this.itemType = itemType;
            this.amount = 1;
        }

        public ItemStack(ItemType itemType, byte amount)
        {
            this.itemType = itemType;
            this.amount = amount;
        }
    }
}
