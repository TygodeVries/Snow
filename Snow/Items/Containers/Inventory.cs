using Snow.Entities;
using Snow.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Items.Containers
{
    public class Inventory
    {
        private ItemStack[] content;
        private int size;

        private Player player;
        private InventoryType inventoryType;

        public Inventory(int size, Player player, InventoryType inventoryType)
        {
            this.inventoryType = inventoryType;
            this.player = player;
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

            if (player != null)
                player.UpdateInventory();
        }

        public ItemStack GetItem(int index)
        {
            return content[index];
        }

        public ItemStack[] GetContent()
        {
            return content;
        }

        public void AddItem(ItemStack itemStack)
        {
            int start = 0;
            int end = size;

            if (inventoryType == InventoryType.PlayerInventory)
            {
                start = 9;
                end = 45;
            }

            for (int i = start; i < end; i++)
            {
                if (content[i] == null)
                {
                    content[i] = itemStack;
                    break;
                }
            }

            if (player != null)
                player.UpdateInventory();
        }

        public int GetSize()
        {
            return size;
        }
    }

    public enum InventoryType
    {
        PlayerInventory
    }
}
