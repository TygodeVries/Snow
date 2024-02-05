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
    public class World
    {
        public List<Chunk> chunks = new List<Chunk>();
        public int SectionsPerColumn = 24;

        private List<Entity> entities = new List<Entity>();
        private List<int> usedEntityIds = new List<int>();

        public World()
        {
            usedEntityIds.Add(0);
        }

        public List<Entity> GetAllEntities()
        {
            return entities;
        }

        private List<EntityPlayer> entityPlayers = new List<EntityPlayer>();

        public void RemoveEntity(Entity entity)
        {
            scedualedForRemoval.Add(entity);
        }

        List<Entity> scedualedForRemoval = new List<Entity>();

        public void Clean()
        {
            foreach (Entity entity in scedualedForRemoval)
            {
                entities.Remove(entity);
                usedEntityIds.Remove(entity.EntityID);

                if (entity is EntityPlayer)
                {
                    entityPlayers.Remove((EntityPlayer)entity);
                }

                RemoveEntities removeEntities = new RemoveEntities(new int[] { entity.EntityID });
                BroadcastPacket(removeEntities);
            }

            scedualedForRemoval.Clear();
        }

        int GetRandomEntityId()
        {
            int id = usedEntityIds.Last() + 1;

            Console.WriteLine("Now on ID: " + id);

            usedEntityIds.Add(id);
            return id;
        }

        public void BroadcastPacket(ClientboundPacket clientboundPacket)
        {
            foreach(EntityPlayer entity in entityPlayers)
            {
                entity.GetConnection().SendPacket(clientboundPacket);
            }
        }

        public void SpawnEntity(Entity entity)
        {
            entity.EntityID = GetRandomEntityId();
            entity.uuid = UUID.Random();
            entities.Add(entity);

            entity.world = this;

            foreach(EntityPlayer entityPlayer in entityPlayers)
            {
                entityPlayer.GetConnection().RegisterEntity(entity);
            }
            
            if(entity.GetType().Equals(typeof(EntityPlayer)))
            {
                entityPlayers.Add((EntityPlayer) entity);
            }
        }
    }
}
