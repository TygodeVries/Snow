using Snow.Entities;
using Snow.Formats;
using Snow.Network;
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

        public List<Entity> GetAllEntities()
        {
            return entities;
        }

        private List<EntityPlayer> entityPlayers = new List<EntityPlayer>();

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
            entity.uuid = UUID.Random();
            entities.Add(entity);

            entity.world = this;

            foreach(EntityPlayer entityPlayer in entityPlayers)
            {
                entityPlayer.connection.RegisterEntity(entity);
            }

            if(entity.GetType().Equals(typeof(EntityPlayer)))
            {
                entityPlayers.Add((EntityPlayer) entity);
            }
        }
    }
}
