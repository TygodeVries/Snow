using Snow.Containers;
using Snow.Network;
using Snow.Network.Packets.Play.Clientbound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Entities
{
    public class EntityPlayer : Entity
    {
        public EntityPlayer(PlayerConnection connection)
        {
            this.connection = connection;
            this.type = 123;
        }

        private PlayerConnection connection;
        public PlayerConnection GetConnection() { return connection; }

        public string username = "unnamed";

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
            connection.SendAllEntitiesOfWorld(world);
        }


        internal int selectedHotbarSlot = 0;
        public int GetSelectedHotbarSlot()
        {
            return selectedHotbarSlot;
        }
    }
}
