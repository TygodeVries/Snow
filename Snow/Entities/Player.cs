
using Snow.Items;
using Snow.Items.Containers;
using Snow.Network;
using Snow.Network.Packets.Play.Clientbound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Entities
{
    public class Player : Entity
    {
        public Player(Connection connection)
        {
            this.connection = connection;
            this.type = 123;

        }

        private Connection connection;
        public Connection GetConnection() { return connection; }

        private string name = "unnamed";
        public string GetName()
        {
            return name;
        }
        public void SetName(string str)
        {
            name = str;
        }


        private Inventory inventory = new Inventory(44);
        public Inventory GetInventory() { return inventory; }

        public void UpdateInventory()
        {
            ClientboundPacket clientboundPacket = new SetContainerContent(0x00, inventory);
            connection.SendPacket(clientboundPacket);
        }
       
        /// <summary>
        /// Spawn the client into the world
        /// </summary>
        internal void SpawnClient()
        {
            UpdateInventory();
        }

        public ItemStack GetItemMainInHand()
        {
            return inventory.GetItem(36 + GetSelectedHotbarSlot());
        }

        internal int selectedHotbarSlot = 0;
        public int GetSelectedHotbarSlot()
        {
            return selectedHotbarSlot;
        }
    }
}
