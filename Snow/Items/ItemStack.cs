using Snow.Formats.Nbt;
using Snow.Formats.Nbt.Values;
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

        NbtCompoundTag nbt;
        public NbtCompoundTag GetNbtData()
        {
            return nbt;
        }

        private string name;

        public void GenerateNbt()
        {
            string nameJson = $"{{\"text\":\"{name}\",\"italic\":false}}";
            nbt = new NbtCompoundTag()
                .AddField("display", new NbtCompoundTag()
                    .AddField("Name", new NbtStringTag(nameJson)))
                .AddField("CustomModelData", new NbtIntTag(GetItemType().GetCustomModelData()));
        }


        public ItemStack(ItemType itemType) : this(itemType, 1)
        {
        }

        public ItemStack(ItemType itemType, byte amount)
        {
            this.itemType = itemType;
            this.amount = amount;
            this.name = GetItemType().GetDefaultName();
            GenerateNbt();
        }

    }
}
