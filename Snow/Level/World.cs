using Snow.Entities;
using Snow.Network;
using Snow.Network.Entity;
using Snow.Network.Packets.Play.Clientbound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Level
{
    internal class World
    {
        public List<Chunk> chunks = new List<Chunk>();
        public int SectionsPerColumn = 24;

        private List<Entity> entities = new List<Entity>();
        private List<int> usedEntityIds = new List<int>();

        int GetRandomEntityId()
        {
            Random rng = new Random();
            int id = rng.Next(0, 100000);

            if(usedEntityIds.Contains(id))
            {
                return GetRandomEntityId();
            }

            usedEntityIds.Add(id);
            return id;
        }

        public void SpawnEntity(Entity entity)
        {
            entity.Id = GetRandomEntityId();
            entities.Add(entity);

            entity.world = this;

        }

        public void SendEntityToPlayer(EntityPlayer player, Entity entity)
        {
            PlayerConnection connection = player.connection;

            connection.SendPacket(new SpawnEntity(entity);
        }

    }
}
