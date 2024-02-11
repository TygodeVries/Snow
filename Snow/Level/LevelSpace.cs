using Snow.Formats;
using Snow.Level.Entities;
using Snow.Network;
using Snow.Network.Packets.Play.Clientbound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Level
{
    public class LevelSpace
    {
    
        private World activeWorld;

        private List<Entity> entities = new List<Entity>();
        private List<Player> players = new List<Player>();
        private int currentEntityId = 0;

        public void SpawnEntity(Entity entity)
        {
            currentEntityId++;
            entity.levelSpace = this;
            entity.uuid = UUID.Random();
            entity.EntityID = currentEntityId;

            entities.Add(entity);

            SpawnEntity spawnEntityPacket = new SpawnEntity(entity);
            BroadcastPacket(spawnEntityPacket);

            if (entity.GetType() == typeof(Player))
            {
                players.Add((Player) entity);
            }
        }

        public Entity GetEntityWithId(int id)
        {
            foreach(Entity entity in GetAllEntities())
            {
                if(entity.EntityID == id)
                {
                    return entity;
                }
            }

            return null;
        }

        public List<Entity> GetAllEntities()
        {
            return entities;
        }

        public void BroadcastPacket(ClientboundPacket clientboundPacket)
        {
            foreach(Player player in players)
            {
                player.GetConnection().SendPacket(clientboundPacket);
            }
        }
        
    }
}
