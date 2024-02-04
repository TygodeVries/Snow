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
    internal class EntityPlayer : Entity
    {
        public Inventory inventory;
        
        public PlayerConnection connection;

        public EntityPlayer(PlayerConnection connection)
        {
            this.connection = connection;
        }

        public void UpdateInventory()
        {
            ClientboundPacket clientboundPacket = new SetContainerContent(0x00, inventory);
            connection.SendPacket(clientboundPacket);
        }
       
        /// <summary>
        /// Spawn the client into the world
        /// </summary>
        public void SpawnClient()
        {
            UpdateInventory();
            connection.SendAllEntitiesOfWorld(world);
        }
    }
}
